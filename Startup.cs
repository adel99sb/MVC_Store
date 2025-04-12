using Ali_Store.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Ali_Store
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddSession();
            services.AddDbContext<StoreContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage(); // shows detailed errors
            }
            else
            {
                app.UseExceptionHandler("/Home/Error"); // fallback for production
                app.UseHsts();
            }
            //    app.UseStaticFiles(new StaticFileOptions
            //    {
            //        FileProvider = new PhysicalFileProvider(
            //Path.Combine(Directory.GetCurrentDirectory(), "Image")),
            //        RequestPath = "/Image"
            //    });
            app.UseStaticFiles();
            app.UseSession();
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
                        
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=_User}/{action=Login}/{id?}");
            });
        }
    }
}
