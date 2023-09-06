using Microsoft.AspNetCore.Mvc;

namespace WebsiteTester.WebApi.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("/api/[controller]")]
    public class BaseController : Controller
    {
        internal Guid UserId => !User.Identity.IsAuthenticated
            ? Guid.Empty
            : Guid.Parse(User.FindFirst("sub").Value);
    }
}
