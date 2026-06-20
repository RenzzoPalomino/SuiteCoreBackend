using SuiteCoreBackend.Enums;

namespace SuiteCoreBackend.Helpers
{
    public class DateTimeHelper
    {
        public DateTime GetPeruDateTime()
        {
            return DateTime.UtcNow.AddHours((int)DateValues.UTC_MINUS_FIVE);
        }
    }
}
