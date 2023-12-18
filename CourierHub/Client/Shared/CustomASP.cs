using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace CourierHub.Client.Shared;

public class CustomASP : AuthenticationStateProvider {
    private ClaimsPrincipal _user = new();

    public override Task<AuthenticationState> GetAuthenticationStateAsync() {
        return Task.FromResult(new AuthenticationState(_user));
    }

    public void SetRole(ClaimsPrincipal user, string roleName) {
        var indentity = user.Identities.FirstOrDefault();
        if (indentity == null) {
            user = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Role, roleName) }, "Roles"));
        } else {
            indentity.AddClaim(new Claim(ClaimTypes.Role, roleName));
        }
        _user = user;
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
}