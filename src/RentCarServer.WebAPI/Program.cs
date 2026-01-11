using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.RateLimiting;
using RentCarServer.Application;
using RentCarServer.Infrastructure;
using RentCarServer.WebAPI;
using RentCarServer.WebAPI.Controllers;
using RentCarServer.WebAPI.Middlewares;
using RentCarServer.WebAPI.Module;
using RentCarServer.WebAPI.Modules;
using Scalar.AspNetCore;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddAplication();   //Application katmanýný ekliyoruz
builder.Services.AddInfrastructure(builder.Configuration); //Infrastructure katmanýný ekliyoruz buradan builder.Configuration ý geçiriyoruz ki database baðlantý stringi vs. ayarlarý alabilelim.
builder.Services.AddRateLimiter(cfr =>
{
    cfr.AddFixedWindowLimiter("fixed", opt =>
    {
        opt.PermitLimit = 100;
        opt.QueueLimit = 100;
        opt.Window = TimeSpan.FromSeconds(1);
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    });
    cfr.AddFixedWindowLimiter("login-fixed", opt =>
    {
        opt.PermitLimit = 5;
        opt.Window = TimeSpan.FromMinutes(1);
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    });
    cfr.AddFixedWindowLimiter("forgot-password-fixed", opt =>
    {
        opt.PermitLimit = 2;
        opt.Window = TimeSpan.FromMinutes(5);
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    });
    cfr.AddFixedWindowLimiter("reset-password-fixed", opt =>
    {
        opt.PermitLimit = 3;
        opt.Window = TimeSpan.FromMinutes(1);
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    });
    cfr.AddFixedWindowLimiter("check-forgot-password-code-fixed", opt =>
    {
        opt.PermitLimit = 2;
        opt.Window = TimeSpan.FromMinutes(1);
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    });
}); //Rate Limiter ekliyoruz saniyede 100 istek hakký veriyoruz her kullanýcýya. Ama login için saniyede 5 istek hakký veriyoruz ve 1 tane sýraya atýp 1 dakika sonra cevap veriyoruz.
    //Sebep botlar sürekli istek atýk sistemi itlemesin ve login bilgilerini denem yanýlmayla bulamasýn diye.

builder.Services
            .AddControllers()
            .AddOData(opt =>
            opt.Select()
                .Filter()
                .Count()
                .Expand()
                .OrderBy()
                .SetMaxTop(null)
                .AddRouteComponents("odata", MainODataController.GetEdmModel())
            ); //OData ekliyoruz. OData ile API lere geliþmiþ sorgulama özellikleri ekleyebiliyoruz. Mesela ?$filter=Name eq 'John' gibi sorgular atabiliyoruz.

builder.Services.AddCors(); //CORS ekliyoruz ki farklý domainlerden istek atýlabilsin. Örneðin frontend uygulamamýz farklý bir domain de host ediliyorsa CORS olmadan istek atamayýz.
builder.Services.AddOpenApi("v1", options => { options.AddDocumentTransformer<BearerSecuritySchemeTransformer>(); }); //OpenAPI dokümantasyonu ekliyoruz Swagger yerine bunu kullanýyoruz.
builder.Services.AddExceptionHandler<ExceptionHandler>().AddProblemDetails();  //Dönen Hata mesajlarýnýn gözükmesini istediðimiz þekil için yazdýðýmýz Exception Handler ý ekliyoruz.
builder.Services.AddResponseCompression(opt =>
{
    opt.EnableForHttps = true;
}); //responslarý sýkýþtýrarark bant geniþliði tasarrufu saðlar çünkü response un boyutu düþet 1 mb lýk response u 300kb a kadar sýkýþtýrýr.


builder.Services.AddTransient<CheckTokenMiddleware>(); //CheckTokenMiddleware ini ekliyoruz ki her istek atýldýðýnda token ýn geçerliliðini kontrol edebilelim.
builder.Services.AddHostedService<CheckLoginTokenBackgroundService>(); // günde bir kere çalýþan deactive olmuþ tokenlarý silen servis.





var app = builder.Build(); //Uygulamayý oluþturuyoruz.

app.MapOpenApi(); //OpenAPI dokümantasyonunu etkinleþtirir Swagger yerine bunu kullanýyoruz.
app.MapScalarApiReference(); //Scalar API dokümantasyonunu etkinleþtirir
app.UseHttpsRedirection(); //HTTPS yönlendirmesi yapar. Yani HTTP isteklerini otomatik olarak HTTPS ye yönlendirir.
app.UseCors(policy =>
{
    policy.AllowAnyOrigin()
          .AllowAnyHeader()
          .AllowAnyMethod()
          .SetPreflightMaxAge(TimeSpan.FromHours(10)); //CORS politikasýný deneklemek için 10 dakika önbelleðe alýr
}); //CORS politikasýný ayarlýyoruz. Herhangi bir origin den, herhangi bir header ile, herhangi bir method ile istek atýlmasýna izin veriyoruz.
    //Geliþtirme aþamasýnda bu þekilde açýyoruz ama üretim ortamýnda daha kýsýtlayýcý olmalýyýz.

app.UseResponseCompression();  //responslarý sýkýþtýrarark bant geniþliði tasarrufu saðlar çünkü response un boyutu düþet 1 mb lýk response u 300kb a kadar sýkýþtýrýr.
app.UseAuthentication(); // Kimlik doðrulama middleware ini ekliyoruz.
app.UseAuthorization(); // Yetkilendirme middleware ini ekliyoruz.
app.UseExceptionHandler(); //Exception Handler middleware ini ekliyoruz.
app.UseMiddleware<CheckTokenMiddleware>(); //CheckTokenMiddleware ini ekliyoruz ki her istek atýldýðýnda token ýn geçerliliðini kontrol edebilelim.
app.UseRateLimiter(); //Rate Limiter middleware ini ekliyoruz.



app.MapControllers().RequireRateLimiting("fixed").RequireAuthorization(); //Tüm controller lara rate limiting ve authorization ekliyoruz.
                                                                          //Yani tüm endpoint lere saniyede 100 istek hakký veriyoruz ve yetkilendirme istiyoruz.
app.MapAuth(); //Auth module ü ekliyoruz buraya. end pointler için
app.MapBranch(); //Auth module ü ekliyoruz buraya. end pointler için
app.MapRole(); //Role module ü ekliyoruz buraya. end pointler için
app.MapPermission(); //Permission module ü ekliyoruz buraya. end pointler için
app.MapUser(); //user end pointlerini ekliyoruz. modul olarak yazdýklarýmýzý
app.MapCategory(); //category end pointlerini ekliyoruz. modul olarak yazdýklarýmýzý


app.MapGet("/", () => "Hello world").RequireAuthorization();



//await app.CreateFirstUser();
await app.CleanRemovedPermissionsFromRoleAsync();

app.Run();
