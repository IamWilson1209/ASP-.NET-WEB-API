namespace ShuanWebAPI.Parameters
{
    public class DailyExpenseSearchParameters
    {
        public string Category { get; set; }
        public string? Item { get; set; }
        public DateTime? RecordDateTime { get; set; }
        public string? Bank { get; set; }
    }
}
