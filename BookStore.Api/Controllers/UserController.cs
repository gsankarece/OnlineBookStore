using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Common;
using BookStore.Service.Contract;
using BookStore.Service.ViewModels;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BookStore.Api.Controllers
{
    //[Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IBookStoreService svc = null;
        public UserController(IBookStoreService service)
        {
            svc = service;
        }

        [Route("api/user/login")]
        [HttpPost]
        public async Task<IActionResult> Login([FromForm]string emailId, string password)
        {
            try
            {
                var user = svc.Login(emailId, password);
                return Ok(user);
            }
            catch(Exception ex)
            {
                return Ok(new { Message = ex.Message });
            }
        }

        [Route("api/user/register")]
        [HttpPost]
        public async Task<IActionResult> Register([FromForm]User user)
        {
            try
            {
                return Ok(svc.Register(user));
            }
            catch(Exception ex)
            {
                return Ok(new { Message = ex.Message });
            }
        }

    }
}
