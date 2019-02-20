using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using ReaderService.Models;
using System.Diagnostics.CodeAnalysis; 

namespace ReaderService
{
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        [ExcludeFromCodeCoverage]
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        [ExcludeFromCodeCoverage]
        public IConfiguration Configuration { get; }

        [ExcludeFromCodeCoverage]
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<TokensContext>(options =>
                options.UseInMemoryDatabase("Token")); 

            services.AddDbContext<ReadersContext>(options =>
                //options.UseSqlServer(Configuration.GetConnectionString("ConnectionDB")));
                options.UseInMemoryDatabase("Reader")); 

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        [ExcludeFromCodeCoverage]
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
