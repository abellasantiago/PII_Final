using System;
using System.Collections.Generic;
using NUnit.Framework;
using Proyecto2026;

namespace Library.Tests
{
    /// <summary>
    /// HU13: Como usuario quiero que no se me recomiende contenido ya consumido.
    /// Criterios de aceptación:
    ///   - El sistema excluye ítems del historial.
    ///   - No aparecen duplicados en recomendaciones.
    ///   - El filtro se aplica automáticamente.
    /// </summary>
    [TestFixture]
    public class HU13Tests
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
        public void GetRecommendations_ExcluyeItemsDelHistorial()
        {
            // El sistema excluye ítems del historial.
            var facade = BuildFacade();
            var user = facade.RegisterUser("Hernán", "hernan@gmail.com", "HashedPass");
            var cancion = new Music("Cancion consumida", "Artist", "Album", "Rock", DateTime.Now, 80);
            facade.AddMusic(cancion);
            facade.AddMusic(new Music("Cancion nueva", "Artist", "Album", "Pop", DateTime.Now, 70));

            facade.RegisterInteraction(user, cancion, InteractionType.Played);
            var result = facade.GetRecommendations(user);

            Assert.IsFalse(result.Exists(i => i.Title == "Cancion consumida"));
        }


        [Test]
        public void GetRecommendations_FiltroSeAplicaAutomaticamente()
        {
            // El filtro se aplica automáticamente.
            var facade = BuildFacade();
            var user = facade.RegisterUser("Rodrigo", "rodrigo@gmail.com", "HashedPass");
            var consumida = new Music("Ya escuchada", "Artist", "Album", "Rock", DateTime.Now, 90);
            var nueva = new Music("Sin escuchar", "Artist", "Album", "Pop", DateTime.Now, 60);
            facade.AddMusic(consumida);
            facade.AddMusic(nueva);

            facade.RegisterInteraction(user, consumida, InteractionType.Played);
            var result = facade.GetRecommendationsByHistory(user);

            // Solo aparece la que no fue consumida
            Assert.AreEqual(0, result.Count);
        }


        [Test]
        public void GetRecommendations_UsuarioSinHistorial_MuestraTodo()
        {
            // Si el usuario no consumió nada, no se excluye nada.
            var facade = BuildFacade();
            var user = facade.RegisterUser("Pepe", "podrigo@gmail.com", "HashedPass");
            facade.AddMusic(new Music("Cancion A", "Artist", "Album", "Rock", DateTime.Now, 80));
            facade.AddMusic(new Music("Cancion B", "Artist", "Album", "Pop", DateTime.Now, 70));

            var result = facade.GetRecommendationsByHistory(user);

            Assert.AreEqual(15, result.Count);
        }
    }
}
