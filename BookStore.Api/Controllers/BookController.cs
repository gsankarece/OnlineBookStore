using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BookStore.Service.Contract;
using BookStore.Service.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Api.Controllers
{

    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookStoreService svc = null;
        public BookController(IBookStoreService service)
        {
            svc = service;
        }

        [HttpGet]
        [Route("api/books")]

        public async Task<IActionResult> Get(string searchtext)
        {
            var token = Request.Headers["Authorization"];
            if (token.Count == 0)
                return UnAuthorizedAccess();
            var values = token[0].Split(' ');
            if (values.Length < 3)
                return UnAuthorizedAccess();
            string emailId = values[1];
            string accessToken = values[2];
            if (!svc.VerifyAccessToken(emailId, accessToken))
                return UnAuthorizedAccess();

            Task<IEnumerable<BookView>> result = null;
            try
            {
                result = svc.GetBooksAsync(searchtext);

            }
            catch (Exception e)
            {
                return Ok(new { Message = "Something went wrong unable to process request." });
            }
            return Ok(result);
        }

        private OkObjectResult UnAuthorizedAccess()
        {
            return Ok(new { Message = "Unauthorized access" });
        }
    }
}