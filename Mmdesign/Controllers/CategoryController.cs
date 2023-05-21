using Dapper;
using log4net;
using Mmdesign.Helpers;
using Mmdesign.Models;
using Mmdesign.Models.Entity;
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
    public class CategoryController : Controller
    {
        private readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly string connString;

        static CategoryController()
        {
            connString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }

        [Authorize]
        // GET: /Category/Manage
        public ActionResult Manage()
        {
            logger.Info("Category->Manage");

            return View();
        }

        [Authorize]
        [HttpGet]
        public ActionResult AddCategory(int id = 0)
        {
            CategoryViewModel viewModel = new CategoryViewModel();
            if (id > 0)
            {
                viewModel = GetCategoryViewModel(id);
            }
            ViewBag.IdParents = GetIdParents();

            return PartialView("_CategoryAdd", viewModel);
        }

        private List<SelectListItem> GetIdParents()
        {
            using (MyContextDb db = new MyContextDb())
            {
                db.Configuration.LazyLoadingEnabled = false;

                var categories = (from a in db.Categories
                                  where (a.IsActive)
                                  select new SelectListItem()
                                  {
                                      Text = a.Name,
                                      Value = a.Id.ToString()
                                  }).ToList();
                categories.Insert(0, new SelectListItem()
                {
                    Value = "0",
                    Text = "----- Hãy chọn một danh mục -----"
                });
                return categories;
            }
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveCategory(CategoryViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_CategoryAdd", viewModel);
            }
            var (saveSuccess, saveDuplicate, errorMsg) = SaveCategoryToDB(viewModel);
            return Json(new { success = saveSuccess, duplicate = saveDuplicate, error = errorMsg }, JsonRequestBehavior.AllowGet);
        }

        private (bool, bool, string) SaveCategoryToDB(CategoryViewModel viewModel)
        {
            var category = AutoMapper.Mapper.Map<Category>(viewModel);

            if (category.Id == 0)
            {
                category.IsActive = true;
                category.DateCreated = DateTime.Now;
                category.CreatedBy = (HttpContext.User as CustomPrincipal).UserId;

                var existed = FindCategoryByName(category.Name);
                if (existed != null)
                {
                    return (false, true, string.Empty);
                }
            }
            else
            {
                var existed = FindCategoryByIdAndName(category.Id, category.Name);
                if (existed != null && existed.Id != category.Id)
                {
                    return (false, true, string.Empty);
                }

                var update = FindCategoryById(category.Id);
                if (update != null)
                {
                    category.IsActive = update.IsActive;
                    category.OrderNo = update.OrderNo;
                    category.CreatedBy = update.CreatedBy;
                    category.DateCreated = update.DateCreated;
                }
            }

            //Updated By and Update On
            category.DateUpdated = DateTime.Now;
            category.UpdatedBy = (HttpContext.User as CustomPrincipal).UserId;

            using (MyContextDb db = new MyContextDb())
            {
                if (category.Id > 0)
                {
                    db.Categories.Attach(category);
                    db.Entry(category).State = System.Data.Entity.EntityState.Modified;
                }
                else
                {
                    db.Categories.Add(category);
                }
                db.SaveChanges();
            }
            return (true, false, string.Empty);
        }

        private Category FindCategoryById(int id)
        {
            using (MyContextDb db = new MyContextDb())
            {
                db.Configuration.LazyLoadingEnabled = false;

                var category = (from a in db.Categories
                                where (a.IsActive && a.Id.Equals(id))
                                select a).FirstOrDefault();
                return category;
            }
        }

        private Category FindCategoryByName(string namne)
        {
            using (MyContextDb db = new MyContextDb())
            {
                db.Configuration.LazyLoadingEnabled = false;

                var category = (from a in db.Categories
                                where (a.IsActive && a.Name.Equals(namne, StringComparison.OrdinalIgnoreCase))
                                select a).FirstOrDefault();
                return category;
            }
        }

        private Category FindCategoryByIdAndName(int id, string namne)
        {
            using (MyContextDb db = new MyContextDb())
            {
                db.Configuration.LazyLoadingEnabled = false;

                var category = (from a in db.Categories
                                where (a.IsActive && a.Id.Equals(id) && a.Name.Equals(namne, StringComparison.OrdinalIgnoreCase))
                                select a).FirstOrDefault();
                return category;
            }
        }

        private CategoryViewModel GetCategoryViewModel(int id)
        {
            CategoryViewModel viewModel = new CategoryViewModel();
            Category category = new Category();
            using (MyContextDb db = new MyContextDb())
            {
                db.Configuration.LazyLoadingEnabled = false;

                category = (from a in db.Categories
                            where (a.IsActive && a.Id == id)
                            select a).FirstOrDefault();
            }
            viewModel = AutoMapper.Mapper.Map<CategoryViewModel>(category);
            return viewModel;
        }

        [Authorize]
        [HttpPost]
        public ActionResult LoadCategories()
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

                var query = (from a in db.Categories
                             where (a.IsActive)
                             select a);

                if (!string.IsNullOrEmpty(search))
                {
                    query = query.Where(a => a.Name.Contains(search) || a.DateCreated.ToString().Contains(search) || a.DateUpdated.ToString().Contains(search));
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

        [Authorize]
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var (success, error) = DeleteCategory(id);

            return Json(new { success = success, error = error }, JsonRequestBehavior.AllowGet);
        }

        private (bool, string) DeleteCategory(int id)
        {
            bool success = false;
            string error = string.Empty;

            try
            {
                var sql = "UPDATE [dbo].[Categories] SET [IsActive] = 0 WHERE [Id] = @Id";
                using (var conn = new SqlConnection(connString))
                {
                    conn.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@Id", id, DbType.Int32);
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
    }
}