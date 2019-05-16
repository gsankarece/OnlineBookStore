using BookStore.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Common.Repository
{
    public interface IBookRepository
    {
        IList<Book> GetBooks();
        Book Add(Book book);
        Book UpdateStock(Book book);
    }
}
