using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_Data_Access_Layer_.CustomeModel
{
    public class emailCustome
    {
       
            public string Server { get; set; }
            public int Port { get; set; }
            public string SenderName { get; set; }
            public string SenderEmail { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
            public bool EnableSSL { get; set; }
       
  

    }
}
