namespace OpenBreed.Wecs.Abstractions.Extensions
{
    public static class EntityClassExtensions
    {
        #region Public Methods

        public static bool IsOrInheritsFrom(this IEntityClass entityClass, IEntityClass checkedClass) => IsOrInheritsFrom(entityClass, checkedClass.Id);

        public static bool IsOrInheritsFrom(this IEntityClass entityClass, int checkedClassId)
        {
            if (entityClass.Id == checkedClassId)
            {
                return true;
            }

            return entityClass.HasAncestorById(checkedClassId);
        }

        public static bool HasAncestorById(this IEntityClass entityClass, int id)
        {
            var parent = entityClass.Parent;

            while (parent != null)
            {
                if (parent.Id == id)
                {
                    return true;
                }
            }

            return false;
        }

        #endregion Public Methods
    }
}