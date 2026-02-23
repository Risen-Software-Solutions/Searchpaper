using Lioncore.WebApi.Entities;
using Lioncore.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Lioncore.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ManageController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public ManageController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager
    )
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpPut("ChangePassword")]
    public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
    {
        if (ModelState.IsValid == false)
        {
            return ValidationProblem(ModelState);
        }

        var user = await _userManager.GetUserAsync(HttpContext.User);

        if (user == null)
        {
            return BadRequest();
        }

        var result = await _userManager.ChangePasswordAsync(
            user,
            model.OldPassword,
            model.NewPassword
        );

        if (result.Succeeded == false)
        {
            return BadRequest(result);
        }

        await _signInManager.SignInAsync(user, isPersistent: false);

        return NoContent();
    }

    [HttpPut("ChangeInfo")]
    public async Task<IActionResult> ChangeInfo(ChangeInfoModel model)
    {
        if (ModelState.IsValid == false)
        {
            return ValidationProblem(ModelState);
        }

        var user = await _userManager.GetUserAsync(HttpContext.User);

        if (user == null)
        {
            return BadRequest();
        }

        user.FullName = model.FullName;

        var result = await _userManager.UpdateAsync(user);

        if (result.Succeeded == false)
        {
            return BadRequest(result);
        }

        return NoContent();
    }
}
