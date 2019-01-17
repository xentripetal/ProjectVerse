using System.Collections.Generic;
using Verse.API.Models;

namespace UI {
    public static class TileOperationsHandler {
        private static Stack<ITileOperation> _operations = new Stack<ITileOperation>();

        public static void ExecuteOperation(ITileOperation operation) {
            _operations.Push(operation);
            operation.Execute();
        }

        public static void AddTile(Tile tile) {
            var op = new AddTileOperation(tile);
            ExecuteOperation(op);
        }

        public static void DestroyTile(Tile tile) {
            var op = new DestroyTileOperation(tile);
            ExecuteOperation(op);
        }

        public static void Undo() {
            if (_operations.Count == 0)
                return;
            var op = _operations.Pop();
            op.Undo();
        }
    }
}