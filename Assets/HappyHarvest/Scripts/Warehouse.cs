using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace HappyHarvest
{
    public class Warehouse : InteractiveObject
    {
        public override void InteractedWith()
        {
            UIHandler.OpenWarehouse();
        }
    }
}
