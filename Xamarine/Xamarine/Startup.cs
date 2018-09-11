using System.Reflection;
using System.Threading.Tasks;
using Xamarine.Hosting;
using Xamarine.Hosting.Modules;

namespace Xamarine
{
    public class Startup
    {
        public static async Task Initialize()
        {
            await new WebServer(9696)
                  .WithModule(new ResourceFilesModule(Assembly.GetAssembly(typeof(Startup)), "wwwroot"))
                  .RunAsync();
        }
    }
}
