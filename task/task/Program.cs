using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.ResponseCompression;
using task.data.Essentials;
using task.Helper;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddScoped<CustomerSession>();
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
});
builder.Services.AddHsts(options =>
{
    options.Preload = true;
    options.IncludeSubDomains = true;
    options.MaxAge = TimeSpan.FromDays(60);
});

builder.Services.AddHttpsRedirection(options =>
{
    options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
    options.HttpsPort = 443;
});
builder.Services.AddCors();
builder.Services.Configure<GzipCompressionProviderOptions>(options => options.Level = System.IO.Compression.CompressionLevel.Fastest);
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.Providers.Add<GzipCompressionProvider>();

});
builder.Services.AddControllers(config =>
{
    config.EnableEndpointRouting = false;
    config.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
}).AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(m => m.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
app.Use((context, next) =>
{
    if (context.Request.Headers.TryGetValue("X-Forwarded-Proto", out Microsoft.Extensions.Primitives.StringValues proto))
    {
        context.Request.Scheme = proto;
    }
    return next();
});
app.UseHttpsRedirection();
app.UseAuthorization();
app.UseMiddleware<AuthMiddleware>();
app.MapControllers();
app.Run();
