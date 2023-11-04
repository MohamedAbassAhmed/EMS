using AdaptEMS.API.Midellwares;
using AdaptEMS.Entities.DBEntities;
using AdaptEMS.Entities.SharedEntities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaptEMS.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDBContext>();
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDBContext>()
                .AddRoles<IdentityRole>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,

                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidAudience = Configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                };
            });
            services.Configure<IdentityOptions>(options =>
            {
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@";
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
            });
            services.AddAuthorization();
            services.AddControllers().ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = action =>
                {
                    var values = action.ModelState.Values.ToList();
                    string errorString = "";
                    var errorList = new List<string>();
                    foreach (var value in values)
                    {
                        if (value.Errors.Count > 0)
                            errorList.Add(value.Errors.First().ErrorMessage);
                    }
                    errorString += string.Join(',', errorList);

                    return new OkObjectResult(new APIBaseResponse
                    {
                        Success = false,
                        Message = errorString
                    });
                };
            });

            services.AddMvc(options =>
            {
                options.Filters.Add<ExceptionLoggerMedillware>();
            });

            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
            });
            services.AddRazorPages();
            Initialize(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                    //options.RoutePrefix = string.Empty;
                });
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }


            app.UseRouting();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseAuthorization();
            app.UseEndpoints(e =>
            {
                e.MapControllers();
            });
        }
        public static async void Initialize(IServiceCollection services)
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
                    var user = new ApplicationUser { UserName = "admin@EMS.com", AccountType = Consts.AdminAccountType,IsActive=true };
                    result = await userManager.CreateAsync(user, "123456");
                    if(result == IdentityResult.Success)
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
    }
}
