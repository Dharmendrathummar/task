using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using task.data;
using task.data.Essentials;
using task.data.Global.Helpers;

namespace task.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class sellersController : ControllerBase
    {
        private CustomerSession session;
        public sellersController(CustomerSession _session) { session = _session;  }


        [HttpPost]
        public ReturnMessage getlist([FromBody] SellersModal modal)
        {
            ReturnMessage Json = new ReturnMessage();
            try
            {
                return DataSellers.GetList(session,modal.PageIndex, modal.PageSize,modal.Filter, modal.datatypes);
            }
            catch (Exception ex)
            {
                Json.Message = ex.Message;
                return Json;
            }
        }

        public class SellersModal
        {
            public int PageIndex { get; set; }
            public int PageSize { get; set; } = 10;
            public List<string>? datatypes { get; set; }
            public SellersFilter? Filter { get; set; }
        }
    }
}
