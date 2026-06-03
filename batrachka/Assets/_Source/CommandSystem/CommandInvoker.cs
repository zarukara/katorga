using System.Collections.Generic;
using UnityEngine;

namespace CommandSystem
{
    public class CommandInvoker
    {
        private readonly Queue<ICommand> commandQueue = new Queue<ICommand>();
        private readonly Stack<ICommand> commandHistory = new Stack<ICommand>();

        private readonly int maxHistoryCount;
        private readonly bool showDebugLogs;

        public CommandInvoker(int maxHistoryCount = 30, bool showDebugLogs = false)
        {
            this.maxHistoryCount = maxHistoryCount;
            this.showDebugLogs = showDebugLogs;
        }

        public void AddCommand(ICommand command)
        {
            if (command == null)
            {
                return;
            }

            commandQueue.Enqueue(command);
        }

        public void ProcessCommands()
        {
            while (commandQueue.Count > 0)
            {
                ICommand command = commandQueue.Dequeue();

                command.Execute();

                commandHistory.Push(command);

                TrimHistory();

                if (showDebugLogs)
                {
                    Debug.Log("Command executed: " + command.GetType().Name);
                }
            }
        }

        public void UndoLastCommand()
        {
            if (commandHistory.Count == 0)
            {
                if (showDebugLogs)
                {
                    Debug.Log("Command history is empty");
                }

                return;
            }

            ICommand command = commandHistory.Pop();

            command.Undo();

            if (showDebugLogs)
            {
                Debug.Log("Command undone: " + command.GetType().Name);
            }
        }

        private void TrimHistory()
        {
            if (commandHistory.Count <= maxHistoryCount)
            {
                return;
            }

            Stack<ICommand> temporaryStack = new Stack<ICommand>();

            while (commandHistory.Count > 0)
            {
                temporaryStack.Push(commandHistory.Pop());
            }

            temporaryStack.Pop();

            while (temporaryStack.Count > 0)
            {
                commandHistory.Push(temporaryStack.Pop());
            }
        }
    }
}