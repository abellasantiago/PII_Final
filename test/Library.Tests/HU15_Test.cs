using System;
using System.Collections.Generic;
using NUnit.Framework;
using Proyecto2026;

namespace Library.Tests
{
    /// <summary>
    /// HU15: Como usuario quiero recibir recomendaciones de contenido relacionado a uno que ya consumí.
    /// Criterios de aceptación:
    ///   - El sistema identifica atributos del ítem base.
    ///   - Se recomiendan ítems similares.
    ///   - El resultado cambia según el ítem seleccionado.
    /// </summary>
    [TestFixture]
    public class HU15Tests
    {
        private SystemFacade BuildFacade()
        {
            var catalog = new Catalog();
            var strategy = new PopularityStrategy();
            var filters = new List<IRecommendationFilter> { new ExcludeConsumedFilter() };
            var sorter = new PopularitySorter();
            var engine = new RecommendationEngine(strategy, filters, sorter);
            return SystemFacade.Instance;
        }

        [Test]
        public void GetRelatedRecommendations_IdentificaAtributosDelItemBase()
        {
            var facade = BuildFacade();
            var user = facade.RegisterUser("user", "user@mail.com", "hash123");
            var baseSong = new Music("Base", "Artist A", "Album", "Rock", DateTime.Now, 80);
            var related = new Music("Related", "Artist A", "Album", "Pop", DateTime.Now, 70);
            var unrelated = new Music("Unrelated", "Artist B", "Album", "Rock", DateTime.Now, 90);
            facade.AddMusic(baseSong);
            facade.AddMusic(related);
            facade.AddMusic(unrelated);
            facade.RegisterInteraction(user, baseSong, InteractionType.Played);

            var result = facade.GetSameArtistRecommendations(user, baseSong);

            Assert.IsTrue(result.Exists(item => item.Title == "Related"));
            Assert.IsFalse(result.Exists(item => item.Title == "Unrelated"));
        }
    }
}
