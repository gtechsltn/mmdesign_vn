using Dapper;
using Mmdesign.Models;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Http;

namespace Mmdesign.Controllers
{
    public class ValuesController : ApiController
    {
        private static readonly string connString;

        static ValuesController()
        {
            connString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }

        // GET api/<controller>
        public IHttpActionResult Get()
        {
            string sql = "SELECT TOP 10 * FROM [dbo].[Test]";
            using (var conn = new SqlConnection(connString))
            {
                conn.Open();
                var lst = conn.Query<ArticleDto>(sql).ToList();
                return Ok(lst);
            }
        }

        // GET api/<controller>/5
        public IHttpActionResult Get(int id)
        {
            return Ok();
        }

        // POST api/<controller>
        public IHttpActionResult Post([FromBody]string value)
        {
            return Ok();
        }

        // PUT api/<controller>/5
        public IHttpActionResult Put(int id, [FromBody]string value)
        {
            return Ok();
        }

        // DELETE api/<controller>/5
        public IHttpActionResult Delete(int id)
        {
            return Ok();
        }
    }
}