using System;
using System.Collections.Generic;
using HappyHarvest;
using Template2DCommon;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.UIElements;
#endif

namespace HappyHarvest
{
    /// <summary>
    /// Handle the player inventory. This is fixed size (9 right now)
    /// </summary>
    [Serializable]
    public class InventorySystem
    {
        public const int InventorySize = 9;

        [Serializable]
        public class InventoryEntry
        {
            public Item Item;
            public int StackSize;
        }

        public int EquippedItemIdx { get; private set; }
        public Item EquippedItem => Entries[EquippedItemIdx].Item;

        public InventoryEntry[] Entries = new InventoryEntry[InventorySize];

        public void Init()
        {
            EquippedItemIdx = 0;
        }

        //return true if the object could be used
        public bool UseEquippedObject(Vector3Int target)
        {
            if (EquippedItem == null || !EquippedItem.CanUse(target))
                return false;

            bool used = EquippedItem.Use(target);

            if (used)
            {
                if (EquippedItem.UseSound != null && EquippedItem.UseSound.Length > 0)
                {
                    SoundManager.Instance.PlaySFXAt(GameManager.Instance.Player.transform.position,
                        EquippedItem.UseSound[Random.Range(0, EquippedItem.UseSound.Length)], false);
                }

                if (EquippedItem.Consumable)
                {
                    Entries[EquippedItemIdx].StackSize -= 1;

                    if (Entries[EquippedItemIdx].StackSize == 0)
                    {
                        Entries[EquippedItemIdx].Item = null;
                    }

                    UIHandler.UpdateInventory(this);
                }
            }

            return used;
        }

        // Will return true if we have enough space in the inventory to fit the required amount of the given item.
        public bool CanFitItem(Item newItem, int amount)
        {
            int toFit = amount;

            for (int i = 0; i < InventorySize; ++i)
            {
                if (Entries[i].Item == newItem)
                {
                    int size = newItem.MaxStackSize - Entries[i].StackSize;
                    toFit -= size;

                    if (toFit <= 0)
                        return true;
                }
            }

            for (int i = 0; i < InventorySize; ++i)
            {
                if (Entries[i].Item == null)
                {
                    toFit -= newItem.MaxStackSize;
                    if (toFit <= 0)
                        return true;
                }
            }

            return toFit == 0;
        }

        //will return how much of said item can be fit in the inventory (accounting for already existing stack)
        public int GetMaximumAmountFit(Item item)
        {
            int canFit = 0;
            for (int i = 0; i < InventorySize; ++i)
            {
                if (Entries[i].Item == null)
                {
                    canFit += item.MaxStackSize;
                }
                else if (Entries[i].Item == item)
                {
                    canFit += item.MaxStackSize - Entries[i].StackSize;
                }
            }

            return canFit;
        }

        //Second parameter set to true will ignore full stack (useful for warehouse retrieval that fill first non full stack)
        public int GetIndexOfItem(Item item, bool returnOnlyNotFull)
        {
            for (int i = 0; i < InventorySize; ++i)
            {
                if (Entries[i].Item == item &&
                    (!returnOnlyNotFull || Entries[i].StackSize != Entries[i].Item.MaxStackSize))
                {
                    return i;
                }
            }

            return -1;
        }

        public bool AddItem(Item newItem, int amount = 1)
        {
            int remainingToFit = amount;

            //first we check if there is already that item in the inventory
            for (int i = 0; i < InventorySize; ++i)
            {
                if (Entries[i].Item == newItem && Entries[i].StackSize < newItem.MaxStackSize)
                {
                    int fit = Mathf.Min(newItem.MaxStackSize - Entries[i].StackSize, remainingToFit);
                    Entries[i].StackSize += fit;
                    remainingToFit -= fit;
                    UIHandler.UpdateInventory(this);

                    if (remainingToFit == 0)
                        return true;
                }
            }

            //if we reach here we couldn't fit it in existing stack, so we look for an empty place to fit it
            for (int i = 0; i < InventorySize; ++i)
            {
                if (Entries[i].Item == null)
                {
                    Entries[i].Item = newItem;
                    int fit = Mathf.Min(newItem.MaxStackSize - Entries[i].StackSize, remainingToFit);
                    remainingToFit -= fit;
                    Entries[i].StackSize = fit;

                    UIHandler.UpdateInventory(this);

                    if (remainingToFit == 0)
                        return true;
                }
            }

            //we couldn't had so no space left
            return remainingToFit == 0;
        }

