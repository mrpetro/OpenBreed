using OpenBreed.Input.Interface;
using System;
using System.Collections.Generic;

namespace OpenBreed.Input.Generic
{
    public class DefaultActionCodeProvider : IActionCodeProvider
    {
        #region Public Methods

        private Dictionary<string, Type> typeLookup = new Dictionary<string, Type>();
        private readonly List<Enum> lookup = new List<Enum>();

        public int GetCode<TAction>(TAction action) where TAction : Enum
        {
            throw new NotImplementedException();
        }

        public void Register<TAction>(TAction action) where TAction : struct, Enum
        {
            var actionType = typeof(TAction);

            if (typeLookup.TryGetValue(actionType.Name, out Type type))
                return;

            var names = Enum.GetNames<TAction>();

            for (int i = 0; i < names.Length; i++)
            {
                var name = names[i];

                //lookup.Add()
            }
        }

        public bool TryGetCode(string actionTypeName, string actionName, out int actionCode)
        {
            if (!typeLookup.TryGetValue(actionTypeName, out Type type))
            {
                actionCode = -1;
                return false;
            }

            throw new NotImplementedException();
        }

        #endregion Public Methods
    }
}