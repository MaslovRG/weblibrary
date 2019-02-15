using System;
using System.Net;
using System.Net.Http.Formatting;
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
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore; 
using BookService.Models;

namespace BookService
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
            services.AddDbContext<BooksContext>(options =>
                //options.UseSqlServer(Configuration.GetConnectionString("ConnectionDB"))); 
                options.UseInMemoryDatabase("Book")); 

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
