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
    public class DataSellers
    {

        public static ReturnMessage GetList(CustomerSession session, int PageIndex = 0, int PageSize = 10, SellersFilter? Filter = null, List<string>? datatypes = null)
        {
            ReturnMessage returnMessage = new ReturnMessage();

            try
            {
                using (TaskDbContext db = new TaskDbContext())
                {

                    if (Filter == null) Filter = new SellersFilter();
                    if (datatypes == null) datatypes = new List<string>();

                    // IQueryable<Sellers> sellers = db.Sellers.AsNoTracking();
                    IQueryable<Sellers> sellers = db.Sellers;

                    if (!string.IsNullOrEmpty(Filter.Name))
                    {
                        sellers = sellers.Where(m => m.Name == Filter.Name);
                    }
                    if (!string.IsNullOrEmpty(Filter.Location))
                    {
                        sellers = sellers.Where(m => m.Location == Filter.Location);
                    }

                    if (Filter.IsBook != null)
                    {
                        if (Filter.IsBook == true)
                        {
                            sellers = sellers.Where(m => m.Books.Any());
                        }
                        else
                        {
                            sellers = sellers.Where(m => !m.Books.Any());
                        }
                    }

                    int actualIndex = (PageIndex * PageSize);
                    int Count = sellers.Count();
                    sellers = ((actualIndex <= (Count + PageSize)) && actualIndex < Count) ? sellers.Skip(actualIndex).Take(PageSize) : new List<Sellers>().AsQueryable();
                    List<SellersJ> Listsellers = new List<SellersJ>();
                    foreach (Sellers seller in sellers)
                    {
                        Listsellers.Add(convert(seller, datatypes));
                    }

                    returnMessage.Obj = Listsellers;
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
        public static SellersJ convert(Sellers sellers, List<string> datatypes = null)
        {
            try
            {
                if (datatypes == null) new List<string>();

                SellersJ sellersJ = new SellersJ()
                {
                    Id = sellers.SellerId,
                    Name = sellers.Name,
                    Location = sellers.Location
                };
                if (datatypes.Contains("book"))
                {
                    sellersJ.BooksJ = new List<BooksJ>();
                    if (sellers.Books !=null)
                    {
                        foreach (Books books in sellers.Books)
                        {
                            sellersJ.BooksJ.Add(DataBooks.convert(books));
                        }
                    }
                }
                return sellersJ;
            }
            catch (Exception ex)
            {

            }
            return null;
        }
    }



    public class SellersFilter
    {
        public string Name { get; set; } = "";
        public string Location { get; set; } = "";
        public bool? IsBook { get; set; }
    }

}