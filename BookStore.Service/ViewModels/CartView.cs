using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Service.ViewModels
{
    public class CartView
    {
        public CartView()
        {
            Books = new List<BookView>();

        }
        public Guid OrderNumber { get; set; }
        public IList<BookView> Books { get; set; }
        public decimal Total { get; set; }

    }
}
