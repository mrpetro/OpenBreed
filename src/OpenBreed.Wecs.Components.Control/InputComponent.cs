using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Components.Control
{
    public abstract class TriggerComponent : IEntityComponent
    {
        protected TriggerComponent(bool singleTime)
        {
            SingleTime = singleTime;
        }

        public bool SingleTime { get; }
        public Action Callback { get; }
    }

    public class ButtonTriggerComponent : TriggerComponent
    {
        public ButtonTriggerComponent(bool singleTime)
            : base(singleTime)
        {
            //Callbacks = callback;
        }

        public int ButtonsPressed { get; set; }
    }
}
