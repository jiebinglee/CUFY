using ChinaUnicom.Fuyang.Core.Users.Models;
using ChinaUnicom.Fuyang.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChinaUnicom.Fuyang.Core.Users.Services
{
    public interface IUserService : IDependency
    {
        User GetUser(string userName, string password);
        User Insert(User user);
        User GetUser(int UserId);
    }
}
