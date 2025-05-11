using OpenBreed.Core.Interface.Managers;
using OpenBreed.Rendering.Abstractions;
using OpenBreed.Rendering.Abstractions.Events;
using OpenBreed.Rendering.Abstractions.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.UI.Mvc.Extensions
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