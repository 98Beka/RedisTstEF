using EFCoreSecondLevelCacheInterceptor;
using EasyCaching.Redis;
using EasyCaching.Serialization.MessagePack;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using RedisTst.Data;
using MessagePack.Resolvers;
using MessagePack.Formatters;
using MessagePack;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEasyCaching(options =>
{
    options.UseRedis(builder.Configuration);

    options.WithMessagePack(so =>
    {
        so.EnableCustomResolver = true;
        so.CustomResolvers = CompositeResolver.Create(
        new IMessagePackFormatter[]
        {
                DBNullFormatter.Instance // This is necessary for the null values
        },
        new IFormatterResolver[]
        {
                NativeDateTimeResolver.Instance,
                ContractlessStandardResolver.Instance,
                StandardResolverAllowPrivate.Instance,
                TypelessContractlessStandardResolver.Instance,
        });
    },
    "Bek"); //have to add this user to "ACL users" in the Redis 
});

// Add services to the container.
builder.Services.AddDbContext<DataContext>((serviceProvider, optionsBuilder) => {
    optionsBuilder
        .UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
        .AddInterceptors(serviceProvider.GetRequiredService<SecondLevelCacheInterceptor>());
});

builder.Services.AddEFSecondLevelCache(options =>
    options
        .UseEasyCachingCoreProvider("DefaultRedis", isHybridCache: false)
        .DisableLogging(true)
        .CacheAllQueries(CacheExpirationMode.Absolute, TimeSpan.FromMinutes(10))
        .UseCacheKeyPrefix("EF_"));
    
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
