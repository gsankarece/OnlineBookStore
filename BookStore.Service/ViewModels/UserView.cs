using BookStore.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Service.ViewModels
{
    public class UserView
    {
        
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public CartView ShoppingCart { get; set; }
    }
}
