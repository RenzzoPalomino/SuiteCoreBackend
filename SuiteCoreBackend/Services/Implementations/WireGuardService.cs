using Microsoft.Extensions.Options;
using Renci.SshNet;
using SuiteCoreBackend.DTOs.Vpn;
using SuiteCoreBackend.Services.Interfaces;
using SuiteCoreBackend.Settings;
using System.Text.RegularExpressions;

namespace SuiteCoreBackend.Services.Implementations
{
    public class WireGuardService : IWireGuardService
    {
        private readonly WireGuardSettings _settings;

        public WireGuardService(IOptions<WireGuardSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task<VpnGatewayStatusDto> GetGatewayStatusAsync()
        {
            return await Task.Run(() =>
            {
                using var client = CreateSshClient();
                client.Connect();

                var uptimeRaw = RunCommand(client, "cat /proc/uptime").Split(' ')[0];
                var uptime = FormatUptime(double.TryParse(
                    uptimeRaw, System.Globalization.NumberStyles.Any,
                    System.Globalization.CultureInfo.InvariantCulture, out var seconds)
                    ? seconds : 0);

                client.Disconnect();

                return new VpnGatewayStatusDto
                {
                    Server = "radius-vpn",
                    ManagementIp = _settings.Host,
                    Uptime = uptime,
                    IsOnline = true
                };
            });
        }

        public async Task<WireGuardStatusDto> GetStatusAsync()
        {
            return await Task.Run(() =>
            {
                using var client = CreateSshClient();
                client.Connect();

                var wgOutput = RunSudoCommand(client, $"wg show {_settings.Interface}");
                var confOutput = RunSudoCommand(client, $"cat /etc/wireguard/{_settings.Interface}.conf");

                client.Disconnect();

                var peers = ParsePeers(wgOutput, confOutput);

                return new WireGuardStatusDto
                {
                    Interface = _settings.Interface,
                    Network = ExtractNetwork(confOutput),
                    ServerIp = ExtractServerIp(confOutput),
                    IsActive = !string.IsNullOrWhiteSpace(wgOutput),
                    ConnectedPeers = peers.Count(p => p.IsOnline),
                    Peers = peers
                };
            });
        }

        public async Task<WireGuardStatsDto> GetStatsAsync()
        {
            return await Task.Run(() =>
            {
                using var client = CreateSshClient();
                client.Connect();

                var procNetDev = RunCommand(client, "cat /proc/net/dev");

                client.Disconnect();

                return ParseInterfaceStats(procNetDev, _settings.Interface);
            });
        }

        // ── Helpers SSH ──────────────────────────────────────────────────────────

        private SshClient CreateSshClient() =>
            new SshClient(_settings.Host, _settings.Port, _settings.Username, _settings.Password);

        private static string RunCommand(SshClient client, string command)
        {
            using var cmd = client.CreateCommand(command);
            return cmd.Execute().Trim();
        }

        // sudo sin TTY requiere -S para leer la contraseña desde stdin.
        // SSH.NET no asigna pseudo-terminal, por lo que sudo normal falla silenciosamente.
        private string RunSudoCommand(SshClient client, string command)
        {
            using var cmd = client.CreateCommand($"sudo -S {command} 2>/dev/null");
            var asyncResult = cmd.BeginExecute();
            using var stdin = new StreamWriter(cmd.CreateInputStream());
            stdin.WriteLine(_settings.Password);
            stdin.Flush();
            cmd.EndExecute(asyncResult);
            return cmd.Result.Trim();
        }

        // ── Parsers ──────────────────────────────────────────────────────────────

        private static List<WireGuardPeerDto> ParsePeers(string wgOutput, string confOutput)
        {
            var peers = new List<WireGuardPeerDto>();

            // Dividir bloques por "peer:"
            var peerBlocks = Regex.Split(wgOutput, @"(?=^peer:)", RegexOptions.Multiline)
                .Where(b => b.TrimStart().StartsWith("peer:"))
                .ToList();

            foreach (var block in peerBlocks)
            {
                var publicKey = GetLine(block, "peer:")?.Trim();
                var allowedIps = GetField(block, "allowed ips");
                var handshakeRaw = GetField(block, "latest handshake");
                var transfer = GetField(block, "transfer");

                var vpnIp = allowedIps?.Split('/')[0].Trim() ?? string.Empty;
                var clientName = ResolveClientName(confOutput, publicKey ?? string.Empty, vpnIp);

                var isOnline = IsRecentHandshake(handshakeRaw);
                var (rx, tx) = ParseTransfer(transfer);

                peers.Add(new WireGuardPeerDto
                {
                    ClientName = clientName,
                    VpnIp = vpnIp,
                    Handshake = handshakeRaw ?? "Sin handshake",
                    RxBytes = rx,
                    TxBytes = tx,
                    IsOnline = isOnline
                });
            }

            return peers;
        }

        // Busca el AllowedIPs de un peer en wg0.conf para resolver su nombre legible.
        // wg show no expone el nombre del peer; wg0.conf tampoco tiene nombres formales,
        // así que se usa la IP VPN como identificador visible cuando no hay alias.
        private static string ResolveClientName(string conf, string publicKey, string vpnIp)
        {
            if (string.IsNullOrEmpty(publicKey)) return vpnIp;

            var lines = conf.Split('\n');
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Contains(publicKey))
                {
                    // Buscar comentario # Name en la línea anterior si existe
                    if (i > 0 && lines[i - 1].TrimStart().StartsWith("#"))
                        return lines[i - 1].TrimStart().TrimStart('#').Trim();
                }
            }

