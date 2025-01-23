using System.Text;
using AutoMapper;
using Backend.Data;
using Backend.Mapper;
using Backend.Services;
using Backend.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddDbContext<ApplicationDbContext>(options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    });
builder.Services.AddControllers().AddNewtonsoftJson();

builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IMemberService, MemberService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services
    .AddAuthentication( JwtBearerDefaults.AuthenticationScheme )
    .AddJwtBearer
    (
        opt =>
        {
            // http, bukan https. dalam kata lain lokal
            opt.RequireHttpsMetadata = false;
            // untuk save token
            opt.SaveToken = true;
            // setting parameter agar bisa digunakan dalam lingkup lokal/http
            opt.TokenValidationParameters = new TokenValidationParameters
            {
                // Digunakan untuk memvalidasi akses token yang dikirim ke backend
                // Di validasi berdasrkan KEY yang di generate oleh backend ini
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["JwtSettings:Key"])),
                ValidateIssuer = false,
                ValidateAudience = false,
            };
        }
    );

// Auto Mapper
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddScoped<IMapper>(mp =>
{
    return new Mapper(AutoMapperConfig.RegisterMappings());
});
builder.Services.AddSingleton(AutoMapperConfig.RegisterMappings());

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

app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
