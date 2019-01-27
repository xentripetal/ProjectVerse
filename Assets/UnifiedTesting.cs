using UnityEngine;
using Verse.API;
using Verse.API.Events;

public class UnifiedTesting : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {
        World.EventBus.Register<TestEvent>(Test2, 2);
        World.EventBus.Register<TestEvent>(Test3, 3);
        World.EventBus.Register<TestEvent>(Test, 1);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            World.EventBus.Post(new TestEvent("TestMessage"));
        }
    }

    private void Test(TestEvent testEvent) {
        Debug.Log("1 - " + testEvent.Message);
    }

    private void Test2(TestEvent testEvent) {
        Debug.Log("2 - " + testEvent.Message);
    }

    private void Test3(TestEvent testEvent) {
        Debug.Log("3 - " + testEvent.Message);
    }
}