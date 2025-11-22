using OpenBreed.Scripting.Interface;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Scripting.Extensions;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Game.Wecs.Extensions
{
    public static class ScriptManExtensions
    {
        #region Public Methods

        public static void TryOnCollision(this IScriptMan scriptMan, IEntity entityA, IEntity entityB, Vector2 projection)
        {
            var functionId = entityA.GetFunctionId("OnCollision");

            if (functionId is null)
                return;

            var scriptFunction = scriptMan.GetFunction(functionId);

            if (scriptFunction is null)
                return;

            scriptFunction.Invoke(entityA, entityB, projection);
        }

        #endregion Public Methods
    }
}