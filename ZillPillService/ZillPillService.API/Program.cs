using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Net;
using System.Reflection;
using System.Text;
using ZillPillService.Application.Services;
using ZillPillService.Infrastructure.Context;
using ZillPillService.Infrastructure.ServicesContract;

using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;

Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("privatesettings.json", false, true);

var path = Path.Combine(AppContext.BaseDirectory, "connectionsettings.json");
builder.Configuration.AddJsonFile(path, false, true);

builder.Services.AddTransient<IJwtGenerator, JwtGenerator>();
builder.Services.AddTransient<IMedicalProductService, MedicalProductService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<INotificationService, NotificationService>();
builder.Services.AddTransient<ICheckShedullerService, CheckShedullerService>();
builder.Services.AddTransient<ICountriesService, CountriesService>();
builder.Services.AddTransient<ErrorNotificationService>();

#region FireBase

using var stream = new FileStream("zill-pill-v2-firebase-adminsdk-hzxhi-01d0a1cc19.json", FileMode.Open, FileAccess.Read);
FirebaseApp.Create(new AppOptions()
{
    Credential = GoogleCredential.FromStream(stream)
});

#endregion FireBase

#region add JWT authenticaton

var key = builder.Configuration["AuthOptions:TokenKey"];
var _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
var issuer = builder.Configuration["AuthOptions:Issuer"];
var audience = builder.Configuration["AuthOptions:Audience"];

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.RequireHttpsMetadata = false;
        opt.SaveToken = true;
        opt.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidIssuer = issuer,
            ValidateAudience = true,
            ValidAudience = audience,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = _key,
        };
        opt.Events = new JwtBearerEvents()
        {
            OnChallenge = (context) => { return Task.CompletedTask; },
            OnForbidden = (context) => { return Task.CompletedTask; }
        };
    });

#endregion add JWT authenticaton

builder.Services.AddHttpClient("smsAreaApi", s =>
{
    s.BaseAddress = new Uri(builder.Configuration["SmsAreaSettings:BaseUrl"]);
})
    .ConfigurePrimaryHttpMessageHandler(() =>
    {
        var user = builder.Configuration["SmsAreaSettings:User"];
        var pass = builder.Configuration["SmsAreaSettings:Pass"];
        return new HttpClientHandler()
        {
            UseDefaultCredentials = true,
            Credentials = new NetworkCredential(user, pass)
        };
    });

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

#region add metric context

builder.Services.AddDbContext<AppDataBaseContext>(options =>
{
    var conStr = builder.Configuration.GetConnectionString("AppConnectionString");
    options.UseSqlServer(conStr);
});

#region initial data
//using var serviceProvider = builder.Services.BuildServiceProvider();
//var context = serviceProvider.GetRequiredService<AppDataBaseContext>();
//AppDataBaseContext.SeedInitilData(context);
#endregion initial data

#endregion add metric context

#region add swagger

builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme. <br /> 
                        Enter your token in the text input below.<br /> 
                        Result Header: ""Authorization: Bearer { token }""",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = JwtBearerDefaults.AuthenticationScheme
                            },
                            Scheme = "oauth2",
                            Name = JwtBearerDefaults.AuthenticationScheme,
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "ZillPillService.API",
        Version = "v1",
        Description = "A simple ASP.NET Core Web API",
        Contact = new OpenApiContact
        {
            Name = "Kirill",
            Email = string.Empty,
            Url = new Uri("https://github.com/PKkDev"),
        }
    });
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

#endregion add swagger

#region add cors

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder => builder
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader()
    );
});

#endregion add cors

var app = builder.Build();

app.UseExceptionHandler("/error");
// app.UseDeveloperExceptionPage();

#region use swagger

app.UseSwagger();

app.UseSwaggerUI(c =>
{
    var path = builder.Environment.IsDevelopment() ? "/swagger/v1/swagger.json" : "/ZillPillService/swagger/v1/swagger.json";
    c.SwaggerEndpoint(path, "ZillPillService.API v1");
});

#endregion

#region use cors

app.UseCors("CorsPolicy");

#endregion

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();

app.Run();
