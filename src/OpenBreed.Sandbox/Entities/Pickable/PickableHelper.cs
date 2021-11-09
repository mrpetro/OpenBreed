using OpenBreed.Common;
using OpenBreed.Common.Tools;
using OpenBreed.Rendering.Interface;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Entities.Xml;
using OpenBreed.Wecs.Worlds;
using OpenTK;

namespace OpenBreed.Sandbox.Entities.Pickable
{
    public class PickableHelper
    {
        #region Private Fields

        private const string PICKABLE_PREFIX = @"Entities\Common\Pickables";
        private readonly IDataLoaderFactory dataLoaderFactory;
        private readonly IEntityFactory entityFactory;

        #endregion Private Fields

        #region Public Constructors

        public PickableHelper(IDataLoaderFactory dataLoaderFactory, IEntityFactory entityFactory)
        {
            this.dataLoaderFactory = dataLoaderFactory;
            this.entityFactory = entityFactory;
        }

        #endregion Public Constructors

        #region Public Methods

        public void LoadStamps()
        {
            var tileStampLoader = dataLoaderFactory.GetLoader<ITileStamp>();

            tileStampLoader.Load("Tiles/Stamps/Pickable/L4/Ammo/Lying");
            tileStampLoader.Load("Tiles/Stamps/Pickable/L4/Ammo/Picked");
            tileStampLoader.Load("Tiles/Stamps/Pickable/L4/MedkitSmall/Lying");
            tileStampLoader.Load("Tiles/Stamps/Pickable/L4/MedkitSmall/Picked");
            tileStampLoader.Load("Tiles/Stamps/Pickable/L4/MedkitBig/Lying");
            tileStampLoader.Load("Tiles/Stamps/Pickable/L4/MedkitBig/Picked");
            tileStampLoader.Load("Tiles/Stamps/Pickable/L4/CreditsSmall/Lying");
            tileStampLoader.Load("Tiles/Stamps/Pickable/L4/CreditsSmall/Picked");
            tileStampLoader.Load("Tiles/Stamps/Pickable/L4/CreditsBig/Lying");
            tileStampLoader.Load("Tiles/Stamps/Pickable/L4/CreditsBig/Picked");
            tileStampLoader.Load("Tiles/Stamps/Pickable/L4/AreaScanner/Lying");
            tileStampLoader.Load("Tiles/Stamps/Pickable/L4/AreaScanner/Picked");
            tileStampLoader.Load("Tiles/Stamps/Pickable/L4/Smartcard/Lying");
            tileStampLoader.Load("Tiles/Stamps/Pickable/L4/Smartcard/Picked");
            tileStampLoader.Load("Tiles/Stamps/Pickable/L4/KeycardStandard/Lying");
            tileStampLoader.Load("Tiles/Stamps/Pickable/L4/KeycardStandard/Picked");
            tileStampLoader.Load("Tiles/Stamps/Pickable/L4/ExtraLife/Lying");
            tileStampLoader.Load("Tiles/Stamps/Pickable/L4/ExtraLife/Picked");
            tileStampLoader.Load("Tiles/Stamps/Pickable/L4/PowerUpS/Lying");
            tileStampLoader.Load("Tiles/Stamps/Pickable/L4/PowerUpS/Picked");
            tileStampLoader.Load("Tiles/Stamps/Pickable/L4/PowerUpA/Lying");
            tileStampLoader.Load("Tiles/Stamps/Pickable/L4/PowerUpA/Picked");
            tileStampLoader.Load("Tiles/Stamps/Pickable/L4/PowerUpF/Lying");
            tileStampLoader.Load("Tiles/Stamps/Pickable/L4/PowerUpF/Picked");
        }

        public void AddItem(World world, int x, int y, string name)
        {
            var path = $@"{PICKABLE_PREFIX}\{name}.xml";

            var pickableTemplate = XmlHelper.RestoreFromXml<XmlEntityTemplate>(path);
            var pickable = entityFactory.Create(pickableTemplate);

            pickable.Get<PositionComponent>().Value = new Vector2(16 * x, 16 * y);

            pickable.EnterWorld(world.Id);
        }

        #endregion Public Methods
    }
}