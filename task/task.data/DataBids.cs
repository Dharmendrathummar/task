using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using task.data.Essentials;
using task.data.Global.Helpers;
using task.data.Model;
using static System.Reflection.Metadata.BlobBuilder;

namespace task.data
{
    public class DataBids
    {

        public static ReturnMessage GetList(CustomerSession session, int PageIndex = 0, int PageSize = 10, BidsFilter? Filter = null, List<string>? datatypes = null)
        {
            ReturnMessage returnMessage = new ReturnMessage();

            try
            {
                using (TaskDbContext db = new TaskDbContext())
                {

                    if (Filter == null) Filter = new BidsFilter();
                    if (datatypes == null) datatypes = new List<string>();

                    // IQueryable<Sellers> sellers = db.Sellers.AsNoTracking();
                    IQueryable<Bids> bids = db.Bids.Where(m => m.IsBuy == null).AsQueryable();
                    //bids = bids.Where(m => m.);
                    if (Filter.SellerId != null)
                    {
                        bids = bids.Where(m => m.SellerId == Filter.SellerId);
                    }

                    if (Filter.BookId != null)
                    {
                        bids = bids.Where(m => m.BookId == Filter.BookId);
                    }

                    if (Filter.price != null)
                    {
                        bids = bids.Where(m => m.Price == Filter.price);
                    }

                    int actualIndex = (PageIndex * PageSize);
                    int Count = bids.Count();
                    bids = ((actualIndex <= (Count + PageSize)) && actualIndex < Count) ? bids.Skip(actualIndex).Take(PageSize) : new List<Bids>().AsQueryable();
                    List<BidsJ> ListBids = new List<BidsJ>();
                    foreach (Bids bid in bids)
                    {
                        //bid.
                        ListBids.Add(convert(bid, datatypes));
                    }

                    returnMessage.Obj = ListBids;
                    returnMessage.Success = true;
                }
            }

            catch (Exception ex)
            {
                returnMessage.Message = ex.Message;
                returnMessage.Success = false;
            }
            return returnMessage;
        }
        public static BidsJ convert(Bids bid, List<string> datatypes)
        {
            try
            {
                BidsJ bidsJ = new BidsJ()
                {
                    Id = bid.BidId,
                    Price = bid.Price
                };
                if (datatypes.Contains("book"))
                {
                    if (bid.Books != null)
                    {
                        bidsJ.BooksJ = DataBooks.convert(bid.Books);
                    }
                }
                if (datatypes.Contains("seller"))
                {
                    if (bid.Sellers != null)
                    {
                        bidsJ.SellersJ = DataSellers.convert(bid.Sellers, null);
                    }
                }
                return bidsJ;
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        public static ReturnMessage Bids(CustomerSession session, int? BookId, int? SellerId, decimal? Price)
        {
            ReturnMessage returnMessage = new ReturnMessage();
            try
            {
                if (SellerId != null && SellerId > 0 && BookId != null && BookId > 0)
                {
                    if (Price != null)
                    {
                        using (TaskDbContext db = new TaskDbContext())
                        {
                            if (db.Sellers.Any(m => m.SellerId == SellerId))
                            {
                                if (db.Books.Any(m => m.BookId == BookId && m.SellerId == null))
                                {
                                    if (!db.Bids.Any(m => m.SellerId == SellerId && m.BookId == BookId && m.IsBuy == null))
                                    {
                                        Bids bids = new Bids()
                                        {
                                            Price = (decimal)Price,
                                            SellerId = (int)SellerId,
                                            BookId = (int)BookId
                                        };
                                        db.Bids.Add(bids);
                                        db.SaveChanges();
                                        returnMessage.Message = "Done";
                                        returnMessage.Success = true;
                                    }
                                    else
                                    {
                                        returnMessage.Message = "Already Bid Found";
                                    }
                                }
                                else
                                {
                                    returnMessage.Message = "Book not found";
                                }

                            }
                            else
                            {
                                returnMessage.Message = "Seller not found";
                            }
                        }
                    }
                    else
                    {
                        returnMessage.Message = "Please Provide Valid Price";
                    }
                }
                else
                {
                    returnMessage.Message = "Please Provide Valid Data";
                }
            }
            catch (Exception ex)
            {
                returnMessage.Message = ex.Message;
            }
            return returnMessage;
        }
        public static ReturnMessage Approve(CustomerSession session, int? BidId, int? BookId)
        {
            ReturnMessage returnMessage = new ReturnMessage();
            try
            {
                if (BidId != null && BidId > 0 && BookId != null && BookId > 0)
                {
                    using (TaskDbContext db = new TaskDbContext())
                    {
                        if (db.Bids.Any(m => m.BidId == BidId && m.IsBuy == null))
                        {
                            IEnumerable<Bids> bids = db.Bids.Where(m => m.BookId == BookId && m.IsBuy == null);
                            if (bids != null)
                            {
                                foreach (Bids bid in bids)
                                {
                                    if (bid.BidId == BidId)
                                    {
                                        Books books = db.Books.FirstOrDefault(m => m.BookId == BookId);
                                        books.SellerId = bid.SellerId;
                                        bid.IsBuy = true;
                                    }
                                    else
                                    {
                                        bid.IsBuy = false;
                                    }
                                }
                            }
                            db.SaveChanges();
                            returnMessage.Message = "Done";
                            returnMessage.Success = true;
                        }
                        else
                        {
                            returnMessage.Message = "Please Provide BidId";
                        }
                    }

                }
                else
                {
                    returnMessage.Message = "Please Provide BidId";
                }
            }
            catch (Exception ex)
            {
                returnMessage.Message = ex.Message;
            }
            return returnMessage;
        }
    }



    public class BidsFilter
    {
        public int? SellerId { get; set; }
        public int? BookId { get; set; }
        public decimal? price { get; set; }
    }

}