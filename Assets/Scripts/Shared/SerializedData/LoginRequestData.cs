using DarkRift;

namespace ProjectVerse.Shared.SerializedData {
    public class LoginRequestData : IDarkRiftSerializable {
        public string Name;

        public LoginRequestData(string name) {
            Name = name;
        }
        
        public void Deserialize(DeserializeEvent e) {
            Name = e.Reader.ReadString();
        }

        public void Serialize(SerializeEvent e) {
            e.Writer.Write(Name);
        }
    }
}