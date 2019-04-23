using UnityEngine;
using Verse.API;
using Verse.API.Events;
using Verse.API.Events.EventBus;

public class UnifiedTesting : MonoBehaviour {
    private static int ia = 0;
    private static int ib = 0;
    private static int ic = 0;

    // Start is called before the first frame update
    void Start() {
        World.EventBus.Post(new TestEvent("Message"));
    }

    [Subscribe(1)]
    public static void Test(TestEvent testEvent) {
        ia++;
        Debug.Log("A");
    }

    [Subscribe(3)]
    public static void Test2(TestEvent testEvent) {
        ib++;
        Debug.Log("B");
    }

    [Subscribe(2)]
    public static void Test3(TestEvent testEvent) {
        ic++;
        Debug.Log("C");
    }
}