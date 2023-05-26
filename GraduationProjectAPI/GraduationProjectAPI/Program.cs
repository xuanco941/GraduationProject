using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using GraduationProjectAPI.Models;
using GraduationProjectAPI.Colorization;
using Microsoft.AspNetCore.Http.Features;
using GraduationProjectAPI.Services.Auth;
using GraduationProjectAPI.Services.Image;
using GraduationProjectAPI.Services.Email;
using GraduationProjectAPI.Services.User;
using GraduationProjectAPI.Services.HistoryImageColorized;
using GraduationProjectAPI.Services.Context;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();


//service auth
builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]))
        };
        //options.SaveToken = true;
    });


//service db 
builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));// shorthand getSection("ConnectionStrings")["DefaultConnection"]




//http context
builder.Services.AddHttpContextAccessor();



//img
// Thêm hỗ trợ cho việc nhận ảnh
builder.Services.Configure<IISServerOptions>(options =>
{
    options.AllowSynchronousIO = true;
});
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = long.MaxValue; // Giới hạn kích thước tệp tin nhận lên
    options.MemoryBufferThreshold = int.MaxValue; // Giới hạn bộ đệm trong trường hợp tệp tin quá lớn
    options.ValueCountLimit = int.MaxValue; // Giới hạn số lượng trường dữ liệu
});


//Cors
builder.Services.AddCors(options => options.AddPolicy("CorsPolicy", builder =>
{
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader();
}));


//service config
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
builder.Services.AddSingleton<IWebHostEnvironment>(builder.Environment);








//services
builder.Services.AddTransient<IAuthService,AuthService>();
builder.Services.AddScoped<DeOldify>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddTransient<IEmailService, EmailService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IHistoryImageColorizedService, HistoryImageColorizedService>();
builder.Services.AddTransient<IHttpContextMethod, HttpContextMethod>();
















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


app.UseCors("CorsPolicy");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


DeOldifyModel.Initialize(new BinaryReader(File.OpenRead(Path.Combine("Colorization","Stable.model"))));

app.Run();