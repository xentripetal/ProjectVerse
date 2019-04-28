using System.Text;

namespace Barebones.MasterServer {
    /// <summary>
    ///     Observable property of type string
    /// </summary>
    public class ObservableString : ObservableBase {
        public ObservableString(short key, string defaultVal = "") : base(key) {
            Value = defaultVal;
        }

        public string Value { get; private set; }

        public void Set(string value) {
            Value = value;
            MarkDirty();
        }

        public override byte[] ToBytes() {
            return Encoding.UTF8.GetBytes(Value);
        }

        public override void FromBytes(byte[] data) {
            Value = Encoding.UTF8.GetString(data);
            MarkDirty();
        }

        public override string SerializeToString() {
            return Value;
        }

        public override void DeserializeFromString(string value) {
            Value = value;
        }

        public override byte[] GetUpdates() {
            return ToBytes();
        }

        public override void ApplyUpdates(byte[] data) {
            FromBytes(data);
        }

        public override void ClearUpdates() {
        }
    }
}