using UnityEngine;
using Verse.API.Events;
using Verse.API.Events.EventBus;

namespace Verse.Core.Systems {
    public static class Test {
        [Subscribe]
        public static void TestPost(TestEvent testEvent) {
            Debug.LogError("Test");
        }
    }
}