using System;
using System.Collections.Generic;
using NUnit.Framework;
using Proyecto2026;

namespace Library.Tests
{
    /// <summary>
    /// HU9: Como usuario quiero ordenar recomendaciones por distintos criterios para elegir mejor.
    /// Criterios de aceptación:
    ///   - El usuario puede definir un criterio de orden.
    ///   - Los resultados se muestran ordenados según ese criterio.
    ///   - Cambiar el criterio modifica el orden.
    /// </summary>
    [TestFixture]
    public class HU09Tests
    {
        private SystemFacade BuildFacade()
        {
            var catalog = new Catalog();
            var strategy = new PopularityStrategy();
            var filters = new List<IRecommendationFilter>();
            var sorter = new PopularitySorter();
            var engine = new RecommendationEngine(strategy, filters, sorter);
            return new SystemFacade(catalog, engine);
        }

        [Test]
        public void SetRecommendationSorter_UsuarioPuedeDefinirCriterioDeOrden()
        {
            var facade = BuildFacade();
            var user = facade.RegisterUser("usuario", "usuario@mail.com", "hash123");
            facade.AddMusic(new Music("Antigua popular", "Artist", "Album", "Rock", DateTime.Now.AddYears(-5), 100));
            facade.AddMusic(new Music("Nueva menos popular", "Artist", "Album", "Pop", DateTime.Now, 96));

            facade.SetRecommendationSorter(new NewestSorter());
            var result = facade.GetRecommendations(user);

            Assert.That(result[0].Title, Is.EqualTo("Nueva menos popular"));
        }

        [Test]
        public void GetRecommendations_ResultadosOrdenadosPorPopularidad()
        {
            var facade = BuildFacade();
            var user = facade.RegisterUser("usuario", "usuario@mail.com", "hash123");

            var result = facade.GetRecommendations(user);

            Assert.That(result[0].Title, Is.EqualTo("Blinding Lights"));
        }
    }
}
