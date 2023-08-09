using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using task.data.Essentials;
using task.data;
using task.data.Global.Helpers;

namespace task.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class loginController : ControllerBase
    {

        [HttpPost]
        public ReturnMessage access([FromBody] LoginModal modal)
        {
            ReturnMessage Json = new ReturnMessage();
            try
            {
                if (modal.Login == null) { Json.Message = "Please Provide Details"; return Json; }
                return DataLogin.Check(modal.Login);
            }
            catch (Exception ex)
            {
                Json.Message = ex.Message;
                return Json;
            }
        }

        public class LoginModal
        {

            public CustomerJ Login { get; set; }
        }
    }
}
