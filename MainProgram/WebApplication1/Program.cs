using MainProgram.Data;
using MainProgram.Extensions;
using MainProgram.Interfaces;
using MainProgram.Middlewares;
using MainProgram.Services;
using MainProgram.Repositories;
using Microsoft.EntityFrameworkCore;
using Minio;
using MainProgram.Auth;

var builder = WebApplication.CreateBuilder(args);

var connectionStringUsersDb = builder.Configuration.GetConnectionString("UsersDb");
var connectionStringPostsDb = builder.Configuration.GetConnectionString("PostsDb");

builder.Services.AddSingleton<IMinioClient>(serviceProvider =>
{
    var minioClient = new MinioClient()
        .WithEndpoint("localhost:9000") 
        .WithCredentials("minio-access-key", "minio-secret-key")
        .Build();
    return minioClient;
});

builder.Services.AddDbContext<ApplicationDbContextUsers>(options =>
    options.UseNpgsql(connectionStringUsersDb));

builder.Services.AddDbContext<ApplicationDbContextPosts>(options =>
    options.UseNpgsql(connectionStringPostsDb));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerWithAuth();
builder.Services.AddTransient<ExceptionHandlingMiddleware>();
builder.Services.AddRepositories();
builder.Services.AddServices();
builder.Services.AddJwtAuth(builder.Configuration);

builder.Services.AddSingleton<IMinioRepository, MinioRepository>();
builder.Services.AddScoped<IUserRepository, UsersRepository>();
builder.Services.AddScoped<IPostRepository, PostsRepository>();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPostCervice, PostService>();
builder.Services.AddScoped<ITokenService, TokenService>();

var app = builder.Build();

app.UseCors(cors =>
{
    cors.AllowAnyHeader();
    cors.AllowAnyMethod();
    cors.AllowAnyOrigin();
});

app.UseSwagger();
app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/program/swagger.json", "MainProgram API v1"));

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();

app.Run();