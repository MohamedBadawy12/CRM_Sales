using CRM_Sales_Application.CQRS.SalesAgents.Commands;
using CRM_Sales_Application.CQRS.SalesAgents.Queries;
using CRM_Sales_Application.CQRS.Teams.Queries;
using CRM_Sales_Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRM_Sales_MVC.Controllers
{
    [Authorize]
    public class SalesAgentController : Controller
    {
        private readonly IMediator _mediator;

        public SalesAgentController(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task<IActionResult> Index(string search = "", int page = 1)
        {
            int pageSize = 8;

            var allAgents = await _mediator.Send(new GetAllSalesAgentsQuery());
            var agentsList = allAgents.ToList();

            // Search
            var filtered = agentsList.AsEnumerable();
            if (!string.IsNullOrEmpty(search))
            {
                filtered = agentsList.Where(a =>
                    a.AgentName.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    (a.TeamName != null &&
                     a.TeamName.Contains(search, StringComparison.OrdinalIgnoreCase)) ||
                    (a.LeaderName != null &&
                     a.LeaderName.Contains(search, StringComparison.OrdinalIgnoreCase)) ||
                    a.Role.Contains(search, StringComparison.OrdinalIgnoreCase)
                );
            }

            var filteredList = filtered.OrderByDescending(a => a.CreatedAt).ToList();

            // Pagination
            int totalCount = filteredList.Count;
            int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            var paged = filteredList.Skip((page - 1) * pageSize).Take(pageSize);

            ViewBag.Search = search;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalCount = totalCount;

            return View(paged);
        }

        public async Task<IActionResult> Create()
        {
            var teams = await _mediator.Send(new GetAllTeamsQuery());
            ViewBag.Teams = teams;
            ViewBag.Leaders = new List<SalesAgentDto>();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateSalesAgentDto dto)
        {
            if (!ModelState.IsValid)
            {
                var teams = await _mediator.Send(new GetAllTeamsQuery());
                ViewBag.Teams = teams;
                ViewBag.Leaders = dto.TeamId != Guid.Empty
                    ? await _mediator.Send(new GetLeadersQuery())
                    : new List<SalesAgentDto>();
                return View(dto);
            }

            await _mediator.Send(new CreateSalesAgentCommand(
                dto.AgentName, dto.Role, dto.TeamId, dto.LeaderId));

            TempData["Success"] = "Sales Agent created successfully!";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var agent = await _mediator.Send(new GetSalesAgentByIdQuery(id));
            if (agent == null) return NotFound();

            var teams = await _mediator.Send(new GetAllTeamsQuery());
            var leaders = await _mediator.Send(new GetLeadersQuery());
            ViewBag.Teams = teams;
            ViewBag.Leaders = leaders;

            var dto = new UpdateSalesAgentDto
            {
                Id = agent.Id,
                AgentName = agent.AgentName,
                Role = agent.Role,
                TeamId = agent.TeamId,
                LeaderId = agent.LeaderId
            };
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateSalesAgentDto dto)
        {
            if (!ModelState.IsValid)
            {
                var teams = await _mediator.Send(new GetAllTeamsQuery());
                var leaders = await _mediator.Send(new GetLeadersQuery());
                ViewBag.Teams = teams;
                ViewBag.Leaders = leaders;
                return View(dto);
            }

            await _mediator.Send(new UpdateSalesAgentCommand(
                dto.Id, dto.AgentName, dto.Role, dto.TeamId, dto.LeaderId));

            TempData["Success"] = "Sales Agent updated successfully!";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _mediator.Send(new DeleteSalesAgentCommand(id));
            TempData["Success"] = "Sales Agent deleted successfully!";
            return RedirectToAction(nameof(Index));
        }

        // AJAX - Get Leaders
        [HttpGet]
        public async Task<IActionResult> GetLeaders()
        {
            var leaders = await _mediator.Send(new GetLeadersQuery());
            var result = leaders.Select(l => new
            {
                id = l.Id,
                agentName = l.AgentName
            });
            return Json(result);
        }

        // AJAX - Get Agents By Leader
        [HttpGet]
        public async Task<IActionResult> GetByLeader(Guid leaderId)
        {
            var agents = await _mediator.Send(new GetAgentsByLeaderIdQuery(leaderId));
            return Json(agents.Select(a => new { id = a.Id, agentName = a.AgentName }));
        }
    }
}
