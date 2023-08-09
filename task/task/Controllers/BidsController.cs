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
    public class bidsController : ControllerBase
    {
        private CustomerSession session;
        public bidsController(CustomerSession _session) { session = _session; }


        [HttpPost]
        public ReturnMessage getlist([FromBody] BidsModal modal)
        {
            ReturnMessage Json = new ReturnMessage();
            try
            {
                return DataBids.GetList(session, modal.PageIndex, modal.PageSize, modal.Filter,modal.datatypes);
            }
            catch (Exception ex)
            {
                Json.Message = ex.Message;
                return Json;
            }
        }

        [HttpPost]
        public ReturnMessage approve([FromBody] BidsModal modal)
        {
            ReturnMessage Json = new ReturnMessage();
            try
            {
                if (!(modal.BidId != null && modal.BidId > 0)) { Json.Message = "Please Provide BidId"; return Json; };
                if (!(modal.BookId != null && modal.BookId > 0)) { Json.Message = "Please Provide BookId"; return Json; };
                return DataBids.Approve(session, modal.BidId, modal.BookId);
            }
            catch (Exception ex)
            {
                Json.Message = ex.Message;
                return Json;
            }
        }

        [HttpPost]
        public ReturnMessage bid([FromBody] BidsModal modal)
        {
            ReturnMessage Json = new ReturnMessage();
            try
            {
                if (!(modal.SellerId != null && modal.SellerId > 0)) { Json.Message = "Please Provide BidId"; return Json; };
                if (!(modal.BookId != null && modal.BookId > 0)) { Json.Message = "Please Provide BookId"; return Json; };
                return DataBids.Bids(session, modal.BookId, modal.SellerId, modal.Price);
            }
            catch (Exception ex)
            {
                Json.Message = ex.Message;
                return Json;
            }
        }

        public class BidsModal
        {
            public int? BidId { get; set; }
            public int? SellerId { get; set; }
            public int? BookId { get; set; }
            public decimal? Price { get; set; }
            public int PageIndex { get; set; }
            public int PageSize { get; set; } = 10;
            public List<string>? datatypes { get; set; }
            public BidsFilter? Filter { get; set; }
        }
    }
}
