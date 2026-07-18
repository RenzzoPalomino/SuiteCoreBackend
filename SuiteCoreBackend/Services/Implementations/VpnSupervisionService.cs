using Microsoft.Extensions.Options;
using SuiteCoreBackend.DTOs.VpnSupervision;
using SuiteCoreBackend.Services.Interfaces;
using SuiteCoreBackend.Settings;
using System.Text.Json;

namespace SuiteCoreBackend.Services.Implementations
{
    public class VpnSupervisionService : IVpnSupervisionService
    {
        private readonly HttpClient _httpClient;
        private readonly ScnoSettings _settings;

        public VpnSupervisionService(HttpClient httpClient, IOptions<ScnoSettings> settings)
        {
            _httpClient = httpClient;
            _settings = settings.Value;
            _httpClient.BaseAddress = new Uri(_settings.BaseUrl);
        }

        public async Task<VpnHealthDto> GetHealthAsync()
        {
            var json = await _httpClient.GetStringAsync("/api/v1/vpn/health");
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            var healthDetail = new VpnHealthDetailDto();
            if (root.TryGetProperty("health", out var healthEl))
            {
                var components = new List<VpnHealthComponentDto>();
                if (healthEl.TryGetProperty("components", out var compsEl))
                {
                    foreach (var comp in compsEl.EnumerateArray())
                    {
                        components.Add(new VpnHealthComponentDto
                        {
                            Name    = comp.GetStringOrEmpty("name"),
                            Status  = comp.GetStringOrEmpty("status"),
                            Success = comp.TryGetProperty("success", out var s) && s.GetBoolean()
                        });
                    }
                }

                healthDetail = new VpnHealthDetailDto
                {
                    Status           = healthEl.GetStringOrEmpty("status"),
                    ComponentsTotal  = healthEl.TryGetProperty("components_total", out var ct) ? ct.GetInt32() : 0,
                    Healthy          = healthEl.TryGetProperty("healthy",          out var hy) ? hy.GetInt32() : 0,
                    Warning          = healthEl.TryGetProperty("warning",          out var wn) ? wn.GetInt32() : 0,
                    Critical         = healthEl.TryGetProperty("critical",         out var cr) ? cr.GetInt32() : 0,
                    Components       = components
                };
            }

            return new VpnHealthDto
            {
                Status = root.GetStringOrEmpty("status"),
                Health = healthDetail
            };
        }

        public async Task<TailscaleSupervisionStatusDto> GetTailscaleStatusAsync()
        {
            var json = await _httpClient.GetStringAsync("/api/v1/vpn/tailscale/status");
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            var nodes = new TailscaleNodesDto();
            if (root.TryGetProperty("nodes", out var nodesEl))
            {
                var items = new List<TailscaleNodeDto>();
                if (nodesEl.TryGetProperty("items", out var itemsEl))
                {
                    foreach (var node in itemsEl.EnumerateArray())
                    {
                        var ips = node.TryGetProperty("tailscale_ips", out var ipsEl)
                            ? ipsEl.EnumerateArray().Select(i => i.GetString() ?? string.Empty).ToList()
                            : new List<string>();

                        items.Add(new TailscaleNodeDto
                        {
                            Name         = node.GetStringOrEmpty("name"),
                            Hostname     = node.GetStringOrEmpty("hostname"),
                            Os           = node.GetStringOrEmpty("os"),
                            Online       = node.TryGetProperty("online", out var on) && on.GetBoolean(),
                            TailscaleIps = ips,
                            LastSeen     = node.GetStringOrEmpty("last_seen"),
                            Relay        = node.GetStringOrEmpty("relay"),
                            Active       = node.TryGetProperty("active", out var ac) && ac.GetBoolean()
                        });
                    }
                }

                nodes = new TailscaleNodesDto
                {
                    Total   = nodesEl.TryGetProperty("total",   out var tt) ? tt.GetInt32() : 0,
                    Online  = nodesEl.TryGetProperty("online",  out var on2) ? on2.GetInt32() : 0,
                    Offline = nodesEl.TryGetProperty("offline", out var of) ? of.GetInt32() : 0,
                    Items   = items
                };
            }

            var rawAvailable = new TailscaleRawAvailableDto();
            if (root.TryGetProperty("raw_available", out var rawEl))
            {
                rawAvailable = new TailscaleRawAvailableDto
                {
                    StatusJson = rawEl.TryGetProperty("status_json", out var sj) && sj.GetBoolean(),
                    StatusText = rawEl.TryGetProperty("status_text", out var st) && st.GetBoolean()
                };
            }

            return new TailscaleSupervisionStatusDto
            {
                Status       = root.GetStringOrEmpty("status"),
                Health       = root.GetStringOrEmpty("health"),
                CheckedAt    = root.GetStringOrEmpty("checked_at"),
                Component    = root.GetStringOrEmpty("component"),
                Description  = root.GetStringOrEmpty("description"),
                Host         = ParseHost(root),
                Service      = ParseService(root),
                Interface    = ParseInterface(root),
                TailscaleIp  = root.GetStringOrEmpty("tailscale_ip"),
                Version      = root.GetStringOrEmpty("version"),
                Nodes        = nodes,
                RawAvailable = rawAvailable
            };
        }

