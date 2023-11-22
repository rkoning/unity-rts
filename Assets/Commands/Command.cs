using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace rkoning.RTS {
    public enum CommandType {
        Move
    }

    public abstract class Command
    {
        public CommandType Type { get; set; }
        public bool Done { get; set; }
    }

    public class MoveCommand : Command {
        public Vector3 Target {
            get;
            set;
        }

        public MoveCommand(Vector3 target) {
            Type = CommandType.Move;
            Target = target;
        }
    }
}