using Microsoft.AspNetCore.Mvc;
using Shop.Data;

namespace SSC.Controllers
{
    public class CommonController : Controller
    {
        protected Guid GetUserId()
        {
            return new Guid(User.Claims.FirstOrDefault(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value);
        }

        protected async Task<IActionResult> ExecuteForResult(Func<Task<DbResult>> action)
        {
            if (ModelState.IsValid)
            {
                var result = await action(); 
                if (result.Success)
                {
                    return Ok(new { message = result.Message });
                }
                else
                {
                    BadRequestErrorMessage(result.Message);
                }
            }
            return InvalidData();
        }

        protected IActionResult InvalidData()
        {
            return BadRequest(new { errors = new { Message = new string[] { "Invalid data" } } });
        }

        protected IActionResult BadRequestErrorMessage(string message)
        {
            return BadRequest(new { errors = new { Message = new string[] { message } } });
        }
    }
}
