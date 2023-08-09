using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task.data.Model
{
    public class Customers
    {
        public int CustomerId { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
