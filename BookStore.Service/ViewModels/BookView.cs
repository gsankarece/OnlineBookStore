using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Service.ViewModels
{
    public class BookView
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public decimal Price { get; set; }
        public int Quanity { get; set; }
    }
}
