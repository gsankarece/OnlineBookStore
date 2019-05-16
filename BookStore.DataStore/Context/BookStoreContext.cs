using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.DataStore.Context
{
    using BookStore.Common;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using System.IO;
    using System.Net.Http;

    public class BookStoreContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }

        public BookStoreContext()
        {

        }
        public BookStoreContext(DbContextOptions options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);            
            modelBuilder.Entity<Book>().HasData(new DataSeeder().books);
            modelBuilder.Entity<User>().HasData(new User[] {
                new User { Id = Guid.NewGuid(), Email = "admin@bookstore.com", Password = "admin123", UserName = "admin" }
            });
        }
    }

    public class DataSeeder
    {
        public DataSeeder()
        {
            DownloadBooks();
        }

        public IList<Book> books = null;
        private void DownloadBooks()
        {

            RawData data = null;

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    using (var stream = client.GetStreamAsync(@"https://raw.githubusercontent.com/contribe/contribe/dev/arbetsprov-net/books.json").Result)
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            var result = reader.ReadToEnd();
                            //File.Create(AppDomain.CurrentDomain.BaseDirectory + "data.json");
                            //File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "data.json", result);
                            data = JsonConvert.DeserializeObject<RawData>(result);
                        }
                    }
                }

                books = data.Books;
                AssignId();
            }
            catch (Exception ex)
            {

            }


        }

        private void AssignId()
        {
            foreach (var book in books)
            {
                book.Id = Guid.NewGuid();
            }
        }


    }
}
