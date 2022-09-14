using com.tweetapp.DatabaseSettings;
using com.tweetapp.Interfaces;
using com.tweetapp.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace com.tweetapp
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
            services.Configure<UserDatabaseSettings>(Configuration.GetSection(nameof(UserDatabaseSettings)));

            services.AddSingleton<IUserDatabaseSettings>(sp => sp.GetRequiredService<IOptions<UserDatabaseSettings>>().Value);

            services.AddSingleton<UserRepository>();

            services.Configure<TweetDatabaseSettings>(Configuration.GetSection(nameof(TweetDatabaseSettings)));

            services.AddSingleton<ITweetDatabaseSettings>(sp => sp.GetRequiredService<IOptions<TweetDatabaseSettings>>().Value);

            services.Configure<ReplyTweetDatabaseSettings>(Configuration.GetSection(nameof(ReplyTweetDatabaseSettings)));

            services.AddSingleton<IReplyTweetDatabaseSettings>(sp => sp.GetRequiredService<IOptions<ReplyTweetDatabaseSettings>>().Value);

            services.AddSingleton<TweeterRepository>();

            services.AddControllers();

            services.AddSwaggerGen();

            services.AddCors();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(options =>
            options.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1.0");
            });
        }
    }
}
