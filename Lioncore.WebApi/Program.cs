using System.Diagnostics;
using System.Net;
using CliWrap;
using Lioncore.WebApi.Context;
using Lioncore.WebApi.Database;
using Lioncore.WebApi.Entities;
using Lioncore.WebApi.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Yarp.ReverseProxy.Forwarder;

namespace Lioncore.WebApi;

public class Program
{
    private static CancellationTokenSource ct { get; } = new CancellationTokenSource();

    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        builder.Services.AddDbContext<LioncoreContext>(opt =>
        {
            var connectionString = builder.Configuration["ConnectionStrings:LioncoreDatabase"];

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new NullReferenceException("Connection string cannot be null");
            }

            opt.UseMySQL(connectionString);
        });

        builder.Services.AddAuthorization();

        builder
            .Services.AddIdentityApiEndpoints<ApplicationUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<LioncoreContext>();

        builder.Services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequiredLength = 6;
            options.Password.RequiredUniqueChars = 0;
        });

        builder.Services.Configure<IdentityOptions>(options =>
        {
            options.SignIn.RequireConfirmedEmail = true;
        });

        builder.Services.Configure<EmailSenderOptions>(
            builder.Configuration.GetSection(EmailSenderOptions.EmailSender)
        );

        builder.Services.AddTransient<IEmailSender, EmailSender>();

        builder.Services.AddHttpForwarder();

        builder.Services.AddHostedService<ViteHostedService>();

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            SeedDatabase.Initialize(services);
        }

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.UseSwaggerUi(options =>
            {
                options.DocumentPath = "/openapi/v1.json";
            });

            var httpClient = new HttpMessageInvoker(
                new SocketsHttpHandler
                {
                    UseProxy = false,
                    AllowAutoRedirect = false,
                    AutomaticDecompression = DecompressionMethods.None,
                    UseCookies = true,
                    EnableMultipleHttp2Connections = true,
                    ActivityHeadersPropagator = new ReverseProxyPropagator(
                        DistributedContextPropagator.Current
                    ),
                    ConnectTimeout = TimeSpan.FromSeconds(15),
                }
            );

            var transformer = HttpTransformer.Default; // or HttpTransformer.Default;
            var requestConfig = new ForwarderRequestConfig
            {
                ActivityTimeout = TimeSpan.FromSeconds(100),
            };

            var viteHost = app.Configuration["Vite:Host"];

            if (string.IsNullOrEmpty(viteHost))
            {
                throw new NullReferenceException("Vite host cannot be null");
            }

            app.MapForwarder("/{**catch-all}", viteHost, requestConfig, transformer, httpClient);
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.UseWebSockets();

        app.MapControllers();

        app.MapGroup("/api").MapIdentityApi<ApplicationUser>();

        app.Run();
    }
}
