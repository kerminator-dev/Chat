using Chat.API.Extensions;
using Chat.API.Hubs;
using Chat.API.Services.Implementation;
using Chat.API.Services.Interfaces;
using Chat.API.Services.Providers;
using Chat.API.Services.TokenGenerators;
using Microsoft.AspNetCore.Http.Connections;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();
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
builder.Services.AddScoped<DialogueProvider>();
builder.Services.AddScoped<IDialogueRepository, DatabaseDialogueRepository>();
builder.Services.AddScoped<UserProvider>();


var app = builder.Build();

app.UseCors(builder =>
{
    builder.AllowAnyHeader()
           .AllowAnyMethod()
           .AllowAnyOrigin();
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHub<ChatHub>("/chathub", options =>
{
    options.Transports = HttpTransportType.WebSockets;
    options.TransportMaxBufferSize = 32;
    options.ApplicationMaxBufferSize = 32;
});

app.Run();