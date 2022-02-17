using OnlineChat.Models.Chats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineChat.Models.Users
{
    public interface IUserRepository
    {
        public IEnumerable<User> GetAllUsers();

        public User GetUserById(int id);

        public User GetUserByPassAndEmail(string pass, string email);

        public bool AddUser(User user);

        public bool ChangeStatus(string newStatus, int id);

        public bool DeleteUser(int id);
        public User GetUserOnEmail(string Email);
    }
}
