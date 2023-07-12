using System.IO;
using System.Linq;
using System.Reflection;
using Unity.Entities;
using Unity.NetCode;
using UnityEngine;

namespace Verse.Core {
	public class Bootstrap : ClientServerBootstrap {
		private DirectoryInfo GetModsFolder() {
			var basePath = new DirectoryInfo(Application.persistentDataPath);
		#if UNITY_EDITOR
			basePath = Directory.GetParent(Application.dataPath);
		#endif
			return basePath.GetDirectories().First(x => x.Name == "Mods");
		}

		private void loadMods() {
			var modsFolder = GetModsFolder();
			if (modsFolder != null) {
				var dllsLoaded = false;
				var dirs = modsFolder.GetDirectories();
				foreach (var dir in dirs) {
					Debug.Log("Loading mod " + dir.Name);
					foreach (var file in dir.EnumerateFiles()) {
						if (file.Extension == ".dll") {
							dllsLoaded = true;
							Debug.Log("Loading dll " + file.Name);
							Assembly.LoadFile(file.FullName);
						}
					}
				}

				if (dllsLoaded) {
					TypeManager.Shutdown();
					TypeManager.Initialize();
				}

			} else {
				Debug.Log("No mods directory, skipping mod loading.");
			}
		}

		public override bool Initialize(string defaultWorldName) {
			AutoConnectPort = 7979; // Enabled auto connect
			loadMods();
			return base.Initialize(defaultWorldName);
		}
	}
}