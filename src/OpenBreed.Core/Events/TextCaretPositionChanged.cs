﻿using System;

namespace OpenBreed.Core.Events
{
    public class TextDataChanged : EventArgs
    {
        #region Public Constructors

        public TextDataChanged(string text)
        {
            Text = text;
        }

        #endregion Public Constructors

        #region Public Properties

        public string Text { get; }

        #endregion Public Properties
    }
}