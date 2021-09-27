using UnityEngine;
using UnityEngine.UIElements;

namespace DefaultNamespace.UI {
	public class Counter : MonoBehaviour {
		private Label counterLabel;
		private Button counterButton;

		private int count;

		private void OnEnable() {
			var rootVisualElement = GetComponent<UIDocument>().rootVisualElement;
			counterLabel = rootVisualElement.Q<Label>("counter-label");
			counterButton= rootVisualElement.Q<Button>("counter-button");
			counterButton.RegisterCallback<ClickEvent>(e => IncrementCounter());
		}

		private void IncrementCounter() {
			counterLabel.text = $"Count: {count++}";
		}
	}
}