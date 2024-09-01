using OpenBreed.Core.Interface.Managers;
using OpenBreed.Rendering.Interface.Events;
using OpenBreed.Rendering.Interface.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Extensions
{
    public static class EventManExtensions
    {
        #region Public Methods

        public static void SubscribeToView<TEvent>(this IEventsMan eventsMan, IRenderView view, EventCallback<TEvent> callback) where TEvent : ViewCursorEvent
        {
            void OnEvent(TEvent e)
            {
                if (e.View != view)
                {
                    return;
                }

                callback.Invoke(e);
            }

            eventsMan.Subscribe<TEvent>(OnEvent);
        }

        #endregion Public Methods
    }
}