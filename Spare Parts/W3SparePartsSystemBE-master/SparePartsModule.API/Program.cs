using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SparePartsModule.API;
using SparePartsModule.Core;
using SparePartsModule.Domain.Context;
using SparePartsModule.Infrastructure.Configration;
using SparePartsModule.Infrastructure.ViewModels.Dtos;
using SparePartsModule.Interface;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
ConfigurationManager configuration = builder.Configuration;


// Add services to the container. 
builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
{
    options.InvalidModelStateResponseFactory = context =>
        new BadRequestObjectResult(context)
        {



            Value = ApiResponseFactory.CreateBadRequestResponse("1", context)
        };
});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen(c =>
//{
//    // c.DescribeAllParametersInCamelCase();
//    //c.OperationFilter<SwaggerParameterOperationFilter>();

//});

//builder.Services.AddAutoMapper();
//builder.Services.AddScopedAutoMapper();//register mapper
//builder.Services.Configure<EmailConfiguration>(configuration.GetSection("EmailConfiguration"));

builder.Services.AddDbContext<SparePartsModuleContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});
builder.Services.AddDbContext<MarkaziaMasterContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultMarkaziaMaster"));
});
builder.Services.AddDbContext<DX_AdmimistrationContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultAdmin"));
});

builder.Services.AddDbContext<VehicleSystemContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultVehicle"));
});

//builder.Services.AddScoped<FileHelper, FileHelper>();

builder.Services.AddScoped<SparePartsModuleContext>();
builder.Services.AddScoped<MarkaziaMasterContext>();
builder.Services.AddScoped<DX_AdmimistrationContext>();
builder.Services.AddScoped<VehicleSystemContext>();
builder.Services.Configure<EmailConfiguration>(configuration.GetSection("EmailConfiguration"));
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    var Key = Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"] ?? "NoKeysYallah");
    o.SaveToken = false;
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = false,
        ValidateIssuerSigningKey = false,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidAudience = builder.Configuration["JWT:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Key),
    };
});

// Assign it to PDFsharp.


builder.Services.AddSingleton<IJWTManagerManager, JWTManagerManager>();
builder.Services.AddScopedService();
builder.Services.AddSwaggerGen(option =>
{
    //var filePath = Path.Combine(System.AppContext.BaseDirectory, "Abc.Customer.API.xml");
    //option.IncludeXmlComments(filePath);
   //  option.OperationFilter<SwaggerParameterOperationFilter>();
    option.EnableAnnotations();
    option.SwaggerDoc("v1", new OpenApiInfo()
    {
        Title = "SpareParts Library App",
      

    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, "SparePartsModule.API.xml");
    option.IncludeXmlComments(xmlPath,true);

 
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
   
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});
builder.Services.AddCors(optionss =>
{
    optionss.AddPolicy("AllowAllHeaders",
        po =>
        {
            po.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
            .WithOrigins(
               
                "http://localhost:4200",
                "https://localhost:4200",
                "https://markazia.netlify.app",
                "http://localhost",
                "https://markaziatoyta.crashcourseonlin.net",
                "https://markazia123.netlify.app",
                "http://167.86.97.165:8053",
                "http://167.86.97.165:8053/",
                "http://sparepart.programmersapis.com",
                "http://sparepartssystemdev.markaziatest.com",
                "http://sparepartssystemstage.markaziatest.com",
                "https://sparepartssystemdev.markaziatest.com",
                "http://sparepartssystem.markaziasystem.com",
                "http://sparepartssystemdev.markaziadigital.com",
                "https://sparepartssystem.markaziasystem.com",
                "http://cmstagebe.markaziatest.com",
                "http://cmlayerstagebe.markaziatest.com",
                "http://cmstage.markaziatest.com"
                //  "http://sparepartssystemdev.markaziatest.com/",

                //  "http://*.sparepartssystemdev.com"
                );

         //   po.SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost" || new Uri(origin).Host == "sparepartssystemdev");


        });
});
var app = builder.Build();

app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions
{
    ServeUnknownFileTypes = true,
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Uploads")),
    RequestPath = "/Uploads"
});
// Configure the HTTP request pipeline.
app.UseDeveloperExceptionPage();
app.UseSwagger();
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("./v1/swagger.json", "SpareParts Library API"); //originally "./swagger/v1/swagger.json"
        c.DisplayRequestDuration();
    });
  
}
else
{
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "SpareParts Library API"); //originally "./swagger/v1/swagger.json"
                                                                                        // c.SwaggerEndpoint("/swagger/v1/swagger.json", "Markazia POS API"); //originally "./swagger/v1/swagger.json"

        c.RoutePrefix = string.Empty;

        c.DisplayRequestDuration();
    });
}


app.UseMiddleware<CustomExceptionMiddleware>();
app.UseCors("AllowAllHeaders");

app.UseAuthorization();
app.UseAuthorization();
app.MapControllers();

app.Run();
