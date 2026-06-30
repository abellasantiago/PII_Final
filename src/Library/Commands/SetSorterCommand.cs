using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using Proyecto2026;

namespace Proyecto2026.Commands
{
    /// <summary>
    /// Comando para cambiar el criterio de ordenamiento de las recomendaciones.
    /// </summary>
    public class SetSorterCommand : ModuleBase<SocketCommandContext>
    {
        /// <summary>
        /// Ejecuta el comando para cambiar el criterio de ordenamiento de las recomendaciones.
        /// </summary>
        /// <param name="criterio"></param>
        /// <returns></returns>
        [Command("setsorter")]
        [Summary("Cambia el criterio de orden de las recomendaciones. Opciones: popularity, newest.")]
        public async Task ExecuteAsync(
            [Remainder]
            [Summary("Criterio de ordenamiento: popularity o newest")]
            string criterio = null)
        {
            if (string.IsNullOrWhiteSpace(criterio))
            {
                await ReplyAsync("Indicá el criterio: !setsorter popularity  o  !setsorter newest");
                return;
            }

            criterio = criterio.Trim().ToLower();

            if (criterio == "popularity")
            {
                SystemFacade.Instance.SetRecommendationSorter(new PopularitySorter());
                await ReplyAsync("Criterio de orden cambiado a: popularidad.");
            }
            else if (criterio == "newest")
            {
                SystemFacade.Instance.SetRecommendationSorter(new NewestSorter());
                await ReplyAsync("Criterio de orden cambiado a: más reciente.");
            }
            else
            {
                await ReplyAsync("Criterio no reconocido. Las opciones son: popularity, newest.");
            }
        }
    }
}