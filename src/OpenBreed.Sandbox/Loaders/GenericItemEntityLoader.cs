using OpenBreed.Model.Maps;
using OpenBreed.Sandbox.Entities.Builders;
using OpenBreed.Sandbox.Entities.Pickable;
using OpenBreed.Wecs.Worlds;

namespace OpenBreed.Sandbox.Loaders
{
    public class GenericItemEntityLoader : IMapWorldEntityLoader
    {
        #region Private Fields

        private readonly PickableHelper pickableHelper;

        #endregion Private Fields

        #region Public Constructors

        public GenericItemEntityLoader(PickableHelper pickableHelper)
        {
            this.pickableHelper = pickableHelper;
        }

        #endregion Public Constructors

        #region Public Methods

        public void Load(MapMapper mapper, MapModel map, bool[,] visited, int ix, int iy, string templateName, string flavor, int gfxValue, World world)
        {
            if (!mapper.TryGetFlavor(templateName, gfxValue, out flavor))
                return;

            var split = flavor.Split('/');

            templateName = split[0];
            flavor = split[1];

            pickableHelper.AddItem(world, ix, iy, templateName, mapper.Level, gfxValue, flavor);
            visited[ix, iy] = true;
        }

        #endregion Public Methods
    }
}