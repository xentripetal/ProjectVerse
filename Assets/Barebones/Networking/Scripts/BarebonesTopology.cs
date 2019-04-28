using UnityEngine.Networking;

namespace Barebones.Networking {
    public class BarebonesTopology {
        static BarebonesTopology() {
            var config = new ConnectionConfig();
            ReliableChannel = config.AddChannel(QosType.Reliable);
            UnreliableChannel = config.AddChannel(QosType.Unreliable);
            ReliableSequencedChannel = config.AddChannel(QosType.ReliableSequenced);

            Topology = new HostTopology(config, 5000);
        }

        public static HostTopology Topology { get; }

        public static int ReliableChannel { get; }
        public static int UnreliableChannel { get; }
        public static int ReliableSequencedChannel { get; }
    }
}