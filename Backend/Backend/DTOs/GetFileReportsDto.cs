namespace Backend.DTOs
{
    public class GetFileReportsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string TypeFile { get; set; }
        public DateOnly ReportDate { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
