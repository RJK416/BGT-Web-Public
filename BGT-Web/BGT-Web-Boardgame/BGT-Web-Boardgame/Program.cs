using BGT_Web_Boardgame.Data;
using BGT_Web_Boardgame.Repository;
using BGT_Web_Boardgame.Services;
using BGT_Web_Contracts;
using MassTransit;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<BoardgameDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<BoardgameRepo, BoardgameRepo>();
builder.Services.AddScoped<BoardgameService, BoardgameService>();
builder.Services.AddScoped<UserExistProducer, UserExistProducer>();


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

    x.AddRequestClient<UserExistsRequest>();

    x.UsingRabbitMq((context, cfg) =>
    {
        var mq = builder.Configuration.GetSection("RabbitMq");
        var host = mq["Host"] ?? "localhost";
        var vhost = mq["VirtualHost"] ?? "/";
        var username = mq["Username"] ?? "guest";
        var password = mq["Password"] ?? "guest";
        var port = int.TryParse(mq["Port"], out var p) ? p : 5672;

        var uri = new Uri($"rabbitmq://{host}:{port}{vhost}");
        cfg.Host(uri, h =>
        {
            h.Username(username);
            h.Password(password);
        });

        // Explicitly tell MassTransit that requests for UserExistsRequest
        // go to the Account service queue
        EndpointConvention.Map<UserExistsRequest>(
            new Uri("queue:account.rpc.user-exists"));
    });
});


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
