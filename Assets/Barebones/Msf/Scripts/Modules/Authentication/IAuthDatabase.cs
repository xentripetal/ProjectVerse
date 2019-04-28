using System;

namespace Barebones.MasterServer {
    public interface IAuthDatabase {
        /// <summary>
        ///     Should create an empty object with account data.
        /// </summary>
        /// <returns></returns>
        IAccountData CreateAccountObject();

        void GetAccount(string username, Action<IAccountData> callback);
        void GetAccountByToken(string token, Action<IAccountData> callback);
        void GetAccountByEmail(string email, Action<IAccountData> callback);

        void SavePasswordResetCode(IAccountData account, string code, Action doneCallback);
        void GetPasswordResetData(string email, Action<IPasswordResetData> callback);

        void SaveEmailConfirmationCode(string email, string code, Action doneCallback);
        void GetEmailConfirmationCode(string email, Action<string> callback);

        void UpdateAccount(IAccountData account, Action doneCallback);
        void InsertNewAccount(IAccountData account, Action doneCallback);
        void InsertToken(IAccountData account, string token, Action doneCallback);
    }
}