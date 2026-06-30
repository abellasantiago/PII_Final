using System;
using System.Collections.Generic;
using NUnit.Framework;
using Proyecto2026;

namespace Library.Tests
{
    /// <summary>
    /// HU14: Como usuario quiero guardar contenido para consumir más tarde.
    /// Criterios de aceptación:
    ///   - El usuario puede guardar ítems.
    ///   - Existe una lista de guardados.
    ///   - Los ítems guardados pueden recuperarse.
    /// </summary>
    [TestFixture]
    public class HU14Tests
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
        public void SaveContent_UsuarioPuedeGuardarItems()
        {
            var facade = BuildFacade();
            var user = facade.RegisterUser("usuario", "usuario@mail.com", "hash123");
            var song = new Music("Cancion guardada", "Artist", "Album", "Rock", DateTime.Now, 80);

            facade.SaveContent(user, song);

            Assert.That(user.History.Count, Is.EqualTo(1));
            Assert.That(user.History[0].Type, Is.EqualTo(InteractionType.Saved));
        }

        [Test]
        public void GetSavedContent_ExisteListaDeGuardados()
        {
            var facade = BuildFacade();
            var user = facade.RegisterUser("usuario", "usuario@mail.com", "hash123");

            var saved = facade.GetSavedContent(user);

            Assert.IsNotNull(saved);
            Assert.That(saved.Count, Is.EqualTo(0));
        }

        [Test]
        public void GetSavedContent_ItemsGuardadosPuedenRecuperarse()
        {
            var facade = BuildFacade();
            var user = facade.RegisterUser("usuario", "usuario@mail.com", "hash123");
            var songA = new Music("Guardada A", "Artist", "Album", "Rock", DateTime.Now, 80);
            var songB = new Music("Guardada B", "Artist", "Album", "Pop", DateTime.Now, 70);

            facade.SaveContent(user, songA);
            facade.SaveContent(user, songB);
            var saved = facade.GetSavedContent(user);

            Assert.That(saved.Count, Is.EqualTo(2));
            Assert.IsTrue(saved.Exists(item => item.Title == "Guardada A"));
            Assert.IsTrue(saved.Exists(item => item.Title == "Guardada B"));
        }
    }
}
