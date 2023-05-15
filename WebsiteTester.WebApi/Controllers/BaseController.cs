using Microsoft.AspNetCore.Mvc;

namespace WebsiteTester.WebApi.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("/api/[controller]")]
    public class BaseController : Controller
    {

    }
}