        //return the actual amount removed
        public int Remove(int index, int count)
        {
            if (index < 0 || index >= Entries.Length)
                return 0;

            int amount = Mathf.Min(count, Entries[index].StackSize);

            Entries[index].StackSize -= amount;

            if (Entries[index].StackSize == 0)
            {
                Entries[index].Item = null;
            }

            UIHandler.UpdateInventory(this);
            return amount;
        }

        public void EquipNext()
        {
            EquippedItemIdx += 1;
            if (EquippedItemIdx >= InventorySize) EquippedItemIdx = 0;

            UIHandler.UpdateInventory(this);
        }

        public void EquipPrev()
        {
            EquippedItemIdx -= 1;
            if (EquippedItemIdx < 0) EquippedItemIdx = InventorySize - 1;

            UIHandler.UpdateInventory(this);
        }

        public void EquipItem(int index)
        {
            if (index < 0 || index >= Entries.Length)
                return;

            EquippedItemIdx = index;
            UIHandler.UpdateInventory(this);
        }

        // Save the content of the inventory in the given list.
        public void Save(ref List<InventorySaveData> data)
        {
            foreach (var entry in Entries)
            {
                if (entry.Item != null)
                {
                    data.Add(new InventorySaveData()
                    {
                        Amount = entry.StackSize,
                        ItemID = entry.Item.UniqueID
                    });
                }
                else
                {
                    data.Add(null);
                }
            }
        }

        // Load the content in the given list inside that inventory.
        public void Load(List<InventorySaveData> data)
        {
            for (int i = 0; i < data.Count; ++i)
            {
                if (data[i] != null)
                {
                    Entries[i].Item = GameManager.Instance.ItemDatabase.GetFromID(data[i].ItemID);
                    Entries[i].StackSize = data[i].Amount;
                }
                else
                {
                    Entries[i].Item = null;
                    Entries[i].StackSize = 0;
                }
            }
        }
    }

    [Serializable]
    public class InventorySaveData
    {
        public int Amount;
        public string ItemID;
    }

#if UNITY_EDITOR

    [CustomPropertyDrawer(typeof(InventorySystem))]
    public class InventoryDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var container = new VisualElement();

            container.Add(new Label("Starting Inventory"));

            ListView list = new ListView();
            list.showBoundCollectionSize = false;
            list.bindingPath = nameof(InventorySystem.Entries);
            list.virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight;
            list.style.flexGrow = 1;
            list.reorderable = true;
            list.showAlternatingRowBackgrounds = AlternatingRowBackground.ContentOnly;
            list.showBorder = true;

            container.Add(list);

            return container;
        }
    }

    [CustomPropertyDrawer(typeof(InventorySystem.InventoryEntry))]
    public class InventoryEntryDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var container = new VisualElement();

            var itemProperty = property.FindPropertyRelative(nameof(InventorySystem.InventoryEntry.Item));
            var stackProperty = property.FindPropertyRelative(nameof(InventorySystem.InventoryEntry.StackSize));

            container.style.flexDirection = FlexDirection.Row;

            var itemLabel = new Label($"Item : ");
            itemLabel.style.width = Length.Percent(10);
            var itemField = new PropertyField(itemProperty, "");
            itemField.style.width = Length.Percent(40);
            var stackSizeLabel = new Label("Count : ");
            stackSizeLabel.style.width = Length.Percent(10);
            var stackField = new PropertyField(stackProperty, "");
            stackField.style.width = Length.Percent(40);

            container.Add(itemLabel);
            container.Add(itemField);
            container.Add(stackSizeLabel);
            container.Add(stackField);

            return container;
        }
    }

#endif

}