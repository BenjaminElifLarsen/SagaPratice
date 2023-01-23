using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;
[Route("[controller]")]
[ApiController]
public class AccessController : ControllerBase
{
    [HttpPost]
    [AllowAnonymous]
    [Route(nameof(Login))]
    public async Task<IActionResult> Login()
    {
        throw new NotImplementedException();
    }
    [HttpPost]
    [AllowAnonymous]
    [Route(nameof(RefreshToken))]
    public async Task<IActionResult> RefreshToken()
    {
        throw new NotImplementedException();
    }

    [HttpDelete]
    [AllowAnonymous]
    [Route(nameof(Login))]
    public async Task<IActionResult> RevokeRefreshToken()
    {
        throw new NotImplementedException();
    }

}
