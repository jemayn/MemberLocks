using Azure;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Common.Security;
using Umbraco.Cms.Web.Website.Controllers;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace MemberLoginTest.Login;

public class LoginController : SurfaceController
{
    private readonly IMemberSignInManager _memberSignInManager;

    public LoginController(
        IUmbracoContextAccessor umbracoContextAccessor,
        IUmbracoDatabaseFactory databaseFactory,
        ServiceContext services,
        AppCaches appCaches,
        IProfilingLogger profilingLogger,
        IPublishedUrlProvider publishedUrlProvider,
        IMemberSignInManager memberSignInManager
    )
        : base(
            umbracoContextAccessor,
            databaseFactory,
            services,
            appCaches,
            profilingLogger,
            publishedUrlProvider
        )
    {
        _memberSignInManager = memberSignInManager;
    }

    [HttpPost]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> HandleDirectLogin([FromBody] DirectLoginModel model)
    {
        if (string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.Password))
        {
            return Unauthorized("Invalid email or password");
        }

        SignInResult result = await _memberSignInManager.PasswordSignInAsync(
            model.Email,
            model.Password,
            false,
            true
        );

        if (result.Succeeded)
        {
            return Ok("H5YR");
        }
        return Unauthorized();
    }
}

public class DirectLoginModel
{
    public string Email { get; set; }
    public string Password { get; set; }
}
