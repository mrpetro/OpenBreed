using System.Collections.Generic;

namespace OpenBreed.Sandbox.Entities.Builders
{
    public class MapMapper
    {
        #region Public Fields

        public const int GFX_ANY = -1;

        #endregion Public Fields

        #region Private Fields

        private readonly Dictionary<(int, int), (string, string)> keyValuePairs = new Dictionary<(int, int), (string, string)>();

        #endregion Private Fields

        #region Internal Constructors

        internal MapMapper()
        {
        }

        #endregion Internal Constructors

        #region Public Properties

        public string Level { get; private set; }

        #endregion Public Properties

        #region Public Methods

        public void SetLevel(string atlasName)
        {
            Level = atlasName;
        }

        public bool Map(int actionValue, int gfxValue, out string templateName, out string flavor)
        {
            if (keyValuePairs.TryGetValue((actionValue, gfxValue), out (string Name, string Flavor) item))
            {
                templateName = item.Name;
                flavor = item.Flavor;
                return true;
            }
            else if (keyValuePairs.TryGetValue((actionValue, GFX_ANY), out item))
            {
                templateName = item.Name;
                flavor = item.Flavor;
                return true;
            }
            else
            {
                templateName = null;
                flavor = null;
                return false;
            }
        }

        public void Register(int actionValue, int gfxValue, string templateName, string flavor)
        {
            keyValuePairs.Add((actionValue, gfxValue), (templateName, flavor));
        }

        #endregion Public Methods
    }
}