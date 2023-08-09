using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task.data.Model
{
    public partial class Books
    {
        public int BookId { get; set; }
        public string Title { get; set; } = null!;
        public int? SellerId { get; set; }
        public string Author { get; set; } = null!;
        public decimal Price { get; set; }
        public virtual Sellers? Sellers { get; set; }
        public virtual ICollection<Bids> Bids { get; } = new List<Bids>();
    }
}
