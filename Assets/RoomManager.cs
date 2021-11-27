using System;
using System.Management.Instrumentation;
using UnityEngine;

namespace Verse {
	public class RoomManager : MonoBehaviour {
		public static RoomManager Instance;

		public void Awake() {
			if (Instance != null) {
				throw new InvalidOperationException("RoomManager already exists");
            }
		}
	}
}