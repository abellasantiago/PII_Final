using System;
using System.Collections.Generic;
using NUnit.Framework;
using Proyecto2026;

namespace Library.Tests
{
    /// <summary>
    /// HU5: Como usuario quiero recibir recomendaciones basadas en mis preferencias para mejorar la relevancia.
    /// Criterios de aceptación:
    ///   - El sistema utiliza atributos definidos en preferencias.
    ///   - Las recomendaciones coinciden con al menos una preferencia.
    ///   - Cambiar preferencias modifica los resultados.
    /// </summary>
    [TestFixture]
    public class HU05Tests
    {
        private SystemFacade BuildFacade()
        {
            var catalog = new Catalog();
            var strategy = new BasedOnPreferences();
            var filters = new List<IRecommendationFilter> { new ExcludeConsumedFilter() };
            var sorter = new PopularitySorter();
            var engine = new RecommendationEngine(strategy, filters, sorter);
            return new SystemFacade(catalog, engine);
        }


        [Test]
        public void GetRecommendations_UsaAtributosDePreferencias()
        {
            // El sistema utiliza atributos definidos en preferencias.
            var facade = BuildFacade();
            var user = facade.RegisterUser("rodrigo_q", "rodrigoq@gmail.com", "HashedPass");
            facade.AddMusic(new Music("Reggae Song", "Artist", "Album", "Reggae", DateTime.Now, 75));
            facade.AddMusic(new Music("Cumbia Song", "Artist", "Album", "Cumbia", DateTime.Now, 60));

            // Solo le gusta Reggae
            facade.SetPreferenceTag(user, new List<string> { "Reggae" });
            var result = facade.GetRecommendations(user);

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Reggae Song", result[0].Title);
        }


        [Test]
        public void GetRecommendations_RecomendacionesCoincideConAlMenosUnaPreferencia()
        {
            // Las recomendaciones coinciden con al menos una preferencia.
            var facade = BuildFacade();
            var user = facade.RegisterUser("rodrigo_q", "rodrigoq@gmail.com", "HashedPass");
            facade.AddMusic(new Music("Rock Song", "Artist", "Album", "Rock", DateTime.Now, 80));
            facade.AddMusic(new Music("Jazz Song", "Artist", "Album", "Jazz", DateTime.Now, 65));
            facade.AddMusic(new Music("Pop Song", "Artist", "Album", "Pop", DateTime.Now, 90));

            facade.SetPreferenceTag(user, new List<string> { "Rock", "Jazz" });
            var result = facade.GetRecommendations(user);

            // Todas las recomendaciones deben tener Rock o Jazz en sus tags
            foreach (var item in result)
            {
                bool tienePreferencia = item.Tags.Contains("Rock") || item.Tags.Contains("Jazz");
                Assert.IsTrue(tienePreferencia, $"'{item.Title}' no coincide con ninguna preferencia");
            }
        }


        [Test]
        public void GetRecommendations_CambiarPreferenciasModificaResultados()
        {
            // Cambiar preferencias modifica los resultados.
            var facade = BuildFacade();
            var user = facade.RegisterUser("rodrigo_q", "rodrigoq@gmail.com", "HashedPass");
            facade.AddMusic(new Music("Rock Song", "Artist", "Album", "Rock", DateTime.Now, 80));
            facade.AddMusic(new Music("Cumbia Song", "Artist", "Album", "Cumbia", DateTime.Now, 70));

            facade.SetPreferenceTag(user, new List<string> { "Rock" });
            var resultadoRock = facade.GetRecommendations(user);

            facade.SetPreferenceTag(user, new List<string> { "Cumbia" });
            var resultadoCumbia = facade.GetRecommendations(user);

            Assert.AreEqual("Rock Song", resultadoRock[0].Title);
            Assert.AreEqual("Cumbia Song", resultadoCumbia[0].Title);
        }
    }
}
