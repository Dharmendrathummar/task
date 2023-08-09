using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using task.data.Global.Helpers;

namespace task.data.Essentials
{
    [Serializable]
    public class CustomerSession
    {
        public string CustomerId { get; set; } = "";
        public bool IsValid { get; set; }
        public DateTime CCD { get; set; } // Customer Login DateTime

        public string ApiUrl { get; set; }
        public string UserName { get; set; }

        public DateTime TokenExpiry { get; set; }

        public List<exParameter> Claims { get; set; }
    }

}
