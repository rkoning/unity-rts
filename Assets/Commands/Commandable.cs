using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rkoning.RTS {
    public class Commandable : MonoBehaviour
    {
        public Queue<Command> commandQueue = new();
        public Command currentCommand;

        public delegate void CommandEvent(Command c);
        public event CommandEvent OnCommandCompleted;
        public event CommandEvent OnNewCommand;

        public void SendCommand(Command c) {
            ClearCommands();
            StartNewCommand(c);
        }

        public void QueueCommand(Command c) {
            commandQueue.Enqueue(c);
        }

        public void ClearCommands() {
            commandQueue.Clear();
        }

        public Command GetCurrentCommand() {
            return currentCommand;
        }

        public void CompleteCommand() {
            OnCommandCompleted?.Invoke(currentCommand);
            if (commandQueue.TryDequeue(out var next)) 
                StartNewCommand(next);
        }

        private void StartNewCommand(Command c) {
            currentCommand = c;
            OnNewCommand(c);
        }
    }
}