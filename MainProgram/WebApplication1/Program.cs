using MainProgram.Data;
using MainProgram.Extensions;
using MainProgram.Middlewares;
using MainProgram.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// �������� ������ �����������
var connectionStringUsersDb = builder.Configuration.GetConnectionString("UsersDb");
var connectionStringPostsDb = builder.Configuration.GetConnectionString("PostsDb");

// ������������ DbContext ��� �������������
builder.Services.AddDbContext<ApplicationDbContextUsers>(options =>
    options.UseNpgsql(connectionStringUsersDb));

// ������������ DbContext ��� ������
builder.Services.AddDbContext<ApplicationDbContextPosts>(options =>
    options.UseNpgsql(connectionStringPostsDb));

// ������ ���� ��������� ������������
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerWithAuth();
builder.Services.AddTransient<ExceptionHandlingMiddleware>();
builder.Services.AddRepositories();
builder.Services.AddServices();
builder.Services.AddJwtAuth(builder.Configuration);

builder.Services.AddScoped<IPostRepository, PostsRepository>();

var app = builder.Build();

// ��������� � ������ ����������
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
