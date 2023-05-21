using Dapper;
using Dapper.Contrib.Extensions;
using log4net;
using Mmdesign.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Dynamic;
using System.Reflection;
using System.Web.Mvc;

namespace Mmdesign.Controllers
{
    [Authorize]
    public class MenuController : Controller
    {
        private readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly string connString;

        static MenuController()
        {
            connString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }

        // GET: /Menu/Index
        public ActionResult Index()
        {
            logger.Info(connString);
            GetMenus();
            return View();
        }

        // GET: /Menu/Manage
        public ActionResult Manage()
        {
            return View();
        }

        // GET: /Menu/Details
        public ActionResult Details(int id = 0)
        {
            var menu = GetMenuById(id);
            return View(menu);
        }

        // GET: /Menu/Edit
        public ActionResult Edit(int id)
        {
            var menu = GetMenuById(id);
            return View(menu);
        }

        // POST: /Menu/Edit
        [Authorize]
        [HttpPost]
        public ActionResult Edit(Menu menu)
        {
            var (sucess, error) = SaveMenu(menu);
            return Json(new { sucess = sucess, error = error }, JsonRequestBehavior.AllowGet);
        }

        // POST: /Menu/Delete
        [Authorize]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var (sucess, error) = DeleteMenu(id);
            return Json(new { sucess = sucess, error = error }, JsonRequestBehavior.AllowGet);
        }

        // POST: /Menu/LoadMenus
        [Authorize]
        [HttpPost]
        public ActionResult LoadMenus()
        {
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();

            var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();

            var search = Request.Form.GetValues("search[value]")[0];

            var pageSize = length != null ? Convert.ToInt32(length) : 0;
            var skip = start != null ? Convert.ToInt32(start) : 0;
            var recordsTotal = 0;

            using (MyContextDb db = new MyContextDb())
            {
                db.Configuration.LazyLoadingEnabled = false;

                var query = (from a in db.Menus
                             where (a.IsActive)
                             select a);

                if (!string.IsNullOrEmpty(search))
                {
                    query = query.Where(a => a.Name.Contains(search)
                                || a.Controller.Contains(search)
                                || a.Action.Contains(search)
                                || a.Slug.Contains(search)
                                || a.Params.Contains(search)
                                || a.Title.Contains(search)
                                || a.IsHorizontal.Equals(true)
                                || (!a.DateCreated.HasValue || a.DateCreated.ToString().Contains(search))
                                || (!a.DateUpdated.HasValue || a.DateUpdated.ToString().Contains(search))
                            );
                }

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    query = query.OrderBy(sortColumn + " " + sortColumnDir);
                }

                recordsTotal = query.Count();

                var data = (skip == 0 && pageSize == -1) ? query.ToList() : query.Skip(skip).Take(pageSize).ToList();

                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }, JsonRequestBehavior.AllowGet);
            }
        }

        private List<Menu> GetMenus()
        {
            List<Menu> allMenus = new List<Menu>();
            string sql = "SELECT * FROM [dbo].[Menus] WHERE [isActive] = 1 AND ([ParentId] IS NULL OR [ParentId] = 0)";
            using (var conn = new SqlConnection(connString))
            {
                conn.Open();
                allMenus = conn.Query<Menu>(sql).ToList();
            }

            var projects = allMenus.ToList();
            return projects;
        }

        private Menu GetMenuById(int id)
        {
            Menu fod;
            string sql = "SELECT * FROM [dbo].[Menus] WHERE [isActive] = 1 AND ([Id] = @Id)";
            using (var conn = new SqlConnection(connString))
            {
                conn.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@Id", id, DbType.Int64);
                fod = conn.QueryFirst<Menu>(sql, dynamicParameters);
            }
            return fod;
        }

        private (bool, string) DeleteMenu(int id)
        {
            bool success = false;
            string error = string.Empty;

            try
            {
                var sql = "UPDATE [dbo].[Menus] SET [IsActive] = 0 WHERE [Id] = @Id";
                using (var conn = new SqlConnection(connString))
                {
                    conn.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@Id", id, DbType.Int64);
                    var rowsAffected = conn.Execute(sql, dynamicParameters);
                    success = rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                error = ex.ToString();
            }
            return (success, error);
        }

        private (bool, string) SaveMenu(Menu menu)
        {
            bool success = true;
            string error = string.Empty;

            try
            {
                using (var conn = new SqlConnection(connString))
                {
                    conn.Open();
                    conn.Update(menu);
                }
            }
            catch (Exception ex)
            {
                success = false;
                error = ex.ToString();
            }
            return (success, error);
        }
    }
}