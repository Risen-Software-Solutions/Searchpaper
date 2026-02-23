using System.Web;
using Lioncore.WebApi.Entities;
using Lioncore.WebApi.Models;
using Lioncore.WebApi.Templates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lioncore.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AccountController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IEmailSender _emailSender;
    private readonly ILoggerFactory _loggerFactory;

    private readonly IConfiguration _configuration;

    public AccountController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IEmailSender emailSender,
        ILoggerFactory loggerFactory,
        IConfiguration configuration
    )
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _emailSender = emailSender;
        _loggerFactory = loggerFactory;
        _configuration = configuration;
    }

    [HttpPost("Register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register(RegisterModel model)
    {
        if (ModelState.IsValid == false)
        {
            return ValidationProblem(ModelState);
        }

        var user = new ApplicationUser
        {
            UserName = model.Email,
            Email = model.Email,
            FullName = model.FullName,
        };

        var result = await _userManager.CreateAsync(user, model.Password);

        if (result.Succeeded == false)
        {
            return BadRequest(result);
        }

        await using var htmlRenderer = new HtmlRenderer(
            HttpContext.RequestServices,
            _loggerFactory
        );

        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

        var callbackUrl =
            $"{_configuration["SpaUrl"]}/confirmemail?userId={user.Id}&code={HttpUtility.UrlEncode(code)}";

        var html = await htmlRenderer.Dispatcher.InvokeAsync(async () =>
        {
            var dictionary = new Dictionary<string, object?> { { "Link", callbackUrl } };

            var parameters = Microsoft.AspNetCore.Components.ParameterView.FromDictionary(
                dictionary
            );
            var output = await htmlRenderer.RenderComponentAsync<EmailConfirm>(parameters);

            return output.ToHtmlString();
        });

        await _emailSender.SendEmailAsync(model.Email, "Confirme seu Email", html);

        return Created();
    }

    [HttpGet("ConfirmEmail")]
    [AllowAnonymous]
    public async Task<IActionResult> ConfirmEmail(
        [FromQuery] string userId,
        [FromQuery] string code
    )
    {
        if (userId == null || code == null)
        {
            return BadRequest();
        }
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return BadRequest();
        }

        var result = await _userManager.ConfirmEmailAsync(user, code.Replace(" ", "+"));

        if (result.Succeeded == false)
        {
            return BadRequest(result);
        }

        return Ok();
    }

    [HttpPost("ForgotPassword")]
    [AllowAnonymous]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
    {
        if (ModelState.IsValid == false)
        {
            return ValidationProblem(ModelState);
        }

        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
        {
            // Don't reveal that the user does not exist
            return NoContent();
        }

        var code = await _userManager.GeneratePasswordResetTokenAsync(user);

        var callbackUrl =
            $"{_configuration["SpaUrl"]}/resetpassword?email={user.Email}&code={HttpUtility.UrlEncode(code)}";

        await using var htmlRenderer = new HtmlRenderer(
            HttpContext.RequestServices,
            _loggerFactory
        );

        var html = await htmlRenderer.Dispatcher.InvokeAsync(async () =>
        {
            var dictionary = new Dictionary<string, object?> { { "Link", callbackUrl } };

            var parameters = Microsoft.AspNetCore.Components.ParameterView.FromDictionary(
                dictionary
            );
            var output = await htmlRenderer.RenderComponentAsync<ResetPassword>(parameters);

            return output.ToHtmlString();
        });

        await _emailSender.SendEmailAsync(model.Email, "Recuperar sua Senha", html);

        return NoContent();
    }

    // POST: /Account/ResetPassword
    [HttpPost("ResetPassword")]
    [AllowAnonymous]
    public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
    {
        if (ModelState.IsValid == false)
        {
            return ValidationProblem(ModelState);
        }

        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            // Don't reveal that the user does not exist
            return NoContent();
        }
        var result = await _userManager.ResetPasswordAsync(
            user,
            model.Code.Replace(" ", "+"),
            model.Password
        );
        if (result.Succeeded == false)
        {
            return BadRequest(result);
        }

        return NoContent();
    }

    [HttpGet("Info")]
    public async Task<IActionResult> Info()
    {
        var user = await _userManager.GetUserAsync(HttpContext.User);

        if (user == null)
        {
            return Unauthorized();
        }

        return Ok(new { user.FullName, user.Email });
    }

    [HttpPost("LogOff")]
    public async Task<IActionResult> LogOff()
    {
        await _signInManager.SignOutAsync();
        return NoContent();
    }
}
