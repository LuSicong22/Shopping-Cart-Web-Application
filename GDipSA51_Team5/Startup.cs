using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using GDipSA51_Team5.Data;
using GDipSA51_Team5.Models;

namespace GDipSA51_Team5
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            // add our database context into DI container
            services.AddSingleton<CartItems>();

            services.AddDbContext<Team5_Db>(opt =>
                 opt.UseLazyLoadingProxies().UseSqlServer(
                Configuration.GetConnectionString("DbConn")
                 ));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, Team5_Db db)
        {

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Gallery}/{action=Index}/{id?}");
            });

            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            SeedData seed = new SeedData(db);
            seed.Init();
        }
    }
}
