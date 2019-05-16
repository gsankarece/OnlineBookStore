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
    public class OrderRepository : IOrderRepository
    {
        private BookStoreContext ctx = null;

        public OrderRepository(BookStoreContext context)
        {
            ctx = context;
        }

        public bool CancelOrder(User user)
        {
            throw new NotImplementedException();
        }

        public ShoppingCart NewOrder(User user)
        {
            user.ShoppingCart.OrderDate = DateTime.Now;
            ctx.ShoppingCarts.Add(user.ShoppingCart);
            ctx.SaveChanges();
            return user.ShoppingCart;
        }

        public ShoppingCart Find(Guid orderId)
        {
           return ctx.ShoppingCarts.AsNoTracking().Where(o => o.Id == orderId).FirstOrDefault();
        }

        public IEnumerable<ShoppingCart> OrderHistory(string email)
        {
           return ctx.ShoppingCarts.AsNoTracking().AsEnumerable<ShoppingCart>();
           // throw new NotImplementedException();
            //var result = from cart in ctx.ShoppingCarts
            //             join user in ctx.Users on cart.User.Email equals user.Email
            //             select cart;
            //return result.AsEnumerable<ShoppingCart>();
        }

        public ShoppingCart UpdateOrder(User user)
        {
            throw new NotImplementedException();
        }

        public int AvailableStock(Book book)
        {
            var result = ctx.Books.Where(b => b.Title == book.Title && b.Author == book.Author).FirstOrDefault();
            return result == null ? 0 : result.InStock;
        }

        public void DecreaseStock(ShoppingCart cart)
        {
            foreach (var item in cart.Books)
            {
                var book = ctx.Books.Where(b => b.Title == item.Title).FirstOrDefault();
                
            }
        }
    }
}
