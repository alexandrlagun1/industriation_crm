using industriation_crm.Shared.Models;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace Bwasm.Cookie.Providers;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity());
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        return new AuthenticationState(claimsPrincipal);
    }
    public void ClearAuthInfo()
    {
        claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity());
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
    public void SetAuthInfo(user user)
    {
        var identity = new ClaimsIdentity(new[]{
        new Claim(ClaimTypes.Email, user.login),
        new Claim(ClaimTypes.Name, $"{user.name}"),
        new Claim("retail_synch", $"{user.retail_synch}"),
        new Claim(ClaimTypes.NameIdentifier, user.id.ToString()),
        new Claim(ClaimTypes.Role, user.role_id.ToString()),
    }, "AuthCookie");

        claimsPrincipal = new ClaimsPrincipal(identity);
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
}