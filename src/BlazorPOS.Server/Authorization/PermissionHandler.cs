using Microsoft.AspNetCore.Authorization;

namespace BlazorPOS.Server.Authorization
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public string Permission { get; }

        public PermissionRequirement(string permission)
        {
            Permission = permission;
        }
    }

    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context, 
            PermissionRequirement requirement)
        {
            var user = context.User;
            var userRole = user.FindFirst(c => c.Type == ClaimTypes.Role)?.Value;

            if (HasPermission(userRole, requirement.Permission))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }

        private bool HasPermission(string userRole, string requiredPermission)
        {
            return userRole switch
            {
                "Admin" => true,
                "Manager" => IsManagerAllowed(requiredPermission),
                "Cashier" => IsCashierAllowed(requiredPermission),
                _ => false
            };
        }

        private bool IsManagerAllowed(string permission)
        {
            var managerPermissions = new[] 
            { 
                "ViewInventory", 
                "EditProduct", 
                "ViewReports" 
            };

            return managerPermissions.Contains(permission);
        }

        private bool IsCashierAllowed(string permission)
        {
            var cashierPermissions = new[] 
            { 
                "CreateSale", 
                "ViewOwnSales" 
            };

            return cashierPermissions.Contains(permission);
        }
    }
}