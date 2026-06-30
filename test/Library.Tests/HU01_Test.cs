using System;
using System.Collections.Generic;
using NUnit.Framework;
using Proyecto2026;

namespace Library.Tests
{
    /// <summary>
    /// HU1: Como usuario quiero registrarme para poder utilizar el sistema.
    /// Criterios de aceptación:
    ///   - Dado que no estoy registrado, cuando ingreso mis datos, entonces se crea una cuenta válida.
    ///   - El sistema valida que el usuario no exista previamente.
    ///   - El usuario queda habilitado para usar el sistema.
    /// </summary>
    [TestFixture]
    public class HU01Tests
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
        public void RegisterUser_DatosValidos_CreaUsuarioCorrectamente()
        {
            // Cuando ingreso mis datos, se crea una cuenta válida.
            var facade = BuildFacade();

            var user = facade.RegisterUser("santiago", "santiago@gmail.com", "HashedPass");

            Assert.IsNotNull(user);
            Assert.AreEqual("santiago", user.Username);
        }


        [Test]
        public void RegisterUser_UsuarioYaExiste_LanzaExcepcion()
        {
            // El sistema valida que el usuario no exista previamente.
            var facade = BuildFacade();
            facade.RegisterUser("santiago", "santiago@gmail.com", "HashedPass");

            Assert.Throws<InvalidOperationException>(() =>
                facade.RegisterUser("santiago", "otro@gmail.com", "OtraPass"));
        }


        [Test]
        public void RegisterUser_UsuarioRegistrado_PuedeUsarElSistema()
        {
            // El usuario queda habilitado para usar el sistema (puede pedir recomendaciones).
            var facade = BuildFacade();
            facade.AddMusic(new Music("Song A", "Artist", "Album", "Rock", DateTime.Now, 80));

            var user = facade.RegisterUser("santiago", "santiago@gmail.com", "HashedPass");
            var result = facade.GetRecommendations(user);

            Assert.IsNotNull(result);
        }
        
    }
}
