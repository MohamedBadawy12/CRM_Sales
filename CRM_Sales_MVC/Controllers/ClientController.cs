using CRM_Sales_Application.CQRS.Clients.Commands;
using CRM_Sales_Application.CQRS.Clients.Queries;
using CRM_Sales_Application.CQRS.Projects.Queries;
using CRM_Sales_Application.CQRS.SalesAgents.Queries;
using CRM_Sales_Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRM_Sales_MVC.Controllers
{
    [Authorize]
    public class ClientController : Controller
    {
        private readonly IMediator _mediator;

        public ClientController(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task<IActionResult> Index(
            string type = "All",
            string search = "",
            int? month = null,
            int? year = null,
            int page = 1)
        {
            int pageSize = 10;

            var allClients = await _mediator.Send(new GetAllClientsQuery());
            var allList = allClients.ToList();

            // Filter by type
            var filtered = type switch
            {
                "Walk" => allList.Where(c => c.Type == "Walk"),
                "Follow" => allList.Where(c => c.Type == "Follow"),
                _ => allList.AsEnumerable()
            };

            // Text Search
            if (!string.IsNullOrEmpty(search))
            {
                filtered = filtered.Where(c =>
                    c.ClientName.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    c.Phone.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    (c.ProjectName != null &&
                     c.ProjectName.Contains(search, StringComparison.OrdinalIgnoreCase)) ||
                    (c.AgentName != null &&
                     c.AgentName.Contains(search, StringComparison.OrdinalIgnoreCase))
                );
            }

            // Month Filter
            if (month.HasValue)
                filtered = filtered.Where(c => c.CreatedAt.Month == month.Value);

            // Year Filter
            if (year.HasValue)
                filtered = filtered.Where(c => c.CreatedAt.Year == year.Value);

            var filteredList = filtered.OrderByDescending(c => c.CreatedAt).ToList();

            // Pagination
            int totalCount = filteredList.Count;
            int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            var paged = filteredList.Skip((page - 1) * pageSize).Take(pageSize);

            ViewBag.CurrentFilter = type;
            ViewBag.Search = search;
            ViewBag.Month = month;
            ViewBag.Year = year;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalAll = allList.Count;
            ViewBag.TotalWalk = allList.Count(c => c.Type == "Walk");
            ViewBag.TotalFollow = allList.Count(c => c.Type == "Follow");

            return View(paged);
        }

        public async Task<IActionResult> Create()
        {
            var projects = await _mediator.Send(new GetAllProjectsQuery());
            var leaders = await _mediator.Send(new GetLeadersQuery());
            var allAgents = await _mediator.Send(new GetAllSalesAgentsQuery());

            var lastAgentId = HttpContext.Session.GetString("LastAgentId");

            ViewBag.Projects = projects;
            ViewBag.Leaders = leaders;
            ViewBag.AllAgents = allAgents;
            ViewBag.LastAgentId = lastAgentId;

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateClientDto dto)
        {
            if (!ModelState.IsValid)
            {
                var projects = await _mediator.Send(new GetAllProjectsQuery());
                var leaders = await _mediator.Send(new GetLeadersQuery());
                var allAgents = await _mediator.Send(new GetAllSalesAgentsQuery());
                ViewBag.Projects = projects;
                ViewBag.Leaders = leaders;
                ViewBag.AllAgents = allAgents;
                return View(dto);
            }

            await _mediator.Send(new CreateClientCommand(
                dto.ClientName, dto.Phone, dto.ProjectId,
                dto.Type, dto.AgentId, dto.PreviousAgentId));

            HttpContext.Session.SetString("LastAgentId", dto.AgentId.ToString());
            HttpContext.Session.SetString("LastLeaderId",
                (await _mediator.Send(new GetSalesAgentByIdQuery(dto.AgentId)))?.LeaderId?.ToString() ?? "");

            TempData["Success"] = "Client added successfully!";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var client = await _mediator.Send(new GetClientByIdQuery(id));
            if (client == null) return NotFound();

            var projects = await _mediator.Send(new GetAllProjectsQuery());
            var leaders = await _mediator.Send(new GetLeadersQuery());
            var allAgents = await _mediator.Send(new GetAllSalesAgentsQuery());
            var agentsByLeader = await _mediator.Send(
                new GetAgentsByLeaderIdQuery(client.AgentId));

            ViewBag.Projects = projects;
            ViewBag.Leaders = leaders;
            ViewBag.AllAgents = allAgents;
            ViewBag.Agents = agentsByLeader;

            var dto = new UpdateClientDto
            {
                Id = client.Id,
                ClientName = client.ClientName,
                Phone = client.Phone,
                ProjectId = client.ProjectId,
                Type = client.Type,
                AgentId = client.AgentId,
                PreviousAgentId = client.PreviousAgentId
            };
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateClientDto dto)
        {
            if (!ModelState.IsValid)
            {
                var projects = await _mediator.Send(new GetAllProjectsQuery());
                var leaders = await _mediator.Send(new GetLeadersQuery());
                ViewBag.Projects = projects;
                ViewBag.Leaders = leaders;
                return View(dto);
            }

            await _mediator.Send(new UpdateClientCommand(
                dto.Id, dto.ClientName, dto.Phone, dto.ProjectId,
                dto.Type, dto.AgentId, dto.PreviousAgentId));

            TempData["Success"] = "Client updated successfully!";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _mediator.Send(new DeleteClientCommand(id));
            TempData["Success"] = "Client deleted successfully!";
            return RedirectToAction(nameof(Index));
        }
    }
}
