using Coravel;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Subscribers.Models.Subscribers;
using Subscribers.Repositories;
using Subscribers.Repositories.Context;
using Subscribers.Repositories.Repositories;
using Subscribers.Services.Process;
using Subscribers.Services.Services;
using Subscribers.Services.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<SubscriberDatabaseContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DBConnectionString")));

builder.Services.AddCors();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddValidatorsFromAssembly(typeof(SubscriberModelValidator).Assembly);
builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddScheduler();

builder.Services.AddScoped<SubscriberDatabaseContext>();
builder.Services.AddScoped<ISubscribersService, SubscribersService>();
builder.Services.AddScoped(typeof(IBaseCsvReaderService<>), typeof(BaseCsvReaderService<>));
builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
builder.Services.AddTransient<ProcessCheckAllSubscribersExpirationDate>();

builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Users API",
        Description = "ASP.NET Core Web API",
    });
});

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
var app = builder.Build();

app.Services.UseScheduler(scheduler =>
{
    scheduler.Schedule<ProcessCheckAllSubscribersExpirationDate>()
        .DailyAtHour(10)
        .PreventOverlapping(nameof(ProcessCheckAllSubscribersExpirationDate));
});

app.UseCors();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.MapControllers();

app.Run();
