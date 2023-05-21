using Dapper;
using log4net;
using Mmdesign.Helpers;
using Mmdesign.Models;
using Mmdesign.Models.Entity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Mmdesign.Controllers
{
    public class ProjectController : Controller
    {
        private readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly string connString;

        static ProjectController()
        {
            connString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }


        // GET: /Project/Index/
        // GET: /Project/
        [Authorize]
        [HttpGet]
        public ActionResult Index()
        {
            var projectViewModel = new ProjectViewModel()
            {
                TopListProjects = GetTopListProjects()
            };

            return View(projectViewModel);
        }

        [CompressFilter]
        [WhitespaceFilter]
        [Authorize]
        [HttpGet]
        public ActionResult Manage()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult LoadProjects()
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

                var query = (from a in db.Projects
                             where (a.IsActive.HasValue && a.IsActive.Value)
                             select a);

                if (!string.IsNullOrEmpty(search))
                {
                    query = query.Where(a => a.Name.Contains(search) || a.Seo.Contains(search) || a.Keyword.Contains(search) || a.Title.Contains(search) || a.Description.Contains(search) || a.ShortDesc.Contains(search) || a.Investor.Contains(search) || a.Address.Contains(search) || a.Architect.Contains(search) || a.Intro.Contains(search) || a.IntroContent.Contains(search) || a.Intro1.Contains(search) || a.Intro1Content.Contains(search) || a.Intro2.Contains(search) || a.Intro2Content.Contains(search) || a.Picture.Contains(search) || a.Picture1.Contains(search) || a.Picture2.Contains(search) || a.Picture3.Contains(search) || a.Picture4.Contains(search) || a.CategoryClasses.Contains(search) || a.Id.ToString().Contains(search) || a.CategoryId.ToString().Contains(search) || a.YearOfCompletion.ToString().Contains(search) || a.LandArea.ToString().Contains(search) || a.ConstructionArea.ToString().Contains(search) || a.Created.ToString().Contains(search));
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

        /// <summary>
        /// Quản lý hình ảnh sản phẩm
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public ActionResult Picture()
        {
            return View();
        }

        /// <summary>
        /// Quản lý hình ảnh sản phẩm
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public ActionResult LoadProjectImages()
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

            using (var db = new SqlConnection(connString))
            {
                const string sqlSelect = "SELECT ";
                const string sqlFieldsCount = " COUNT(*) ";
                const string sqlFields = @" Id, ProjectId, ImageUrl, OrderNo, Description, ROW_NUMBER() OVER(ORDER BY OrderNo) RowNumber ";
                const string sqlFrom = @" FROM [dbo].[ProjectImages] ";

                var sqlWhere = @" WHERE IsActive = 1 ";

                if (!string.IsNullOrEmpty(search))
                {
                    sqlWhere += " AND (ImageUrl IS NULL OR ImageUrl = @Search)";
                }

                var sqlRecordsTotal = sqlSelect + sqlFieldsCount + sqlFrom + sqlWhere;
                var sqlSelectWithRank = sqlSelect + sqlFields + sqlFrom + sqlWhere;

                var dynamicParameters = new DynamicParameters();
                if (!string.IsNullOrEmpty(search))
                {
                    dynamicParameters.Add("@Search", search, DbType.String);
                }

                //Records Count
                recordsTotal = db.ExecuteScalar<int>(sqlRecordsTotal);

                var sqlQuery = string.Empty;
                List<ProjectImageDto> data = new List<ProjectImageDto>();

                if (skip == 0 && pageSize == -1)
                {
                    sqlQuery = "SELECT *, OrderNo as RowNumber FROM (" + sqlSelectWithRank + ") tbl ORDER BY RowNumber";
                    if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                    {
                        sqlQuery = sqlQuery.Replace("ORDER BY RowNumber", string.Concat("ORDER BY ", sortColumn, " ", sortColumnDir));
                    }
                }
                else
                {
                    sqlQuery = "SELECT * FROM (" + sqlSelectWithRank + ") tbl ORDER BY RowNumber OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY";
                    if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                    {
                        sqlQuery = sqlQuery.Replace("ORDER BY RowNumber", string.Concat("ORDER BY ", sortColumn, " ", sortColumnDir));
                    }
                    dynamicParameters.Add("@Skip", skip, DbType.Int32);
                    dynamicParameters.Add("@Take", pageSize, DbType.Int32);
                }

                Debug.WriteLine(sqlQuery);
                data = db.Query<ProjectImageDto>(sqlQuery, dynamicParameters).ToList();

                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: /Project/Edit
        // Reference: https://www.abeautifulsite.net/posts/whipping-file-inputs-into-shape-with-bootstrap-3/
        // Reference: https://stackoverflow.com/questions/11235206/twitter-bootstrap-form-file-element-upload-button
        // Reference: https://www.geeksforgeeks.org/datatables-autowidth-option/
        // Reference: https://www.ihbc.org.uk/consultationsdb_new/examples/
        // Reference: https://datatables.net/forums/discussion/71177/server-side-pagination-with-individual-column-searching
        // Reference: https://www.c-sharpcorner.com/article/server-side-processing-with-custom-range-filtering/
        // Reference: http://www.dotnetawesome.com/2015/12/implement-custom-server-side-filtering-jquery-datatables.html
        // Reference: https://stackoverflow.com/questions/54866816/how-to-filter-server-side-jquery-datatable-with-range-date-picker
        [Authorize]
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.IdCategories = GetIdCategories();
            var project = new ProjectModel();
            project.Id = 0;
            project.Created = DateTime.Now;
            return View(nameof(Edit), project);
        }

        [Authorize]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var project = GetProjectById(id);
            if (project == null)
            {
                project = new ProjectModel();
                project.Id = 0;
            }

            ViewBag.IdCategories = GetIdCategories();

            return View(project);
        }

        private List<SelectListItem> GetIdCategories()
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

        // POST: /Project/Edit (tạo dự án)
        // Reference: https://www.c-sharpcorner.com/uploadfile/b696c4/how-to-upload-and-display-image-in-mvc/
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(ProjectModel entity)
        {
            if (entity == null)
            {
                ModelState.AddModelError("", "Bạn chưa nhập các trường dữ liệu bắt buộc.");
            }

            if (entity.CategoryId == 0)
            {
                ModelState.AddModelError("", "Vui lòng chọn danh mục.");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.IdCategories = GetIdCategories();
                entity.Created = DateTime.Now;
                return View(entity);
            }

            HttpPostedFileBase file = Request.Files["ImagePicture"];
            HttpPostedFileBase file1 = Request.Files["ImagePicture1"];
            HttpPostedFileBase file2 = Request.Files["ImagePicture2"];
            HttpPostedFileBase file3 = Request.Files["ImagePicture3"];
            HttpPostedFileBase file4 = Request.Files["ImagePicture4"];

            byte[] fileBytes = ConvertToBytes(file);
            byte[] file1Bytes = ConvertToBytes(file1);
            byte[] file2Bytes = ConvertToBytes(file2);
            byte[] file3Bytes = ConvertToBytes(file3);
            byte[] file4Bytes = ConvertToBytes(file4);

            if (entity != null)
            {
                if (entity.ImagePicture != null)
                {
                    string imagePicture = Path.GetFileName(entity.ImagePicture.FileName);
                    string imagePicturePath = Path.Combine(Server.MapPath("~/assets/images/"), imagePicture);
                    entity.Picture = $"~/assets/images/{Path.GetFileName(entity.ImagePicture.FileName)}";
                    entity.ImagePicture.SaveAs(imagePicturePath);
                }
                if (entity.ImagePicture1 != null)
                {
                    string imagePicture1 = Path.GetFileName(entity.ImagePicture1.FileName);
                    string imagePicture1Path = Path.Combine(Server.MapPath("~/assets/images/"), imagePicture1);
                    entity.Picture1 = $"~/assets/images/{Path.GetFileName(entity.ImagePicture1.FileName)}";
                    entity.ImagePicture1.SaveAs(imagePicture1Path);
                }
                if (entity.ImagePicture2 != null)
                {
                    string imagePicture2 = Path.GetFileName(entity.ImagePicture2.FileName);
                    string imagePicture2Path = Path.Combine(Server.MapPath("~/assets/images/"), imagePicture2);
                    entity.Picture2 = $"~/assets/images/{Path.GetFileName(entity.ImagePicture2.FileName)}";
                    entity.ImagePicture2.SaveAs(imagePicture2Path);
                }
                if (entity.ImagePicture3 != null)
                {
                    string imagePicture3 = Path.GetFileName(entity.ImagePicture3.FileName);
                    string imagePicture3Path = Path.Combine(Server.MapPath("~/assets/images/"), imagePicture3);
                    entity.Picture3 = $"~/assets/images/{Path.GetFileName(entity.ImagePicture3.FileName)}";
                    entity.ImagePicture3.SaveAs(imagePicture3Path);
                }
                if (entity.ImagePicture4 != null)
                {
                    string imagePicture4 = Path.GetFileName(entity.ImagePicture4.FileName);
                    string imagePicture4Path = Path.Combine(Server.MapPath("~/assets/images/"), imagePicture4);
                    entity.Picture4 = $"~/assets/images/{Path.GetFileName(entity.ImagePicture4.FileName)}";
                    entity.ImagePicture4.SaveAs(imagePicture4Path);
                }
            }

            //Id
            //CategoryId
            //Name
            //Created

            var project = AutoMapper.Mapper.Map<Project>(entity);

            //Default value
            if (project.Created.Equals(new DateTime(1, 1, 1)))
            {
                project.Created = DateTime.Now;
            }

            var loggedUserId = (HttpContext.User as CustomPrincipal).UserId;
            if (project.Id == 0)
            {
                project.IsActive = true;

                project.DateCreated = DateTime.Now;
                project.CreatedBy = loggedUserId;
            }
            else
            {
                ProjectModel existed = GetProjectById(entity.Id);
                if (existed != null)
                {
                    project.IsActive = existed.IsActive;
                    project.OrderNo = existed.OrderNo;

                    project.DateCreated = existed.DateCreated;
                    project.CreatedBy = existed.CreatedBy;

                    if (fileBytes.Length == 0)
                    {
                        project.Picture = existed.Picture;
                    }
                    if (file1Bytes.Length == 0)
                    {
                        project.Picture1 = existed.Picture1;
                    }
                    if (file2Bytes.Length == 0)
                    {
                        project.Picture2 = existed.Picture2;
                    }
                    if (file3Bytes.Length == 0)
                    {
                        project.Picture3 = existed.Picture3;
                    }
                    if (file4Bytes.Length == 0)
                    {
                        project.Picture4 = existed.Picture4;
                    }
                }
            }

            //Update in all cases
            project.DateUpdated = DateTime.Now;
            project.UpdatedBy = loggedUserId;

            using (MyContextDb db = new MyContextDb())
            {
                db.Entry(project).State = project.Id == 0 ? EntityState.Added : EntityState.Modified;

                db.SaveChanges();
            }

            return RedirectToAction(nameof(Manage));
        }

        // GET: /Project/Delete
        [Authorize]
        [HttpGet]
        public ActionResult Delete()
        {
            var projectViewModel = new ProjectViewModel()
            {
                TopListProjects = GetTopListProjects()
            };

            return View(projectViewModel);
        }

        // GET: /Project/Delete (xóa dự án)
        [Authorize]
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var (success, error) = DeleteProject(id);

            if (success)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Error = error;
                return View();
            }
        }

        private (bool, string) DeleteProject(int id)
        {
            bool success = false;
            string error = string.Empty;

            try
            {
                var sql = "UPDATE [dbo].[Projects] SET [IsActive] = 0 WHERE [Id] = @Id";
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

        // GET: /Project/Details
        [HttpGet]
        public ActionResult Details(int id)
        {
            ProjectViewModel projectViewModel = null;
            ProjectModel projectModel = null;
            try
            {
                projectModel = GetProjectById(id);
            }
            catch (Exception ex)
            {
                log.Error(ex);

                EmailSender.SendMail(ex);

                //throw;
            }

            if (projectModel == null)
            {
#if DEBUG
                return View(new ProjectViewModel());
#else
                return Redirect("~/Error/NotFound");
#endif
            }

            projectViewModel = new ProjectViewModel()
            {
                Project = projectModel,
                RelatedProjects = GetTop5RelatedProjects(id)
            };

            return View(projectViewModel);
        }

        private List<ProjectModel> GetTopListProjects()
        {
            List<ProjectModel> allProjects = new List<ProjectModel>();
            string sql = "SELECT TOP 100 * FROM [dbo].[Projects]";
            using (var conn = new SqlConnection(connString))
            {
                conn.Open();
                allProjects = conn.Query<ProjectModel>(sql).ToList();
            }

            var projects = allProjects.Take(6).ToList();
            return projects;
        }

        private List<ProjectModel> GetTop5RelatedProjects(int id)
        {
            List<ProjectModel> allProjects = new List<ProjectModel>();
            string sql = "SELECT TOP 100 * FROM [dbo].[Projects] WHERE [Id] <> @Id";
            using (var conn = new SqlConnection(connString))
            {
                conn.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@Id", id, DbType.Int32);
                allProjects = conn.Query<ProjectModel>(sql, dynamicParameters).ToList();
            }

            var projects = allProjects.Take(5).ToList();
            return projects;
        }

        private ProjectModel GetProjectById(int id)
        {
            var sql = "SELECT TOP 1 * FROM [dbo].[Projects] WHERE [IsActive] = 1 AND [Id] = @Id";
            using (var conn = new SqlConnection(connString))
            {
                conn.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@Id", id, DbType.Int32);
                var projectDetail = conn.Query<ProjectModel>(sql, dynamicParameters).FirstOrDefault();
                return projectDetail;
            }
        }

        private byte[] ConvertToBytes(HttpPostedFileBase image)
        {
            byte[] imageBytes = null;
            if (image != null)
            {
                BinaryReader reader = new BinaryReader(image.InputStream);
                imageBytes = reader.ReadBytes((int)image.ContentLength);
            }
            return imageBytes;
        }
    }
}