using OpenBreed.Core.Common.Systems.Components;

namespace OpenBreed.Core.Common.Components
{
    public struct Slot
    {
        #region Public Constructors

        public Slot(string name, int initItemId = -1)
        {
            Name = name;
            ItemId = initItemId;
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Name of this slot. I.e. "Shoulder", "Head", "Torso", "Hand", etc...
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Id of item which occupies this equipment slot
        /// </summary>
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