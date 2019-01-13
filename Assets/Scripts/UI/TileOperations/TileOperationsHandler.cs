using System.Collections.Generic;
using UnityEngine;
using Verse.API.Models;

namespace UI {
    public class TileOperationsHandler : MonoBehaviour {
        private Stack<ITileOperation> _operations = new Stack<ITileOperation>();

        public void ExecuteOperation(ITileOperation operation) {
            _operations.Push(operation);
            operation.Execute();
        }

        public void AddTile(Tile tile) {
            var op = new AddTileOperation(tile);
            ExecuteOperation(op);
        }

        public void DestroyTile(Tile tile) {
            var op = new DestroyTileOperation(tile);
            ExecuteOperation(op);
        }

        public void Undo() {
            if (_operations.Count == 0)
                return;
            var op = _operations.Pop();
            op.Undo();
        }
    }
}