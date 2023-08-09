using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using task.data.Global.Helpers;
using task.data.Essentials;
using task.data.Model;
using task.data.Access;

namespace task.data.Access
{
    public class DataLogin
    {
        public static ReturnMessage Login(CustomerJ customerJ)
        {
            ReturnMessage Obj = new ReturnMessage();
            try
            {
                var IsValid = true;
                if (customerJ == null) { Obj.Message = "Please Provide Details";  return Obj; }
                if (string.IsNullOrEmpty(customerJ.Password)) { Obj.Message = "Please Provide Password Code"; return Obj; }
                if (string.IsNullOrEmpty(customerJ.UserName)) { Obj.Message = "Please Provide UserName"; return Obj; }

                if (IsValid)
                {
                    using (TaskDbContext Db = new TaskDbContext())
                    {

                        List<Customers> Customers = Db.Customers.ToList();
                        if (Db.Customers.Any(m => m.Password == customerJ.Password))
                        {
                            Customers? customers = Db.Customers.FirstOrDefault(M => M.Password == customerJ.Password);
                            if (customers != null)
                            {
                                CustomerJ customerSession = new CustomerJ()
                                {
                                    UserName = customers.Username,
                                    Id = customers.CustomerId,
                                    CD = DataDateTime.Now
                                };
                                AuthTokenJ access = DataToken.GetToken(customerSession);
                                if (access.IsDone)
                                {
                                    Obj.Obj = new
                                    {
                                        customerSession,
                                        Access = access
                                    };

                                    Obj.Success = true;
                                }
                                else
                                {
                                    Obj.Message = "User Access Error !";
                                }
                            }
                        }
                        else
                        {
                            Obj.Message = "User Not Found";
                        }
                    }
                }
                else
                {
                    Obj.Message = "Please Provide Details";
                }
            }
            catch (Exception ex)
            {

            }
            return Obj;
        }
    }
}
