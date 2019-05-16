using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Common
{
    public class User
    {
        
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public virtual ShoppingCart ShoppingCart { get; set; }
    }
}
