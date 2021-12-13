using System;
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
        private readonly Dictionary<string, Dictionary<int, string>> keyValuePairsEx = new Dictionary<string, Dictionary<int, string>>();

        #endregion Private Fields

        #region Internal Constructors

        internal MapMapper(string level)
        {
            Level = level;

            SetupMapping();
        }

        private void SetupMapping()
        {
            switch (Level)
            {
                case "Vanilla/L1":
                    this.SetupL1();
                    break;
                case "Vanilla/L3":
                    this.SetupL3();
                    break;
                case "Vanilla/L4":
                    this.SetupL4();
                    break;
                case "Vanilla/L5":
                    this.SetupL5();
                    break;
                case "Vanilla/L6":
                    this.SetupL6();
                    break;
                default:
                    throw new NotImplementedException($"Mapping for level '{Level}'");
            }
        }

        #endregion Internal Constructors

        #region Public Properties

        public string Level { get; }

        #endregion Public Properties

        #region Public Methods

        public bool TryGetFlavor(string templateName, int gfxValue, out string flavor)
        {
            if (!keyValuePairsEx.TryGetValue(templateName, out Dictionary<int, string> subDict))
            {
                flavor = null;
                return false;
            }

            if (subDict.TryGetValue(gfxValue, out flavor))
                return true;

            if (subDict.TryGetValue(GFX_ANY, out flavor))
                return true;

            flavor = null;
            return true;
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

        public void Register(string templateName, int gfxValue, string flavor)
        {
            if (!keyValuePairsEx.TryGetValue(templateName, out Dictionary<int, string> subDict))
            {
                subDict = new Dictionary<int, string>();
                keyValuePairsEx.Add(templateName, subDict);
            }

            subDict[gfxValue] = flavor;
        }

        #endregion Public Methods
    }
}