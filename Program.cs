using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MVCPrototipo.DataTest;

namespace MVCPrototipo
{
    public class Program
    {
        public static void Main(string[] args)
        {
           var hostserver = CreateHostBuilder(args).Build();
            using(var ambiente = hostserver.Services.CreateScope()){
                var services = ambiente.ServiceProvider;

                try{
                    var context =  services.GetRequiredService<ContextoCurso>();
                    context.Database.Migrate();
                    ContextoDataCursoLoad.InsertarData(context).Wait();
                }catch(Exception e){
                    var logging = services.GetRequiredService<ILogger<Program>>();
                    logging.LogError(e, "Ocurrio un error en la migracion");
                }
            }
            hostserver.Run();

        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
