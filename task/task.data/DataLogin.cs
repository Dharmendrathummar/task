using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using task.data.Essentials;
using task.data.Global.Helpers;

namespace task.data
{
    public class DataLogin
    {
        public static ReturnMessage Check(CustomerJ customerJ)
        {
            try
            {
                return  task.data.Access.DataLogin.Login(customerJ);
            }
            catch (Exception ex)
            {
                return new ReturnMessage() { Obj = ex };
            }
        }

        
    }
}
