using System;

namespace Barebones.MasterServer {
    /// <summary>
    ///     Represents generic database for profiles
    /// </summary>
    public interface IProfilesDatabase {
        /// <summary>
        ///     Should restore all values of the given profile,
        ///     or not change them, if there's no entry in the database
        /// </summary>
        /// <returns></returns>
        void RestoreProfile(ObservableServerProfile profile, Action doneCallback);

        /// <summary>
        ///     Should save updated profile into database
        /// </summary>
        void UpdateProfile(ObservableServerProfile profile, Action doneCallback);
    }
}