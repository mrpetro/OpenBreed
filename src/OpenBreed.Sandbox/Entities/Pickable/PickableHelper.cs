using OpenBreed.Audio.Interface.Data;
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

        private const string PICKABLE_PREFIX = @"Defaults\Templates\ABTA\Common\Pickables";
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
            //var soundLoader = dataLoaderFactory.GetLoader<ISoundSampleDataLoader>();

            //soundLoader.Load("Vanilla/Common/Ammo/Picked");
            //soundLoader.Load("Vanilla/Common/CreditsSmall/Picked");
            //soundLoader.Load("Vanilla/Common/CreditsBig/Picked");
            //soundLoader.Load("Vanilla/Common/MedkitSmall/Picked");
            //soundLoader.Load("Vanilla/Common/MedkitBig/Picked");
            //soundLoader.Load("Vanilla/Common/KeycardStandard/Picked");
            //soundLoader.Load("Vanilla/Common/KeycardRed/Picked");
            //soundLoader.Load("Vanilla/Common/KeycardGreen/Picked");
            //soundLoader.Load("Vanilla/Common/KeycardBlue/Picked");
            //soundLoader.Load("Vanilla/Common/ExtraLife/Picked"); 
        }

        public void AddItem(World world, int x, int y, string name, string tileAtlasName, int gfxValue, string flavor = null)
        {
            var path = $@"{PICKABLE_PREFIX}\{name}.xml";

            var pickable = entityFactory.Create(path)
                .SetParameter("tileSet", tileAtlasName)
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