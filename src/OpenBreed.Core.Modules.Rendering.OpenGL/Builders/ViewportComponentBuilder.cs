using OpenBreed.Core.Common.Builders;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Modules.Rendering.Components;
using OpenTK.Graphics;
using System;

namespace OpenBreed.Core.Modules.Rendering.Builders
{
    /// <summary>
    /// Builder for viewport components
    /// </summary>
    public class ViewportComponentBuilder : BaseComponentBuilder
    {
        #region Private Constructors

        private ViewportComponentBuilder(ICore core) : base(core)
        {
        }

        #endregion Private Constructors

        #region Internal Properties

        internal float Width { get; private set; }
        internal float Height { get; private set; }
        internal object CameraEntityTag { get; private set; }
        internal bool DrawBorder { get; private set; }
        internal bool DrawBackground { get; private set; }
        internal bool Clipping { get; private set; }
        internal Color4 BackgroundColor { get; private set; }

        #endregion Internal Properties

        #region Public Methods

        public static IComponentBuilder New(ICore core)
        {
            return new ViewportComponentBuilder(core);
        }

        public override IEntityComponent Build()
        {
            return new ViewportComponent(this);
        }

        public override void SetProperty(object key, object value)
        {
            var propertyName = Convert.ToString(key);
            switch (propertyName)
            {
                case nameof(Width):
                    Width = Convert.ToSingle(value);
                    break;

                case nameof(Height):
                    Height = Convert.ToSingle(value);
                    break;

                case nameof(CameraEntityTag):
                    CameraEntityTag = value;
                    break;

                case nameof(DrawBorder):
                    DrawBorder = Convert.ToBoolean(value);
                    break;

                case nameof(DrawBackground):
                    DrawBackground = Convert.ToBoolean(value);
                    break;

                case nameof(Clipping):
                    Clipping = Convert.ToBoolean(value);
                    break;

                case nameof(BackgroundColor):
                    BackgroundColor = (Color4)value;
                    break;

                default:
                    break;
            }
        }

        #endregion Public Methods
    }
}