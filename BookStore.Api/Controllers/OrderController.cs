using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Common;
using BookStore.Service.Contract;
using BookStore.Service.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Api.Controllers
{

    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IBookStoreService svc = null;
        public OrderController(IBookStoreService service)
        {
            svc = service;
        }

        [HttpGet]
        [Route("api/order")]
        public async Task<IActionResult> AllOrders(User user)
        {
            try
            {
                var orders = svc.AllOrders(user);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return NotFound(new { Message = "Something went wrong, unable to get your orders." });
            }
        }

        [HttpPost]
        [Route("api/order/new")]
        public async Task<IActionResult> NewOrder(UserView user)
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

            try
            {
                var order = svc.NewOrder(user);
                return Ok(order.Result);
            }
            catch (Exception ex)
            {
                return Ok(new { Message = "Something went wrong, unable to get your orders." });
            }

        }

        [HttpPost]
        [Route("api/order/stockcheck")]
        public async Task<IActionResult> StockCheck(UserView user)
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
            try
            {
                var stockInfo = svc.CheckAvailableStock(user);
                return Ok(stockInfo.Result);
            }
            catch (Exception)
            {
                return Ok(new { Message = "Something went wrong, unable to get your orders." });
            }
        }

        [HttpPut]
        [Route("api/order/edit")]
        public async Task<IActionResult> EditOrder(UserView user)
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
            try
            {
                var order = svc.UpdateOrder(user);
                return Ok(order.Result);
            }
            catch (Exception)
            {
                return Ok(new { Message = "Something went wrong, unable to get your orders." });
            }
        }

        [HttpPost]
        [Route("api/order/cancel")]
        public async Task<IActionResult> CancelOrder(UserView user)
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
            try
            {
                var status = svc.CancelOrder(user);
                return Ok(status.Result);
            }
            catch (Exception)
            {
                return Ok(new { Message = "Something went wrong, unable to get your orders." });
            }
        }

        private OkObjectResult UnAuthorizedAccess()
        {
            return Ok(new { Message = "Unauthorized access" });
        }
    }
}