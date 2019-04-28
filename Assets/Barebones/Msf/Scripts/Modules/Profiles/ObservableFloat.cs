using Barebones.Networking;

namespace Barebones.MasterServer {
    /// <summary>
    ///     Observable integer
    /// </summary>
    public class ObservableFloat : ObservableBase {
        public ObservableFloat(short key, int defaultValue = 0) : base(key) {
            Value = defaultValue;
        }

        public float Value { get; private set; }

        public void Add(int val) {
            Value += val;
            MarkDirty();
        }

        public void Set(int val) {
            Value = val;
            MarkDirty();
        }

        public bool TryTake(int amount) {
            if (Value >= amount) {
                Add(-amount);
                return true;
            }

            return false;
        }

        public override byte[] ToBytes() {
            var data = new byte[4];
            EndianBitConverter.Big.CopyBytes(Value, data, 0);

            return data;
        }

        public override void FromBytes(byte[] data) {
            Value = EndianBitConverter.Big.ToSingle(data, 0);

            MarkDirty();
        }

        public override string SerializeToString() {
            return Value.ToString();
        }

        public override void DeserializeFromString(string value) {
            Value = float.Parse(value);
        }

        public override byte[] GetUpdates() {
            return ToBytes();
        }

        public override void ApplyUpdates(byte[] data) {
            FromBytes(data);
            MarkDirty();
        }

        public override void ClearUpdates() {
        }
    }
}