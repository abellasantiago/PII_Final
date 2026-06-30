using System;
using System.Collections.Generic;
using NUnit.Framework;
using Proyecto2026;

namespace Library.Tests
{
    /// <summary>
    /// HU11: Como usuario quiero marcar contenido como me gusta o no me gusta para mejorar mis recomendaciones.
    /// Criterios de aceptación:
    ///   - El usuario puede marcar un ítem como me gusta o no me gusta.
    ///   - Esa información afecta recomendaciones futuras.
    ///   - El sistema diferencia entre consumo y valoración.
    /// </summary>
    [TestFixture]
    public class HU11Tests
    {
        private SystemFacade BuildFacade(IRecommendationStrategy strategy, List<IRecommendationFilter> filters)
        {
            var catalog = new Catalog();
            var sorter = new PopularitySorter();
            var engine = new RecommendationEngine(strategy, filters, sorter);
            return SystemFacade.Instance;
        }

        [Test]
        public void LikeContent_YDislikeContent_RegistranValoraciones()
        {
            var facade = BuildFacade(new PopularityStrategy(), new List<IRecommendationFilter>());
            var user = facade.RegisterUser("usuario", "usuario@mail.com", "hash123");
            var liked = new Music("Me gusta", "Artist", "Album", "Rock", DateTime.Now, 80);
            var disliked = new Music("No me gusta", "Artist", "Album", "Reggaeton", DateTime.Now, 70);

            facade.LikeContent(user, liked);
            facade.DislikeContent(user, disliked);

            Assert.That(user.History.Count, Is.EqualTo(2));
            Assert.That(user.History[0].Type, Is.EqualTo(InteractionType.Liked));
            Assert.That(user.History[1].Type, Is.EqualTo(InteractionType.Disliked));
        }

        [Test]
        public void DislikeContent_AfectaRecomendacionesFuturas()
        {
            var filters = new List<IRecommendationFilter> { new ExcludeTagsFilter() };
            var facade = BuildFacade(new PopularityStrategy(), filters);
            var user = facade.RegisterUser("usuario2", "usuario2@mail.com", "hash123");
            var reggaeton = new Music("Reggaeton", "Artist", "Album", "Reggaeton", DateTime.Now, 100);
            var rock = new Music("Rock", "Artist", "Album", "Rock", DateTime.Now, 50);
            facade.AddMusic(reggaeton);
            facade.AddMusic(rock);

            facade.DislikeContent(user, reggaeton);
            var result = facade.GetRecommendationsByPreferences(user);

            Assert.IsFalse(result.Exists(item => item.Title == "Reggaeton"));
            Assert.IsTrue(result.Exists(item => item.Title == "Rock"));
        }

        [Test]
        public void LikeContent_NoCuentaComoContenidoConsumido()
        {
            var facade = BuildFacade(new PopularityStrategy(), new List<IRecommendationFilter>());
            var user = facade.RegisterUser("usuario3", "usuario3@mail.com", "hash123");
            var liked = new Music("Me gusta", "Artist", "Album", "Rock", DateTime.Now, 80);

            facade.LikeContent(user, liked);
            var history = facade.GetConsumedHistory(user);

            Assert.That(history.Count, Is.EqualTo(0));
        }
    }
}
