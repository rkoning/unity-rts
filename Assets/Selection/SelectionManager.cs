using System.Collections.Generic;

namespace rkoning.RTS {
    public static class SelectionManager
    {
        private readonly static List<Selectable> allSelectables = new();

        public static void Register(Selectable self) {
            allSelectables.Add(self);
        }

        public static void Deregister(Selectable self)  {
            allSelectables.Remove(self);
        }

        // TODO: Turn allSelectables into a list with factions
        public static List<Selectable> GetAllSelectables() {
            return allSelectables;
        }
    }
}