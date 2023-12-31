using Crud.Repository;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container

builder.Services.AddControllers();

// Swagger config
builder.Services.AddSwaggerGen();

//System Text Json config
builder.Services.AddMvc()
                .AddJsonOptions(j =>
                {
                    j.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;

                });

// Db connection
string connectionString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<IMDBContext>(config =>
{
    config.UseSqlServer(connectionString);
});

//

var app = builder.Build();

// Configure HTTP request pipeline

// also swagger confing inside http request
app.UseSwagger();

app.UseSwaggerUI();
//

app.UseHttpsRedirection();

app.UseAuthorization(); // De momento esto no lo vamos a usar

app.MapControllers();

app.Run();