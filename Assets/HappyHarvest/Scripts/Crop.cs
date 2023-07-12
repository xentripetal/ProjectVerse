using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.VFX;

namespace HappyHarvest
{
    /// <summary>
    /// Class used to designated a crop on the map. Store all the stage of growth, time to grow etc.
    /// </summary>
    [CreateAssetMenu(fileName = "Crop", menuName = "2D Farming/Crop")]
    public class Crop : ScriptableObject, IDatabaseEntry
    {
        public string Key => UniqueID;

        public string UniqueID = "";
        
        public TileBase[] GrowthStagesTiles;

        public Product Produce;
        
        public float GrowthTime = 1.0f;
        public int NumberOfHarvest = 1;
        public int StageAfterHarvest = 1;
        public int ProductPerHarvest = 1;
        public float DryDeathTimer = 30.0f;
        public VisualEffect HarvestEffect;

        public int GetGrowthStage(float growRatio)
        {
            return Mathf.FloorToInt(growRatio * (GrowthStagesTiles.Length-1));
        }
    }
}
