using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using SecurityCourseIntial.Repositories;

var builder = WebApplication.CreateBuilder(args);


// limit the maximum file upload size to 1MB

builder.Services.Configure<FormOptions>(options =>
{
    options.ValueLengthLimit = int.MaxValue;
    options.MultipartBodyLengthLimit = 1 * 1024 * 1024;
    options.MultipartHeadersLengthLimit = int.MaxValue;
});



// Add services to the container.
builder.Services.AddControllersWithViews();


var connectionString = "Server=tcp:database.windows.net,1433;Initial Catalog=Northwind;Persist Security Info=False;User ID=;Password=!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";


builder.Services.AddScoped<UserCommentRepository>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    return new UserCommentRepository(connectionString);
});

builder.Services.AddHttpClient();

// enable sessions
// session variable will be used to determine if you are logged in.
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = false;
    options.Cookie.IsEssential = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.Strict;
});

// enable HttpContext from a view
builder.Services.AddHttpContextAccessor();

builder.Configuration.AddUserSecrets<Program>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
/*
// Custom middleware to handle large file uploads
app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (BadHttpRequestException ex) when (ex.StatusCode == 413)
    {
        context.Response.Redirect("/error/filetoolarge");
    }
});

// Map the error page for large files
app.MapGet("/error/filetoolarge", () =>
{
    return Results.Content("The uploaded file is too large. Please upload a smaller file.");
});
*/

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSession();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
