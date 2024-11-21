using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace car_rent_api2.Server.Controllers;

[Route("api/[controller]")]
public class IdentityController : ControllerBase
{
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;

    public IdentityController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }
    
    [HttpPost("google-login")]
    [AllowAnonymous]
    public IActionResult GoogleLogin()
    {
        var redirectUrl = "/api/Identity/google-login-callback";
        var properties = _signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);

        return Challenge(properties, "Google");
    }
    
    [HttpGet]
    [AllowAnonymous]
    [Route("google-login-callback")]
    public async Task<IActionResult> GoogleLoginCallback()
    {
        Console.WriteLine("Google callback");
        
        var info = await _signInManager.GetExternalLoginInfoAsync();
        if (info == null)
        {
            return Unauthorized("Error during external login.");
        }

        // Sign in the user with Google
        var signInResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);

        if (signInResult.Succeeded)
        {
            return Ok("Login successful!");
        }

        // If the user does not exist, create them
        var email = info.Principal.FindFirstValue(ClaimTypes.Email);
        var user = new IdentityUser { UserName = email, Email = email };
        var result = await _userManager.CreateAsync(user);

        if (result.Succeeded)
        {
            await _userManager.AddLoginAsync(user, info);
            await _signInManager.SignInAsync(user, isPersistent: false);
            return Ok("User created and logged in successfully!");
        }

        return BadRequest("Failed to create user.");
    }
}