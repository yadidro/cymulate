using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using RoyPhishingProj.BusinessLogicLayer;
using RoyPhishingProj.DBLayer;
using RoyPhishingProj.APILayer;


var builder = WebApplication.CreateBuilder(args);

// Register the phishing email service
builder.Services.AddScoped<IPhishingEmailService, PhishingEmailService>();
builder.Services.AddTransient<IEmailService, EmailService>();

// Add Swagger if needed
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<MongoDBSettings>(
    builder.Configuration.GetSection("MongoDB"));

builder.Services.AddSingleton<MongoDBService>();
builder.Services.AddControllers();
builder.Services.AddControllers().AddJsonOptions(options =>
    options.JsonSerializerOptions.AddContext<MyJsonContext>()); ;
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes("super_secret_key"))
        };
    });


var app = builder.Build();

// Configure middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAngularApp");

app.UseHttpsRedirection();

app.MapControllers();

app.Run();