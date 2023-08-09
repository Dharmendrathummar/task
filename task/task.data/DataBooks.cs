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

namespace task.data
{
    public class DataBooks
    {

        public static ReturnMessage GetList(CustomerSession session, int PageIndex = 0, int PageSize = 10, BooksFilter Filter = null)
        {
            ReturnMessage returnMessage = new ReturnMessage();

            try
            {

                if (Filter == null) Filter = new BooksFilter();
                using (TaskDbContext db = new TaskDbContext())
                {
                    IQueryable<Books> books = db.Books.AsNoTracking();


                    if (!string.IsNullOrEmpty(Filter.Name))
                    {
                        books = books.Where(m => m.Title == Filter.Name);
                    }
                    if (!string.IsNullOrEmpty(Filter.Author))
                    {
                        books = books.Where(m => m.Author == Filter.Author);
                    }
                    if (Filter.Price != null)
                    {
                        books = books.Where(m => m.Price == Filter.Price);
                    }
                    if (Filter.SellerId !=null)
                    {
                        books = books.Where(m => m.SellerId == Filter.SellerId);
                    }
                    else
                    {
                        if (Filter.IsOffer != null)
                        {
                            if (Filter.IsOffer == true)
                            {
                                books = books.Where(m => m.SellerId == null);
                            }
                            else
                            {
                                books = books.Where(m => m.SellerId != null);
                            }
                        }
                    }

                   

                    int actualIndex = (PageIndex * PageSize);
                    int Count = books.Count();
                    books = ((actualIndex <= (Count + PageSize)) && actualIndex < Count) ? books.Skip(actualIndex).Take(PageSize) : new List<Books>().AsQueryable();
                    List<BooksJ> Listbooks = new List<BooksJ>(); 
                    foreach(Books book in books)
                    {
                        Listbooks.Add(convert(book));
                    }

                    returnMessage.Obj = Listbooks;
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
        public static BooksJ convert(Books book)
        {
            return new BooksJ()
            {
                Id = book.BookId,
                Title = book.Title,
                Author = book.Author,
                Price = book.Price,
                SellerId = book.SellerId,
            };
        }
    }

    

    public class BooksFilter
    {
        public string Name { get; set; } = "";
        public string Author { get; set; } = "";
        public decimal? Price { get; set; }
        public int? SellerId { get; set; }
        public bool? IsOffer { get; set; }
    }
}
