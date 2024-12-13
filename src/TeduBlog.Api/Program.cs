using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TeduBlog.Api;
using TeduBlog.Core.Identity;
using TeduBlog.Core.Repositories;
using TeduBlog.Core.SeedWorks;
using TeduBlog.Data;
using TeduBlog.Data.Repositories;
using TeduBlog.Data.SeedWorks;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var connectionString = configuration.GetConnectionString("DefaultConnection");


// Add services to the container.

//Config DB Context and ASP.NET Core Identity
builder.Services.AddDbContext<TeduBlogContext>(options =>
                options.UseSqlServer(connectionString));

builder.Services.AddIdentity<AppUser, AppRole>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddEntityFrameworkStores<TeduBlogContext>();

builder.Services.Configure<IdentityOptions>(options =>
{
    //Password settings.
    options.Password.RequireDigit = true;// yêu cầu số
    options.Password.RequireLowercase = true;// yêu cầu chữ thường
    options.Password.RequireNonAlphanumeric = true;//yêu cầu ký tự đặc biệt
    options.Password.RequireUppercase = true;//yêu cầu chữ hoa
    options.Password.RequiredLength = 6;//yêu cầu độ dài tối thiểu
    options.Password.RequiredUniqueChars = 1;//số lượng Unichar

    //Lockout settings.
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);//thời gian log out tài khoản
    options.Lockout.MaxFailedAccessAttempts = 5;// đăng nhập bao lâu thì log out
    options.Lockout.AllowedForNewUsers = true;//cho phép lưu thông tin tk user

    //User settings.
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = false;

});

// Add services to the container
builder.Services.AddScoped(typeof(IRepository<,>), typeof(RepositoryBase<,>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

//Business service and repositories
//builder.Services.AddScoped<IPostRepository, PostRepository>();
var services = typeof(PostRepository).Assembly.GetTypes()
    .Where(x => x.GetInterfaces().Any(i => i.Name == typeof(IRepository<,>).Name)
    && !x.IsAbstract && x.IsClass && !x.IsGenericType);

foreach (var service in services)
{
    var allInterfaces = service.GetInterfaces();
    var direcInterface = allInterfaces.Except(allInterfaces.SelectMany(x => x.GetInterfaces())).FirstOrDefault();
    if(direcInterface != null)
    {
        builder.Services.Add(new ServiceDescriptor(direcInterface, service, ServiceLifetime.Scoped));
    }
}

//Default config for Asp.Net Core
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

//Seeding data
app.MigrateDatabase();

app.Run();
