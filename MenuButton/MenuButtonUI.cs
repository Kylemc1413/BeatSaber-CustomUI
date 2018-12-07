﻿using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using CustomUI.BeatSaber;
using System.Collections;
using UnityEngine.SceneManagement;

namespace CustomUI.MenuButton
{
    public class MenuButtonUI : MonoBehaviour
    {
        private static readonly WaitUntil _bottomPanelExists = new WaitUntil(() => GameObject.Find("MainMenuViewController/BottomPanel"));
        const int ButtonsPerRow = 4;
        const float RowSeparator = 9f;
        
        private RectTransform bottomPanel;
        private RectTransform menuButtonsOriginal;

        private int rowCount = 0;
        private RectTransform currentRow;
        private int buttonsInCurrentRow;

        private static MenuButtonUI _instance = null;
        public static MenuButtonUI Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameObject("MenuButtonUI").AddComponent<MenuButtonUI>();
                    _instance.Init();
                }
                return _instance;
            }
            private set
            {
                _instance = value;
            }
        }

        void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
            SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;
        }

        void OnDestroy()
        {
            SceneManager.activeSceneChanged -= SceneManager_activeSceneChanged;
        }

        private void SceneManager_activeSceneChanged(Scene from, Scene to)
        {
            if (to.name == "EmptyTransition")
            {
                if (Instance)
                {
                    Instance.StopAllCoroutines();
                    Destroy(Instance.gameObject);
                }
                Instance = null;
            }
        }

        private void Init()
        {
            // Find Menu buttons
            buttonsInCurrentRow = ButtonsPerRow;
        }
        
        private RectTransform AddRow()
        {
            RectTransform newRow = RectTransform.Instantiate(menuButtonsOriginal, bottomPanel);
            newRow.name = "CustomMenuButtonsRow" + rowCount.ToString();

            foreach (Transform child in newRow)
            {
                child.name = String.Empty;
                GameObject.Destroy(child.gameObject);
            }
            rowCount++;
            newRow.anchoredPosition += Vector2.up * RowSeparator * rowCount;
            return newRow;
        }

        private static IEnumerator AddButtonDelayed(string text, UnityAction onClick, Sprite icon)
        {
            yield return _bottomPanelExists;

            lock (Instance)
            {
                Instance.bottomPanel = GameObject.Find("MainMenuViewController/BottomPanel").transform as RectTransform;
                Instance.menuButtonsOriginal = Instance.bottomPanel.Find("Buttons") as RectTransform;

                if (Instance.buttonsInCurrentRow >= ButtonsPerRow)
                {
                    Instance.currentRow = Instance.AddRow();
                    Instance.buttonsInCurrentRow = 0;
                }
                Button newButton = BeatSaberUI.CreateUIButton(Instance.currentRow as RectTransform, "QuitButton", onClick, text, icon);
                Instance.buttonsInCurrentRow++;
            }
        }

        public static void AddButton(string buttonText, UnityAction onClick, Sprite icon = null)
        {
            Instance.StartCoroutine(AddButtonDelayed(buttonText, onClick, icon));
        }
    }
}
