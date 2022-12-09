using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shop.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject1
{
    public class BaseApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder
                .UseEnvironment("Testing")
                .ConfigureServices(services => 
                {
                    var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<DataContext>));
                    services.Remove(descriptor);

                    services.AddDbContext<DataContext>(option => option.UseInMemoryDatabase(databaseName: "ShopDbTest"));
                });
        }
    }
}
