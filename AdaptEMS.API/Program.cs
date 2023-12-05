using AdaptEMS.API.Midellwares;
using AdaptEMS.Entities.DBEntities;
using AdaptEMS.Entities.SharedEntities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDBContext>(options =>
{
    options.UseSqlServer(configuration.GetConnectionString("defaultConnection"));

});
builder.Services.AddMvc(options =>
{
    options.Filters.Add<ExceptionLoggerMedillware>();
});
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDBContext>()
    .AddDefaultTokenProviders();

builder.Services.AddControllers();
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = false,
        ValidAudience = configuration["JWT:Issuer"],
        ValidIssuer = configuration["JWT:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]))
    };
});
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequiredLength = 0;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireDigit = false;
});
builder.WebHost.UseEnvironment("Development");

builder.Services.AddHttpContextAccessor();
Initialize(builder.Services);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
 static async void Initialize(IServiceCollection services)
{
    var scopeFactory = services
                  .BuildServiceProvider()
                  .GetRequiredService<IServiceScopeFactory>();
    using (var scope = scopeFactory.CreateScope())
    {

        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        IdentityResult result;
        var adminRoleExist = await roleManager.RoleExistsAsync(Consts.AdminAccountType);
        if (!adminRoleExist)
        {
            var role = new IdentityRole(Consts.AdminAccountType);
            result = await roleManager.CreateAsync(role);
        }
        var employeeRoleExist = await roleManager.RoleExistsAsync(Consts.EmployeeAccountType);
        if (!employeeRoleExist)
        {
            var role = new IdentityRole(Consts.EmployeeAccountType);
            result = await roleManager.CreateAsync(role);
        }
        var admin = await userManager.FindByNameAsync("admin@EMS.com");
        if (admin == null)
        {
            var user = new ApplicationUser { UserName = "admin@EMS.com", AccountType = Consts.AdminAccountType, IsActive = true };
            result = await userManager.CreateAsync(user, "123456");
            if (result == IdentityResult.Success)
            {
                result = await userManager.AddToRoleAsync(user, Consts.AdminAccountType);
            }
        }

        //var employee = await userManager.FindByNameAsync("employee@EMS.com");
        //if (employee == null)
        //{
        //    employee = new ApplicationUser() { UserName = "employee@EMS.com", AccountType = Consts.EmployeeAccountType };
        //    var result = await userManager.CreateAsync(employee, "123456");
        //    if (result.Succeeded)
        //    {
        //        var employeeRoleExist = await roleManager.RoleExistsAsync(Consts.EmployeeAccountType);
        //        if (!employeeRoleExist)
        //        {
        //            var role = new IdentityRole(Consts.EmployeeAccountType);
        //            result = await roleManager.CreateAsync(role);
        //        }
        //        result = await userManager.AddToRoleAsync(employee, Consts.EmployeeAccountType);
        //    }
        //}
    }
}