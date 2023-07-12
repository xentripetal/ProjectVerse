using UnityEngine;


namespace HappyHarvest
{
    [CreateAssetMenu(fileName = "SeedBag", menuName = "2D Farming/Items/SeedBag")]
    public class SeedBag : Item
    {
        public Crop PlantedCrop;

        public override bool CanUse(Vector3Int target)
        {
            return GameManager.Instance.Terrain.IsPlantable(target);
        }

        public override bool Use(Vector3Int target)
        {
            GameManager.Instance.Terrain.PlantAt(target, PlantedCrop);
            return true;
        }
    }
}
