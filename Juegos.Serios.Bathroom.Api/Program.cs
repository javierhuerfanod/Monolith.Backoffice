using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Juegos.Serios.Bathroom.Application;
using Juegos.Serios.Bathroom.Domain;
using Juegos.Serios.Bathroom.Infrastructure;
using Juegos.Serios.Shared.Api.UtilCross;
using Juegos.Serios.Shared.Api.UtilCross.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
string kvUrl = builder.Configuration.GetSection("KVUrl").Value!;
string tenantId = builder.Configuration.GetSection("TenantId").Value!;
string clientId = builder.Configuration.GetSection("ClientId").Value!;
string clientSecret = builder.Configuration.GetSection("ClientSecret").Value!;
string simulatedJuegosSeriosMonolithBackofficeSecrets = builder.Configuration.GetSection("BackOffice").Value!;
ClientSecretCredential credential = new(tenantId, clientId, clientSecret);
SecretClient client = new(new Uri(kvUrl), credential);
builder.Configuration.AddAzureKeyVault(client, new AzureKeyVaultConfigurationOptions());
var simulatedJuegosSeriosMonolithBackofficeJson = client.GetSecret(simulatedJuegosSeriosMonolithBackofficeSecrets).Value.Value;
if (!string.IsNullOrEmpty(simulatedJuegosSeriosMonolithBackofficeJson))
{
    builder.Configuration.AddJsonStream(new MemoryStream(Encoding.UTF8.GetBytes(simulatedJuegosSeriosMonolithBackofficeJson)));
}
// Add services to the container.
builder.Services.AddControllers()
      .AddJsonOptions(options =>
      {
          options.JsonSerializerOptions.Converters.Add(new NullableDateTimeConverter());
      })
    .ConfigureApiBehaviorOptions(options =>
    {
        options.SuppressModelStateInvalidFilter = true;
    });

// Configuring Swagger/OpenAPI
builder.Services.AddSwaggerGen(options =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
    options.OperationFilter<ApplicationTokenHeaderParameter>();

    // Configure Swagger to include JWT Authentication
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Please enter 'Bearer' [space] and then your token in the text input below. Example: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});

// Add authentication services
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
#pragma warning disable CS8604 // Posible argumento de referencia nulo
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
#pragma warning restore CS8604 // Posible argumento de referencia nulo
    });

// Registering domain, application, and infrastructure layers
builder.Services.AddBathroomInfrastuctureServices(builder.Configuration);
builder.Services.AddBathroomDomainServices();
builder.Services.AddBathroomApplicationServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Bathroom Api V1");
        options.OAuthClientId("swagger-ui");
        options.OAuthAppName("Swagger UI");
    });
}

app.UseHttpsRedirection();

app.UseRouting();  // Ensure UseRouting is called before UseAuthentication and UseAuthorization

app.UseAuthentication(); // Apply authentication middleware
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
