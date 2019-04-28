using System;

namespace Barebones.MasterServer {
    public class PeerSecurityExtension {
        public string AesKey;
        public byte[] AesKeyEncrypted;
        public int PermissionLevel;
        public Guid UniqueGuid;
    }
}