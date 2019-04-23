using System.Xml;
using UnityEditor.VersionControl;
using Verse.API;

namespace DefaultNamespace {
    public class Entity {
        
    }
    
    public class TODO {
        #region Streamable Events

        void OnEntityStreamIn(Entity entity) { }

        void OnEntityStreamOut(Entity entity) { }
        
        void OnEntityCreated(Entity entity) { }
        
        void OnEntityDestroyed(Entity entity) { }

        void OnConnected(Player player) { }
        
        void OnAuthenticated(Player player) { }
        
        void OnLoadRoom() { }
        
        void OnUnloadRoom() { }

        #endregion
    }
}