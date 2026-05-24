using LifeOS.Api.Middlewares;
using LifeOS.Application;
using LifeOS.Application.Auth.Commands;
using LifeOS.Application.Auth.Validators;
using LifeOS.Application.Abstractions.Security;
using AutoMapper;
using FluentValidation;
using LifeOS.Application.Users.Profiles;
using LifeOS.Application.Users.Commands;
using LifeOS.Application.Users.Validators;
using LifeOS.Infra.IoC;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IMapper>(_ =>
{
    var mapperConfiguration = new MapperConfiguration(configuration =>
    {
        configuration.AddMaps(typeof(UserProfile).Assembly);
    }, _.GetRequiredService<ILoggerFactory>());

    return mapperConfiguration.CreateMapper();
});
builder.Services.AddScoped<IValidator<RegisterUserCommand>, RegisterUserCommandValidator>();
builder.Services.AddScoped<IValidator<LoginUserCommand>, LoginUserCommandValidator>();
builder.Services.AddScoped<IValidator<GoogleLoginCommand>, GoogleLoginCommandValidator>();
builder.Services.AddScoped<IValidator<RefreshTokenCommand>, RefreshTokenCommandValidator>();
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection(JwtSettings.SectionName));
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var jwtSection = builder.Configuration.GetSection("Jwt");
var jwtKey = jwtSection["Key"] ?? throw new InvalidOperationException("JWT configuration is missing the Key value.");
var jwtIssuer = jwtSection["Issuer"] ?? throw new InvalidOperationException("JWT configuration is missing the Issuer value.");
var jwtAudience = jwtSection["Audience"] ?? throw new InvalidOperationException("JWT configuration is missing the Audience value.");

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            ClockSkew = TimeSpan.Zero
        };
    });

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
