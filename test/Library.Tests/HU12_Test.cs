using System;
using System.Collections.Generic;
using NUnit.Framework;
using Proyecto2026;

namespace Library.Tests
{
    /// <summary>
    /// HU12: Como usuario quiero consultar mi historial para recordar qué consumí.
    /// Criterios de aceptación:
    ///   - El usuario puede consultar su historial.
    ///   - El historial muestra los ítems consumidos.
    ///   - El historial se actualiza automáticamente al consumir.
    /// </summary>
    [TestFixture]
    public class HU12Tests
    {
        private SystemFacade BuildFacade()
        {
            var catalog = new Catalog();
            var strategy = new PopularityStrategy();
            var filters = new List<IRecommendationFilter>();
            var sorter = new PopularitySorter();
            var engine = new RecommendationEngine(strategy, filters, sorter);
            return SystemFacade.Instance;
        }

        [Test]
        public void GetConsumedHistory_UsuarioPuedeConsultarHistorial()
        {
            var facade = BuildFacade();
            var user = facade.RegisterUser("usuario", "usuario@mail.com", "hash123");

            var history = facade.GetConsumedHistory(user);

            Assert.IsNotNull(history);
        }

        [Test]
        public void GetConsumedHistory_MuestraItemsConsumidos()
        {
            var facade = BuildFacade();
            var user = facade.RegisterUser("usuario", "usuario@mail.com", "hash123");
            var songA = new Music("Cancion A", "Artist", "Album", "Rock", DateTime.Now, 80);
            var songB = new Music("Cancion B", "Artist", "Album", "Pop", DateTime.Now, 70);

            facade.RegisterInteraction(user, songA, InteractionType.Played);
            facade.RegisterInteraction(user, songB, InteractionType.Played);
            var history = facade.GetConsumedHistory(user);

            Assert.That(history.Count, Is.EqualTo(2));
            Assert.IsTrue(history.Exists(item => item.Title == "Cancion A"));
            Assert.IsTrue(history.Exists(item => item.Title == "Cancion B"));
        }

        [Test]
        public void GetConsumedHistory_SeActualizaAutomaticamenteAlConsumir()
        {
            var facade = BuildFacade();
            var user = facade.RegisterUser("usuario", "usuario@mail.com", "hash123");
            var song = new Music("Cancion", "Artist", "Album", "Rock", DateTime.Now, 80);

            var before = facade.GetConsumedHistory(user);
            facade.RegisterInteraction(user, song, InteractionType.Played);
            var after = facade.GetConsumedHistory(user);

            Assert.That(before.Count, Is.EqualTo(0));
            Assert.That(after.Count, Is.EqualTo(1));
            Assert.That(after[0].Title, Is.EqualTo("Cancion"));
        }
    }
}
