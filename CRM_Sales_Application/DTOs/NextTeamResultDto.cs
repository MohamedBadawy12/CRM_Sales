namespace CRM_Sales_Application.DTOs
{
    public class NextTeamResultDto
    {
        public bool HasData { get; set; }
        public Guid? NextTeamId { get; set; }
        public string NextTeamName { get; set; }
        public string NextFloor { get; set; }
        public string LastTeamName { get; set; }
        public string LastFloor { get; set; }
    }
}
