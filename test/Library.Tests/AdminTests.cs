using NUnit.Framework;
using Proyecto2026;

namespace Library.Tests
{
    [TestFixture]
    public class AdminTests
    {
        [Test]
        public void Constructor_CreaAdminConDatosCorrectos()
        {
            var admin = new Admin("admin", "admin@mail.com", "hash123");

            Assert.That(admin.Username, Is.EqualTo("admin"));
            Assert.That(admin.Email, Is.EqualTo("admin@mail.com"));
        }
    }
}
