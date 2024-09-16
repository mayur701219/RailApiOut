using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rail.BO.MiscModels
{
    public class UserModel
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? Correlation { get; set; }
        public UserModel(string? userName, string? password, string? correlation)
        {
            UserName = userName;
            Password = password;
            Correlation = correlation;
        }
    }
}
