public void ConfigureServices(IServiceCollection services)
{
    // Existing configurations...

    // Authorization
    services.AddAuthorization(options =>
    {
        options.AddPolicy("CreateSale", policy => 
            policy.Requirements.Add(new PermissionRequirement("CreateSale")));
        options.AddPolicy("ViewInventory", policy => 
            policy.Requirements.Add(new PermissionRequirement("ViewInventory")));
        options.AddPolicy("EditProduct", policy => 
            policy.Requirements.Add(new PermissionRequirement("EditProduct")));
    });

    services.AddSingleton<IAuthorizationHandler, PermissionHandler>();
}