using System;
using System.Collections.Generic;
using NUnit.Framework;
using Proyecto2026;

namespace Library.Tests
{
    /// <summary>
    /// HU10: Como usuario quiero registrar mis interacciones para mejorar recomendaciones futuras.
    /// Criterios de aceptación:
    ///   - Cada consumo de ítem genera una interacción.
    ///   - Las interacciones quedan almacenadas en el historial.
    ///   - El sistema utiliza estas interacciones para recomendar.
    /// </summary>
    [TestFixture]
    public class HU10Tests
    {
        private SystemFacade BuildFacade()
        {
            var catalog = new Catalog();
            var strategy = new BasedOnHistory();
            var filters = new List<IRecommendationFilter>();
            var sorter = new PopularitySorter();
            var engine = new RecommendationEngine(strategy, filters, sorter);
            return SystemFacade.Instance;
        }


        [Test]
        public void RegisterInteraction_GeneraInteraccionEnHistorial()
        {
            // Cada consumo de ítem genera una interacción.
            var facade = BuildFacade();
            var user = facade.RegisterUser("Jorge", "jorge@gmail.com", "HashedPass");
            var music = new Music("Song 4", "Artist", "Album", "Rock", DateTime.Now, 70);
            facade.AddMusic(music);

            facade.RegisterInteraction(user, music, InteractionType.Played);

            Assert.AreEqual(1, user.History.Count);
        }


        [Test]
        public void RegisterInteraction_VariasInteracciones_QuedanEnHistorial()
        {
            // Las interacciones quedan almacenadas en el historial.
            var facade = BuildFacade();
            var user = facade.RegisterUser("Franco", "franco@gmail.com", "HashedPass");
            var song_7 = new Music("Cancion 7", "Artist", "Album", "Rock", DateTime.Now, 60);
            var song_8 = new Music("Cancion 8", "Artist", "Album", "Pop", DateTime.Now, 40);
            facade.AddMusic(song_7);
            facade.AddMusic(song_8);

            facade.RegisterInteraction(user, song_7, InteractionType.Played);
            facade.RegisterInteraction(user, song_8, InteractionType.Played);

            Assert.AreEqual(2, user.History.Count);
        }


        [Test]
        public void RegisterInteraction_HistorialAfectaRecomendaciones()
        {
            // El sistema utiliza interacciones para recomendar.
            // Pepe escuchó Rock, entonces el sistema le recomienda más Rock.
            var facade = BuildFacade();
            var user = facade.RegisterUser("Pedro", "pedro@gmail.com", "HashedPass");
            var rock_escuchado = new Music("Rock que escucho", "Artist", "Album", "Rock", DateTime.Now, 80);
            var rock_no_escuchado = new Music("Otro rock que no escuchó", "Artist", "Album", "Rock", DateTime.Now, 60);
            var canción_pop = new Music("Pop que no escuchó", "Artist", "Album", "Pop", DateTime.Now, 90);
            facade.AddMusic(rock_escuchado);
            facade.AddMusic(rock_no_escuchado);
            facade.AddMusic(canción_pop);

            facade.RegisterInteraction(user, rock_escuchado, InteractionType.Played);
            var result = facade.GetRecommendations(user);

            Assert.IsFalse(result.Exists(i => i.Title == "Rock que escucho"));
        }

    }
}
