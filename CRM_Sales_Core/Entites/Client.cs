namespace CRM_Sales_Core.Entites
{
    public class Client : BaseEntity
    {
        public string ClientName { get; private set; }
        public string Phone { get; private set; }

        public Guid ProjectId { get; private set; }
        public Project Project { get; private set; }

        public string Type { get; private set; } // "Walk" or "Follow"

        public Guid AgentId { get; private set; }
        public SalesAgent Agent { get; private set; }

        public Guid? PreviousAgentId { get; private set; }
        public SalesAgent PreviousAgent { get; private set; }

        protected Client() { }

        public Client(string clientName, string phone, Guid projectId,
                     string type, Guid agentId, Guid? previousAgentId = null)
        {
            ClientName = clientName;
            Phone = phone;
            ProjectId = projectId;
            Type = type;
            AgentId = agentId;
            PreviousAgentId = previousAgentId;
        }

        public void Update(string clientName, string phone, Guid projectId,
                          string type, Guid agentId, Guid? previousAgentId = null)
        {
            ClientName = clientName;
            Phone = phone;
            ProjectId = projectId;
            Type = type;
            AgentId = agentId;
            PreviousAgentId = previousAgentId;
            SetUpdatedAt();
        }
    }
}
