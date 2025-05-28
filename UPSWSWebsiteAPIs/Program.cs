using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using UPSWSWebsiteAPIs.BusniessLayer.Interfaces;

var builder = WebApplication.CreateBuilder(args);
 
////=====================Do not change here=====================
builder.Services.AddHttpClient();
builder.Services.AddMemoryCache();

//====================Do not change here================================
builder.Services.Scan(scan => scan
    .FromAssemblyOf<IJWTAuthTokenService>()
    .AddClasses(c => c.Where(t => t.Name.EndsWith("Repository")))
    .AsImplementedInterfaces()
    .WithTransientLifetime());

builder.Services.Scan(scan => scan
    .FromAssemblyOf<IJWTAuthTokenService>()
    .AddClasses(c => c.Where(t => t.Name.EndsWith("Service")))
    .AsImplementedInterfaces()
    .WithScopedLifetime());
//====================Do not change here================================

builder.Services.AddAuthentication(
    options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "UPSWSWebsiteAPIs", Version = "v1" });

    // Add JWT Security Definition
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Enter 'Bearer {token}' without quotes",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer"
    });

    // Require Bearer token for protected endpoints
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});

 
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();  

builder.Services.AddCors(optoins => optoins.AddPolicy("AllowAll", builder =>
{
    builder.AllowAnyHeader();
    builder.AllowAnyMethod();
    builder.AllowAnyOrigin();
}));
var app = builder.Build();
app.UseRouting();
app.UseCors();
app.UseCors(cor => cor
.AllowAnyHeader()
.AllowAnyMethod()
.AllowAnyOrigin()
);
app.UseCors("AllowAll");
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "UPSWSWebsiteAPIs V1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

