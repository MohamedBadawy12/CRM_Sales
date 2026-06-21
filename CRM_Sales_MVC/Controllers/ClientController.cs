using CRM_Sales_Application.CQRS.Clients.Commands;
using CRM_Sales_Application.CQRS.Clients.Queries;
using CRM_Sales_Application.CQRS.Projects.Queries;
using CRM_Sales_Application.CQRS.SalesAgents.Queries;
using CRM_Sales_Application.DTOs;
using CRM_Sales_Application.Interfaces;
using CRM_Sales_Application.Results;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRM_Sales_MVC.Controllers
{
    [Authorize]
    public class ClientController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IClientExportService _exportService;

        public ClientController(IMediator mediator, IClientExportService exportService)
        {
            _mediator = mediator;
            _exportService = exportService;
        }
        public async Task<IActionResult> Index(
            string type = "All", string search = "",
            int? month = null, int? year = null, int page = 1)
        {
            var result = await GetFilteredClients(type, search, month, year, page);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_ClientsTablePartial", result.Clients);
            }

            return View(result.Clients);
        }

        private async Task<ClientIndexResult> GetFilteredClients(
            string type, string search, int? month, int? year, int page)
        {
            int pageSize = 20;

            var allClients = await _mediator.Send(new GetAllClientsQuery());
            var allList = allClients.ToList();

            var filtered = type switch
            {
                "Walk" => allList.Where(c => c.Type == "Walk"),
                "Follow" => allList.Where(c => c.Type == "Follow"),
                _ => allList.AsEnumerable()
            };

            if (!string.IsNullOrEmpty(search))
            {
                filtered = filtered.Where(c =>
                    c.ClientName.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    c.Phone.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    (c.ProjectName?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false) ||
                    (c.AgentName?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false)
                );
            }

            if (month.HasValue) filtered = filtered.Where(c => c.CreatedAt.Month == month.Value);
            if (year.HasValue) filtered = filtered.Where(c => c.CreatedAt.Year == year.Value);

            var filteredList = filtered.OrderByDescending(c => c.CreatedAt).ToList();

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

            var nextTeamInfo = await _mediator.Send(new GetNextTeamInSequenceQuery());
            ViewBag.NextTeamInfo = nextTeamInfo;

            return new ClientIndexResult { Clients = paged };
        }


        public async Task<IActionResult> Create()
        {
            var projects = await _mediator.Send(new GetAllProjectsQuery());
            var leaders = await _mediator.Send(new GetLeadersQuery());
            var allAgents = await _mediator.Send(new GetAllSalesAgentsQuery());

            var lastAgentId = HttpContext.Session.GetString("LastAgentId");

            var nextTeamInfo = await _mediator.Send(new GetNextTeamInSequenceQuery());

            Guid? suggestedLeaderId = null;
            if (nextTeamInfo.HasData && nextTeamInfo.NextTeamId.HasValue)
            {
                var leaderOfNextTeam = leaders.FirstOrDefault(
                    l => l.TeamId == nextTeamInfo.NextTeamId.Value);
                suggestedLeaderId = leaderOfNextTeam?.Id;
            }

            ViewBag.Projects = projects;
            ViewBag.Leaders = leaders;
            ViewBag.AllAgents = allAgents;
            ViewBag.LastAgentId = lastAgentId;
            ViewBag.NextTeamInfo = nextTeamInfo;
            ViewBag.SuggestedLeaderId = suggestedLeaderId;

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

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Ok();
            }

            TempData["Success"] = "Client deleted successfully!";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Deleted()
        {
            var deletedClients = await _mediator.Send(new GetDeletedClientsQuery());
            return View(deletedClients);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Restore(Guid id)
        {
            await _mediator.Send(new RestoreClientCommand(id));

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                return Ok();

            TempData["Success"] = "Client restored successfully!";
            return RedirectToAction(nameof(Deleted));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RestoreSelected(string ids)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                var idList = ids.Split(',')
                    .Where(x => Guid.TryParse(x, out _))
                    .Select(Guid.Parse);

                foreach (var id in idList)
                {
                    await _mediator.Send(new RestoreClientCommand(id));
                }
            }

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                return Ok();

            TempData["Success"] = "Selected clients restored successfully!";
            return RedirectToAction(nameof(Deleted));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PermanentDelete(Guid id)
        {
            await _mediator.Send(new HardDeleteClientCommand(id));

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                return Ok();

            TempData["Success"] = "Client permanently deleted!";
            return RedirectToAction(nameof(Deleted));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PermanentDeleteSelected(string ids)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                var idList = ids.Split(',')
                    .Where(x => Guid.TryParse(x, out _))
                    .Select(Guid.Parse);

                foreach (var id in idList)
                {
                    await _mediator.Send(new HardDeleteClientCommand(id));
                }
            }

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                return Ok();

            TempData["Success"] = "Selected clients permanently deleted!";
            return RedirectToAction(nameof(Deleted));
        }

        [HttpPost]
        public async Task<IActionResult> ExportExcel(string ids, string type = "All",
             string search = "", int? month = null, int? year = null)
        {
            var clients = await _mediator.Send(
                new GetClientsForExportQuery(ids, type, search, month, year));

            var bytes = _exportService.ExportToExcel(clients);
            string fileName = $"Clients_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

            return File(bytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileName);
        }

        [HttpPost]
        public async Task<IActionResult> ExportPdf(string ids, string type = "All",
              string search = "", int? month = null, int? year = null)
        {
            var clients = await _mediator.Send(
                new GetClientsForExportQuery(ids, type, search, month, year));

            var bytes = _exportService.ExportToPdf(clients);
            string fileName = $"Clients_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";

            return File(bytes, "application/pdf", fileName);
        }

        [HttpGet]
        public async Task<IActionResult> PrintView(string ids, string type = "All",
             string search = "", int? month = null, int? year = null)
        {
            var clients = await _mediator.Send(
                new GetClientsForExportQuery(ids, type, search, month, year));

            return View(clients);
        }
    }
}
