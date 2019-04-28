namespace Barebones.Networking {
    /// <summary>
    ///     Generic packet handler
    /// </summary>
    public class PacketHandler : IPacketHandler {
        private readonly IncommingMessageHandler _handler;

        public PacketHandler(short opCode, IncommingMessageHandler handler) {
            OpCode = opCode;
            _handler = handler;
        }

        public short OpCode { get; }

        public void Handle(IIncommingMessage message) {
            _handler.Invoke(message);
        }
    }
}