namespace OrderSystem.Application.Authorization;

public class AuthorizationBase
{
    public static AuthorizationResponse ValidUser(UserClaim userClaim, Guid userId)
    {
        Guid userIdClaim = getGuid(userClaim.Id);

        var validGuidResponse = ValidGuid(userClaim);
        if (!validGuidResponse.Success)
            return validGuidResponse;

        if (userId != userIdClaim)
            return new AuthorizationResponse() { Success = false, Message = "User access denied" };

        return new AuthorizationResponse() { Success = true, Message = "User access allowed" };
    }

    public static AuthorizationResponse ValidGuid(UserClaim userClaim)
    {
        var userId = getGuid(userClaim.Id);
        if (userId == Guid.Empty)
            return new AuthorizationResponse() { Success = false, Message = "the authenticated id is not a valid guid" };
        else
            return new AuthorizationResponse() { Success = true, Message = "Valid Guid" };
    }

    public static Guid getGuid(string id)
    {
        if (Guid.TryParse(id, out Guid result))
        {
            return result;
        }
        else
        {
            return Guid.Empty;
        }
    }
}
