
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Play.Catalog.Service.Entities;
using Play.Common.MongoDB;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMongo().AddMongoRepository<Item>("items");




builder.Services.AddControllers(options => options.SuppressAsyncSuffixInActionNames = false);
var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.MapControllers();
app.Run();

