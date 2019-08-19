using OpenBreed.Core.Blueprints;
using System;
using System.Collections.Generic;

namespace OpenBreed.Core
{
    public class BlueprintMan
    {
        private Dictionary<string, IBlueprint> repository = new Dictionary<string, IBlueprint>();

        #region Public Constructors

        public BlueprintMan(ICore core)
        {
            Core = core;
        }

        #endregion Public Constructors

        #region Public Properties

        public ICore Core { get; }

        #endregion Public Properties

        #region Public Methods

        public IBlueprint GetByName(string blueprintName)
        {
            IBlueprint blueprint;
            repository.TryGetValue(blueprintName, out blueprint);
            return blueprint;
        }

        public void Import(string filePath)
        {
            var blueprint = Other.RestoreFromXml<BlueprintXml>(filePath);
            repository.Add(blueprint.Name, blueprint);
        }

        #endregion Public Methods
    }
}