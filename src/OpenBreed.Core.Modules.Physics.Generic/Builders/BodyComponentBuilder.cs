﻿using OpenBreed.Core.Common.Builders;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Modules.Physics.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Core.Modules.Physics.Builders
{
    public class BodyComponentBuilder : IComponentBuilder
    {
        #region Private Fields

        private float cofFactor;
        private float corFactor;
        private string type;

        #endregion Private Fields

        #region Public Methods

        public static IComponentBuilder New()
        {
            return new BodyComponentBuilder();
        }

        public IEntityComponent Build()
        {
            return new Body() { CofFactor = cofFactor, CorFactor = corFactor };
        }

        public void SetProperty(object key, object value)
        {
            var propertyName = Convert.ToString(key);
            switch (propertyName)
            {
                case "CofFactor":
                    cofFactor = Convert.ToSingle(value);
                    break;
                case "CorFactor":
                    corFactor = Convert.ToSingle(value);
                    break;
                case "type":
                    type = Convert.ToString(value);
                    break;
                default:
                    break;
            }
        }

        #endregion Public Methods
    }
}
