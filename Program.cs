using Doan.Models;
using Doan.service;
using Doan.service.Llm;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OpenAI.Chat;
using SixLabors.ImageSharp.Formats;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<DoanContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
// C?u hình Session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);  // Th?i gian h?t h?n session
    options.Cookie.HttpOnly = true;  // Ch? có th? truy c?p qua HTTP
    options.Cookie.IsEssential = true;  // Cookie c?n thi?t
});

// Add services to the container.
if (!builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>(optional: true);

}
var configuration = builder.Configuration.Get<Config>()?? new Config();
builder.Services.AddSingleton(configuration);
//register open AI
if (configuration.Provider == "OpenAI")
{ 
builder.Services.AddScoped<ILlmChatProvider, OpenAIChatProvider>();
    builder.Services.AddSingleton(new ChatClient(configuration.OpenAI.ChatModel, configuration.OpenAI.ApiKey));
}
else
{
    builder.Services.AddScoped<ILlmChatProvider, OllamaChatProvider>();
}
builder.Services.AddScoped<RagPipeline>();



// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
// ngân hàng 
builder.Services.AddSingleton<Doan.Payments.VNPayService>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();

    var tmnCode = config["VNPay:TmnCode"];
    var hashSecret = config["VNPay:HashSecret"];
    var paymentUrl = config["VNPay:PaymentUrl"];
    var returnUrl = config["VNPay:ReturnUrl"];

    return new Doan.Payments.VNPayService(
        tmnCode,
        hashSecret,
        paymentUrl,
        returnUrl
    );
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseSession();  // **B?t session ?? theo dõi gi? hàng**
app.UseAuthorization();
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Home}/{id?}"
    );

app.MapStaticAssets();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapControllers();

app.Run();
