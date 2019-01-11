﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace CustomUI.BeatSaber
{
    class CustomUIKeyboard : MonoBehaviour
    {
        public event Action<char> textKeyWasPressedEvent;
        public event Action deleteButtonWasPressedEvent;
        public event Action cancelButtonWasPressedEvent;
        public event Action okButtonWasPressedEvent;

        public bool HideCancelButton { get { return hideCancelButton; } set { hideCancelButton = value; _cancelButton.gameObject.SetActive(!value); } }
        public bool OkButtonInteractivity { get { return okButtonInteractivity; } set { okButtonInteractivity = value; _okButton.interactable = value; } }

        private bool okButtonInteractivity;
        private bool hideCancelButton;

        TextMeshProButton _keyButtonPrefab;
        Button _cancelButton;
        Button _okButton;


        public void Awake()
        {
            _keyButtonPrefab = Resources.FindObjectsOfTypeAll<TextMeshProButton>().First(x => x.name == "KeyboardButton");
            
            string[] array = new string[]
            {
                "q",
                "w",
                "e",
                "r",
                "t",
                "y",
                "u",
                "i",
                "o",
                "p",
                "a",
                "s",
                "d",
                "f",
                "g",
                "h",
                "j",
                "k",
                "l",
                "z",
                "x",
                "c",
                "v",
                "b",
                "n",
                "m",
                "<-",
                "space",
                "OK",
                "Cancel"
            };

            for (int i = 0; i < array.Length; i++)
            {
                RectTransform parent = transform.GetChild(i) as RectTransform;
                //TextMeshProButton textMeshProButton = Instantiate(_keyButtonPrefab, parent);
                TextMeshProButton textMeshProButton = parent.GetComponentInChildren<TextMeshProButton>();
                textMeshProButton.text.text = array[i];
                RectTransform rectTransform = textMeshProButton.transform as RectTransform;
                rectTransform.localPosition = Vector2.zero;
                rectTransform.localScale = Vector3.one;
                rectTransform.anchoredPosition = Vector2.zero;
                rectTransform.anchorMin = Vector2.zero;
                rectTransform.anchorMax = Vector3.one;
                rectTransform.offsetMin = Vector2.zero;
                rectTransform.offsetMax = Vector2.zero;
                Navigation navigation = textMeshProButton.button.navigation;
                navigation.mode = Navigation.Mode.None;
                textMeshProButton.button.navigation = navigation;
                textMeshProButton.button.onClick.RemoveAllListeners();
                if (i < array.Length - 4)
                {
                    string key = array[i];
                    textMeshProButton.button.onClick.AddListener(delegate ()
                    {
                        textKeyWasPressedEvent?.Invoke(key[0]);
                    });
                }
                else if (i == array.Length - 4)
                {
                    textMeshProButton.button.onClick.AddListener(delegate ()
                    {
                        deleteButtonWasPressedEvent?.Invoke();
                    });
                }
                else if (i == array.Length - 1)
                {
                    (textMeshProButton.transform as RectTransform).sizeDelta = new Vector2(7f, 1.5f);
                    _cancelButton = textMeshProButton.button;
                    _cancelButton.gameObject.SetActive(!HideCancelButton);
                    textMeshProButton.button.onClick.AddListener(delegate ()
                    {
                        cancelButtonWasPressedEvent?.Invoke();
                    });
                }
                else if (i == array.Length - 2)
                {
                    _okButton = textMeshProButton.button;
                    _okButton.interactable = OkButtonInteractivity;
                    textMeshProButton.button.onClick.AddListener(delegate ()
                    {
                        okButtonWasPressedEvent?.Invoke();
                    });
                }
                else
                {
                    textMeshProButton.button.onClick.AddListener(delegate ()
                    {
                        textKeyWasPressedEvent?.Invoke(' ');
                    });
                }
            }

            name = "CustomUIKeyboard";

            (transform as RectTransform).anchoredPosition -= new Vector2(0f, 0f);

            for (int i = 1; i <= 11; i++)
            {
                TextMeshProButton textButton = Instantiate(_keyButtonPrefab);
                string key = i == 11 ? "_" : i.ToString().Last().ToString();

                textButton.text.text = key;
                textButton.button.onClick.AddListener(delegate ()
                {
                    textKeyWasPressedEvent?.Invoke(key[0]);
                });

                RectTransform buttonRect = textButton.GetComponent<RectTransform>();
                RectTransform component2 = transform.GetChild(i == 11? 9 : i - 1).gameObject.GetComponent<RectTransform>();

                RectTransform buttonHolder = Instantiate(component2, component2.parent, false);
                Destroy(buttonHolder.GetComponentInChildren<Button>().gameObject);

                buttonHolder.anchoredPosition -= new Vector2(i == 11 ? -5f : 5f, -10f);

                buttonRect.SetParent(buttonHolder, false);

                buttonRect.localPosition = Vector2.zero;
                buttonRect.localScale = Vector3.one;
                buttonRect.anchoredPosition = Vector2.zero;
                buttonRect.anchorMin = Vector2.zero;
                buttonRect.anchorMax = Vector3.one;
                buttonRect.offsetMin = Vector2.zero;
                buttonRect.offsetMax = Vector2.zero;
            }

        }
    }
}
