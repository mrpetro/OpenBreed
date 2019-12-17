using OpenBreed.Core.Commands;
using OpenBreed.Core.Common.Systems;
using OpenBreed.Core.Entities;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Core.Helpers
{
    public class CommandHandler : IMsgHandler
    {
        #region Private Fields

        private readonly Queue<CommandData> queue = new Queue<CommandData>();

        private readonly ICommandExecutor executor;

        #endregion Private Fields

        #region Public Constructors

        public CommandHandler(ICommandExecutor executor)
        {
            this.executor = executor;
        }

        #endregion Public Constructors

        #region Public Methods

        public void Enqueue(object sender, ICommand cmd)
        {
            queue.Enqueue(new CommandData(sender, cmd));
        }

        public void ExecuteEnqueued()
        {
            while (queue.Count > 0)
            {
                var ed = queue.Dequeue();
                Execute(ed.Sender, ed.Cmd);
            }
        }

        public bool Handle(object sender, IMsg cmd)
        {
            Enqueue(sender, (ICommand)cmd);
            return true;
        }

        #endregion Public Methods

        #region Private Methods

        private void Execute(object sender, ICommand cmd)
        {
            executor.ExecuteCommand(sender, cmd);
        }

        #endregion Private Methods

        #region Private Structs

        private struct CommandData
        {
            #region Internal Fields

            internal object Sender;
            internal ICommand Cmd;

            #endregion Internal Fields

            #region Internal Constructors

            internal CommandData(object sender, ICommand cmd)
            {
                Sender = sender;
                Cmd = cmd;
            }

            #endregion Internal Constructors
        }

        #endregion Private Structs
    }
}