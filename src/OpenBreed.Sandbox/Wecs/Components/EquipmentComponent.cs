﻿using OpenBreed.Wecs.Components;

namespace OpenBreed.Sandbox.Wecs.Components
{
    public struct EquipmentSlot
    {
        #region Public Constructors

        public EquipmentSlot(string name, int initItemId = -1)
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

        public EquipmentComponent(EquipmentSlot[] slots)
        {
            Slots = slots;
        }

        #endregion Public Constructors

        #region Public Properties

        public EquipmentSlot[] Slots { get; }

        #endregion Public Properties
    }
}