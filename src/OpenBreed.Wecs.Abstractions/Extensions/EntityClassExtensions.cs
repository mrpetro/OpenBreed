namespace OpenBreed.Wecs.Abstractions.Extensions
{
    public static class EntityClassExtensions
    {
        #region Public Methods

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