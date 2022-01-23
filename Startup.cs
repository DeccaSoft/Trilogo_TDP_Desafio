using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Treinando.Data;
using Treinando.Controllers;
using Treinando.Models;
using System.Text;
//using System.Text.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Aula6.Services;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;

namespace Treinando
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        
        public void ConfigureServices(IServiceCollection services)  //Método utilizado para se adicionar Serviços de Terceiros e Injeção de Dependências
        {
            services.Configure<ApiBehaviorOptions>(options => 
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            //Autenticação pro JWT
            var key = Encoding.ASCII.GetBytes(Configuration.GetSection("Jwt:Secret").Value);    //Chave

            //Configuração das Validações (Autenticação tipo JWT)
            services.AddAuthentication(o =>
                {                                           
                    o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                }).AddJwtBearer(o =>                                                            //Como será a Validação
                {
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });


            //Container de Injeção de Dependências (Service Provider)
            services.AddScoped(provider => 
            {
                var optionsBuilder = new DbContextOptionsBuilder<DBContext>()
                    .UseMySql(Configuration.GetConnectionString("DefaultConnection"), new MySqlServerVersion(new Version(8, 0, 27)));
                var context = new DBContext(optionsBuilder.Options); 
                context.Database.EnsureCreated();
                return context;
            });

            services.AddControllers();
            //.AddNewtonsoftJson(o => {o.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;});    //Utilizado até a verão 3.1
            services.AddControllers().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            services.AddControllers().AddNewtonsoftJson(o => {o.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;});     //Não Retorna os Nulos


            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo {Title = "Aula6", Version = "v1"}); });

            // Cilcos de Vida
            //services.AddScoped<>();    => Um objeto por Requisição (request / response)
            //services.AddSingleton<>(); => Um único objeto por aplicação
            //services.AddTransient<>(); => Novo objeto criado toda vez

            //services.AddScoped<DBContext>();
            services.AddScoped<UserServices>();
            services.AddScoped<ProductServices>();
            services.AddScoped<OrderServices>();
            services.AddScoped<AuthenticationServices>();
            services.AddScoped<ReportServices>();
            services.AddScoped<AddressServices>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) //Método que define COMO eu quero que a aplicação de comporte
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Aula6 v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}