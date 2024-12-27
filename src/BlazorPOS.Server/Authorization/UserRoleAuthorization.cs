using BlazorPOS.Shared.Enums;

namespace BlazorPOS.Server.Authorization
{
    public static class UserRoleAuthorization
    {
        public static bool CanAccessFeature(UserRole userRole, Feature feature)
        {
            return feature switch
            {
                Feature.ProductManagement => userRole == UserRole.Admin || userRole == UserRole.Manager,
                Feature.SalesProcessing => userRole != UserRole.Admin,
                Feature.InventoryAdjustment => userRole == UserRole.Admin || userRole == UserRole.Manager,
                _ => false
            };
        }
    }
}