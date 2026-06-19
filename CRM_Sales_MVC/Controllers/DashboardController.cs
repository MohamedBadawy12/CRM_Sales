using CRM_Sales_Application.CQRS.Clients.Queries;
using CRM_Sales_Application.CQRS.Projects.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRM_Sales_MVC.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly IMediator _mediator;
        public DashboardController(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task<IActionResult> Index()
        {
            var allClients = await _mediator.Send(new GetAllClientsQuery());
            var allProjects = await _mediator.Send(new GetAllProjectsQuery());
            var nextTeamInfo = await _mediator.Send(new GetNextTeamInSequenceQuery());

            var today = DateTime.Now.Date;
            var clientsList = allClients.ToList();

            ViewBag.TotalClients = allClients.Count();
            ViewBag.WalkClients = allClients.Count(c => c.Type == "Walk");
            ViewBag.FollowClients = allClients.Count(c => c.Type == "Follow");
            ViewBag.TotalProjects = allProjects.Count();
            ViewBag.NextTeamInfo = nextTeamInfo;

            ViewBag.TodayWalk = clientsList
                .Count(c => c.Type == "Walk" && c.CreatedAt.Date == today);
            ViewBag.TodayFollow = clientsList
                .Count(c => c.Type == "Follow" && c.CreatedAt.Date == today);
            ViewBag.TodayTotal = clientsList
                .Count(c => c.CreatedAt.Date == today);

            ViewBag.RecentClients = clientsList
                .OrderByDescending(c => c.CreatedAt)
                .Take(10)
                .ToList();

            return View();
        }
    }
}
