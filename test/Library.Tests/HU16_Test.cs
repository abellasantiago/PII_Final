using System;
using System.Collections.Generic;
using NUnit.Framework;
using Proyecto2026;

namespace Library.Tests
{
    /// <summary>
    /// HU16: Como administrador quiero eliminar ítems para mantener el catálogo actualizado.
    /// Criterios de aceptación:
    ///   - Se pueden eliminar ítems existentes.
    ///   - Los ítems eliminados no aparecen en recomendaciones.
    ///   - No se rompe el sistema si un ítem fue consumido previamente.
    /// </summary>
    [TestFixture]
    public class HU16Tests
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
        public void RemoveMusic_PermiteEliminarItemsExistentes()
        {
            var facade = BuildFacade();
            var admin = new Admin("admin", "admin@mail.com", "hash123");
            var music = new Music("Song", "Artist", "Album", "Rock", DateTime.Now, 80);
            facade.AddMusic(music);

            facade.RemoveMusic(admin, music);

            Assert.IsTrue(music.IsDeleted);
        }

        [Test]
        public void GetRecommendations_NoRetornaItemsEliminadosPorAdmin()
        {
            var facade = BuildFacade();
            var admin = new Admin("admin", "admin@mail.com", "hash123");
            var user = facade.RegisterUser("user", "user@mail.com", "hash123");
            var deleted = new Music("Deleted", "Artist", "Album", "Rock", DateTime.Now, 100);
            var available = new Music("Available", "Artist", "Album", "Pop", DateTime.Now, 50);
            facade.AddMusic(deleted);
            facade.AddMusic(available);

            facade.RemoveMusic(admin, deleted);
            var result = facade.GetRecommendations(user);

            Assert.IsFalse(result.Exists(item => item.Title == "Deleted"));
        }

        [Test]
        public void RemoveMusic_ItemConsumidoPreviamente_NoRompeSistema()
        {
            var facade = BuildFacade();
            var admin = new Admin("admin", "admin@mail.com", "hash123");
            var user = facade.RegisterUser("user", "user@mail.com", "hash123");
            var consumed = new Music("Consumed", "Artist", "Album", "Rock", DateTime.Now, 100);
            var available = new Music("Available", "Artist", "Album", "Pop", DateTime.Now, 50);
            facade.AddMusic(consumed);
            facade.AddMusic(available);
            facade.RegisterInteraction(user, consumed, InteractionType.Played);

            facade.RemoveMusic(admin, consumed);

            Assert.DoesNotThrow(() => facade.GetRecommendations(user));
            Assert.That(user.History.Count, Is.EqualTo(1));
            Assert.That(user.History[0].Item.Title, Is.EqualTo("Consumed"));
        }
    }
}
