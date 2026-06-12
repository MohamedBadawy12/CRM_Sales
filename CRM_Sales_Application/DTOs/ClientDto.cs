namespace CRM_Sales_Application.DTOs
{
    public class ClientDto
    {
        public Guid Id { get; set; }
        public string ClientName { get; set; }
        public string Phone { get; set; }
        public Guid ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string Type { get; set; }
        public Guid AgentId { get; set; }
        public string AgentName { get; set; }
        public Guid? PreviousAgentId { get; set; }
        public string? PreviousAgentName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
    public class CreateClientDto
    {
        public string ClientName { get; set; }
        public string Phone { get; set; }
        public Guid ProjectId { get; set; }
        public string Type { get; set; }
        public Guid AgentId { get; set; }
        public Guid? PreviousAgentId { get; set; }
    }

    public class UpdateClientDto
    {
        public Guid Id { get; set; }
        public string ClientName { get; set; }
        public string Phone { get; set; }
        public Guid ProjectId { get; set; }
        public string Type { get; set; }
        public Guid AgentId { get; set; }
        public Guid? PreviousAgentId { get; set; }
    }
}
