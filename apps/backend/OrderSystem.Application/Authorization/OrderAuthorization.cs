using OrderSystem.Application.DTOs.Order;
using OrderSystem.Application.Orders.Commands.CreateOrder;
using OrderSystem.Domain.Entities;

namespace OrderSystem.Application.Authorization;

public class OrderAuthorization : AuthorizationBase
{
    public static AuthorizationResponse CreateOrder(UserClaim userClaim, CreateOrderCommand createOrderCommand)
    {
        var validGuidResponse = ValidGuid(userClaim);
        if (!validGuidResponse.Success)
            return validGuidResponse;

        if (getGuid(userClaim.Id) != createOrderCommand.UserId)
            return new AuthorizationResponse() { Success = false, Message = "the authenticated user id is different from the order user id" };

        return new AuthorizationResponse() { Success = true, Message = "authorized user" };
    }

    public static AuthorizationResponse GetById(UserClaim userClaim, OrderDto? orderDto)
    {
        if (orderDto == null)
            throw new NullReferenceException("orderDto is null");

        if (userClaim.Role == UserRole.Admin)
            return new AuthorizationResponse() { Success = true, Message = "User Admin authorized" };

        var validGuidResponse = ValidGuid(userClaim);
        if (!validGuidResponse.Success)
            return validGuidResponse;

        if (getGuid(userClaim.Id) != orderDto.UserId)
            return new AuthorizationResponse() { Success = false, Message = "the authenticated user id is different from the request user id" };

        return new AuthorizationResponse() { Success = true, Message = "authorized user" };
    }





}
