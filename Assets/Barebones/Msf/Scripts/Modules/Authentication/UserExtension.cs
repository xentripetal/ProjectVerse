using Barebones.Networking;

namespace Barebones.MasterServer {
    /// <summary>
    ///     Instance of this class will be added to
    ///     extensions of a peer who has logged in
    /// </summary>
    public class UserExtension : IUserExtension {
        public UserExtension(IPeer peer) {
            Peer = peer;
        }

        public IPeer Peer { get; }
        public string Username => AccountData.Username;

        public AccountInfoPacket CreateInfoPacket() {
            return new AccountInfoPacket {
                Username = AccountData.Username,
                IsAdmin = AccountData.IsAdmin,
                IsGuest = AccountData.IsGuest,
                Properties = AccountData.Properties
            };
        }

        public void Load(IAccountData accountData) {
            AccountData = accountData;
        }

        public IAccountData AccountData { get; set; }
    }
}