        public async Task<WireGuardSupervisionStatusDto> GetWireGuardStatusAsync()
        {
            var json = await _httpClient.GetStringAsync("/api/v1/vpn/wireguard/status");
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            var wireguard = new WireGuardDetailDto();
            if (root.TryGetProperty("wireguard", out var wgEl))
            {
                var interfaceInfo = new WireGuardInterfaceInfoDto();
                if (wgEl.TryGetProperty("interface", out var ifaceEl))
                {
                    interfaceInfo = new WireGuardInterfaceInfoDto
                    {
                        Name          = ifaceEl.GetStringOrEmpty("name"),
                        PublicKey     = ifaceEl.GetStringOrEmpty("public_key"),
                        PrivateKey    = ifaceEl.GetStringOrEmpty("private_key"),
                        ListeningPort = ifaceEl.GetStringOrEmpty("listening_port")
                    };
                }

                var peers = new List<WireGuardPeerInfoDto>();
                if (wgEl.TryGetProperty("peers", out var peersEl))
                {
                    foreach (var peer in peersEl.EnumerateArray())
                    {
                        peers.Add(new WireGuardPeerInfoDto
                        {
                            PublicKey        = peer.GetStringOrEmpty("public_key"),
                            Endpoint         = peer.GetStringOrEmpty("endpoint"),
                            AllowedIps       = peer.GetStringOrEmpty("allowed_ips"),
                            LatestHandshake  = peer.GetStringOrEmpty("latest_handshake"),
                            Transfer         = peer.GetStringOrEmpty("transfer"),
                            Rx               = peer.GetStringOrEmpty("rx"),
                            Tx               = peer.GetStringOrEmpty("tx")
                        });
                    }
                }

                var dump = new WireGuardDumpDto();
                if (wgEl.TryGetProperty("dump", out var dumpEl))
                {
                    var interfaceDump = new WireGuardInterfaceDumpDto();
                    if (dumpEl.TryGetProperty("interface_dump", out var ifDumpEl))
                    {
                        interfaceDump = new WireGuardInterfaceDumpDto
                        {
                            PrivateKey  = ifDumpEl.GetStringOrEmpty("private_key"),
                            PublicKey   = ifDumpEl.GetStringOrEmpty("public_key"),
                            ListenPort  = ifDumpEl.GetStringOrEmpty("listen_port"),
                            Fwmark      = ifDumpEl.GetStringOrEmpty("fwmark")
                        };
                    }

                    var peersDump = new List<WireGuardPeerDumpDto>();
                    if (dumpEl.TryGetProperty("peers_dump", out var peersDumpEl))
                    {
                        foreach (var peerDump in peersDumpEl.EnumerateArray())
                        {
                            peersDump.Add(new WireGuardPeerDumpDto
                            {
                                PublicKey             = peerDump.GetStringOrEmpty("public_key"),
                                PresharedKey          = peerDump.GetStringOrEmpty("preshared_key"),
                                Endpoint              = peerDump.GetStringOrEmpty("endpoint"),
                                AllowedIps            = peerDump.GetStringOrEmpty("allowed_ips"),
                                LatestHandshakeEpoch  = peerDump.TryGetProperty("latest_handshake_epoch", out var lhe) ? lhe.GetInt64() : 0,
                                TransferRxBytes       = peerDump.TryGetProperty("transfer_rx_bytes",      out var rxb) ? rxb.GetInt64() : 0,
                                TransferTxBytes       = peerDump.TryGetProperty("transfer_tx_bytes",      out var txb) ? txb.GetInt64() : 0,
                                PersistentKeepalive   = peerDump.GetStringOrEmpty("persistent_keepalive")
                            });
                        }
                    }

                    dump = new WireGuardDumpDto
                    {
                        InterfaceDump = interfaceDump,
                        PeersDump = peersDump
                    };
                }

                wireguard = new WireGuardDetailDto
                {
                    Interface  = interfaceInfo,
                    PeersTotal = wgEl.TryGetProperty("peers_total", out var pt) ? pt.GetInt32() : 0,
                    Peers      = peers,
                    Dump       = dump
                };
            }

            var connectivityTest = new WireGuardConnectivityTestDto();
            if (root.TryGetProperty("connectivity_test", out var connEl))
            {
                connectivityTest = new WireGuardConnectivityTestDto
                {
                    Target  = connEl.GetStringOrEmpty("target"),
                    PingOk  = connEl.TryGetProperty("ping_ok", out var po) && po.GetBoolean(),
                    Message = connEl.GetStringOrEmpty("message")
                };
            }

            return new WireGuardSupervisionStatusDto
            {
                Status           = root.GetStringOrEmpty("status"),
                Health           = root.GetStringOrEmpty("health"),
                CheckedAt        = root.GetStringOrEmpty("checked_at"),
                Component        = root.GetStringOrEmpty("component"),
                Description      = root.GetStringOrEmpty("description"),
                Host             = ParseHost(root),
                Service          = ParseService(root),
                Interface        = ParseInterface(root),
                WireGuard        = wireguard,
                ConnectivityTest = connectivityTest
            };
        }

