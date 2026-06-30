using System;
using System.Collections.Generic;
using NUnit.Framework;
using Proyecto2026;

namespace Library.Tests
{
    /// <summary>
    /// HU7: Como usuario nuevo quiero recibir recomendaciones basadas en popularidad para tener una buena experiencia inicial.
    /// Criterios de aceptación:
    ///   - Si el usuario no tiene historial, se usa una estrategia alternativa (popularidad).
    ///   - Se muestran ítems populares o recientes.
    ///   - El sistema no falla ante ausencia de datos.
    /// </summary>
    [TestFixture]
    public class HU07Tests
    {
        private SystemFacade BuildFacade()
        {
            var catalog = new Catalog();
            var strategy = new PopularityStrategy();
            var filters = new List<IRecommendationFilter> { new ExcludeConsumedFilter() };
            var sorter = new PopularitySorter();
            var engine = new RecommendationEngine(strategy, filters, sorter);
            return new SystemFacade(catalog, engine);
        }


        [Test]
        public void GetRecommendations_UsuarioSinHistorial_RetornaItemsDelCatalogo()
        {
            // Si el usuario no tiene historial, se usa estrategia alternativa (popularidad).
            var facade = BuildFacade();
            var user = facade.RegisterUser("nuevo", "nuevo@gmail.com", "HashedPass");
            facade.AddMusic(new Music("Song 1", "Artist", "Album", "Rock", DateTime.Now, 85));

            var result = facade.GetRecommendations(user);

            Assert.IsTrue(result.Count > 0);
        }


        [Test]
        public void GetRecommendations_UsuarioSinHistorial_RetornaItemMasPopularPrimero()
        {
            // Se muestran ítems populares: el de mayor popularidad aparece primero.
            var facade = BuildFacade();
            var user = facade.RegisterUser("nuevo", "nuevo@gmail.com", "HashedPass");
            facade.AddMusic(new Music("Canción desconocida", "Artist", "Album", "Cumbia", DateTime.Now, 5));
            facade.AddMusic(new Music("Canción hit", "Artist", "Album", "Rock", DateTime.Now, 99));

            var result = facade.GetRecommendations(user);

            Assert.AreEqual("Canción hit", result[0].Title);
        }


        [Test]
        public void GetRecommendations_UsuarioSinHistorial_NoFalla()
        {
            // El sistema no falla ante ausencia de datos.
            var facade = BuildFacade();
            var user = facade.RegisterUser("nuevo", "nuevo@gmail.com", "HashedPass");
            facade.AddMusic(new Music("Song 2", "Artist", "Album", "Pop", DateTime.Now, 95));

            Assert.DoesNotThrow(() => facade.GetRecommendations(user));
        }

    }
}
