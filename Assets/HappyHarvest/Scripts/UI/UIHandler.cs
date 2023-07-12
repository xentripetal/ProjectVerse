using System;
using System.Collections.Generic;
using Template2DCommon;
using UnityEngine;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;


namespace HappyHarvest
{
    /// <summary>
    /// Handle everything related to the main gameplay UI. Will retrieve all the UI Element and contains various static
    /// functions that updates/change the UI so they can be called from any other class interacting with the UI.
    /// </summary>
    public class UIHandler : MonoBehaviour
    {
        protected static UIHandler s_Instance;

        public enum CursorType
        {
            Normal,
            Interact,
            System
        }
        
        [Header("Cursor")]
        public Texture2D NormalCursor;
        public Texture2D InteractCursor;

        [Header("UI Document")]
        public VisualTreeAsset MarketEntryTemplate;
        
        [Header("Sounds")] 
        public AudioClip MarketSellSound;
        
        protected UIDocument m_Document;
        
        protected List<VisualElement> m_InventorySlots;
        protected List<Label> m_ItemCountLabels;

        protected Label m_CointCounter;

        protected VisualElement m_MarketPopup;
        protected VisualElement m_MarketContentScrollview;

        protected Label m_TimerLabel;

        protected Button m_BuyButton;
        protected Button m_SellButton;

        protected bool m_HaveFocus = true;
        protected CursorType m_CurrentCursorType;

        protected SettingMenu m_SettingMenu;
        protected WarehouseUI m_WarehouseUI;

        // Fade to balck helper
        protected VisualElement m_Blocker;
        protected System.Action m_FadeFinishClbk;
        
        private Label m_SunLabel;
        private Label m_RainLabel;
        private Label m_ThunderLabel;

        void Awake()
        {
            s_Instance = this;

            m_Document = GetComponent<UIDocument>();

            m_InventorySlots = m_Document.rootVisualElement.Query<VisualElement>("InventoryEntry").ToList();
            m_ItemCountLabels = m_Document.rootVisualElement.Query<Label>("ItemCount").ToList();

            for (int i = 0; i < m_InventorySlots.Count; ++i)
            {
                var i1 = i;
                m_InventorySlots[i].AddManipulator(new Clickable(() =>
                {
                    GameManager.Instance.Player.ChangeEquipItem(i1);
                }));
            }

            Debug.Assert(m_InventorySlots.Count == InventorySystem.InventorySize,
                "Not enough items slots in the UI for inventory");

            m_CointCounter = m_Document.rootVisualElement.Q<Label>("CoinAmount");

            m_MarketPopup = m_Document.rootVisualElement.Q<VisualElement>("MarketPopup");
            m_MarketPopup.Q<Button>("CloseButton").clicked += CloseMarket;
            m_MarketPopup.visible = false;

            m_BuyButton = m_MarketPopup.Q<Button>("BuyButton");
            m_BuyButton.clicked += ToggleToBuy;
            m_SellButton = m_MarketPopup.Q<Button>("SellButton");
            m_SellButton.clicked += ToggleToSell;

            m_MarketContentScrollview = m_MarketPopup.Q<ScrollView>("ContentScrollView");

            m_TimerLabel = m_Document.rootVisualElement.Q<Label>("Timer");

            m_SettingMenu = new SettingMenu(m_Document.rootVisualElement);
            m_SettingMenu.OnOpen += () => { GameManager.Instance.Pause(); };
            m_SettingMenu.OnClose += () => { GameManager.Instance.Resume(); };

            m_WarehouseUI = new WarehouseUI(m_Document.rootVisualElement.Q<VisualElement>("WarehousePopup"), MarketEntryTemplate);

            m_Blocker = m_Document.rootVisualElement.Q<VisualElement>("Blocker");
            
            m_Blocker.style.opacity = 1.0f;
            m_Blocker.schedule.Execute(() => { FadeFromBlack(() => { }); }).ExecuteLater(500);

            m_Blocker.RegisterCallback<TransitionEndEvent>(evt =>
            {
                m_FadeFinishClbk?.Invoke();
            });

            m_SunLabel = m_Document.rootVisualElement.Q<Label>("SunLabel");
            m_RainLabel = m_Document.rootVisualElement.Q<Label>("RainLabel");
            m_ThunderLabel = m_Document.rootVisualElement.Q<Label>("ThunderLabel");
            
            m_SunLabel.AddManipulator(new Clickable(() => { GameManager.Instance.WeatherSystem?.ChangeWeather(WeatherSystem.WeatherType.Sun); }));
            m_RainLabel.AddManipulator(new Clickable(() => { GameManager.Instance.WeatherSystem?.ChangeWeather(WeatherSystem.WeatherType.Rain); }));
            m_ThunderLabel.AddManipulator(new Clickable(() => { GameManager.Instance.WeatherSystem?.ChangeWeather(WeatherSystem.WeatherType.Thunder); }));
        }
        
        
        void Update()
        {
            m_TimerLabel.text = GameManager.Instance.CurrentTimeAsString();
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            m_HaveFocus = hasFocus;
            if(!hasFocus)
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            else
                ChangeCursor(m_CurrentCursorType);
        }

