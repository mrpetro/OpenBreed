using OpenBreed.Common;
using OpenBreed.Common.Tools;
using OpenBreed.Common.Tools.Xml;
using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.Interface.Data;
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

        private void LoadStamps(IDataLoader<ITileStamp> tileStampLoader, string name)
        {
            tileStampLoader.Load($"L4/{name}/F1/Lying");
            tileStampLoader.Load($"L4/{name}/F1/Picked");
            tileStampLoader.Load($"L4/{name}/F2/Lying");
            tileStampLoader.Load($"L4/{name}/F2/Picked");
            tileStampLoader.Load($"L4/{name}/F3/Lying");
            tileStampLoader.Load($"L4/{name}/F3/Picked");
        }

        public void LoadStamps()
        {
            var tileStampLoader = dataLoaderFactory.GetLoader<ITileStampDataLoader>();

            LoadStamps(tileStampLoader, "Ammo");
            LoadStamps(tileStampLoader, "MedkitSmall");
            LoadStamps(tileStampLoader, "MedkitBig");
            LoadStamps(tileStampLoader, "CreditsSmall");
            LoadStamps(tileStampLoader, "CreditsBig");
            LoadStamps(tileStampLoader, "AreaScanner");
            LoadStamps(tileStampLoader, "SmartCard");
            LoadStamps(tileStampLoader, "KeycardStandard");
            LoadStamps(tileStampLoader, "ExtraLife");
            LoadStamps(tileStampLoader, "PowerUpS");
            LoadStamps(tileStampLoader, "PowerUpA");
            LoadStamps(tileStampLoader, "PowerUpF");

            tileStampLoader.Load("L4/KeycardRed/Lying");
            tileStampLoader.Load("L4/KeycardRed/Picked");
            tileStampLoader.Load("L4/KeycardGreen/Lying");
            tileStampLoader.Load("L4/KeycardGreen/Picked");
            tileStampLoader.Load("L4/KeycardBlue/Lying");
            tileStampLoader.Load("L4/KeycardBlue/Picked");
            tileStampLoader.Load("L4/KeycardSpecial/Lying");
            tileStampLoader.Load("L4/KeycardSpecial/Picked");
        }

        public void AddItem(World world, int x, int y, string name, int gfxValue, string flavor = null)
        {
            var path = $@"{PICKABLE_PREFIX}\{name}.xml";

            var pickable = entityFactory.Create(path)
                .SetParameter("startX", 16 * x)
                .SetParameter("startY", 16 * y)
                .SetParameter("imageIndex", gfxValue)
                .SetParameter("flavor", flavor)
                .Build();

            pickable.EnterWorld(world.Id);
        }

        #endregion Public Methods
    }
}