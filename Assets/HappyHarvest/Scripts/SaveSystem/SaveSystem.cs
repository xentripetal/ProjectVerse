using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace HappyHarvest
{
    public class SaveSystem
    {
        private static SaveData s_CurrentData = new SaveData();
        
        [System.Serializable]
        public struct SaveData
        {
            public PlayerSaveData PlayerData;
            public DayCycleHandlerSaveData TimeSaveData;
            public SaveData[] ScenesData;
        }

        [System.Serializable]
        public struct SceneData
        {
            public string SceneName;
            public TerrainDataSave TerrainData;
        }

        private static Dictionary<string, SceneData> s_ScenesDataLookup = new Dictionary<string, SceneData>();

        public static void Save()
        {
            GameManager.Instance.Player.Save(ref s_CurrentData.PlayerData);
            GameManager.Instance.DayCycleHandler.Save(ref s_CurrentData.TimeSaveData);

            string savefile = Application.persistentDataPath + "/save.sav";
            File.WriteAllText(savefile, JsonUtility.ToJson(s_CurrentData));
        }

        public static void Load()
        {
            string savefile = Application.persistentDataPath + "/save.sav";
            string content = File.ReadAllText(savefile);

            s_CurrentData = JsonUtility.FromJson<SaveData>(content);
            
            SceneManager.sceneLoaded += SceneLoaded;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
        }

        static void SceneLoaded(Scene scene, LoadSceneMode mode)
        {
            GameManager.Instance.Player.Load(s_CurrentData.PlayerData);
            GameManager.Instance.DayCycleHandler.Load(s_CurrentData.TimeSaveData);
            //GameManager.Instance.Terrain.Load(s_CurrentData.TerrainData);

            SceneManager.sceneLoaded -= SceneLoaded;
        }

        public static void SaveSceneData()
        {
            if (GameManager.Instance.Terrain != null)
            {
                var sceneName = GameManager.Instance.LoadedSceneData.UniqueSceneName;
                var data = new TerrainDataSave();
                GameManager.Instance.Terrain.Save(ref data);

                s_ScenesDataLookup[sceneName] = new SceneData()
                {
                    SceneName = sceneName,
                    TerrainData = data
                };
            }
        }

        public static void LoadSceneData()
        {
            if (s_ScenesDataLookup.TryGetValue(GameManager.Instance.LoadedSceneData.UniqueSceneName, out var data))
            {
                GameManager.Instance.Terrain.Load(data.TerrainData);
            }
        }
    }
}