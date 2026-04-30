using System;

namespace OpenBreed.Wecs.Abstractions.Attributes
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public class SourceWorldWithNameFilterAttribute : WorldEventFilterAttribute
    {
        public SourceWorldWithNameFilterAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }

    [AttributeUsage(AttributeTargets.Parameter)]
    public class EntityTriggerActionFilter : EntityEventFilterAttribute
    {
        public EntityTriggerActionFilter(string triggerName, string actionName)
        {
            TriggerName = triggerName;
            ActionName = actionName;
        }

        public string TriggerName { get; }
        public string ActionName { get; }
    }

    [AttributeUsage(AttributeTargets.Parameter)]
    public class TargetWorldAsSourceFilter : WorldEventFilterAttribute
    {
        public TargetWorldAsSourceFilter()
        {
        }
    }

    [AttributeUsage(AttributeTargets.Parameter)]
    public class NotifyWorldWithNameAttribute : WorldFilterAttribute
    {
        public NotifyWorldWithNameAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public override bool IsValid(IWorld world)
        {
            return world.Name.StartsWith(Name);
        }
    }

    public abstract class WorldEventFilterAttribute : Attribute
    {
    }

    public abstract class EntityEventFilterAttribute : Attribute
    {
    }

    public abstract class WorldFilterAttribute : Attribute
    {
        public abstract bool IsValid(IWorld world);
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