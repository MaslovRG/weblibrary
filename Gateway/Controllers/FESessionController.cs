using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Gateway.Models;
using Gateway.Models.Readers;
using Gateway.Models.Books;
using Gateway.Models.Session; 
using X.PagedList;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Gateway.Controllers
{
    [Route("session")]
    public class FESessionController : Controller
    {
        private SessionController sessionController;

        public FESessionController(SessionController nSessionController)
        {
            sessionController = nSessionController; 
        }

        [HttpGet]
        public IActionResult Login([FromQuery]int? code, [FromQuery]string message, [FromQuery]string redirect)
        {
            ViewBag.Code = code;
            ViewBag.Message = message;
            ViewBag.Redirect = redirect; 
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Post(User user, [FromQuery]string redirect)
        {
            var result = await sessionController.GetToken(user); 

            if (result == null || result.StatusCode != 200)
            {
                if (result == null)
                    return RedirectToAction(nameof(Login), new
                    {
                        code = 500,
                        message = "Internal error",
                        redirect
                    });
                else
                    return RedirectToAction(nameof(Login), new
                    {
                        code = result.StatusCode,
                        message = result.Value,
                        redirect
                    }); 
            }

            TokenValue tokens = (TokenValue)result.Value;
            HttpContext.Response.Cookies.Append("AccessToken", tokens.AccessToken, new CookieOptions
            {
                Expires = tokens.ExpirityAccess
            });
            HttpContext.Response.Cookies.Append("RefreshToken", tokens.RefreshToken, new CookieOptions
            {
                Expires = tokens.ExpiritRefresh
            });
            redirect = redirect ?? "/book";
            return Redirect(redirect); 
        }

        [HttpGet("refresh")]
        public async Task<IActionResult> Refresh([FromQuery]string redirect)
        {
            var RefreshToken = Request.Cookies["RefreshToken"]; 
            var result = await sessionController.Refresh(RefreshToken);
            if (result == null || result.StatusCode != 200)
                return RedirectToAction(nameof(Login), new { redirect });

            TokenValue tokens = (TokenValue)result.Value;
            HttpContext.Response.Cookies.Append("AccessToken", tokens.AccessToken, new CookieOptions
            {
                Expires = tokens.ExpirityAccess
            });
            HttpContext.Response.Cookies.Append("RefreshToken", tokens.RefreshToken, new CookieOptions
            {
                Expires = tokens.ExpiritRefresh
            });
            redirect = redirect ?? "/book";
            return Redirect(redirect);
        }        
    }
}
