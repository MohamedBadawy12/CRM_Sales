namespace CRM_Sales_Application.DTOs
{
    public class ProjectDto
    {
        public Guid Id { get; set; }
        public string ProjectName { get; set; }
        public string Location { get; set; }
        public string Area { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreateProjectDto
    {
        public string ProjectName { get; set; }
        public string Location { get; set; }
        public string Area { get; set; }
    }

    public class UpdateProjectDto
    {
        public Guid Id { get; set; }
        public string ProjectName { get; set; }
        public string Location { get; set; }
        public string Area { get; set; }
    }
}
