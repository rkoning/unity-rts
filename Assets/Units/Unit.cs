using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Unity.VisualScripting;

namespace rkoning.RTS {

    public enum UnitState {
        Moving,
        Attacking,
        Idle
    }

    public class Unit : MonoBehaviour
    {
        private Seeker seeker;
        private AIPath aIPath;
        private Commandable commandable;
        private Command currentCommand;
        public UnitState state = UnitState.Idle;

        // Start is called before the first frame update
        void Start()
        {
            seeker = GetComponent<Seeker>();
            aIPath = GetComponent<AIPath>();
            commandable = GetComponent<Commandable>();
            commandable.OnNewCommand += InterpretCommand;
        }

        // Update is called once per frame
        void Update()
        {
            if (currentCommand == null)
                return;
                
            if (aIPath.reachedEndOfPath && currentCommand.Type == CommandType.Move) {
                commandable.CompleteCommand();
            }
            switch (state) {
                case UnitState.Attacking:
                    break;
                case UnitState.Moving:
                    break;
                default:
                    return;
            }
        }

        public void InterpretCommand(Command c) {
            switch(c.Type) {
                case CommandType.Move:
                    seeker.StartPath(transform.position, (c as MoveCommand).Target);
                    currentCommand = c;
                    break;
                default:
                    Debug.LogWarning($"Invalid Command: {c.Type} for Unit {name}");
                    break;
            }
        }
    }
}
