namespace CRM_Sales_Application.DTOs
{
    public class SalesAgentDto
    {
        public Guid Id { get; set; }
        public string AgentName { get; set; }
        public string Role { get; set; }
        public Guid TeamId { get; set; }
        public string TeamName { get; set; }
        public Guid? LeaderId { get; set; }
        public string? LeaderName { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreateSalesAgentDto
    {
        public string AgentName { get; set; }
        public string Role { get; set; }
        public Guid TeamId { get; set; }
        public Guid? LeaderId { get; set; }
    }

    public class UpdateSalesAgentDto
    {
        public Guid Id { get; set; }
        public string AgentName { get; set; }
        public string Role { get; set; }
        public Guid TeamId { get; set; }
        public Guid? LeaderId { get; set; }
    }
}
