using System;
using System.Collections.Generic;
using NUnit.Framework;
using Proyecto2026;

namespace Library.Tests
{
    /// <summary>
    /// HU3: Como usuario quiero visualizar un listado de recomendaciones para decidir qué consumir.
    /// Criterios de aceptación:
    ///   - El sistema muestra una lista de ítems recomendados.
    ///   - La lista contiene al menos un criterio de orden.
    ///   - Cada ítem muestra información básica (nombre, atributos relevantes).
    /// </summary>
    [TestFixture]
    public class HU03Tests
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
        public void GetRecommendations_RetornaListaDeItems() 
        {
            // El sistema muestra una lista de ítems recomendados.

            // Arrange:
            var facade = BuildFacade();
            var user = facade.RegisterUser("Santiago", "santiago@gmail.com", "HashedPass");

            // Act:
            var result = facade.GetRecommendations(user);

            // Assert:
            Assert.IsNotNull(result);
            Assert.AreEqual(10, result.Count);
        }


        [Test]
        public void GetRecommendations_ListaOrdenadaPorPopularidad() 
        {
            // La lista contiene al menos un criterio de orden (popularidad).

            // Arrange:
            var facade = BuildFacade();
            var user = facade.RegisterUser("Rodrigo", "rodrigo@gmail.com", "HashedPass");

            // Act:
            var result = facade.GetRecommendations(user);

            // Assert:
            Assert.AreEqual("Blinding Lights", result[0].Title);
        }


        [Test]
        public void GetRecommendations_CadaItemTieneInformacionBasica()
        {
            // Cada ítem muestra información básica (nombre, atributos relevantes).

            // Arrange:
            var facade = BuildFacade();
            var user = facade.RegisterUser("Gero", "gero@gmail.com", "HashedPass");
            facade.AddMusic(new Music("Mi Cancion", "Artista X", "Album Y", "Cumbia", DateTime.Now, 60));

            // Act:
            var result = facade.GetRecommendations(user);

            // Assert:
            var item = result[0];
            Assert.IsFalse(string.IsNullOrEmpty(item.Title));
            Assert.IsNotNull(item.Tags);
            Assert.IsTrue(item.Tags.Count > 0);
        }
        
    }
}
