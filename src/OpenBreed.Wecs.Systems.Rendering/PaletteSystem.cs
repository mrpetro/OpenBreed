using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Wecs.Attributes;
using OpenBreed.Wecs.Components.Rendering;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Core;
using OpenBreed.Wecs.Worlds;
using System.Collections.Generic;

namespace OpenBreed.Wecs.Systems.Rendering
{
    [RequireEntityWith(
        typeof(PaletteComponent))]
    public class PaletteSystem : UpdatableSystemBase<PaletteSystem>
    {
        #region Private Fields

        private readonly IPaletteMan paletteMan;
        private readonly IPrimitiveRenderer primitiveRenderer;

        #endregion Private Fields

        #region Internal Constructors

        internal PaletteSystem(
            IPaletteMan paletteMan,
            IPrimitiveRenderer primitiveRenderer)
        {
            this.paletteMan = paletteMan;
            this.primitiveRenderer = primitiveRenderer;
        }

        #endregion Internal Constructors

        #region Public Methods

        protected override void UpdateEntity(IEntity entity, IWorldContext context)
        {
            var paletteComponent = entity.Get<PaletteComponent>();
            var palette = paletteMan.GetById(paletteComponent.PaletteId);
            //primitiveRenderer.SetPalette(palette);

            context.World.GetModule<IRenderablePalette>().SetPalette(palette);
        }

        #endregion Public Methods
    }
}