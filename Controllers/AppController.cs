using DutchTreat.Repositories;
using DutchTreat.Services;
using DutchTreat.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace DutchTreat.Controllers
{
    public class AppController : Controller
  {
    private readonly IMailService _mailService;
	private readonly IDutchRepository _context;
    public AppController(IMailService mailService, IDutchRepository context)
    {
      _mailService = mailService;
      _context = context;
    }
    public IActionResult Index()
    {
      return View();
    }
        [Authorize]
        [HttpGet("contact")]
    public IActionResult Contact()
    {
      return View();
    }
    [HttpPost("contact")]
    public IActionResult Contact(ContactViewModel model)
    {
      if (ModelState.IsValid)
      {
        // Send the Email
        _mailService.SendMessage("shawn@wildermuth.com", model.Subject, $"From: {model.Name} - {model.Email}, Message: {model.Message}");
        ViewBag.UserMessage = "Mail Sent...";
        ModelState.Clear();
      }
      return View();
    }

    public IActionResult About ()
    {
      return View();
    }
		[Authorize]
		public IActionResult Shop()
    {
            //var results = _context.Products.OrderBy(p => p.Title).ToList();
            var result = _context.FindAll();
            return View(result.ToList());
    }
    }
}
