using BookStore.Common;
using BookStore.DataStore.Context;
using BookStore.DataStore.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace BookStore.Tests
{
    [TestClass]
    public class UnitTest1
    {
        private object key = new object();
        private IList<Book> Books = new List<Book>();
        [TestMethod]
        public void GetBookResponse()
        {
            //Task.Factory.StartNew(() => DownloadBooks());
            //Task.Factory.StartNew(() => DownloadBooks());
            //Task.Factory.StartNew(() => DownloadBooks());
            //Task.Factory.StartNew(() => DownloadBooks());
            //Task.Factory.StartNew(() => DownloadBooks());
            DownloadBooks();
            Assert.IsNotNull(Books);
        }

        private void DownloadBooks()
        {
            HttpClient client = new HttpClient();
            BookStoreContext ctx = new BookStoreContext();
            BookRepository repo = new BookRepository(ctx);
            var data = repo.GetBooks();
            new CallBack(SyncData).Invoke(data);

        }

        public delegate void CallBack(IList<Book> books);
        private void SyncData(IList<Book> books)
        {
            this.Books = books;
            Debug.WriteLine($"Received books data with {this.Books.Count} books.");
        }
    }
}
