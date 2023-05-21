using System.Web.Mvc;

namespace Mmdesign.Controllers
{
    public class ErrorController : Controller
    {
        // GET: /Error/Index
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        // GET: /Error/NotFound
        [HttpGet]
        public ActionResult NotFound()
        {
            return View();
        }

        // GET: /Error/AccessDenied
        [HttpGet]
        public ActionResult AccessDenied()
        {
            return View();
        }
    }
}