            return $"Cliente-{vpnIp.Split('.').Last()}";
        }

        private static string? GetLine(string block, string prefix)
        {
            var line = block.Split('\n')
                .FirstOrDefault(l => l.TrimStart().StartsWith(prefix, StringComparison.OrdinalIgnoreCase));
            return line?.Substring(line.IndexOf(prefix) + prefix.Length).Trim();
        }

        private static string? GetField(string block, string label)
        {
            var line = block.Split('\n')
                .FirstOrDefault(l => l.TrimStart().StartsWith(label, StringComparison.OrdinalIgnoreCase));
            if (line == null) return null;
            var idx = line.IndexOf(':');
            return idx >= 0 ? line[(idx + 1)..].Trim() : null;
        }

        // Handshake reciente = dentro de los últimos 3 minutos (180 segundos).
        // WireGuard actualiza el handshake cada ~2 min mientras el peer está activo.
        private static bool IsRecentHandshake(string? handshake)
        {
            if (string.IsNullOrWhiteSpace(handshake)) return false;
            var secondsMatch = Regex.Match(handshake, @"(\d+)\s+second");
            var minutesMatch = Regex.Match(handshake, @"(\d+)\s+minute");
            var hoursMatch = Regex.Match(handshake, @"(\d+)\s+hour");
            var daysMatch = Regex.Match(handshake, @"(\d+)\s+day");

            if (daysMatch.Success || hoursMatch.Success) return false;

            var totalSeconds = 0;
            if (minutesMatch.Success) totalSeconds += int.Parse(minutesMatch.Groups[1].Value) * 60;
            if (secondsMatch.Success) totalSeconds += int.Parse(secondsMatch.Groups[1].Value);

            return totalSeconds <= 180;
        }

        // Parsea "X MiB received, Y MiB sent" → bytes
        private static (long rx, long tx) ParseTransfer(string? transfer)
        {
            if (string.IsNullOrWhiteSpace(transfer)) return (0, 0);

            var rxMatch = Regex.Match(transfer, @"([\d.]+)\s*(B|KiB|MiB|GiB)\s+received");
            var txMatch = Regex.Match(transfer, @"([\d.]+)\s*(B|KiB|MiB|GiB)\s+sent");

            return (ToBytes(rxMatch), ToBytes(txMatch));
        }

        private static long ToBytes(Match match)
        {
            if (!match.Success) return 0;
            var value = double.Parse(match.Groups[1].Value, System.Globalization.CultureInfo.InvariantCulture);
            return match.Groups[2].Value switch
            {
                "KiB" => (long)(value * 1024),
                "MiB" => (long)(value * 1024 * 1024),
                "GiB" => (long)(value * 1024 * 1024 * 1024),
                _     => (long)value
            };
        }

        // Extrae Address del [Interface] en wg0.conf → red VPN
        private static string ExtractNetwork(string conf)
        {
            var match = Regex.Match(conf, @"Address\s*=\s*([\d./]+)");
            if (!match.Success) return string.Empty;
            var ip = match.Groups[1].Value.Split('/')[0];
            return $"{string.Join(".", ip.Split('.').Take(3))}.0/24";
        }

        private static string ExtractServerIp(string conf)
        {
            var match = Regex.Match(conf, @"Address\s*=\s*([\d.]+)");
            return match.Success ? match.Groups[1].Value : string.Empty;
        }

        private static string FormatUptime(double totalSeconds)
        {
            var ts = TimeSpan.FromSeconds(totalSeconds);
            if (ts.Days > 0) return $"{ts.Days}d {ts.Hours}h";
            if (ts.Hours > 0) return $"{ts.Hours}h {ts.Minutes}m";
            return $"{ts.Minutes}m";
        }

        // /proc/net/dev: bytes packets errs drop fifo frame compressed multicast (RX)
        //                bytes packets errs drop fifo colls carrier compressed (TX)
        private static WireGuardStatsDto ParseInterfaceStats(string procNetDev, string iface)
        {
            var line = procNetDev.Split('\n')
                .FirstOrDefault(l => l.TrimStart().StartsWith(iface + ":"));

            if (line == null)
                return new WireGuardStatsDto { Interface = iface };

            var values = line[(line.IndexOf(':') + 1)..].Trim()
                .Split(' ', StringSplitOptions.RemoveEmptyEntries);

            static long Parse(string[]? arr, int idx) =>
                arr != null && idx < arr.Length && long.TryParse(arr[idx], out var v) ? v : 0;

            return new WireGuardStatsDto
            {
                Interface = iface,
                RxBytes   = Parse(values, 0),
                RxPackets = Parse(values, 1),
                RxErrors  = Parse(values, 2),
                TxBytes   = Parse(values, 8),
                TxPackets = Parse(values, 9),
                TxErrors  = Parse(values, 10),
                Timestamp = DateTime.UtcNow
            };
        }
    }
}
