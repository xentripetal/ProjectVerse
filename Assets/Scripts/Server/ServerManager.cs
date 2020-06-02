using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Threading;
using System.Xml.Linq;
using DarkRift.Server;
using DarkRift.Server.Unity;
using DarkRiftTests.Server;
using UnityEngine;
using Verse.Utilities;
using LogType = DarkRift.LogType;

namespace ProjectVerse.Server {
    public static class ServerManager {
        public static DarkRiftServer Server { get; private set; } = null;
        public static IPAddress IpAddress { get; private set; } = IPAddress.Loopback;
        public static ushort Port { get; private set; } = 4296;

        public static bool Start(IPAddress ipAddress, ushort port) {
            if (Server != null) {
                Debug.LogError("Attempted to create duplicate server.");
                return false;
            }

            var data = GenerateSpawnData(ipAddress, port);
            Server = new DarkRiftServer(data);
            Server.Start();
            if (!Server.Loaded) {
                Debug.LogError("Failed to start server.");
                return false;
            }

            IpAddress = ipAddress;
            Port = port;
            return true;
        }

        private static ServerSpawnData GenerateSpawnData(IPAddress ipAddress, ushort port) {
            //TODO Change to use custom JSON and allow plugins to change spawndata.
            // This is a temporary solution as ServerSpawnData does not like to be configured without its XML.
            // A custom ServerSpawnData class needs to be made.

            var config = File.ReadAllText(Path.Combine(FileConstants.ConfigFolder, FileConstants.ServerConfigFileName));
            ServerSpawnData spawnData = ServerSpawnData.CreateFromXml(XDocument.Parse(config), new NameValueCollection());
            spawnData.DispatcherExecutorThreadID = Thread.CurrentThread.ManagedThreadId;
            spawnData.Listeners.NetworkListeners[0].Address = ipAddress;
            spawnData.Listeners.NetworkListeners[0].Port = port;

            // Inaccessible from XML, set from inspector
            spawnData.EventsFromDispatcher = true;
            
            // Add types
            spawnData.PluginSearch.PluginTypes.AddRange(UnityServerHelper.SearchForPlugins());
            spawnData.PluginSearch.PluginTypes.Add(typeof(UnityConsoleWriter));
            return spawnData;
        } 

        /// <summary>
        /// Immediately closes the server with no communication to the clients or other systems.
        /// There is currently not a non-forceful closure, but it is in progress.
        /// </summary>
        /// <returns>Success of the server closure</returns>
        public static bool ForceClose() {
            if (Server == null) {
                Debug.LogError("Attempted to close non-existing server.");
                return false;
            }

            if (Server.Disposed) {
                Debug.LogError("Attempted to close already closed-existing server.");
                return false;
            }
            
            Server.Dispose();

            if (!Server.Disposed) {
                Debug.LogError("Failed to dispose of server.");
                return false;
            }

            return true;
        }

    }
}