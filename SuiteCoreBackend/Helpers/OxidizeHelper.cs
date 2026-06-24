using System.Text.Json;

namespace SuiteCoreBackend.Helpers
{
    public static class OxidizeHelper
    {
        public static long? ConvertToEpoch(string? dateValue)
        {
            if (string.IsNullOrWhiteSpace(dateValue))
                return null;

            try
            {
                // Oxidized devuelve fechas como: 2026-06-20 21:43:41 +0000
                // .NET trabaja mejor con: 2026-06-20 21:43:41 +00:00
                var normalizedDate = System.Text.RegularExpressions.Regex.Replace(
                    dateValue,
                    @" ([+-]\d{2})(\d{2})$",
                    " $1:$2"
                );

                var dateTimeOffset = DateTimeOffset.Parse(normalizedDate);

                return dateTimeOffset.ToUnixTimeSeconds();
            }
            catch
            {
                return null;
            }
        }

        public static string NormalizeOxidizedConfig(string content)
        {
            if (string.IsNullOrWhiteSpace(content))
                return string.Empty;

            var trimmedContent = content.Trim();

            // Caso: Oxidized devuelve JSON array de líneas
            if (trimmedContent.StartsWith("["))
            {
                try
                {
                    var lines = JsonSerializer.Deserialize<List<string>>(trimmedContent);

                    if (lines != null)
                    {
                        return string.Concat(lines);
                    }
                }
                catch
                {
                    // Si falla el parseo, devuelve el contenido original
                    return content;
                }
            }

            // Caso: Oxidized devuelve texto plano
            return content;
        }
    }
}
