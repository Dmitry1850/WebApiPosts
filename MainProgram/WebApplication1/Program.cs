using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MainProgram.Services;
using MainProgram.Interfaces;
using MainProgram.Auth;
using MainProgram.Middlewares;
using MainProgram.Extensions;
using MainProgram.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Minio;
using MainProgram.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerWithAuth();
builder.Services.AddTransient<ExceptionHandlingMiddleware>();
builder.Services.AddRepositories();
builder.Services.AddServices();
builder.Services.AddJwtAuth(builder.Configuration); 

builder.Services.AddScoped<IPostRepository, PostsRepository>();

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

