using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Common.Repository
{
    public interface IUserRepository
    {
        User Add(User user);
        User Update(User user);
        User Find(User user);
        User Find(string email);
        bool Delete(User user);
        IList<User> GetUsers();
    }
}
