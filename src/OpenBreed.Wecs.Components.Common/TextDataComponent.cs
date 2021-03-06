﻿using OpenBreed.Wecs.Components;
using System;

namespace OpenBreed.Wecs.Components.Common
{
    public class TextDataComponent : IEntityComponent
    {
        #region Public Constructors

        public TextDataComponent(string data = "")
        {
            Data = data;
        }

        #endregion Public Constructors

        #region Public Properties

        public string Data { get; private set; }

        public void Insert(int position, string text)
        {
            Data = Data.Insert(position, text);
        }

        public void Remove(int position, int length)
        {
            Data = Data.Remove(position, length);
        }

        #endregion Public Properties
    }
}