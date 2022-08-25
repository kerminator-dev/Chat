using Chat.API.Extensions;
using Chat.API.Hubs;
using Chat.API.Services.Authenticators;
using Chat.API.Services.ConnectionRepositories;
using Chat.API.Services.MessageRepositories;
using Chat.API.Services.MessagingServices;
using Chat.API.Services.Messangers;
using Chat.API.Services.PasswordHashers;
using Chat.API.Services.RefreshTokenRepositories;
using Chat.API.Services.TokenGenerators;
using Chat.API.Services.TokenValidators;
using Chat.API.Services.UserRepositories;
using Microsoft.AspNetCore.Http.Connections;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplicationDbContext(builder.Configuration);
builder.Services.AddAuthenticationConfiguration(builder.Configuration);
builder.Services.AddSingleton<TokenGenerator>();
builder.Services.AddSingleton<RefreshTokenGenerator>();
builder.Services.AddSingleton<AccessTokenGenerator>();
builder.Services.AddSingleton<RefreshTokenValidator>();
builder.Services.AddScoped<AuthenticationProvider>();
builder.Services.AddSingleton<IPasswordHasher, BCryptPasswordHasher>();
builder.Services.AddScoped<IUserRepository, DatabaseUserRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, DatabaseRefreshTokenRepository>();
builder.Services.AddSignalR().AddJsonProtocol();
builder.Services.AddScoped<ChatHub>();
builder.Services.AddScoped<IConnectionRepository, DatabaseConnectionRepository>();
builder.Services.AddScoped<IMessageRepository, DatabaseMessageRepository>();
builder.Services.AddScoped<IMessagingService, SignalRMessagingService>();
builder.Services.AddScoped<MessageProvider>();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHub<ChatHub>("/chathub", options =>
{
    options.Transports = HttpTransportType.WebSockets;
});

app.Run();