using System;
using System.Collections;
using System.Reflection;
using Fasterflect;
using UnityEngine;

namespace Verse.Engine {
	// Are attributes the right way to do this? We need some way to register subscribers and call them in a specific
	// order. I guess we can use events and have some sort of event registration attribute for static handlers
	// while instance handlers can register using this bootstrap system. Though it might be considered a bug
	// to depend on the registration order implementation of events
	[System.AttributeUsage(System.AttributeTargets.Method)]
	public class BootstrapSystemAttribute : System.Attribute {
		public BootstrapSystemAttribute() {
			
		}
	}
	
	/// <summary>
	/// Initialization logic for the entire engine. This and the main camera are the only gameobjects that are directly attached
	/// to the main scene. Everything else will be have been spawned from something <c>Bootstrap</c> has called.
	/// </summary>
	public class Bootstrap : MonoBehaviour {
		public enum Scene {
			MainMenu,
			Editor,
			Playground
		}

		public Scene StartingScene = Scene.MainMenu;
		public void Start() {
			LoadMods();

			// TODO Placeholder code. This will need to be reworked into the a mod loader system that finds assemblies from loaded
			// mods and calls them in order. Should probably also be refactored into some sort of StaticSystemCaller. 
			// Also I don't think System is the right word. 
			var bootstrapSystems =  Assembly.GetAssembly(typeof(Bootstrap)).GetTypes()[0]
				.MethodsWith(Flags.StaticPublic, typeof(BootstrapSystemAttribute));
			
			foreach (var bootstrapSystem in bootstrapSystems) {
				if (bootstrapSystem.Parameters().Count == 0) {
					bootstrapSystem.Call();
				}
			}
		}
		
		

		protected void LoadMods() {
			Debug.Log("Loading mods");
		}
	}
}