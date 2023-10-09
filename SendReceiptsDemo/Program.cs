using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using SendMeetingsDemo.Data.Repositories;
using SendReceiptsDemo.Data;
using SendReceiptsDemo.Data.Repositories;
using SendReceiptsDemo.Data.Services;
using System.Security.Claims;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();

#region Db Context
//intro dbcontext to Core service for work with database
builder.Services.AddDbContext<SendReceiptContext>(options =>
{
    //options.UseSqlServer("Data Source=.;Initial Catalog=SendOnlyMeetingsDB;Integrated Security=true");
    options.UseSqlServer(@"Server=.;Initial Catalog=rajabije_test;User ID=rajabije_test2;Password=460k0?msV;MultipleActiveResultSets=true");
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

});

#endregion
// next step: add migration for create sql server database from code

#region IoC

builder.Services.AddScoped<IMeetingRep, MeetingRep>();
builder.Services.AddScoped<ICustomerRep, CustomerRep>();
builder.Services.AddScoped<IBankAccountRep, BankAccountRep>();
builder.Services.AddScoped<IAdminRep, AdminRep>();
builder.Services.AddScoped<IRightRep, RightRep>();
builder.Services.AddScoped<IMessageRep, MessageRep>();
builder.Services.AddScoped<INotificationRep, NotificationRep>();
builder.Services.AddScoped<IContentRep, ContentRep>();
builder.Services.AddScoped<ITokenRep, TokenRep>();
#endregion

#region Authentication

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(option =>
    {
        option.LoginPath = "/Account/Login";
        option.LogoutPath = "/Account/Logout";
        option.ExpireTimeSpan = TimeSpan.FromDays(10);
    });
#endregion

builder.Services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.Use(async (context, next) =>
{
    // Do work that doesn't write to the Response.
    if (context.Request.Path.StartsWithSegments("/pics"))
    {
        if (!context.User.Identity.IsAuthenticated)
        {
            context.Response.Redirect("/Home/Error");
        }
    }
    if (context.Request.Path.StartsWithSegments("/Admin/AddAdmin") || context.Request.Path.StartsWithSegments("/Admin/EditAdmin") || context.Request.Path.StartsWithSegments("/Admin/DeleteAdmin"))
    {
        if (context.User.FindFirstValue("AdminType")?.ToString() != "مدیر ارشد")
        {
            context.Response.Redirect("/Admin/ShowAdmins");
        }
    }
    if (context.Request.Path.StartsWithSegments("/Admin/AddRight") || context.Request.Path.StartsWithSegments("/Admin/DeleteRight"))
    {
        if (context.User.FindFirstValue("AdminType")?.ToString() != "مدیر ارشد" && context.User.FindFirstValue("AdminType")?.ToString() != "مدیر مشتری")
        {
            context.Response.Redirect("/Admin/ShowRights");
        }
    }
    //if (context.Request.Path.StartsWithSegments("/Admin/MakeMessage") || context.Request.Path.StartsWithSegments("/Admin/DeleteMessage"))
    //{
    //    if (context.User.FindFirstValue("AdminType")?.ToString() != "مدیر ارشد")
    //    {
    //        context.Response.Redirect("/Admin/ShowMessages");
    //    }
    //}
    if (context.Request.Path.StartsWithSegments("/Admin/AddCustomer") || context.Request.Path.StartsWithSegments("/Admin/AddCustomersGroup") || context.Request.Path.StartsWithSegments("/Admin/EditCustomer") || context.Request.Path.StartsWithSegments("/Admin/DeleteCustomer"))
    {
        if (context.User.FindFirstValue("AdminType")?.ToString() != "مدیر ارشد" && context.User.FindFirstValue("AdminType")?.ToString() != "مدیر مشتری")
        {
            context.Response.Redirect("/Admin/");
        }
    }
    if (context.Request.Path.StartsWithSegments("/Admin/AddAccount") || context.Request.Path.StartsWithSegments("/Admin/EditAccount") || context.Request.Path.StartsWithSegments("/Admin/DeleteAccount"))
    {
        if (context.User.FindFirstValue("AdminType")?.ToString() != "مدیر ارشد" && context.User.FindFirstValue("AdminType")?.ToString() != "مدیر مشتری")
        {
            context.Response.Redirect("/Admin/ShowAccounts");
        }
    }

    if (context.Request.Path.StartsWithSegments("/Admin/DeleteMeeting"))
    {
        if (context.User.FindFirstValue("AdminType")?.ToString() != "مدیر ارشد")
        {
            context.Response.Redirect("/Admin/ShowMeetings");
        }
    }
    if (context.Request.Path.StartsWithSegments("/Admin/ShowNewMeetings") || context.Request.Path.StartsWithSegments("/Admin/ShowOldMeetings") || context.Request.Path.StartsWithSegments("/Admin/DeleteMeeting") || context.Request.Path.StartsWithSegments("/Admin/CheckMeeting") || context.Request.Path.StartsWithSegments("/Admin/PrintRequest") || context.Request.Path.StartsWithSegments("/Admin/PrintScore"))
    {
        if (context.User.FindFirstValue("AdminType")?.ToString() == "مدیر مشتری")
        {
            context.Response.Redirect("/Admin/");
        }
    }

    await next.Invoke();
    // Do logging or other work that doesn't write to the Response.
});

app.UseDirectoryBrowser(new DirectoryBrowserOptions()
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "pics")),
    RequestPath = "/pics"

});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Enter}/{id?}");

app.Run();