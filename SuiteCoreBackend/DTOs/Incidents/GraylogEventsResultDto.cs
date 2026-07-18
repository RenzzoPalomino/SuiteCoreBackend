namespace SuiteCoreBackend.DTOs.Incidents
{
    /// <summary>Resultado de una consulta de eventos a Graylog (usado por events y security-events).</summary>
    public class GraylogEventsResultDto
    {
        public bool Success { get; set; }
        public string Service { get; set; } = string.Empty;
        public string Query { get; set; } = string.Empty;
        public string BuiltQuery { get; set; } = string.Empty;
        public int RangeSeconds { get; set; }
        public int Limit { get; set; }
        public int TotalResults { get; set; }
        public int MessagesCount { get; set; }
        public List<GraylogEventItemDto> Items { get; set; } = new();
    }

    /// <summary>Evento individual devuelto por Graylog.</summary>
    public class GraylogEventItemDto
    {
        public string Id { get; set; } = string.Empty;
        public string Index { get; set; } = string.Empty;
        public string Timestamp { get; set; } = string.Empty;
        public string Source { get; set; } = string.Empty;
        public int Level { get; set; }
        public string Facility { get; set; } = string.Empty;
        public string ApplicationName { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string FullMessage { get; set; } = string.Empty;
        public string RemoteIp { get; set; } = string.Empty;
        public List<string> Streams { get; set; } = new();
    }
}
