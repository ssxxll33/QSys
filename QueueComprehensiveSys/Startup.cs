using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FineUICore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace QueueComprehensiveSys
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
            services.AddDistributedMemoryCache();
            services.AddSession();

            // ���������������
            services.Configure<FormOptions>(x =>
            {
                x.ValueCountLimit = 1024;   // ��������ĸ������ƣ�Ĭ��ֵ��1024��
                x.ValueLengthLimit = 4194304;   // �����������ֵ�ĳ������ƣ�Ĭ��ֵ��4194304 = 1024 * 1024 * 4��
            });

            // FineUI ����
            services.AddFineUI(Configuration);

            services.AddControllersWithViews().AddMvcOptions(options =>
            {
                // �Զ���ģ�Ͱ󶨣�Newtonsoft.Json��
                options.ModelBinderProviders.Insert(0, new JsonModelBinderProvider());
            }).AddNewtonsoftJson(); 
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();
           
            app.UseRouting();
             
            app.UseWebSockets(); 

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSession();
            // FineUI �м����ȷ�� UseFineUI λ�� UseEndpoints ��ǰ�棩
            app.UseFineUI();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                   name: "area",
                   pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                 );
            });
        }
    }
}
