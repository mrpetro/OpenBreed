using OpenBreed.Model.Maps;
using OpenBreed.Sandbox.Entities.Builders;
using OpenBreed.Sandbox.Entities.Pickable;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Worlds;

namespace OpenBreed.Sandbox.Loaders
{
    public class KeycardEntityLoader : IMapWorldEntityLoader
    {
        #region Private Fields

        private readonly PickableHelper pickableHelper;

        #endregion Private Fields

        #region Public Constructors

        public KeycardEntityLoader(PickableHelper pickableHelper)
        {
            this.pickableHelper = pickableHelper;
        }

        #endregion Public Constructors

        #region Public Methods

        public IEntity Load(MapMapper mapper, MapModel map, bool[,] visited, int ix, int iy, string actionName, string flavor, int gfxValue, IWorld world)
        {
            if (!mapper.TryGetEntityType(actionName, out string entityType, out string option))
                return null;

            if (!mapper.TryGetFlavor("Keycard", gfxValue, out flavor))
                return null;

            var entity = pickableHelper.AddItem(world, ix, iy, entityType, mapper.Level, gfxValue, option, flavor);
            visited[ix, iy] = true;
            return entity;
        }

        #endregion Public Methods
    }
}