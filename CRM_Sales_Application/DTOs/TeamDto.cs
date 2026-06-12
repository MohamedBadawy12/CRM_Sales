using CRM_Sales_Core.Enums;

namespace CRM_Sales_Application.DTOs
{
    public class TeamDto
    {
        public Guid Id { get; set; }
        public string TeamName { get; set; }
        public string Floor { get; set; }
        public DateTime CreatedAt { get; set; }
    }
    public class CreateTeamDto
    {
        public string TeamName { get; set; }
        public Floor Floor { get; set; }
    }

    public class UpdateTeamDto
    {
        public Guid Id { get; set; }
        public string TeamName { get; set; }
        public Floor Floor { get; set; }
    }

}
