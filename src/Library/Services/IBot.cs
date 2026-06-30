using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Proyecto2026.Services
{
    /// <summary>
    /// La interfaz del Bot de Discord para usar con inyección de dependencias.
    /// </summary>
    public interface IBot
    {
        Task StartAsync(ServiceProvider services);

        Task StopAsync();
    }
}