        //Need to be called by the player everytime the inventory change.
        public static void UpdateInventory(InventorySystem system)
        {
            s_Instance.UpdateInventory_Internal(system);
        }

        public static void UpdateCoins(int amount)
        {
            s_Instance.UpdateCoins_Internal(amount);
        }

        public static void OpenMarket()
        {
           s_Instance.OpenMarket_Internal();
           GameManager.Instance.Pause();
        }

        public static void CloseMarket()
        {
            SoundManager.Instance.PlayUISound();
            s_Instance.m_MarketPopup.visible = false;
            GameManager.Instance.Resume();
        }

        public static void OpenWarehouse()
        {
            s_Instance.m_WarehouseUI.Open();
        }

        public static void ChangeCursor(CursorType cursorType)
        {
            if (s_Instance.m_HaveFocus)
            {
                switch (cursorType)
                {
                    case CursorType.Interact:
                        Cursor.SetCursor(s_Instance.InteractCursor, Vector2.zero, CursorMode.Auto);
                        break;
                    case CursorType.Normal:
                        Cursor.SetCursor(s_Instance.NormalCursor, Vector2.zero, CursorMode.Auto);
                        break;
                    case CursorType.System:
                        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
                        break;
                }
            }

            s_Instance.m_CurrentCursorType = cursorType;
        }

        public static void UpdateWeatherIcons(WeatherSystem.WeatherType currentWeather)
        {
            s_Instance.m_SunLabel.EnableInClassList("on-button", currentWeather == WeatherSystem.WeatherType.Sun);
            s_Instance.m_RainLabel.EnableInClassList("on-button", currentWeather == WeatherSystem.WeatherType.Rain);
            s_Instance.m_ThunderLabel.EnableInClassList("on-button", currentWeather == WeatherSystem.WeatherType.Thunder);
            
            s_Instance.m_SunLabel.EnableInClassList("off-button", currentWeather != WeatherSystem.WeatherType.Sun);
            s_Instance.m_RainLabel.EnableInClassList("off-button", currentWeather != WeatherSystem.WeatherType.Rain);
            s_Instance.m_ThunderLabel.EnableInClassList("off-button", currentWeather != WeatherSystem.WeatherType.Thunder);
        }

        public static void SceneLoaded()
        {
            //we hide the weather control if there is no weather sytsem in that scene
            s_Instance.m_SunLabel.parent.style.display =
                GameManager.Instance.WeatherSystem == null ? DisplayStyle.None : DisplayStyle.Flex;
        }

        private void OpenMarket_Internal()
        {
            m_MarketPopup.visible = true;
            
            //we open the Sell Tab by default
            ToggleToSell();

            GameManager.Instance.Player.ToggleControl(false);
        }

