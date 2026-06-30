using System;
using System.Collections.Generic;
using NUnit.Framework;
using Proyecto2026;

namespace Library.Tests
{
    [TestFixture]
    public class PopularityByArtistStrategyTests
    {
        [Test]
        public void Recommend_RetornaSoloMusicaDelArtista()
        {
            var user = new User("user", "user@mail.com", "hash123");
            var sameArtist = new Music("Same artist", "Artist A", "Album", "Rock", DateTime.Now, 80);
            var otherArtist = new Music("Other artist", "Artist B", "Album", "Rock", DateTime.Now, 90);
            var strategy = new PopularityByArtistStrategy("Artist A", 10);

            var result = strategy.Recommend(user, new List<IContent> { sameArtist, otherArtist });

            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].Title, Is.EqualTo("Same artist"));
        }

        [Test]
        public void Recommend_OrdenaPorPopularidadYRespetaTopN()
        {
            var user = new User("user", "user@mail.com", "hash123");
            var low = new Music("Low", "Artist A", "Album", "Rock", DateTime.Now, 10);
            var high = new Music("High", "Artist A", "Album", "Rock", DateTime.Now, 100);
            var strategy = new PopularityByArtistStrategy("Artist A", 1);

            var result = strategy.Recommend(user, new List<IContent> { low, high });

            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].Title, Is.EqualTo("High"));
        }

        [Test]
        public void Constructor_ArtistaNull_LanzaExcepcion()
        {
            Assert.Throws<ArgumentNullException>(() => new PopularityByArtistStrategy(null, 10));
        }
    }
}
