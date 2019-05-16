using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Common
{
    public class ShoppingCart
    {
        public ShoppingCart()
        {
            Books = new List<Book>();
        }
        public Guid Id { get; set; }
        public IList<Book> Books { get; set; }
        public decimal Total { get; set; }
        public string Email { get; set; }
        public DateTime OrderDate { get; set; }

    }
}
