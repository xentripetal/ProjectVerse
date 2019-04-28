namespace Verse.API.Events {
    public class FrameUpdateEvent {
    }

    public class TestEvent {
        public readonly string Message;

        public TestEvent(string message) {
            Message = message;
        }
    }
}