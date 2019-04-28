using System;

namespace Barebones.MasterServer {
    [Serializable]
    public class HelpBox {
        private string _constructorValue;
        [NonSerialized] public float Height;
        [NonSerialized] public string Text;
        [NonSerialized] public HelpBoxType Type;

        public HelpBox(string text, float height, HelpBoxType type = HelpBoxType.Info) {
            Text = text;
            Height = height;
            Type = type;
        }

        public HelpBox(string text, HelpBoxType type = HelpBoxType.Info) {
            Text = text;
            Height = 40;
            Type = type;
        }

        public HelpBox() {
            Height = 40;
            Text = "";
            Type = HelpBoxType.Info;
        }
    }
}