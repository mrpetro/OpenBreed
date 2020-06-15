using OpenBreed.Core.Commands;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Core.Helpers
{
    public class CommandHandler : IMsgHandler
    {
        #region Private Fields

        private readonly Queue<ICommand> queue = new Queue<ICommand>();

        private readonly ICommandExecutor executor;

        #endregion Private Fields

        #region Public Constructors

        public CommandHandler(ICommandExecutor executor)
        {
            this.executor = executor;
        }

        #endregion Public Constructors

        #region Public Properties

        public bool AnyEnqueued => queue.Any();

        #endregion Public Properties

        #region Public Methods

        public void Enqueue(ICommand cmd)
        {
            queue.Enqueue(cmd);
        }

        public void ExecuteEnqueued()
        {
            while (queue.Count > 0)
            {
                var cmd = queue.Dequeue();
                Execute(cmd);
            }
        }

        public bool Handle(IMsg cmd)
        {
            Enqueue((ICommand)cmd);
            return true;
        }

        #endregion Public Methods

        #region Private Methods

        private void Execute(ICommand cmd)
        {
            executor.ExecuteCommand(cmd);
        }

        #endregion Private Methods
    }
}