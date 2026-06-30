using System;
using System.Collections.Generic;
using System.ComponentModel;
using NUnit.Framework;
using Proyecto2026;

namespace Library.Tests
{
    /// <summary>
    /// HU2: Como usuario quiero definir mis preferencias para personalizar mi experiencia.
    /// Criterios de aceptación:
    ///   - El usuario puede seleccionar uno o más atributos (género, categoría, etc.).
    ///   - Las preferencias quedan almacenadas.
    ///   - Las recomendaciones consideran dichas preferencias.
    /// </summary>
    [TestFixture]
    public class HU02Tests
    {
        private SystemFacade BuildFacade()
        {
            var catalog = new Catalog();
            var strategy = new BasedOnPreferences();
            var filters = new List<IRecommendationFilter> { new ExcludeConsumedFilter() };
            var sorter = new PopularitySorter();
            var engine = new RecommendationEngine(strategy, filters, sorter);
            return new SystemFacade(catalog, engine);
        }


        [Test]
        public void SetPreferenceTag_UsuarioPuedeSeleccionarAtributos()
        {
            // El usuario puede seleccionar uno o más atributos.
            var facade = BuildFacade();
            var user = facade.RegisterUser("maria", "maria@gmail.com", "HashedPass");

            facade.SetPreferenceTag(user, new List<string> { "Rock", "Jazz" });

            Assert.AreEqual(2, user.Preferences.PreferredTags.Count);
            Assert.IsTrue(user.Preferences.PreferredTags.Contains("Rock"));
            Assert.IsTrue(user.Preferences.PreferredTags.Contains("Jazz"));
        }


        [Test]
        public void SetPreferenceTag_PreferenciasAlmacenadas()
        {
            // Las preferencias quedan almacenadas.
            var facade = BuildFacade();
            var user = facade.RegisterUser("juan andres", "juanadnres@gmail.com", "HashedPass");

            facade.SetPreferenceTag(user, new List<string> {"Pop"});

            Assert.IsTrue(user.Preferences.PreferredTags.Contains("Pop"));
        }

        [Test]
        public void SetExcludedTag_UsuarioPuedeSeleccionarExclusiones()
        {
            // El usuario puede seleccionar uno o más atributos a excluir.
            var facade = BuildFacade();
            var user = facade.RegisterUser("maria", "maria@gmail.com", "HashedPass");

            facade.SetExcludedTag(user, new List<string> { "Cumbia", "Plena" });

            //Assert.AreEqual(2, user.Preferences.PreferredTags.Count);
            Assert.IsTrue(user.Preferences.ExcludedTags.Contains("Cumbia"));
            Assert.IsTrue(user.Preferences.ExcludedTags.Contains("Plena"));
        }


        [Test]
        public void SetExcludedTag_ExclusionesAlmacenadas()
        {
            // Las exclusiones quedan almacenadas.
            var facade = BuildFacade();
            var user = facade.RegisterUser("juan franco", "juanfranco@gmail.com", "HashedPass");

            facade.SetExcludedTag(user, new List<string> {"Pop"});

            Assert.IsTrue(user.Preferences.ExcludedTags.Contains("Pop"));
        }


        [Test]
        public void SetPreferences_RecomendacionesConsideranPreferencias()
        {
            // Las recomendaciones consideran dichas preferencias.
            var facade = BuildFacade();
            var user = facade.RegisterUser("rodrigo", "rodrigo@gmail.com", "HashedPass");
            facade.AddMusic(new Music("Cancion Rock", "Artist", "Album", "Rock", DateTime.Now, 80));
            facade.AddMusic(new Music("Cancion Pop", "Artist", "Album", "Pop", DateTime.Now, 90));

            facade.SetPreferenceTag(user, new List<string> {"Rock"});
            var result = facade.GetRecommendations(user);

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Cancion Rock", result[0].Title);
        }


        [Test]
        public void SetPreferences_ActualizarPreferencias_PisaLasAnteriores()
        {
            // Si el usuario cambia sus preferencias, las nuevas reemplazan a las anteriores.
            var facade = BuildFacade();
            var user = facade.RegisterUser("rodrigogarcia", "rodrigogarcia@gmail.com", "HashedPass");
            facade.AddMusic(new Music("Cancion Rock", "Artist", "Album", "Rock", DateTime.Now, 80));
            facade.AddMusic(new Music("Cancion Jazz", "Artist", "Album", "Jazz", DateTime.Now, 70));

            facade.SetPreferenceTag(user, new List<string> {"Rock"});
            facade.SetPreferenceTag(user, new List<string> {"Jazz"});
            var result = facade.GetRecommendations(user);

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Cancion Jazz", result[0].Title);
        }
    }
}
