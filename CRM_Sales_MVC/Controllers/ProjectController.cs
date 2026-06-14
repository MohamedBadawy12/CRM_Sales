using CRM_Sales_Application.CQRS.Projects.Commands;
using CRM_Sales_Application.CQRS.Projects.Queries;
using CRM_Sales_Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRM_Sales_MVC.Controllers
{
    [Authorize]
    public class ProjectController : Controller
    {
        private readonly IMediator _mediator;
        public ProjectController(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task<IActionResult> Index()
        {
            var projects = await _mediator.Send(new GetAllProjectsQuery());
            return View(projects);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateProjectDto dto)
        {
            if (!ModelState.IsValid) return View(dto);

            await _mediator.Send(new CreateProjectCommand(
                dto.ProjectName, dto.Location, dto.Area));

            TempData["Success"] = "Project created successfully!";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var project = await _mediator.Send(new GetProjectByIdQuery(id));
            if (project == null) return NotFound();

            var dto = new UpdateProjectDto
            {
                Id = project.Id,
                ProjectName = project.ProjectName,
                Location = project.Location,
                Area = project.Area
            };
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateProjectDto dto)
        {
            if (!ModelState.IsValid) return View(dto);

            await _mediator.Send(new UpdateProjectCommand(
                dto.Id, dto.ProjectName, dto.Location, dto.Area));

            TempData["Success"] = "Project updated successfully!";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _mediator.Send(new DeleteProjectCommand(id));
            TempData["Success"] = "Project deleted successfully!";
            return RedirectToAction(nameof(Index));
        }
    }
}
