using System;
using System.Collections.Generic;
using NUnit.Framework;
using Proyecto2026;

namespace Library.Tests
{
    /// <summary>
    /// HU4: Como usuario quiero recibir recomendaciones basadas en mi historial para descubrir contenido relevante.
    /// Criterios de aceptación:
    ///   - El sistema utiliza interacciones previas del usuario.
    ///   - Se generan recomendaciones distintas al contenido ya consumido.
    ///   - Cambios en el historial afectan las recomendaciones.
    /// </summary>
    [TestFixture]
    public class HU04Tests
    {
        // BasedOnHistory recomienda ítems con tags similares a los consumidos.
        // ExcludeConsumedFilter asegura que lo ya escuchado no vuelva a aparecer.

        private SystemFacade BuildFacade()
        {
            var catalog = new Catalog();
            var strategy = new BasedOnHistory();
            var filters = new List<IRecommendationFilter> { new ExcludeConsumedFilter() };
            var sorter = new PopularitySorter();
            var engine = new RecommendationEngine(strategy, filters, sorter);
            return new SystemFacade(catalog, engine);
        }


        [Test]
        public void GetRecommendations_UsaInteraccionesPrevias_RetornaItemsRelacionados()
        {
            // El sistema utiliza interacciones previas del usuario.
            var facade = BuildFacade();
            var user = facade.RegisterUser("geronimo", "gero@gmail.com", "HashedPass");

            var rock1 = new Music("Rock consumido", "Artist", "Album", "Rock", DateTime.Now, 80);
            var rock2 = new Music("Otro Rock", "Artist", "Album", "Rock", DateTime.Now, 70);
            facade.AddMusic(rock1);
            facade.AddMusic(rock2);

            // Consumió rock1, debería recibir rock2 (mismo género/tag)
            facade.RegisterInteraction(user, rock1, InteractionType.Played);
            var result = facade.GetRecommendations(user);

            Assert.IsTrue(result.Count > 0);
            Assert.IsTrue(result.Exists(i => i.Title == "Otro Rock"));
        }


        [Test]
        public void GetRecommendations_NoRecomendaContenidoYaConsumido()
        {
            // Se generan recomendaciones distintas al contenido ya consumido.
            var facade = BuildFacade();
            var user = facade.RegisterUser("geronimo", "gero@gmail.com", "HashedPass");

            var cancion = new Music("Cancion consumida", "Artist", "Album", "Rock", DateTime.Now, 80);
            facade.AddMusic(cancion);

            facade.RegisterInteraction(user, cancion, InteractionType.Played);
            var result = facade.GetRecommendations(user);

            Assert.IsFalse(result.Exists(i => i.Title == "Cancion consumida"));
        }


        [Test]
        public void GetRecommendations_CambiosEnHistorialAfectanResultados()
        {
            // Cambios en el historial afectan las recomendaciones.
            var facade = BuildFacade();
            var user = facade.RegisterUser("rodrigo", "rodri@gmail.com", "HashedPass");

            var rock = new Music("Rock Song", "Artist", "Album", "Rock", DateTime.Now, 80);
            var pop = new Music("Pop Song", "Artist", "Album", "Pop", DateTime.Now, 70);
            var jazz = new Music("Jazz Song", "Artist", "Album", "Jazz", DateTime.Now, 60);
            facade.AddMusic(rock);
            facade.AddMusic(pop);
            facade.AddMusic(jazz);

            // Sin historial, BasedOnHistory devuelve todo
            var sinHistorial = facade.GetRecommendations(user);

            // Después de consumir Rock, solo recomienda Rock
            facade.RegisterInteraction(user, rock, InteractionType.Played);
            var conHistorial = facade.GetRecommendations(user);

            Assert.AreNotEqual(sinHistorial.Count, conHistorial.Count);
        }
    }
}
