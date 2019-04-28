using System.Collections.Generic;
using Barebones.Networking;

namespace Barebones.MasterServer {
    /// <summary>
    ///     List of options, which are sent to master server during registration
    /// </summary>
    public class RoomOptions : SerializablePacket {
        /// <summary>
        ///     Number of seconds, after which unconfirmed (pending) accesses will removed
        ///     to allow new players. Make sure it's long enought to allow player to load gameplay scene
        /// </summary>
        public float AccessTimeoutPeriod = 10;

        /// <summary>
        ///     If set to false, users will no longer be able to request access directly.
        ///     This is useful when you want players to get accesses through other means, for example
        ///     through Lobby module,
        /// </summary>
        public bool AllowUsersRequestAccess = true;

        /// <summary>
        ///     If true, room will appear in public listings
        /// </summary>
        public bool IsPublic;

        /// <summary>
        ///     If 0 - player number is not limited
        /// </summary>
        public int MaxPlayers;

        /// <summary>
        ///     Name of the room
        /// </summary>
        public string Name = "Unnamed";

        /// <summary>
        ///     Room password
        /// </summary>
        public string Password = "";

        /// <summary>
        ///     Extra properties that you might want to send to master server
        /// </summary>
        public Dictionary<string, string> Properties;

        /// <summary>
        ///     IP of the machine on which the room was created
        ///     (Only used in the <see cref="RoomController.DefaultAccessProvider" />)
        /// </summary>
        public string RoomIp = "";

        /// <summary>
        ///     Port, required to access the room
        ///     (Only used in the <see cref="RoomController.DefaultAccessProvider" />)
        /// </summary>
        public int RoomPort = -1;

        public override void ToBinaryWriter(EndianBinaryWriter writer) {
            writer.Write(Name);
            writer.Write(RoomIp);
            writer.Write(RoomPort);
            writer.Write(IsPublic);
            writer.Write(MaxPlayers);
            writer.Write(Password);
            writer.Write(AccessTimeoutPeriod);
            writer.Write(AllowUsersRequestAccess);
            writer.Write(Properties);
        }

        public override void FromBinaryReader(EndianBinaryReader reader) {
            Name = reader.ReadString();
            RoomIp = reader.ReadString();
            RoomPort = reader.ReadInt32();
            IsPublic = reader.ReadBoolean();
            MaxPlayers = reader.ReadInt32();
            Password = reader.ReadString();
            AccessTimeoutPeriod = reader.ReadSingle();
            AllowUsersRequestAccess = reader.ReadBoolean();
            Properties = reader.ReadDictionary();
        }
    }
}