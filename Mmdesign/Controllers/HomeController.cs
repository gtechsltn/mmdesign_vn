using Dapper;
using Mmdesign.Models;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;

namespace Mmdesign.Controllers
{
    public class HomeController : Controller
    {
        private static readonly string connString;

        static HomeController()
        {
            connString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }

        // GET: /Home/
        // GET: /Home/Index/
        [HttpGet]
        public ActionResult Index()
        {
            var homeViewModel = new HomeViewModel()
            {
                BestProjects = GetTopBestProjects()
            };

            return View(homeViewModel);
        }

        private List<ProjectModel> GetTopBestProjects()
        {
            List<ProjectModel> allProjects = new List<ProjectModel>();
            string sql = "SELECT TOP 100 * FROM [dbo].[Projects]";
            using (var conn = new SqlConnection(connString))
            {
                conn.Open();
                allProjects = conn.Query<ProjectModel>(sql).ToList();
            }

            var projects = allProjects.Take(10).ToList();
            return projects;
        }

        // GET: /welcome-page/
        // GET: /Home/Article/welcome-page/
        // GET: /Home/Article?slug=welcome-page
        [HttpGet]
        public ActionResult Article(string slug)
        {
            System.Web.HttpContext.Current.Application.Lock();

            List<ControllerActionDTO> allControllerActions = System.Web.HttpContext.Current.Application["AllControllerActions"] as List<ControllerActionDTO>;

            System.Web.HttpContext.Current.Application.UnLock();

            var controllerIndexActions = (from x in allControllerActions where x.ActionName.ToUpperInvariant().Contains("INDEX") select x).ToList();

            var slugUpper = slug.ToUpperInvariant();

            var redirectUrl = string.Empty;

            foreach (var item in controllerIndexActions)
            {
                var controllerName = item.ControllerName.Replace("Controller", "");
                if (controllerName.ToUpperInvariant().Equals(slugUpper))
                {
                    redirectUrl = $"~/{controllerName}/{item.ActionName}";
                    break;
                }
            }

            if (!string.IsNullOrWhiteSpace(redirectUrl))
            {
                return Redirect(redirectUrl);
            }

            if (string.IsNullOrWhiteSpace(slug))
            {
                return Redirect("~/Error/NotFound");
            }

            Article model = GetBySlug(slug);

            if (model == null)
            {
                return Redirect("~/Error/NotFound");
            }

            return View(model);
        }

        private Article GetBySlug(string slug)
        {
            string sql = "SELECT TOP 1 * FROM [dbo].[Articles] WHERE [Slug] = @Slug";
            using (var conn = new SqlConnection(connString))
            {
                conn.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@Slug", slug, DbType.String);
                var article = conn.Query<Article>(sql, dynamicParameters).FirstOrDefault();
                return article;
            }
        }
    }

    internal class ControllerActionDTO
    {
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
    }
}