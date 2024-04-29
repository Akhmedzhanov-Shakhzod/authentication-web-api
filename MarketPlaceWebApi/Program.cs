using MarketPlaceWebApi.Helpers.Constant;
using MarketPlaceWebApi.Helpers.DbContext;
using MarketPlaceWebApi.Helpers.Jwt;
using MarketPlaceWebApi.Helpers.Jwt.Impl;
using MarketPlaceWebApi.Helpers.Middleware;
using MarketPlaceWebApi.Mapper.Account;
using MarketPlaceWebApi.Mapper.Account.Impl;
using MarketPlaceWebApi.Models.Account;
using MarketPlaceWebApi.Services.Account;
using MarketPlaceWebApi.Services.Account.Impl;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

        builder.Services.AddAuthentication()
            .AddBearerToken(IdentityConstants.BearerScheme)
            .AddJwtBearer();

        builder.Services.AddDbContext<AppPostgreSQLDbContext>(options =>
        {
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
        });
        builder.Services.AddIdentityCore<Account>(options => options.SignIn.RequireConfirmedAccount = false)
            .AddRoles<Role>()
            .AddEntityFrameworkStores<AppPostgreSQLDbContext>();

        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        builder.Services.AddScoped<IRoleMapper, DefaultRoleMapper>();
        builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        builder.Services.AddScoped<IJwtUtils, DefaultJwtUtils>();
        builder.Services.AddScoped<IAccountService, DefaultAccountService>();

        builder.Services.AddSwaggerGen(option =>
        {
            option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
            option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });
            option.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type=ReferenceType.SecurityScheme,
                            Id="Bearer"
                        }
                    },
                    new string[]{}
                }
            });
        });

        var app = builder.Build();

        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            try
            {
                var context = services.GetRequiredService<AppPostgreSQLDbContext>();
                context.Database.Migrate();

                var userManager = services.GetRequiredService<UserManager<Account>>();
                var rolesManager = services.GetRequiredService<RoleManager<Role>>();
                await DefaultInitializer.InitializeAsync(userManager, rolesManager, context);
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred while seeding the database.");
            }
        }

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        // global error handler
        app.UseMiddleware<ErrorHandlerMiddleware>();

        app.MapControllers();

        app.Run();
    }
}