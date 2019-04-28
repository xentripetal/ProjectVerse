using Barebones.Networking;

namespace Barebones.MasterServer {
    /// <summary>
    ///     Observable integer
    /// </summary>
    public class ObservableInt : ObservableBase {
        public ObservableInt(short key, int defaultValue = 0) : base(key) {
            Value = defaultValue;
        }

        public int Value { get; private set; }

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
            Value = EndianBitConverter.Big.ToInt32(data, 0);
        }

        public override string SerializeToString() {
            return Value.ToString();
        }

        public override void DeserializeFromString(string value) {
            Value = int.Parse(value);
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