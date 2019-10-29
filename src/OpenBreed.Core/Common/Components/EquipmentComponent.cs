using OpenBreed.Core.Common.Systems.Components;

namespace OpenBreed.Core.Common.Components
{
    public class Slot
    {
        #region Public Constructors

        public Slot(string name, int initItemId = -1)
        {
            Name = name;
            ItemId = initItemId;
        }

        #endregion Public Constructors

        #region Public Properties

        public string Name { get; }
        public int ItemId { get; set; }

        #endregion Public Properties
    }

    public class EquipmentComponent : IEntityComponent
    {
        #region Public Constructors

        public EquipmentComponent(Slot[] slots)
        {
            Slots = slots;
        }

        #endregion Public Constructors

        #region Public Properties

        public Slot[] Slots { get; }

        #endregion Public Properties
    }
}