using System;
using System.Collections.Generic;
using NUnit.Framework;
using Proyecto2026;

namespace Library.Tests
{
    [TestFixture]
    public class NewestSorterTests
    {
        [Test]
        public void Sort_RetornaItemsOrdenadosPorFechaDescendente()
        {
            var old = new Music("Old", "Artist", "Album", "Rock", DateTime.Now.AddYears(-2));
            var recent = new Music("Recent", "Artist", "Album", "Pop", DateTime.Now);
            var sorter = new NewestSorter();

            var result = sorter.Sort(new List<IContent> { old, recent });

            Assert.That(result[0].Title, Is.EqualTo("Recent"));
            Assert.That(result[1].Title, Is.EqualTo("Old"));
        }
    }
}
