using CRM_Sales_Application.CQRS.Clients.Queries;
using CRM_Sales_Application.DTOs;
using CRM_Sales_Core.Entites;
using CRM_Sales_Core.Interfaces;
using MediatR;

namespace CRM_Sales_Application.CQRS.Clients.Handlers
{
    public class GetNextTeamInSequenceHandler : IRequestHandler<GetNextTeamInSequenceQuery, NextTeamResultDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetNextTeamInSequenceHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<NextTeamResultDto> Handle(
            GetNextTeamInSequenceQuery request, CancellationToken cancellationToken)
        {
            var allTeams = (await _unitOfWork.Teams.GetAllAsync())
                .OrderBy(t => t.CreatedAt)
                .ToList();

            if (!allTeams.Any())
                return new NextTeamResultDto { HasData = false };

            var floors = allTeams.Select(t => (int)t.Floor).Distinct().OrderBy(f => f).ToList();

            var teamsByFloor = floors.ToDictionary(
                f => f,
                f => allTeams.Where(t => (int)t.Floor == f).OrderBy(t => t.CreatedAt).ToList()
            );

            var allClients = await _unitOfWork.Clients.GetAllWithIncludesAsync();
            var allAgents = (await _unitOfWork.SalesAgents.GetAllWithIncludesAsync()).ToList();

            var walkClientsOrdered = allClients
                .Where(c => c.Type == "Walk")
                .OrderByDescending(c => c.CreatedAt)
                .ToList();

            if (!walkClientsOrdered.Any())
            {
                var firstFloor = floors.First();
                var firstTeam = teamsByFloor[firstFloor].First();

                return new NextTeamResultDto
                {
                    HasData = true,
                    NextTeamId = firstTeam.Id,
                    NextTeamName = firstTeam.TeamName,
                    NextFloor = firstTeam.Floor.ToString(),
                    LastTeamName = null,
                    LastFloor = null
                };
            }

            var lastClient = walkClientsOrdered.First();
            var lastAgent = allAgents.FirstOrDefault(a => a.Id == lastClient.AgentId);
            var lastTeamOverall = allTeams.FirstOrDefault(t => t.Id == lastAgent?.TeamId);

            if (lastTeamOverall == null)
                return new NextTeamResultDto { HasData = false };

            int lastFloorValue = (int)lastTeamOverall.Floor;
            int lastFloorIndex = floors.IndexOf(lastFloorValue);

            int nextFloorIndex = (lastFloorIndex + 1) % floors.Count;
            int nextFloorValue = floors[nextFloorIndex];
            var teamsInNextFloor = teamsByFloor[nextFloorValue];

            Team nextTeam;

            var lastClientInThatFloor = walkClientsOrdered
                .Select(c => new
                {
                    Client = c,
                    Agent = allAgents.FirstOrDefault(a => a.Id == c.AgentId)
                })
                .Where(x => x.Agent != null)
                .Select(x => new
                {
                    x.Client,
                    x.Agent,
                    Team = allTeams.FirstOrDefault(t => t.Id == x.Agent.TeamId)
                })
                .Where(x => x.Team != null && (int)x.Team.Floor == nextFloorValue)
                .FirstOrDefault();

            if (lastClientInThatFloor == null)
            {
                nextTeam = teamsInNextFloor.First();
            }
            else
            {
                int idx = teamsInNextFloor.FindIndex(t => t.Id == lastClientInThatFloor.Team.Id);
                int nextIdx = (idx + 1) % teamsInNextFloor.Count;
                nextTeam = teamsInNextFloor[nextIdx];
            }

            return new NextTeamResultDto
            {
                HasData = true,
                NextTeamId = nextTeam.Id,
                NextTeamName = nextTeam.TeamName,
                NextFloor = nextTeam.Floor.ToString(),
                LastTeamName = lastTeamOverall.TeamName,
                LastFloor = lastTeamOverall.Floor.ToString()
            };
        }
    }
}
