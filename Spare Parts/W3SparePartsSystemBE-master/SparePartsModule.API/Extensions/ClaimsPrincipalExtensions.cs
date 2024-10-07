using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Hosting;

namespace MarkaziaPOS.API.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static int GetUserId(this ClaimsPrincipal principal)
        {
            if (principal == null)
                throw new ArgumentNullException(nameof(principal));
            int userId =900100;
            if(principal.FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                userId = Convert.ToInt32(principal.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            }
            return userId;
        }

    }
}

