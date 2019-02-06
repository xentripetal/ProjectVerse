using UnityEngine;
using Verse.API.Events;

public class UnifiedTesting : MonoBehaviour {
    private int ia = 0;
    private int ib = 0;
    private int ic = 0;

    // Start is called before the first frame update
    void Start() { }

    private void Test(TestEvent testEvent) {
        ia++;
    }

    private void Test2(TestEvent testEvent) {
        ib++;
    }

    private void Test3(TestEvent testEvent) {
        ic++;
    }
}