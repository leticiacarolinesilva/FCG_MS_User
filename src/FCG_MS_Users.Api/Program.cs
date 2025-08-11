using FCG_MS_Users.Api.Extensions;
using FCG_MS_Users.Infra;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var jwtKey = builder.Configuration.GetValue<string>("Jwt:Key");

builder.Services.AddDbContext<UserRegistrationDbContext>(options =>
    options.UseNpgsql(connectionString));
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.UseCollectionExtensions();

builder.Services.UseAuthenticationExtensions(jwtKey);

builder.Services.UseSwaggerExtensions();

builder.Services.AddFluentValidationAutoValidation(options =>
{
    options.DisableDataAnnotationsValidation = true;
});

builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

builder.Services.UseValidatorExtensions();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<UserRegistrationDbContext>();
    dbContext.Database.Migrate();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
