using DbUp;
using log4net;
using Mmdesign.Controllers;
using Mmdesign.Helpers;
using Mmdesign.Models;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace Mmdesign
{
    public class MvcApplication : HttpApplication
    {
        private readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        protected void Application_Start()
        {
            log4net.Config.XmlConfigurator.Configure();

            log.InfoFormat("{0}  started at: {1}", nameof(Application_Start), DateTimeOffset.Now.ToString(AppConstants.DateTimeFormat.YYYYMMdd_HHmmssfff));

            // Init database
            Database.SetInitializer(new MySeedData());

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Autofac and Automapper configurations
            Bootstrapper.Run();

            AddUploadsAndImagesFolders();

            AddAdminUserIfNotExists();

            RunDbUp();

            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            Application["Version"] = string.Format("{0}.{1}", version.Major, version.Minor);

            Assembly assembly = Assembly.GetExecutingAssembly();

            var allControllerActions = assembly.GetTypes()
                .Where(type => typeof(Controller).IsAssignableFrom(type))
                .SelectMany(type => type.GetMethods())
                .Where(method => method.IsPublic && !method.IsDefined(typeof(System.Web.Mvc.NonActionAttribute)))
                .Select(x => new ControllerActionDTO { ControllerName = x.ReflectedType.Name, ActionName = x.Name })
                .ToList();

            Application["AllControllerActions"] = allControllerActions;

            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = e.ExceptionObject as Exception;
            log.Error(ex);
            EmailSender.SendMail(ex);
        }

        private void AddUploadsAndImagesFolders()
        {
            try
            {
                var basePath = System.Web.Hosting.HostingEnvironment.MapPath("~");
                var uploads = Path.Combine(basePath, "Uploads");
                Directory.CreateDirectory(uploads);
                var images = Path.Combine(uploads, "Images");
                Directory.CreateDirectory(images);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                EmailSender.SendMail(ex);
            }
        }

        private void AddAdminUserIfNotExists()
        {
            MembershipCreateStatus status = MembershipCreateStatus.Success;

            CustomMembership customMembership = new CustomMembership();

            try
            {
                string adminUser = ConfigurationManager.AppSettings["AdminUser"];
                string adminPassword = ConfigurationManager.AppSettings["AdminPassword"];
                string adminEmail = ConfigurationManager.AppSettings["AdminEmail"];

                //Encrypt Admin Password
                adminPassword = MembershipExtensions.GetMD5Hash(adminPassword);

                var u = customMembership.GetUser(adminUser, false);

                if (u == null)
                {
                    MembershipUser membershipUser = customMembership.CreateUser(
                                                                                username: adminUser,
                                                                                password: adminPassword,
                                                                                email: adminEmail,
                                                                                passwordQuestion: "Do you have a wife?",
                                                                                passwordAnswer: "Yes, I have",
                                                                                isApproved: true,
                                                                                providerUserKey: null,
                                                                                status: out status);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);

                EmailSender.SendMail(ex);
            }
        }

        private void RunDbUp()
        {
            //DbUp
            try
            {
                var connString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

                EnsureDatabase.For.SqlDatabase(connString);

                var upgrader =
                    DeployChanges.To
                        .SqlDatabase(connString)
                        .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                        .LogToConsole()
                        .Build();

                var result = upgrader.PerformUpgrade();

                if (!result.Successful)
                {
                    EmailSender.SendMail(new Exception("Install DB not successful"));
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);

                EmailSender.SendMail(ex);
            }
        }

        protected void Session_Start(object sender, EventArgs e)
        {
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
        }

        protected void Application_EndRequest(object sender, EventArgs e)
        {
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// AUTHENTICATION + MEMBERSHIP
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);

                var serializeModel = JsonConvert.DeserializeObject<CustomSerializeModel>(authTicket.UserData);

                CustomPrincipal principal = new CustomPrincipal(authTicket.Name);

                principal.UserId = serializeModel.UserId;
                principal.UserName = serializeModel.UserName;
                principal.Email = serializeModel.Email;
                principal.FirstName = serializeModel.FirstName;
                principal.LastName = serializeModel.LastName;
                principal.Roles = serializeModel.RoleName.ToArray();

                HttpContext.Current.User = principal;
            }
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();

            log.Error(ex);

            EmailSender.SendMail(ex);

            Server.ClearError();

            var routeData = new RouteData();

            routeData.Values["controller"] = "Error";

            if ((ex is HttpException))
            {
                var statusCode = (ex as HttpException).GetHttpCode();
                if (statusCode == 403)
                {
                    routeData.Values["action"] = "AccessDenied";
                }
                else if (statusCode == 404)
                {
                    routeData.Values["action"] = "NotFound";
                }
                else
                {
                    routeData.Values["action"] = "Index";
                }
            }
            else
            {
                routeData.Values["action"] = "Index";
            }

            Response.TrySkipIisCustomErrors = true;
            IController errorController = new ErrorController();
            HttpContextWrapper wrapper = new HttpContextWrapper(Context);
            var requestContext = new RequestContext(wrapper, routeData);
            errorController.Execute(requestContext);

            Response.End();
        }

        protected void Session_End(object sender, EventArgs e)
        {
        }

        protected void Application_End(object sender, EventArgs e)
        {
            log.InfoFormat("{0} finished at: {1}", nameof(Application_Start), DateTimeOffset.Now.ToString(AppConstants.DateTimeFormat.YYYYMMdd_HHmmssfff));
        }
    }
}