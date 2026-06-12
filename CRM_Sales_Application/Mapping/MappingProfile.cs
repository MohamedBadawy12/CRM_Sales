using AutoMapper;
using CRM_Sales_Application.DTOs;
using CRM_Sales_Core.Entites;

namespace CRM_Sales_Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Team
            CreateMap<Team, TeamDto>()
                .ForMember(d => d.Floor, o => o.MapFrom(s => s.Floor.ToString()));
            CreateMap<CreateTeamDto, Team>()
                .ConstructUsing(src => new Team(src.TeamName, src.Floor));
            CreateMap<UpdateTeamDto, Team>();

            // SalesAgent
            CreateMap<SalesAgent, SalesAgentDto>()
                .ForMember(d => d.TeamName, o => o.MapFrom(s => s.Team.TeamName))
                .ForMember(d => d.LeaderName, o => o.MapFrom(s => s.Leader != null ? s.Leader.AgentName : null));
            CreateMap<CreateSalesAgentDto, SalesAgent>()
                .ConstructUsing(src => new SalesAgent(src.AgentName, src.Role, src.TeamId, src.LeaderId));
            CreateMap<UpdateSalesAgentDto, SalesAgent>();

            // Project
            CreateMap<Project, ProjectDto>();
            CreateMap<CreateProjectDto, Project>()
                .ConstructUsing(src => new Project(src.ProjectName, src.Location, src.Area));
            CreateMap<UpdateProjectDto, Project>();

            // Client
            CreateMap<Client, ClientDto>()
                .ForMember(d => d.ProjectName, o => o.MapFrom(s => s.Project.ProjectName))
                .ForMember(d => d.AgentName, o => o.MapFrom(s => s.Agent.AgentName))
                .ForMember(d => d.PreviousAgentName, o => o.MapFrom(s => s.PreviousAgent != null ? s.PreviousAgent.AgentName : null));
            CreateMap<CreateClientDto, Client>()
                .ConstructUsing(src => new Client(src.ClientName, src.Phone, src.ProjectId, src.Type, src.AgentId, src.PreviousAgentId));
            CreateMap<UpdateClientDto, Client>();
        }
    }
}
