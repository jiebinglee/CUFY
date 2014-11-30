using ChinaUnicom.Fuyang.Core.Users.Models;
using ChinaUnicom.Fuyang.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChinaUnicom.Fuyang.Core.Users.Services
{
    public class UserService : IUserService
    {
        readonly IRepository<User> _userRepository;

        public UserService(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public User GetUser(string userName, string password)
        {
            return _userRepository.Get(t => t.UserName == userName 
                && t.Password == password
                && t.Flag == 1);
        }

        public User Insert(User user)
        {
            return _userRepository.Create(user);
        }

        public User GetUser(int UserId)
        {
            return _userRepository.Get(t => t.Id == UserId && t.Flag == 1);
        }

        public int UpdateUser(User user)
        {
            return _userRepository.Update(user);
        }
    }
}
