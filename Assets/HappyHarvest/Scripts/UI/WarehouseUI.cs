using System.Collections;
using System.Collections.Generic;
using Template2DCommon;
using UnityEngine;
using UnityEngine.UIElements;

namespace HappyHarvest
{
    /// <summary>
    /// Handle the WarehouseUI that handle storing/retrieving object.
    /// </summary>
    public class WarehouseUI
    {
        private VisualElement m_Root;

        private VisualTreeAsset m_EntryTemplate;

        private Button m_StoreButton;
        private Button m_RetrieveButton;

        private ScrollView m_Scrollview;

        public WarehouseUI(VisualElement root, VisualTreeAsset entryTemplate)
        {
            m_Root = root;

            m_EntryTemplate = entryTemplate;

            m_StoreButton = m_Root.Q<Button>("StoreButton");
            m_StoreButton.clicked += OpenStore;

            m_RetrieveButton = m_Root.Q<Button>("RetrieveButton");
            m_RetrieveButton.clicked += OpenRetrieve;
            
            m_Root.Q<Button>("CloseButton").clicked += Close;

            m_Scrollview = m_Root.Q<ScrollView>("ContentScrollView");
        }

        public void Open()
        {
            m_Root.visible = true;
            GameManager.Instance.Pause();
            SoundManager.Instance.PlayUISound();
        }

        public void Close()
        {
            m_Root.visible = false;
            GameManager.Instance.Resume();
        }

        void OpenStore()
        {
            m_StoreButton.AddToClassList("activeButton");
            m_RetrieveButton.RemoveFromClassList("activeButton");

            m_StoreButton.SetEnabled(false);
            m_RetrieveButton.SetEnabled(true);

            m_Scrollview.contentContainer.Clear();

            var player = GameManager.Instance.Player;

            for(var i = 0; i < player.Inventory.Entries.Length; ++i)
            {
                var invEntry = player.Inventory.Entries[i];
                var item = invEntry.Item;
                
                if(item == null) continue;

                var entry = m_EntryTemplate.CloneTree();
                entry.Q<Label>("ItemName").text = item.DisplayName;
                entry.Q<VisualElement>("ItemIcone").style.backgroundImage = new StyleBackground(item.ItemSprite);
                
                var button = entry.Q<Button>("ActionButton");
                var i1 = i;
                button.clicked += () =>
                {
                    GameManager.Instance.Storage.Store(invEntry);
                    player.Inventory.Remove(i1, invEntry.StackSize);
                    m_Scrollview.contentContainer.Remove(entry);
                };

                button.text = "Store";

                m_Scrollview.contentContainer.Add(entry);
            }
        }

        void OpenRetrieve()
        {
            m_RetrieveButton.AddToClassList("activeButton");
            m_StoreButton.RemoveFromClassList("activeButton");

            m_RetrieveButton.SetEnabled(false);
            m_StoreButton.SetEnabled(true);
            
            m_Scrollview.contentContainer.Clear();

            var storage = GameManager.Instance.Storage;
            var inventory = GameManager.Instance.Player.Inventory;
            
            for (var i = 0; i < storage.Content.Count; ++i)
            {
                var entry = storage.Content[i];
                
                //we keep empty stack in the storage to avoid modifying the list too often, but we don't need to show
                //them to the retrieve UI as we cannot retrieve 0 thing
                if(entry.StackSize == 0)
                    continue;
                
                var retrieveEntry = m_EntryTemplate.CloneTree();
                retrieveEntry.userData = entry.Item;
                
                var itemLabel = retrieveEntry.Q<Label>("ItemName"); 
                itemLabel.text = entry.Item.DisplayName + $"(x{entry.StackSize})";
                retrieveEntry.Q<VisualElement>("ItemIcone").style.backgroundImage = new StyleBackground(entry.Item.ItemSprite);
                
                var button = retrieveEntry.Q<Button>("ActionButton");
                var i1 = i;
                button.clicked += () =>
                {
                    var retrieveAmount = Mathf.Min(entry.StackSize, entry.Item.MaxStackSize);
                    var existing = inventory.GetIndexOfItem(entry.Item, true);
                    if (existing != -1)
                    {//we have a non empty stack, so fill that one first
                        retrieveAmount -= inventory.Entries[existing].StackSize;
                    }

                    if (retrieveAmount > 0)
                    {
                        GameManager.Instance.Storage.Retrieve(i1, retrieveAmount);
                        inventory.AddItem(entry.Item, retrieveAmount);
                        
                        if (GameManager.Instance.Storage.Content[i1].StackSize == 0)
                        {
                            m_Scrollview.Remove(retrieveEntry);
                        }
                        else
                        {
                            itemLabel.text = entry.Item.DisplayName + $"(x{entry.StackSize})";
                        }
                        
                        //update all remaining entry (disable retrieve button if inventory full, update count)
                        foreach (var child in m_Scrollview.contentContainer.Children())
                        {
                            var entryItem = child.userData as Item;
                            var entryButton = child.Q<Button>("ActionButton");

                            if (inventory.GetMaximumAmountFit(entryItem) == 0)
                            {
                                entryButton.text = "Inventory Full";
                                entryButton.SetEnabled(false);
                            }
                        }
                    }
                };

                button.text = "Retrieve";

                //if we cannot fit even 1 of those item, the retrieve button is disabled
                if (!inventory.CanFitItem(entry.Item, 1))
                {
                    button.text = "Inventory Full";
                    button.SetEnabled(false);
                }
                
                m_Scrollview.Add(retrieveEntry);
            }
        }
    }
}
