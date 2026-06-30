using System;
using System.Collections.Generic;
using NUnit.Framework;
using Proyecto2026;

namespace Library.Tests
{
    /// <summary>
    /// HU8: Como usuario quiero excluir contenido con ciertas características para refinar las recomendaciones.
    /// Criterios de aceptación:
    ///   - El usuario puede definir filtros (por atributo).
    ///   - Los ítems que cumplen el filtro son excluidos.
    ///   - El sistema aplica los filtros antes de mostrar resultados.
    /// </summary>
    [TestFixture]
    public class HU08Tests
    {
        private SystemFacade BuildFacade()
        {
            var catalog = new Catalog();
            var strategy = new PopularityStrategy();
            var filters = new List<IRecommendationFilter>
            {
                new ExcludeConsumedFilter(),
                new ExcludeTagsFilter()
            };
            var sorter = new PopularitySorter();
            var engine = new RecommendationEngine(strategy, filters, sorter);
            return new SystemFacade(catalog, engine);
        }


        [Test]
        public void SetPreferences_UsuarioPuedeDefinirTagsExcluidos()
        {
            // El usuario puede definir filtros por atributo.
            var facade = BuildFacade();
            var user = facade.RegisterUser("santiago", "santiago@gmail.com", "HashedPass");

            facade.SetExcludedTag(user, new List<string> {"Rock"});

            Assert.IsTrue(user.Preferences.ExcludedTags.Contains("Rock"));
        }


        [Test]
        public void GetRecommendations_ItemsConTagExcluido_SonExcluidos()
        {
            // Los ítems que cumplen el filtro son excluidos.
            var facade = BuildFacade();
            var user = facade.RegisterUser("geronimo", "gero@gmail.com", "HashedPass");
            facade.AddMusic(new Music("Cancion Rock", "Artist", "Album", "Rock", DateTime.Now, 80));
            facade.AddMusic(new Music("Cancion Reggaeton", "Artist", "Album", "Reggaeton", DateTime.Now, 90));

            facade.SetExcludedTag(user, new List<string> {"Reggaeton"});
            var result = facade.GetRecommendations(user);

            Assert.IsFalse(result.Exists(i => i.Title == "Cancion Reggaeton"));
        }


        [Test]
        public void GetRecommendations_FiltrosSeAplicanAntesDeResultados()
        {
            // El sistema aplica los filtros antes de mostrar resultados.
            var facade = BuildFacade();
            var user = facade.RegisterUser("geronimo", "gero@gmail.com", "HashedPass");

            facade.SetExcludedTag(user, new List<string> {"rock"});
            var result = facade.GetRecommendationsByPreferences(user);

            Assert.AreEqual(8, result.Count);
        }


        [Test]
        public void GetRecommendations_SinTagsExcluidos_MuestraTodo()
        {
            // Sin tags excluidos, el filtro no saca nada.
            var facade = BuildFacade();
            var user = facade.RegisterUser("geronimo", "gero@gmail.com", "HashedPass");

            facade.SetExcludedTag(user, new List<string>());
            var result = facade.GetRecommendationsByPreferences(user);

            Assert.AreEqual(13, result.Count);
        }
    }
}
