using OpenBreed.Core;
using OpenBreed.Core.Managers;
using OpenBreed.Fsm;
using OpenBreed.Sandbox.Entities.Button.States;
using OpenBreed.Wecs.Worlds;

namespace OpenBreed.Sandbox.Entities.Button
{
    public class ButtonHelper
    {
        #region Public Methods

        public static void AddButton(World world, int x, int y)
        {
            //var button = core.GetManager<IEntityMan>().CreateFromTemplate("Button");

            //button.Get<PositionComponent>().Value = new Vector2(0, 0);

            //world.Core.Commands.Post(new AddEntityCommand(world.Id, button.Id));
            //world.AddEntity(button);
        }



        #endregion Public Methods
    }
}