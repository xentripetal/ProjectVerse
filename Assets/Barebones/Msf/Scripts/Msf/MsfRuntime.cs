namespace Barebones.MasterServer {
    public class MsfRuntime {
        public MsfRuntime() {
#if UNITY_EDITOR
            IsEditor = true;
#else
            IsEditor = false;
#endif
            SupportsThreads = true;
#if !UNITY_EDITOR && (UNITY_WEBGL || !UNITY_WEBPLAYER)
            SupportsThreads = false;
#endif
        }

        public bool IsEditor { get; }

        public bool SupportsThreads { get; }
    }
}