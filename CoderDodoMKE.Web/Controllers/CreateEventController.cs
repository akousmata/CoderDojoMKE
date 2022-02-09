using CoderDojoMKE.Web.Models.Auth;
using CoderDojoMKE.Web.Models.Data;
using CoderDojoMKE.Web.Models.View;
using System.Linq;
using System.Web.Mvc;

namespace CoderDojoMKE.Web.Controllers
{
    [Authorize(Roles = "Mentor,GlobalAdmin")]
    public class CreateEventController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly EventManager _eventManager;

        public CreateEventController()
        {
            _context = new ApplicationDbContext();
            _eventManager = new EventManager(_context);
        }

        public CreateEventController(ApplicationDbContext context)
        {
            _context = context;
            _eventManager = new EventManager(context);
        }

        // GET: CreateEvent
        [HttpGet]
        public ActionResult Index()
        {
            var model = new CreateEventViewModel
            {
                EventInstructions = _context.EventInstructionsSet.Select(e => new SelectListItem { Text = e.Instructions, Value = e.ID.ToString() })
            };

            return View(model);
        }

        // POST: CreateEvent
        [HttpPost]
        public ActionResult Index(CreateEventViewModel evt)
        {
            _eventManager.Create(evt, User.Identity.Name);
            var model = new CreateEventViewModel
            {
                EventInstructions = _context.EventInstructionsSet.Select(e => new SelectListItem { Text = e.Instructions, Value = e.ID.ToString() })
            };
            return View(model);
        }

        // GET: CreateEvent/Instructions
        [HttpGet]
        public ActionResult Instructions()
        {
            return View();
        }

        // GET: CreateEvent/Instructions
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Instructions(string instructions)
        {
            _eventManager.CreateInstructions(instructions);
            return View();
        }
    }
}