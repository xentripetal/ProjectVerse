using Mirror;

namespace Verse
{
    public struct RoomMessage : NetworkMessage
    {
        public string RoomId;
        public RoomOperation Operation;
    }
    
    public enum RoomOperation : byte
    {
        Enter, // Enter the given room as the primary room and hide the previous room. Does not guarantee Preload will have been called
        Preload, // Optionally preload the given room but do not enter it. 
    }
}