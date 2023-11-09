using Microsoft.AspNetCore.Mvc.Razor;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
	options.Cookie.Name = ".Session";
	options.IdleTimeout = TimeSpan.FromMinutes(10);
	options.Cookie.HttpOnly = true;
});

builder.Services.Configure<CookiePolicyOptions>(
 options =>
 {
	 options.CheckConsentNeeded = context => false;
	 options.MinimumSameSitePolicy = SameSiteMode.None;
 });




builder.Services.AddLocalization();
builder.Services.AddLocalization(o => { o.ResourcesPath = "Resources"; });

// set the default culture and supported culture list
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
	options.SetDefaultCulture("pl-PL");
	options.AddSupportedUICultures("en-US", "pl-PL");
	options.FallBackToParentUICultures = true;
	options.RequestCultureProviders.Clear();
});

builder.Services.AddControllersWithViews()
	.AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
	.AddDataAnnotationsLocalization();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.Use(async (ctx, next) =>
{
	await next();
	if (ctx.Response.StatusCode == 404 && !ctx.Response.HasStarted)
	{
		ctx.Request.Path = "/Error/404";
		await next();
	}
});

app.UseRequestLocalization();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
