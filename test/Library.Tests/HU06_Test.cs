using System;
using System.Collections.Generic;
using NUnit.Framework;
using Proyecto2026;

namespace Library.Tests
{
    /// <summary>
    /// HU6: Como usuario quiero recibir recomendaciones basadas en usuarios similares para descubrir contenido nuevo.
    /// Criterios de aceptación:
    ///   - El sistema identifica usuarios con comportamiento similar.
    ///   - Se recomiendan ítems consumidos por usuarios similares.
    ///   - No se incluyen ítems ya consumidos por el usuario.
    /// </summary>
    [TestFixture]
    public class HU06Tests
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
        public void GetRecommendationsFromSimilarUsers_IdentificaUsuariosConComportamientoSimilar()
        {
            var facade = BuildFacade();
            var user = facade.RegisterUser("usuario", "usuario@mail.com", "hash123");
            var similar = facade.RegisterUser("similar", "similar@mail.com", "hash123");
            var diferente = facade.RegisterUser("diferente", "diferente@mail.com", "hash123");

            var rockBase = new Music("Rock base", "Artist", "Album", "Rock", DateTime.Now, 90);
            var recomendado = new Music("Recomendado similar", "Artist", "Album", "Jazz", DateTime.Now, 80);
            var noRelacionado = new Music("No relacionado", "Artist", "Album", "Pop", DateTime.Now, 70);
            facade.AddMusic(rockBase);
            facade.AddMusic(recomendado);
            facade.AddMusic(noRelacionado);

            facade.RegisterInteraction(user, rockBase, InteractionType.Played);
            facade.RegisterInteraction(similar, rockBase, InteractionType.Played);
            facade.RegisterInteraction(similar, recomendado, InteractionType.Played);
            facade.RegisterInteraction(diferente, noRelacionado, InteractionType.Played);

            var result = facade.GetRecommendationsFromSimilarUsers(user);

            Assert.IsTrue(result.Exists(item => item.Title == "Recomendado similar"));
            Assert.IsFalse(result.Exists(item => item.Title == "No relacionado"));
        }

        [Test]
        public void GetRecommendationsFromSimilarUsers_RecomiendaItemsConsumidosPorUsuariosSimilares()
        {
            var facade = BuildFacade();
            var user = facade.RegisterUser("usuario", "usuario@mail.com", "hash123");
            var similar = facade.RegisterUser("similar", "similar@mail.com", "hash123");

            var rockBase = new Music("Rock base", "Artist", "Album", "Rock", DateTime.Now, 90);
            var recomendado = new Music("Cancion recomendada", "Artist", "Album", "Indie", DateTime.Now, 85);
            facade.AddMusic(rockBase);
            facade.AddMusic(recomendado);

            facade.RegisterInteraction(user, rockBase, InteractionType.Played);
            facade.RegisterInteraction(similar, rockBase, InteractionType.Played);
            facade.RegisterInteraction(similar, recomendado, InteractionType.Played);

            var result = facade.GetRecommendationsFromSimilarUsers(user);

            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].Title, Is.EqualTo("Cancion recomendada"));
        }

        [Test]
        public void GetRecommendationsFromSimilarUsers_NoIncluyeItemsYaConsumidos()
        {
            var facade = BuildFacade();
            var user = facade.RegisterUser("usuario", "usuario@mail.com", "hash123");
            var similar = facade.RegisterUser("similar", "similar@mail.com", "hash123");

            var rockBase = new Music("Rock base", "Artist", "Album", "Rock", DateTime.Now, 90);
            var recomendado = new Music("Nueva recomendacion", "Artist", "Album", "Rock", DateTime.Now, 80);
            facade.AddMusic(rockBase);
            facade.AddMusic(recomendado);

            facade.RegisterInteraction(user, rockBase, InteractionType.Played);
            facade.RegisterInteraction(similar, rockBase, InteractionType.Played);
            facade.RegisterInteraction(similar, recomendado, InteractionType.Played);

            var result = facade.GetRecommendationsFromSimilarUsers(user);

            Assert.IsFalse(result.Exists(item => item.Title == "Rock base"));
            Assert.IsTrue(result.Exists(item => item.Title == "Nueva recomendacion"));
        }
    }
}
