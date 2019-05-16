using BookStore.Common;
using BookStore.Common.Repository;
using BookStore.DataStore.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BookStore.DataStore.Repository
{
    public class UserRepository : IUserRepository
    {
        private BookStoreContext ctx = null;

        public UserRepository(BookStoreContext context)
        {
            ctx = context;
        }

        public void CommitChanges()
        {
            ctx.SaveChanges();
        }
        public User Add(User user)
        {
            ctx.Add(user);
            CommitChanges();
            return user;
        }

        public User Update(User user)
        {
            var result = Find(user);
            if (result == null)
                throw new Exception();

            result.Email = user.Email;
            result.Password = user.Password;
            ctx.Users.Update(result);
            CommitChanges();

            return result;
        }

        public User Find(User user)
        {
            return ctx.Users.Where(u => u.Email == user.Email && u.Password == user.Password).FirstOrDefault();
        }

        public User Find(string email)
        {
            return ctx.Users.Where(u => u.Email == email ).FirstOrDefault();
        }

        public bool Delete(User user)
        {
            ctx.Users.Remove(user);
            CommitChanges();
            return true;
        }

        public IList<User> GetUsers()
        {
            return ctx.Users.AsNoTracking().ToList();
        }

    }
}
