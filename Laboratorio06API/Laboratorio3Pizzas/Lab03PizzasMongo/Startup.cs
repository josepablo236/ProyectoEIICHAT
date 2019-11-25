using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lab03PizzasMongo.Models;
using Lab03PizzasMongo.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Lab03PizzasMongo
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
            services.Configure<PizzaDatabaseSettings>(Configuration.GetSection(nameof(PizzaDatabaseSettings)));
            services.AddSingleton<IPizzaDatabaseSettings>(sp => sp.GetRequiredService<IOptions<PizzaDatabaseSettings>>().Value);
            services.AddSingleton<PizzaService>();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
