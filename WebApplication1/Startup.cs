using Autofac;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OnlineChat.Hubs;
using OnlineChat.Models;
using OnlineChat.Models.Chats;
using OnlineChat.Models.Messages;
using OnlineChat.Models.Users;
using OnlineChat.Models.Lecturer;
using OnlineChat.Models.News;

namespace OnlineChat
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
			services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
			   .AddCookie(options => //CookieAuthenticationOptions
				{
				   options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
			   });
			services.AddControllersWithViews();
			services.AddSignalR();
		}
		public void ConfigureContainer(ContainerBuilder builder)
		{

			builder.RegisterType<UserDAO>();
			builder.RegisterType<ChatDAO>();
			builder.RegisterType<MessageDAO>();
			builder.RegisterType<LecturerRepository>();
			builder.RegisterType<WallMessageRepository>();
			builder.RegisterType<NewsRepository>();
			builder.RegisterType<AESCrypt>();
			builder.RegisterType<LogFactory>();
			var connectionString = Configuration.GetConnectionString("DefaultConnection");
			builder.Register(context => new SqlDbConnectionFactory(connectionString)).As<IDbConnectionFactory>();
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
				//app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}
			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthentication();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Home}/{action=Index}/{id?}");
				endpoints.MapHub<ChatHub>("/chatHub");
			});
		}
	}
}