        private void ToggleToSell()
        {
            m_SellButton.AddToClassList("activeButton");
            m_BuyButton.RemoveFromClassList("activeButton");

            m_SellButton.SetEnabled(false);
            m_BuyButton.SetEnabled(true);
            
            //clear all the existing entry. A good target for optimization if profiling show bad perf in UI is to pool
            //instead of delete/recreate entries
            m_MarketContentScrollview.contentContainer.Clear();

            for (int i = 0; i < GameManager.Instance.Player.Inventory.Entries.Length; ++i)
            {
                var item = GameManager.Instance.Player.Inventory.Entries[i].Item;
                if (item == null)
                    continue;

                var clone = MarketEntryTemplate.CloneTree();

                clone.Q<Label>("ItemName").text = item.DisplayName;
                clone.Q<VisualElement>("ItemIcone").style.backgroundImage = new StyleBackground(item.ItemSprite);

                var button = clone.Q<Button>("ActionButton");

                if (item is Product product)
                {
                    int count = GameManager.Instance.Player.Inventory.Entries[i].StackSize;
                    button.text = $"Sell {count} for {product.SellPrice * count}";
                    
                    int i1 = i;
                    button.clicked += () =>
                    {
                        GameManager.Instance.Player.SellItem(i1, count);
                        //we remove this entry, we just sold it.
                        m_MarketContentScrollview.contentContainer.Remove(clone.contentContainer);
                    };
                }
                else
                {
                    button.SetEnabled(false);
                    button.text = "Cannot Sell";
                }
                
                m_MarketContentScrollview.Add(clone.contentContainer);
            }
        }
        
        private void ToggleToBuy()
        {
            m_SellButton.RemoveFromClassList("activeButton");
            m_BuyButton.AddToClassList("activeButton");
            
            m_BuyButton.SetEnabled(false);
            m_SellButton.SetEnabled(true);
            
            //clear all the existing entry. A good target for optimization if profiling show bad perf in UI is to pool
            //instead of delete/recreate entries
            m_MarketContentScrollview.contentContainer.Clear();

            for (int i = 0; i < GameManager.Instance.MarketEntries.Length; ++i)
            {
                var item = GameManager.Instance.MarketEntries[i];

                var clone = MarketEntryTemplate.CloneTree();

                clone.Q<Label>("ItemName").text = item.DisplayName;
                clone.Q<VisualElement>("ItemIcone").style.backgroundImage = new StyleBackground(item.ItemSprite);
                
                var button = clone.Q<Button>("ActionButton");

                if (GameManager.Instance.Player.Coins >= item.BuyPrice)
                {
                    button.text = $"Buy 1 for {item.BuyPrice}";
                    int i1 = i;
                    button.clicked += () =>
                    {
                        if (GameManager.Instance.Player.BuyItem(item))
                        {
                            if (GameManager.Instance.Player.Coins < item.BuyPrice)
                            {
                                button.text = $"Cannot afford cost of {item.BuyPrice}";
                                button.SetEnabled(false);
                            }
                        }
                    };
                    button.SetEnabled(true);
                }
                else
                {
                    button.text = $"Cannot afford cost of {item.BuyPrice}";
                    button.SetEnabled(false);
                }
                
                m_MarketContentScrollview.Add(clone.contentContainer);
            }
        }

        public static void PlayBuySellSound(Vector3 location)
        {
            SoundManager.Instance.PlaySFXAt(location, s_Instance.MarketSellSound, false);
        }

        public static void FadeToBlack(System.Action onFinished)
        {
            s_Instance.m_FadeFinishClbk = onFinished;

            s_Instance.m_Blocker.schedule.Execute(() =>
            {
                s_Instance.m_Blocker.style.opacity = 1.0f;
            }).ExecuteLater(10);
        }

        public static void FadeFromBlack(System.Action onFinished)
        {
            s_Instance.m_FadeFinishClbk = onFinished;
            
            s_Instance.m_Blocker.schedule.Execute(() =>
            {
                s_Instance.m_Blocker.style.opacity = 0.0f;
            }).ExecuteLater(10);
        }

        private void UpdateCoins_Internal(int amount)
        {
            m_CointCounter.text = amount.ToString();
        }

        private void UpdateInventory_Internal(InventorySystem system)
        {
            for (int i = 0; i < system.Entries.Length; ++i)
            {
                var item = system.Entries[i].Item;
                m_InventorySlots[i][0].style.backgroundImage =
                    item == null ? new StyleBackground((Sprite)null) : new StyleBackground(item.ItemSprite);

                if (item == null || system.Entries[i].StackSize < 2)
                {
                    m_ItemCountLabels[i].style.visibility = Visibility.Hidden;
                }
                else
                {
                    m_ItemCountLabels[i].style.visibility = Visibility.Visible;
                    m_ItemCountLabels[i].text = system.Entries[i].StackSize.ToString();
                }


                if (system.EquippedItemIdx == i)
                {
                    m_InventorySlots[i].AddToClassList("equipped");
                }
                else
                {
                    m_InventorySlots[i].RemoveFromClassList("equipped");
                }
            }
        }
    }
}