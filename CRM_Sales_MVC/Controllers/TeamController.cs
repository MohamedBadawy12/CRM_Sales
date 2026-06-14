using CRM_Sales_Application.CQRS.Teams.Commands;
using CRM_Sales_Application.CQRS.Teams.Queries;
using CRM_Sales_Application.DTOs;
using CRM_Sales_Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRM_Sales_MVC.Controllers
{
    [Authorize]
    public class TeamController : Controller
    {
        private readonly IMediator _mediator;

        public TeamController(IMediator mediator)
        {
            _mediator = mediator;
        }
        // GET: /Team
        public async Task<IActionResult> Index()
        {
            var teams = await _mediator.Send(new GetAllTeamsQuery());
            return View(teams);
        }

        // GET: /Team/Create
        public IActionResult Create()
        {
            ViewBag.Floors = Enum.GetValues(typeof(Floor))
                .Cast<Floor>()
                .Select(f => new { Value = (int)f, Text = f.ToString() });
            return View();
        }
        // POST: /Team/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateTeamDto dto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Floors = Enum.GetValues(typeof(Floor))
                    .Cast<Floor>()
                    .Select(f => new { Value = (int)f, Text = f.ToString() });
                return View(dto);
            }

            await _mediator.Send(new CreateTeamCommand(dto.TeamName, dto.Floor));
            TempData["Success"] = "Team created successfully!";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Team/Edit/{id}
        public async Task<IActionResult> Edit(Guid id)
        {
            var team = await _mediator.Send(new GetTeamByIdQuery(id));
            if (team == null) return NotFound();

            ViewBag.Floors = Enum.GetValues(typeof(Floor))
                .Cast<Floor>()
                .Select(f => new { Value = (int)f, Text = f.ToString() });

            var dto = new UpdateTeamDto
            {
                Id = team.Id,
                TeamName = team.TeamName,
                Floor = Enum.Parse<Floor>(team.Floor)
            };
            return View(dto);
        }

        // POST: /Team/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateTeamDto dto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Floors = Enum.GetValues(typeof(Floor))
                    .Cast<Floor>()
                    .Select(f => new { Value = (int)f, Text = f.ToString() });
                return View(dto);
            }

            await _mediator.Send(new UpdateTeamCommand(dto.Id, dto.TeamName, dto.Floor));
            TempData["Success"] = "Team updated successfully!";
            return RedirectToAction(nameof(Index));
        }

        // POST: /Team/Delete/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _mediator.Send(new DeleteTeamCommand(id));
            TempData["Success"] = "Team deleted successfully!";
            return RedirectToAction(nameof(Index));
        }
    }
}
