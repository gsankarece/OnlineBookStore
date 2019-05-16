using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Common.Repository
{
    public interface IOrderRepository
    {
        IEnumerable<ShoppingCart> OrderHistory(string email);
        ShoppingCart NewOrder(User user);
        ShoppingCart UpdateOrder(User user);
        bool CancelOrder(User user);

        int AvailableStock(Book book);
        ShoppingCart Find(Guid orderId);


    }
}