        private static VpnHostDto ParseHost(JsonElement root)
        {
            if (!root.TryGetProperty("host", out var hostEl))
            {
                return new VpnHostDto();
            }

            return new VpnHostDto
            {
                Hostname     = hostEl.GetStringOrEmpty("hostname"),
                ManagementIp = hostEl.GetStringOrEmpty("management_ip")
            };
        }

        private static VpnServiceInfoDto ParseService(JsonElement root)
        {
            if (!root.TryGetProperty("service", out var serviceEl))
            {
                return new VpnServiceInfoDto();
            }

            return new VpnServiceInfoDto
            {
                Name   = serviceEl.GetStringOrEmpty("name"),
                Active = serviceEl.TryGetProperty("active", out var ac) && ac.GetBoolean(),
                State  = serviceEl.GetStringOrEmpty("state")
            };
        }

        private static VpnInterfaceInfoDto ParseInterface(JsonElement root)
        {
            if (!root.TryGetProperty("interface", out var ifaceEl))
            {
                return new VpnInterfaceInfoDto();
            }

            var addresses = ifaceEl.TryGetProperty("addresses", out var addrEl)
                ? addrEl.EnumerateArray().Select(a => a.GetString() ?? string.Empty).ToList()
                : new List<string>();

            return new VpnInterfaceInfoDto
            {
                Name      = ifaceEl.GetStringOrEmpty("name"),
                State     = ifaceEl.GetStringOrEmpty("state"),
                Addresses = addresses
            };
        }
    }
}
