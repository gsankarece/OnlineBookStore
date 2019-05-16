using BookStore.Common;
using BookStore.Service.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Service.Contract
{
    public interface IBookStoreService
    {
        Task<IEnumerable<BookView>> GetBooksAsync(string searchString);
        UserView Register(User user);
        UserView Login(string emailId, string password);

        Task<string> CheckAvailableStock(UserView user);

        Task<CartView> NewOrder(UserView user);
        Task<CartView> UpdateOrder(UserView user);
        Task<bool> CancelOrder(UserView user);
        Task<IEnumerable<ShoppingCart>> AllOrders(User user);
        bool VerifyAccessToken(string email, string token);
    }
}
