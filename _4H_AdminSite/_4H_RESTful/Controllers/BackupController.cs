using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace _4H_RESTful.Controllers
{
    [Produces("application/json")]
    [Route("api/backup")]
    public class BackupController : Controller
    {
        /// <summary>
        /// Get method for pulling backup information off of the server
        /// </summary>
        /// <param name="auth0_id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public HttpResponse Get(string id)
        {
            RecordBundle bundle = DBConnection.GetInstance().Obtain(id);
            if (bundle.identity.Equals("didnotfind")) { 
                Response.StatusCode = 404;
            } else {
                Response.StatusCode = 200;
            }
            Response.ContentType = "application/json";
            Response.WriteAsync(JsonConvert.SerializeObject(bundle));
            return Response;
        }

        /// <summary>
        /// Creates a new entry or updates an existing one
        /// </summary>
        [HttpPost]
        public HttpResponse Post([FromBody] RecordBundle content)
        {
            DBConnection.GetInstance().Insert(content);
            Response.StatusCode = 201;
            return Response;
        }

        /// <summary>
        /// Deletes the provided ID from the database
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public HttpResponse Delete(string id)
        {
            DBConnection.GetInstance().Remove(id);
            Response.StatusCode = 204;
            return Response;
        }
    }
}
