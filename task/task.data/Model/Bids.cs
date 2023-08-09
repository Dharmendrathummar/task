using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task.data.Model
{
    public partial class Bids
    {
        public int BidId { get; set; }
        public int BookId { get; set; }
        public int SellerId { get; set; }
        public decimal Price { get; set; }
        public bool? IsBuy { get; set; }
        public virtual Sellers Sellers { get; set; } = null!;
        public virtual Books Books { get; set; } = null!;  

    }
}
