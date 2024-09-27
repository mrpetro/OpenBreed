//using Microsoft.Extensions.Logging;
//using OpenBreed.Common.Data;
//using OpenBreed.Common.Interface.Data;
//using OpenBreed.Common.Interface.Dialog;
//using OpenBreed.Common.Interface.Drawing;
//using OpenBreed.Database.Interface.Items.TileStamps;
//using OpenBreed.Editor.VM.Base;
//using OpenBreed.Editor.VM;
//using OpenBreed.Model.Palettes;
//using OpenBreed.Model.Tiles;
//using OpenBreed.Rendering.Interface.Managers;
//using OpenBreed.Rendering.Interface;
//using OpenTK.Mathematics;
//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Input;
//using OpenTK.Windowing.Common;
//using OpenBreed.Rendering.Interface.Events;
//using Microsoft.EntityFrameworkCore.Metadata.Internal;
//using OpenBreed.Core.Interface.Managers;
//using OpenBreed.Editor.VM.Extensions;
//using System.Security.Cryptography;
//using Microsoft.Extensions.DependencyInjection;

//namespace OpenBreed.Editor.VM.Base
//{
//    public class RenderContextVM<TDbEntry, TViewControl> : RenderContextBaseVM where TViewControl : RenderViewControlBase<TDbEntry>
//    {
//        #region Private Fields

//        private readonly IEventsMan eventsMan;
//        private readonly IServiceScopeFactory serviceScopeFactory;
//        private RenderViewControlBase<TDbEntry> renderView;
//        private TDbEntry entry;

//        #endregion Private Fields

//        #region Public Constructors

//        public RenderContextVM(IEventsMan eventsMan,
//            IServiceScopeFactory serviceScopeFactory)
//        {
//            this.eventsMan = eventsMan;
//            this.serviceScopeFactory = serviceScopeFactory;
//        }

//        #endregion Public Constructors

//        #region Public Properties

//        #endregion Public Properties

//        #region Private Methods

//        protected override IRenderContext OnInitialize(IGraphicsContext graphicsContext, HostCoordinateSystemConverter hostCoordinateSystemConverter)
//        {
//            var serviceScope = serviceScopeFactory.CreateScope();
//            serviceScope.ServiceProvider.GetRequiredService<IRenderContextFactory>().SetupScope(hostCoordinateSystemConverter, graphicsContext);

//            var renderContext = serviceScope.ServiceProvider.GetRequiredService<IRenderContext>();

//            renderView = ActivatorUtilities.CreateInstance<TViewControl>(serviceScope.ServiceProvider);

//            if (entry is not null)
//            {
//                renderView.Initialize(entry);
//            }

//            return renderContext;
//        }

//        public void Load(TDbEntry entry)
//        {
//            this.entry = entry;
//        }

//        public void Save()
//        {
//            renderView.Save(entry);

//            //renderView.Save(entry);
//        }

//        #endregion Private Methods
//    }



//    public abstract class RenderContextBaseVM : BaseViewModel
//    {
//        #region Private Fields

//        #endregion Private Fields

//        #region Public Constructors

//        public RenderContextBaseVM()
//        {
//        }

//        #endregion Public Constructors

//        #region Public Properties

//        public Func<IGraphicsContext, HostCoordinateSystemConverter, IRenderContext> InitFunc => OnInitialize;

//        #endregion Public Properties

//        #region Private Methods

//        protected abstract IRenderContext OnInitialize(IGraphicsContext graphicsContext, HostCoordinateSystemConverter hostCoordinateSystemConverter);

//        #endregion Private Methods
//    }
//}