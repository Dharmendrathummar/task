using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using task.data.Essentials;

namespace task.data.Global.Helpers
{
    public class CustomerJ
    {
        public int? Id { get; set; }
        public string UserName { get; set; } = "";
        public string Password { get; set; } = "";
        public DateTime? CD { get; set; } // Login Date

    }
    public class BooksJ
    {
        public int? Id { get; set; }
        public string Title { get; set; } = "";
        public string Author { get; set; } = "";
        public Decimal? Price { get; set; }
        public int? SellerId { get; set; }

    }

    public class SellersJ
    {
        public int? Id { get; set; }
        public string Name { get; set; } = "";
        public string Location { get; set; } = "";
        public List<BooksJ>? BooksJ { get; set; }
    }

    public class BidsJ
    {
        public int? Id { get; set; }
        public decimal Price { get; set; }
        public BooksJ? BooksJ { get; set; }
        public SellersJ? SellersJ { get; set; }
    }

    public class AuthTokenJ
    {
        public AuthTokenJ()
        {
            Time = GlobalVariable.TokenExpiry;
        }
        public string Token { get; set; } = "";
        public string RefreshToken { get; set; } = "";
        public int Time { get; set; }
        public bool IsDone { get; set; }
    }

    public class exParameter
    {
        public string Name { get; set; }
        public object Value { get; set; }


        public exParameter(string Name = "", object Value = null)
        {
            this.Name = Name;
            if (Value != null)
                this.Value = Value.ToString();
        }
    }

}
