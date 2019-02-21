using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Gateway.Models; 

namespace Gateway.Controllers
{
    [Route("all")]
    public class FEAllController : Controller
    {
        private AllController allController;

        public FEAllController(AllController nAC)
        {
            allController = nAC; 
        }

        [HttpGet("stats")]
        public async Task<IActionResult> Stats()
        {
            var result = await allController.GetStat(); 

            if (result == null || result.StatusCode != 200)
            {
                Error error = new Error(result);
                return View("Error", error);
            }

            return View(result.Value); 
        }
    }
}