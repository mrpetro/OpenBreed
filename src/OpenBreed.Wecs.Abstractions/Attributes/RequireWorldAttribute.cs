using System;

namespace OpenBreed.Wecs.Abstractions.Attributes
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public class RequireWorldWithNameAttribute : OnEventParameterRequireAttribute
    {
        public RequireWorldWithNameAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public override bool IsValid(IWorldMan worldMan, EventArgs e)
        {
            if (e is not WorldEvent worldEvent)
            {
                return false;
            }

            var world = worldMan.GetById(worldEvent.WorldId);

            return world.Name.StartsWith(Name);
        }
    }

    public abstract class OnEventParameterRequireAttribute : Attribute
    {
        public abstract bool IsValid(IWorldMan worldMan, EventArgs value);
    }

    /// <summary>
    /// Entity System Attribute that can set requirement for specific worlds
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class RequireWorldAttribute : Attribute
    {
        #region Public Constructors

        public RequireWorldAttribute(params string[] worldTypes)
        {
            WorldTypes = worldTypes;
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// World types which are not accepted by the system  
        /// </summary>
        public string[] WorldTypes { get; }

        #endregion Public Properties
    }
}