using BookStore.DataStore.Context;
using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using System.Diagnostics;
using Newtonsoft.Json;
using System.IO;
using BookStore.Common;
using System.Linq;
using BookStore.Common.Repository;
using Microsoft.EntityFrameworkCore;

namespace BookStore.DataStore.Repository
{
    public class BookRepository : IBookRepository
    {
        private BookStoreContext ctx = null;
        public BookRepository(BookStoreContext context)
        {
            ctx = context;
            ctx.Database.EnsureCreated();           
        }

      

        public IList<Book> GetBooks()
        {
            return ctx.Books.AsNoTracking().ToList();
        }

        public Book Add(Book book)
        {
            ctx.Books.Add(book);
            CommitChanges();
            return book;
        }

        private void CommitChanges()
        {
            ctx.SaveChanges();
        }
        public Book UpdateStock(Book book)
        {
            var result = ctx.Books.Where(b => b.Title.ToLower() == book.Title.ToLower() && b.Author.ToLower() == book.Author.ToLower()).FirstOrDefault();
            if (result == null)
                throw new Exception("");
            result.InStock = book.InStock;
            CommitChanges();

            return result;
        }

        
    }
}
