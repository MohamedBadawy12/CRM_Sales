using CRM_Sales_Core.Enums;

namespace CRM_Sales_Core.Entites
{
    public class Team : BaseEntity
    {
        public string TeamName { get; private set; }
        public Floor Floor { get; private set; }
        public ICollection<SalesAgent> SalesAgents { get; private set; }
            = new List<SalesAgent>();

        protected Team() { }

        public Team(string teamName, Floor floor)
        {
            TeamName = teamName;
            Floor = floor;
        }

        public void Update(string teamName, Floor floor)
        {
            TeamName = teamName;
            Floor = floor;
            SetUpdatedAt();
        }
    }
}
