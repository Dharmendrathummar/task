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
    public class booksController : ControllerBase
    {
        private CustomerSession session;
        public booksController(CustomerSession _session) { session = _session;  }


        [HttpPost]
        public ReturnMessage getlist([FromBody] BooksModal modal)
        {
            ReturnMessage Json = new ReturnMessage();
            try
            {
                return DataBooks.GetList(session,modal.PageIndex, modal.PageSize,modal.Filter);
            }
            catch (Exception ex)
            {
                Json.Message = ex.Message;
                return Json;
            }
        }

        public class BooksModal
        {
            public int PageIndex { get; set; }
            public int PageSize { get; set; } = 10;
            public BooksFilter? Filter { get; set; }
        }
    }
}
