using BGT_Web_Account.DB.Context;
using BGT_Web_Account.Interfaces;
using BGT_Web_Account.Messaging;
using BGT_Web_Account.Models.JWTToken;
using BGT_Web_Account.Repository;
using BGT_Web_Account.Services.Account;
using BGT_Web_Account.Services.Authentication;
using BGT_Web_Account.Services.OTP;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Net.Mail;
using System.Text;
using BGT_Web_Contracts;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMemoryCache();

// Hello Bober
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddMassTransit(x =>
{
    // register your consumer(s)
    x.AddConsumer<UserExistsCheckConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        var mq = builder.Configuration.GetSection("RabbitMq");
        var host = mq["Host"] ?? "localhost";
        var vhost = mq["VirtualHost"] ?? "/";
        var username = mq["Username"] ?? "guest";
        var password = mq["Password"] ?? "guest";
        var port = int.TryParse(mq["Port"], out var p) ? p : 5672;

        var connectionUri = new Uri($"rabbitmq://{host}:{port}{vhost}");
        cfg.Host(connectionUri, h =>
        {
            h.Username(username);
            h.Password(password);
        });

        // stable queue for this RPC
        cfg.ReceiveEndpoint("account.rpc.user-exists", e =>
        {
            e.ConfigureConsumer<UserExistsCheckConsumer>(context);
            e.PrefetchCount = 32;
            e.ConcurrentMessageLimit = 8;
        });

        cfg.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(2)));
    });
});

var jwtSettings = builder.Configuration.GetSection("jwt");
builder.Services.Configure<JwtSettings>(jwtSettings);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

builder.Services.AddScoped<JwtTokenGenerator>();


builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IAccountService, AccountService>();

builder.Services.AddScoped<OTPSender>();
builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddScoped<OTPVerifier>();


builder.Services.AddDbContext<AccDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();