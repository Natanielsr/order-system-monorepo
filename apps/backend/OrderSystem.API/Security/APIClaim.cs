using System;
using System.Security.Claims;
using OrderSystem.Application.Authorization;

namespace OrderSystem.API.Security;

public static class APIClaim
{
    public static UserClaim createUserClaim(ClaimsPrincipal user)
    {
        var userId = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var email = user?.FindFirst(ClaimTypes.Email)?.Value;
        var username = user?.FindFirst(ClaimTypes.Name)?.Value;
        var role = user?.FindFirst(ClaimTypes.Role)?.Value;

        if (userId == null)
            throw new NullReferenceException("Null UserId Claim");

        if (email == null)
            throw new NullReferenceException("Null email Claim");

        if (username == null)
            throw new NullReferenceException("Null username Claim");

        if (role == null)
            throw new NullReferenceException("Null role Claim");

        UserClaim userClaim = new UserClaim() { Id = userId, Email = email, Username = username, Role = role };

        return userClaim;
    }
}
