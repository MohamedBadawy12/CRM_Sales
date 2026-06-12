namespace CRM_Sales_Core.Entites
{
    public class SalesAgent : BaseEntity
    {
        public string AgentName { get; private set; }
        public string Role { get; private set; }

        public Guid TeamId { get; private set; }
        public Team Team { get; private set; }

        public Guid? LeaderId { get; private set; }
        public SalesAgent Leader { get; private set; }

        public ICollection<SalesAgent> Agents { get; private set; }
            = new List<SalesAgent>();
        public ICollection<Client> Clients { get; private set; }
            = new List<Client>();

        protected SalesAgent() { }

        public SalesAgent(string agentName, string role, Guid teamId, Guid? leaderId = null)
        {
            AgentName = agentName;
            Role = role;
            TeamId = teamId;
            LeaderId = leaderId;
        }

        public void Update(string agentName, string role, Guid teamId, Guid? leaderId = null)
        {
            AgentName = agentName;
            Role = role;
            TeamId = teamId;
            LeaderId = leaderId;
            SetUpdatedAt();
        }
    }
}
