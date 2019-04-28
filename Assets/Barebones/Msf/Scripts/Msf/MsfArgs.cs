using System;
using System.Linq;

namespace Barebones.MasterServer {
    public class MsfArgs {
        private readonly string[] _args;

        public MsfArgNames Names;

        public MsfArgs() {
            _args = Environment.GetCommandLineArgs();

            // Android fix
            if (_args == null)
                _args = new string[0];

            Names = new MsfArgNames();

            StartMaster = IsProvided(Names.StartMaster);
            MasterPort = ExtractValueInt(Names.MasterPort, 5000);
            MasterIp = ExtractValue(Names.MasterIp);
            MachineIp = ExtractValue(Names.MachineIp);
            DestroyUi = IsProvided(Names.DestroyUi);

            SpawnId = ExtractValueInt(Names.SpawnId, -1);
            AssignedPort = ExtractValueInt(Names.AssignedPort, -1);
            SpawnCode = ExtractValue(Names.SpawnCode);
            ExecutablePath = ExtractValue(Names.ExecutablePath);
            DontSpawnInBatchmode = IsProvided(Names.DontSpawnInBatchmode);
            MaxProcesses = ExtractValueInt(Names.MaxProcesses, 0);

            LoadScene = ExtractValue(Names.LoadScene);

            DbConnectionString = ExtractValue(Names.DbConnectionString);

            LobbyId = ExtractValueInt(Names.LobbyId);
            WebGl = IsProvided(Names.WebGl);
        }

        public class MsfArgNames {
            public string StartMaster => "-msfStartMaster";
            public string MasterPort => "-msfMasterPort";
            public string MasterIp => "-msfMasterIp";

            public string StartSpawner => "-msfStartSpawner";

            public string SpawnId => "-msfSpawnId";
            public string SpawnCode => "-msfSpawnCode";
            public string AssignedPort => "-msfAssignedPort";
            public string LoadScene => "-msfLoadScene";
            public string MachineIp => "-msfMachineIp";
            public string ExecutablePath => "-msfExe";
            public string DbConnectionString => "-msfDbConnectionString";
            public string LobbyId => "-msfLobbyId";
            public string DontSpawnInBatchmode => "-msfDontSpawnInBatchmode";
            public string MaxProcesses => "-msfMaxProcesses";
            public string DestroyUi => "-msfDestroyUi";
            public string WebGl => "-msfWebgl";
        }

        #region Arguments

        /// <summary>
        ///     If true, master server should be started
        /// </summary>
        public bool StartMaster { get; }

        /// <summary>
        ///     Port, which will be open on the master server
        /// </summary>
        public int MasterPort { get; }

        /// <summary>
        ///     Ip address to the master server
        /// </summary>
        public string MasterIp { get; }

        /// <summary>
        ///     Public ip of the machine, on which the process is running
        /// </summary>
        public string MachineIp { get; }

        /// <summary>
        ///     If true, some of the Ui game objects will be destroyed.
        ///     (to avoid memory leaks)
        /// </summary>
        public bool DestroyUi { get; }


        /// <summary>
        ///     SpawnId of the spawned process
        /// </summary>
        public int SpawnId { get; }

        /// <summary>
        ///     Port, assigned to the spawned process (most likely a game server)
        /// </summary>
        public int AssignedPort { get; }

        /// <summary>
        ///     Code, which is used to ensure that there's no tampering with
        ///     spawned processes
        /// </summary>
        public string SpawnCode { get; }

        /// <summary>
        ///     Path to the executable (used by the spawner)
        /// </summary>
        public string ExecutablePath { get; }

        /// <summary>
        ///     If true, will make sure that spawned processes are not spawned in batchmode
        /// </summary>
        public bool DontSpawnInBatchmode { get; }

        /// <summary>
        ///     Max number of processes that can be spawned by a spawner
        /// </summary>
        public int MaxProcesses { get; }

        /// <summary>
        ///     Name of the scene to load
        /// </summary>
        public string LoadScene { get; }

        /// <summary>
        ///     Database connection string (user by some of the database implementations)
        /// </summary>
        public string DbConnectionString { get; }

        /// <summary>
        ///     LobbyId, which is assigned to a spawned process
        /// </summary>
        public int LobbyId { get; }

        /// <summary>
        ///     If true, it will be considered that we want to start server to
        ///     support webgl clients
        /// </summary>
        public bool WebGl { get; }

        #endregion

        #region Helper methods

        /// <summary>
        ///     Extracts a value for command line arguments provided
        /// </summary>
        /// <param name="argName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public string ExtractValue(string argName, string defaultValue = null) {
            if (!_args.Contains(argName))
                return defaultValue;

            var index = _args.ToList().FindIndex(0, a => a.Equals(argName));
            return _args[index + 1];
        }

        public int ExtractValueInt(string argName, int defaultValue = -1) {
            var number = ExtractValue(argName, defaultValue.ToString());
            return Convert.ToInt32(number);
        }

        public bool IsProvided(string argName) {
            return _args.Contains(argName);
        }

        #endregion
    }
}