using UnityEngine;
using UnityEngine.UI;

namespace Barebones.MasterServer {
    /// <summary>
    ///     Represents a view of the lobby team
    /// </summary>
    public class LobbyTeamUi : MonoBehaviour {
        /// <summary>
        ///     If the team name is empty, hides the team header (name)
        /// </summary>
        public bool DisableHeaderIfNameIsEmpty = true;

        public GameObject Header;

        public Button JoinButton;

        public LobbyUi Lobby;

        public LobbyTeamData RawData;

        public bool ShowMinMax = true;
        public Text TeamName;
        public LayoutGroup UsersLayoutGroup;

        /// <summary>
        ///     Name of the team
        /// </summary>
        public string Name { get; protected set; }

        private void Awake() {
            Lobby = Lobby ?? GetComponentInParent<LobbyUi>();
        }

        /// <summary>
        ///     Sets up the team view from the data given
        /// </summary>
        /// <param name="teamName"></param>
        /// <param name="properties"></param>
        public virtual void Setup(string teamName, LobbyTeamData data) {
            RawData = data;

            Name = teamName;
            UpdateName();

            // Toggle header
            if (string.IsNullOrEmpty(teamName) && DisableHeaderIfNameIsEmpty)
                Header.SetActive(false);
            else
                Header.SetActive(true);
        }

        public virtual void UpdateName() {
            var newName = RawData.Name;

            if (ShowMinMax) newName += string.Format(" (min: {0}, max:{1})", RawData.MinPlayers, RawData.MaxPlayers);

            TeamName.text = newName;
        }

        /// <summary>
        ///     Invoked, when user clicks a "Join" button
        /// </summary>
        public virtual void OnJoinClick() {
            var loadingPromise = Msf.Events.FireWithPromise(Msf.EventNames.ShowLoading, "Switching teams");

            Lobby.JoinedLobby.JoinTeam(Name, (successful, error) => {
                loadingPromise.Finish();

                if (!successful) {
                    Msf.Events.Fire(Msf.EventNames.ShowDialogBox,
                        DialogBoxData.CreateError(error));

                    Logs.Error(error);
                }
            });
        }

        /// <summary>
        ///     Resets the team view
        /// </summary>
        public void Reset() {
            Header.gameObject.SetActive(true);
        }
    }
}