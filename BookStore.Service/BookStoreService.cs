using BookStore.Common;
using BookStore.Common.Repository;
using BookStore.Service.Contract;
using BookStore.Service.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Service
{
    public class BookStoreService : IBookStoreService
    {
        private IBookRepository bookRepo = null;
        private IOrderRepository orderRepo = null;
        private IUserRepository userRepo = null;

        public BookStoreService(IBookRepository repo, IUserRepository userRepo, IOrderRepository orderRepo)
        {
            bookRepo = repo;
            this.orderRepo = orderRepo;
            this.userRepo = userRepo;

        }

        #region BOOK
        private IList<Book> GetAll()
        {
            return bookRepo.GetBooks();
        }

        public Task<IEnumerable<BookView>> GetBooksAsync(string searchString)
        {
            if (string.IsNullOrEmpty(searchString))
                throw new Exception("Please specify data to search for.");
            var bookView = GetAll().Where(b => b.Title.Contains(searchString) || b.Author.Contains(searchString))
                            .ToList().ToBookView();
            return Task.FromResult<IEnumerable<BookView>>(bookView);
        }

        #endregion

        #region USER

        public UserView Register(User user)
        {
            if (user == null || user.Email == null || user.Password == null || user.UserName == null)
                throw new Exception("Please verify all required fields");
            if (userRepo.Find(user) != null)
                return null;

            if (user.IsValid())
            {
                var result = userRepo.Add(user);
                return result.ToUserView();
            }
            return null;
        }

        public UserView Login(string emailId, string password)
        {
            var user = userRepo.Find(new User { Email = emailId, Password = password });
            if (user == null)
                throw new Exception("Invalid username or password.");
            if (user != null)
            {
                user.Password = string.Empty;
                user.ShoppingCart = new ShoppingCart();
                var userView = user.ToUserView();
                userView.Token = GenerateToken(user);
                return userView;
            }
            return null;
        }

        public bool VerifyAccessToken(string email, string token)
        {
            bool result = false;
            var user = userRepo.Find(email);
            if (user == null)
                return false;
            var generatedToken = GenerateToken(user);
            if (generatedToken == token)
                result = true;
            return result;
        }

        private string GenerateToken(User user)
        {
            var inputPass = user.Email + user.UserName;
            byte[] plainPassword = UTF8Encoding.ASCII.GetBytes(inputPass);
            var sha = SHA256.Create();
            byte[] cryptPassword = sha.ComputeHash(plainPassword);
            return Convert.ToBase64String(cryptPassword);
        }

        #endregion

        #region ORDERS
        public Task<string> CheckAvailableStock(UserView user)
        {
            ShoppingCart cart = new ShoppingCart();
            StringBuilder sb = new StringBuilder();

            // Check for stock, If stock is available then reduce from stock
            foreach (var book in user.ShoppingCart.Books)
            {
                // Get vailable stock for book
                int stock = orderRepo.AvailableStock(book.ToBook());
                sb.AppendLine($"{book.Title}|{book.Author}|{stock}").Append(";");
            }

            return Task.FromResult(sb.ToString().TrimEnd(';'));
        }
        public Task<CartView> NewOrder(UserView user)
        {
            ShoppingCart cart = new ShoppingCart();
            StringBuilder sb = new StringBuilder();

            // Check for stock, If stock is available then reduce from stock
            foreach (var book in user.ShoppingCart.Books)
            {
                // Get vailable stock for book
                int stock = orderRepo.AvailableStock(book.ToBook());
                if (stock == 0 || stock < 0)
                    return Task.FromException<CartView>(new Exception("Stock not available"));
                // Reduce ordered quanity from stock
                var calculatedStock = stock - book.Quanity;
                
                var obj = book.ToBook();
                obj.InStock = calculatedStock;
                var updateResult = bookRepo.UpdateStock(obj);
                var order = orderRepo.NewOrder(user.ToUser());
                user.ShoppingCart.OrderNumber = order.Id;
                cart.Id = order.Id;
                //cart.Books.Add(order.Books.First());
                cart.Books = order.Books;
            }

            return Task.FromResult<CartView>(cart.ToCartView());
        }

        public Task<CartView> UpdateOrder(UserView user)
        {
            var placedOrder = orderRepo.Find(user.ShoppingCart.OrderNumber);
            foreach (var book in user.ShoppingCart.Books)
            {
                var orderedBook = placedOrder.Books.Where(b => b.Title == book.Title && b.Author == book.Author).FirstOrDefault();
                var difference = book.Quanity - orderedBook.InStock;
                if (difference < 0)
                {
                    // Return Back Stock
                    // Get vailable stock for book
                    int stock = orderRepo.AvailableStock(book.ToBook());
                    if (stock == 0 || stock < 0)
                        return Task.FromException<CartView>(new Exception("Stock not available"));
                    var calculatedStock = stock + difference;
                    var obj = book.ToBook();
                    obj.InStock = calculatedStock;
                    var updateResult = bookRepo.UpdateStock(obj);
                }
                else
                {
                    // Get vailable stock for book
                    int stock = orderRepo.AvailableStock(book.ToBook());
                    if (stock == 0 || stock < 0)
                        return Task.FromException<CartView>(new Exception("Stock not available"));
                    // Reduce ordered quanity from stock
                    var calculatedStock = stock - difference;
                    var obj = book.ToBook();
                    obj.InStock = calculatedStock;
                    var updateResult = bookRepo.UpdateStock(obj);
                }

            }
            var result = orderRepo.UpdateOrder(user.ToUser());
            return Task.FromResult<CartView>(result.ToCartView());
        }

        public Task<bool> CancelOrder(UserView user)
        {
            var result = orderRepo.CancelOrder(user.ToUser());
            return Task.FromResult<bool>(result);
        }

        public Task<IEnumerable<ShoppingCart>> AllOrders(User user)
        {
            return Task.FromResult(orderRepo.OrderHistory(user.Email));
        }
        #endregion
    }

    public static class ServiceHelpers
    {
        public static Book ToBook(this BookView bookVm)
        {
            return new Book
            {
                Author = bookVm.Author,
                Price = bookVm.Price,
                Title = bookVm.Title,
                InStock = bookVm.Quanity
            };
        }

        public static IEnumerable<BookView> ToBookView(this IList<Book> books)
        {
            foreach (var book in books)
            {
                yield return new BookView { Author = book.Author, Price = book.Price, Title = book.Title };
            }
        }
        public static UserView ToUserView(this User user)
        {
            return new UserView
            {
                Email = user.Email,
                UserName = user.Email,
                ShoppingCart = user.ShoppingCart.ToCartView(),
                Token = ""
            };
        }

        public static User ToUser(this UserView user)
        {
            return new User
            {
                Email = user.Email,
                UserName = user.UserName,
                ShoppingCart = user.ShoppingCart.ToShoppingCart()

            };
        }

        public static ShoppingCart ToShoppingCart(this CartView cart)
        {
            ShoppingCart shoppingCart = new ShoppingCart();
            foreach (var cartItem in cart.Books)
            {
                shoppingCart.Books.Add(cartItem.ToBook());
            }
            return shoppingCart;
        }

        public static BookView ToBookView(this Book book)
        {
            return new BookView
            {
                Author = book.Author,
                Price = book.Price,
                Title = book.Title,
                Quanity = book.InStock
            };
        }

        public static CartView ToCartView(this ShoppingCart cart)
        {
            CartView shoppingCart = new CartView();
            foreach (var cartItem in cart.Books)
            {
                shoppingCart.Books.Add(cartItem.ToBookView());
            }
            shoppingCart.OrderNumber = cart.Id;
            return shoppingCart;
        }
    }




    public static class UserValidators
    {
        public static bool IsValid(this User user)
        {
            if (user == null || user.UserName == null || user.UserName == string.Empty || user.Password == null ||
                user.Password == string.Empty || user.Email == null || user.Email == string.Empty)
                return false;

            if (user.UserName.Length < 6 || user.Password.Length < 8)
                return false;



            return true;
        }
    }
}
