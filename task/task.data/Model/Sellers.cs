using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task.data.Model
{
    public partial class Sellers
    {
        public int SellerId { get; set; }
        public string Name { get; set; } = null!;
        public string Location { get; set; } = null!;
        public virtual ICollection<Books> Books { get; } = new List<Books>();
        public virtual ICollection<Bids> Bids { get; } = new List<Bids>();
    }
}
