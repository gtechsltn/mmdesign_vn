using Dapper;
using Dapper.Contrib.Extensions;
using log4net;
using Mmdesign.Helpers;
using Mmdesign.Models;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Mmdesign.Controllers
{
    [CustomAuthorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly string connString;

        static AdminController()
        {
            connString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }

        // GET: /Admin/DbUp
        [HttpGet]
        public ActionResult DbUp()
        {
            StringBuilder error = new StringBuilder();

            //EF6
            try
            {
                using (var context = new MyContextDb())
                {
                    context.Projects.Add(new Models.Entity.Project()
                    {
                        Name = "Chung cư Tecco Garden",
                        Description = "Chung cư Tecco Garden",
                        Created = DateTime.Now,
                        IsActive = true,
                        Investor = "Tecco Group",
                        Address = "Tứ Hiệp, Thanh Trì",
                        LandArea = 14470,
                        ConstructionArea = 6531,
                        YearOfCompletion = 2020,
                        Architect = "Ngô Quang Mạnh",
                        Picture = "assets/images/parallax1.jpg",
                        Picture1 = "assets/images/popup/small-1-1.jpg",
                        Picture2 = "assets/images/popup/small-2-1.jpg",
                        Picture3 = "assets/images/popup/small-3-1.jpg",
                        Picture4 = "assets/images/popup/small-4-1.jpg"
                    });
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                error.Append(ex.ToString());

                log.Error(ex);

                EmailSender.SendMail(ex);
            }

            //Dapper
            try
            {
                using (var conn = new SqlConnection(connString))
                {
                    conn.Open();
                }
            }
            catch (Exception ex)
            {
                error.Append(ex.ToString());

                log.Error(ex);

                EmailSender.SendMail(ex);
            }

            ViewBag.Error = error.ToString();

            return View();
        }

        // GET: /Admin/Index
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        // GET: /Admin/Article
        [HttpGet]
        public ActionResult Article()
        {
            return View();
        }

        // POST: /Admin/Article (tạo bài viết)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Article(ArticleModel data)
        {
            var user = Membership.GetUser(HttpContext.User.Identity.Name, false) as CustomMembershipUser;
            Debug.WriteLine(user.Email);

            if (ModelState.IsValid)
            {
                if (data != null && !string.IsNullOrWhiteSpace(data.Content))
                {
                    Debug.WriteLine(data.Content);

                    var title = data.Title;
                    var slug = title.MakeUrlFriendly();

                    Article slugArticle = GetBySlug(slug);

                    if (slugArticle != null)
                    {
                        ModelState.AddModelError("", "Đường dẫn bài viết đã tồn tại.");
                        return View(data);
                    }
                    else
                    {
                        Article titleArticle = GetByTitle(title);
                        if (titleArticle != null)
                        {
                            ModelState.AddModelError("", "Tiêu đề bài viết đã tồn tại.");
                            return View(data);
                        }
                        else
                        {
                            var article = new Article()
                            {
                                Title = title,
                                Slug = slug,
                                AuthorId = Guid.Parse(ConfigurationManager.AppSettings["AdminAuthorId"]),
                                Content = data.Content,
                                Created = DateTime.Now,
                                Published = DateTime.Now,
                            };

                            using (var conn = new SqlConnection(connString))
                            {
                                conn.Open();
                                var id = conn.Insert(article);
                                if (id > 0)
                                {
                                    return Redirect($"~/{slug}");
                                }
                                else
                                {
                                    ModelState.AddModelError("", "Nội dung bài viết không hợp lệ.");
                                    return View(data);
                                }
                            }
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Nội dung bài viết không hợp lệ.");
                    return View(data);
                }
            }
            else
            {
                ModelState.AddModelError("", "Dữ liệu không hợp lệ.");
                return View(data);
            }
        }

        [HttpPost]
        public string Upload(HttpPostedFileBase file)
        {
            try
            {
                string extension = Path.GetExtension(file.FileName);

                string fileid = Guid.NewGuid().ToString();

                fileid = Path.ChangeExtension(fileid, extension);

                string location = "";

                if (file != null && file.ContentLength > 0)
                {
                    const int megabyte = 1024 * 1024;

                    if (!file.ContentType.StartsWith("image/"))
                    {
                        throw new InvalidOperationException("Invalid MIME content type.");
                    }

                    string[] extensions = { ".gif", ".jpg", ".png" };
                    if (!extensions.Contains(extension))
                    {
                        throw new InvalidOperationException("Invalid file extension.");
                    }

                    if (file.ContentLength > (8 * megabyte))
                    {
                        throw new InvalidOperationException("File size limit exceeded.");
                    }

                    string savePath = Server.MapPath(@"~/Uploads/Images/" + fileid);

                    file.SaveAs(savePath);

                    location = Path.Combine("/Uploads/Images/", fileid).Replace('\\', '/');
                }

                return "<script>top.$('.mce-btn.mce-open').parent().find('.mce-textbox').val('" + location + "').closest('.mce-window').find('.mce-primary').click();</script>";
            }
            catch (Exception ex)
            {
                return "<script>alert('Failed: " + ex.Message + "');</script>";
            }
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

        private Article GetByTitle(string title)
        {
            string sql = "SELECT TOP 1 * FROM [dbo].[Articles] WHERE [Title] = @Title";
            using (var conn = new SqlConnection(connString))
            {
                conn.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@Title", title, DbType.String);
                var article = conn.Query<Article>(sql, dynamicParameters).FirstOrDefault();
                return article;
            }
        }
    }
}