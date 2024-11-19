using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace car_rent_api2.Server.Controllers;

[Route("api/[controller]")]
public class IdentityController : Controller
{
    private readonly SignInManager<IdentityUser> _signInManager;
    
    public IdentityController(SignInManager<IdentityUser> signInManager)
    {
        _signInManager = signInManager;
    }
    
    [HttpPost("google-login")]
    [AllowAnonymous]
    public IActionResult GoogleLogin(){
        var redirectUrl = "/api/identity/google-login-callback";
        var properties = _signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);

        return Challenge(properties, "Google");
    }
}