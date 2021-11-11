using OpenBreed.Common;
using OpenBreed.Common.Tools;
using OpenBreed.Common.Tools.Xml;
using OpenBreed.Rendering.Interface;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Entities.Xml;
using OpenBreed.Wecs.Worlds;
using OpenTK;
using System.Collections.Generic;
using System.Globalization;

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
            tileStampLoader.Load("Tiles/Stamps/Pickable/L4/SmartCard/Lying");
            tileStampLoader.Load("Tiles/Stamps/Pickable/L4/SmartCard/Picked");
            tileStampLoader.Load("Tiles/Stamps/Pickable/L4/KeycardStandard/Lying");
            tileStampLoader.Load("Tiles/Stamps/Pickable/L4/KeycardStandard/Picked");
            tileStampLoader.Load("Tiles/Stamps/Pickable/L4/KeycardRed/Lying");
            tileStampLoader.Load("Tiles/Stamps/Pickable/L4/KeycardRed/Picked");
            tileStampLoader.Load("Tiles/Stamps/Pickable/L4/KeycardGreen/Lying");
            tileStampLoader.Load("Tiles/Stamps/Pickable/L4/KeycardGreen/Picked");
            tileStampLoader.Load("Tiles/Stamps/Pickable/L4/KeycardBlue/Lying");
            tileStampLoader.Load("Tiles/Stamps/Pickable/L4/KeycardBlue/Picked");
            tileStampLoader.Load("Tiles/Stamps/Pickable/L4/KeycardSpecial/Lying");
            tileStampLoader.Load("Tiles/Stamps/Pickable/L4/KeycardSpecial/Picked");
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

            var dictionary = new Dictionary<string, string>();
            dictionary.Add("startX", (16 * x).ToString(CultureInfo.InvariantCulture));
            dictionary.Add("startY", (16 * y).ToString(CultureInfo.InvariantCulture));

            var pickableTemplate = XmlHelper.RestoreFromXml<XmlEntityTemplate>(path, dictionary);
            var pickable = entityFactory.Create(pickableTemplate);

            //pickable.Get<PositionComponent>().Value = new Vector2(16 * x, 16 * y);

            pickable.EnterWorld(world.Id);
        }

        #endregion Public Methods
    }
}