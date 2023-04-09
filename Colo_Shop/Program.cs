namespace Colo_Shop;

using Colo_Shop.IServices;
using Colo_Shop.Services;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();
        builder.Services.AddTransient<IProductServices, ProductServices>();
        builder.Services.AddTransient<IUserServices, UserServices>();
        builder.Services.AddTransient<IBillServices, BillServices>();
        builder.Services.AddTransient<ICartServices, CartServices>();
        builder.Services.AddTransient<IRoleServices, RoleServices>();
        builder.Services.AddTransient<CartDetailService, CartDetailService>();
        builder.Services.AddTransient<BillDetailsService, BillDetailsService>();

        // builder.Services.AddSingleton<IProductServices, ProductServices>();
        // builder.Services.AddScoped<IProductServices, ProductServices>();
        /*
         *AddSingleton : Neu service duoc khoi tao , no co the ton tai cho den khi vong doi cua ung dung ket thuc.
         *Neu cac request khac ma duoc trien khai thi dung lai chinh service do.Phu hop cho cac service co tinh toan cuc va khong thay doi
         *AddScoped    : moi lan co http request thi se tao ra service 1 lan va duoc giu nguyen trong qua trinh request duoc xu ly.Loai nay
         *se duoc su dung cho cac service voi nhung yeu cau http cu the
         *AddTransient : Tao moi service voi moi khi co request. Voi moi yeu cau http se nhan dc 1 doi tuong service khac nhau. phu hop cho cac
         * service ma co the phuc vu nhieu yeu cau http request.
         */
        builder.Services.AddSession(options => { options.IdleTimeout = TimeSpan.FromSeconds(300); });

        // Đăng ký Session với thời gian là 15 giây cho đến khi timeout
        var app = builder.Build(); // Các cấu hình phải viết trước dòng này

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");

            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseSession();
        app.UseRouting();

        app.UseAuthorization();

        app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}