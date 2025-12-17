namespace OpenBreed.Wecs.Abstractions.Extensions
{
    public static class EntityExtensions
    {
        #region Public Methods

        public static bool HasWorld(this IEntity entity)
        {
            return entity.WorldId != WecsConsts.NO_WORLD_ID;
        }

        #endregion Public Methods
    }
}