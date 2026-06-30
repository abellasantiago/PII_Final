using System;
using System.Collections.Generic;
using NUnit.Framework;
using Proyecto2026;

namespace Library.Tests
{
    [TestFixture]
    public class BasedOnSimilarUsersTests
    {
        [Test]
        public void Recommend_UsuarioSinHistorial_RetornaListaVacia()
        {
            var user = new User("user", "user@mail.com", "hash123");
            var strategy = new BasedOnSimilarUsers(new List<User> { user });
            var music = new Music("Song", "Artist", "Album", "Rock", DateTime.Now);

            var result = strategy.Recommend(user, new List<IContent> { music });

            Assert.That(result.Count, Is.EqualTo(0));
        }

        [Test]
        public void Recommend_UsuarioSimilar_RetornaItemsConsumidosPorUsuarioSimilar()
        {
            var user = new User("user", "user@mail.com", "hash123");
            var similar = new User("similar", "similar@mail.com", "hash123");
            var baseSong = new Music("Base", "Artist", "Album", "Rock", DateTime.Now);
            var recommended = new Music("Recommended", "Artist", "Album", "Pop", DateTime.Now);
            user.Consume(baseSong);
            similar.Consume(baseSong);
            similar.Consume(recommended);

            var strategy = new BasedOnSimilarUsers(new List<User> { user, similar });
            var result = strategy.Recommend(user, new List<IContent> { baseSong, recommended });

            Assert.IsTrue(result.Contains(baseSong));
            Assert.IsTrue(result.Contains(recommended));
        }

        [Test]
        public void Recommend_NoDuplicaItemsRecomendados()
        {
            var user = new User("user", "user@mail.com", "hash123");
            var similar = new User("similar", "similar@mail.com", "hash123");
            var baseSong = new Music("Base", "Artist", "Album", "Rock", DateTime.Now);
            var recommended = new Music("Recommended", "Artist", "Album", "Pop", DateTime.Now);
            user.Consume(baseSong);
            similar.Consume(baseSong);
            similar.Consume(recommended);
            similar.Consume(recommended);

            var strategy = new BasedOnSimilarUsers(new List<User> { user, similar });
            var result = strategy.Recommend(user, new List<IContent> { baseSong, recommended });

            Assert.That(result.Count, Is.EqualTo(2));
        }
    }
}
