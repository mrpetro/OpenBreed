using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenBreed.Common.Logging;

namespace OpenBreed.Common.Commands
{
    public delegate void CommandsUpdatedEventHandler(object sender, CommandsUpdatedEventArgs e);

    public class CommandsUpdatedEventArgs : EventArgs
    {
        public ICommand Command { get; set; }

        public CommandsUpdatedEventArgs(ICommand command)
        {
            Command = command;
        }
    }

    public class CommandMan
    {
        private Stack<ICommand> m_UndoCommands = new Stack<ICommand>();
        private Stack<ICommand> m_RedoCommands = new Stack<ICommand>();

        public bool IsUndoAvailable { get { return m_UndoCommands.Count != 0; } }
        public bool IsRedoAvailable { get { return m_RedoCommands.Count != 0; } }

        public event CommandsUpdatedEventHandler CommandsUpdated;

        public void Redo(int steps = 1)
        {
            for (int i = 1; i <= steps; i++)
            {
                if (m_RedoCommands.Count != 0)
                {
                    ICommand command = m_RedoCommands.Pop();
                    command.Execute();
                    m_UndoCommands.Push(command);
                    OnCommandsUpdated(new CommandsUpdatedEventArgs(command));
                }
            }
        }

        public void Undo(int steps = 1)
        {
            for (int i = 1; i <= steps; i++)
            {
                if (m_UndoCommands.Count != 0)
                {
                    ICommand command = m_UndoCommands.Pop();
                    command.UnExecute();
                    m_RedoCommands.Push(command);
                    OnCommandsUpdated(new CommandsUpdatedEventArgs(command));
                }
            }
        }

        public void ExecuteCommand(ICommand command)
        {
            command.Execute();
            m_UndoCommands.Push(command); m_RedoCommands.Clear();
            OnCommandsUpdated(new CommandsUpdatedEventArgs(command));
        }

        protected virtual void OnCommandsUpdated(CommandsUpdatedEventArgs e)
        {
            if (CommandsUpdated != null) CommandsUpdated(this, e);
        }
    }
}
