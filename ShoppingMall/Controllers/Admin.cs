using System.Web.Http;

namespace ShoppingMall.Controllers
{
    [RoutePrefix("api/admin")]
    public class AdminController : ApiController
    {
        [Route("checkLoginByToken")]
        [HttpGet]
        public IHttpActionResult test()
        {

            return Ok(1);
        }
    }
}