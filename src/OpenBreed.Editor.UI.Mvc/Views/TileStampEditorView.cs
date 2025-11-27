using OpenBreed.Core.Abstractions.Managers;
using OpenBreed.Gui.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.UI.Mvc.Views
{
    public class TileStampEditorView : EditorView
    {
        public TileStampEditorView(IEventsMan eventsMan, IInteractionFactoryProvider guiFactoryProvider) : base(eventsMan, guiFactoryProvider)
        {
        }
    }
}
