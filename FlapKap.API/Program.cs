using Dawem.API.MiddleWares;
using Dawem.BusinessLogic;
using Dawem.Validation;
using FlapKap.API;
using FlapKap.API.MiddleWares;
using FlapKap.Data;
using FlapKap.Domain.Entities.User;
using FlapKap.Models;
using FlapKap.Repository;
using FlapKap.Repository.UserManagement;
using FlapKap.Validation.FluentValidation.Users;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();



builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



builder.Services.AddCors(options =>
{
    options.AddPolicy("_allowSpecificOrigins",
        builder => builder
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowAnyOrigin());
});


builder.Services.AddIdentityCore<MyUser>(options =>
{

    options.SignIn.RequireConfirmedAccount = true;
    options.Tokens.ChangePhoneNumberTokenProvider = "FourigitPhone";
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
})
.AddRoles<Role>()
.AddEntityFrameworkStores<ApplicationDBContext>();


builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 0;


});

builder.Services.Configure<IdentityOptions>(options =>
{
    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedPhoneNumber = false;
});

IConfigurationSection appSettingsSection = builder.Configuration.GetSection("appSettings");

builder.Services.AddUserConfiguration();
builder.Services.Configure<Jwt>(appSettingsSection);
builder.Services.Configure<IdentityOptions>(opt => { opt.SignIn.RequireConfirmedEmail = true; opt.User.RequireUniqueEmail = true; });

builder.Services.AddTransient<UserManagerRepository>();
builder.Services.ConfigureSQLContext(builder.Configuration);
builder.Services.ConfigureRepositoryContainer();
builder.Services.ConfigureBLValidation();
builder.Services.ConfigureRepository();
builder.Services.ConfigureBusinessLogic();
builder.Services.AddHttpContextAccessor();
builder.Services.ConfigureJwtAuthentication(builder.Configuration);

builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.UseCamelCasing(true);
    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
    options.SerializerSettings.DefaultValueHandling = DefaultValueHandling.Include;
    options.SerializerSettings.DateParseHandling = DateParseHandling.None;
});


builder.Services.AddValidatorsFromAssemblyContaining<CreateProductModelValidator>();
builder.Services.AddFluentValidationAutoValidation(cpnfig =>
{
    cpnfig.OverrideDefaultResultFactoryWith<FluentValidationResultFactory>();

});

var app = builder.Build();

IServiceScope serviceScope = app.Services.GetService<IServiceScopeFactory>()
    .CreateScope();
ApplicationDBContext context = serviceScope.ServiceProvider.GetRequiredService<ApplicationDBContext>();

context.Database.Migrate();

SeedDB.Initialize(app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope().ServiceProvider);


app.UseMiddleware<RequestInfoMiddleWare>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseStaticFiles();

List<CultureInfo> supportedCultures = new()
{
                    new CultureInfo("en"),
                    new CultureInfo("ar")
};

app.UseCors("_allowSpecificOrigins");

RequestLocalizationOptions requestLocalizationOptions = new()
{
    DefaultRequestCulture = new RequestCulture("en"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
};

app.UseHttpsRedirection();
app.UseMiddleware<UnauthorizedMessageHandlerMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

app.UseRequestLocalization(requestLocalizationOptions);


app.UseMiddleware<ExceptionHandlerMiddleware>();

app.MapControllers();

app.Run();

public partial class Program { }