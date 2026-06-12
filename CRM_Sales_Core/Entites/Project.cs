namespace CRM_Sales_Core.Entites
{
    public class Project : BaseEntity
    {
        public string ProjectName { get; private set; }
        public string Location { get; private set; }
        public string Area { get; private set; }
        public ICollection<Client> Clients { get; private set; }
            = new List<Client>();

        protected Project() { }

        public Project(string projectName, string location, string area)
        {
            ProjectName = projectName;
            Location = location;
            Area = area;
        }

        public void Update(string projectName, string location, string area)
        {
            ProjectName = projectName;
            Location = location;
            Area = area;
            SetUpdatedAt();
        }
    }
}
