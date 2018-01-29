using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Reflection;
using System;
using StarForce;

namespace UnityEditor.UI.TableView
{
    static internal class MenuOptions
    {
        private const string kStandardSpritePath       = "UI/Skin/UISprite.psd";
        private const string kBackgroundSpritePath     = "UI/Skin/Background.psd";
        private const string kInputFieldBackgroundPath = "UI/Skin/InputFieldBackground.psd";
        private const string kKnobPath                 = "UI/Skin/Knob.psd";
        private const string kCheckmarkPath            = "UI/Skin/Checkmark.psd";
        private const string kDropdownArrowPath        = "UI/Skin/DropdownArrow.psd";
        private const string kMaskPath                 = "UI/Skin/UIMask.psd";

        private static MethodInfo m_PlaceUIElementRoot;

        static private DefaultControls.Resources s_StandardResources;

        static private DefaultControls.Resources GetStandardResources()
        {
            if (s_StandardResources.standard == null)
            {
                s_StandardResources.standard = AssetDatabase.GetBuiltinExtraResource<Sprite>(kStandardSpritePath);
                s_StandardResources.background = AssetDatabase.GetBuiltinExtraResource<Sprite>(kBackgroundSpritePath);
                s_StandardResources.inputField = AssetDatabase.GetBuiltinExtraResource<Sprite>(kInputFieldBackgroundPath);
                s_StandardResources.knob = AssetDatabase.GetBuiltinExtraResource<Sprite>(kKnobPath);
                s_StandardResources.checkmark = AssetDatabase.GetBuiltinExtraResource<Sprite>(kCheckmarkPath);
                s_StandardResources.dropdown = AssetDatabase.GetBuiltinExtraResource<Sprite>(kDropdownArrowPath);
                s_StandardResources.mask = AssetDatabase.GetBuiltinExtraResource<Sprite>(kMaskPath);
            }
            return s_StandardResources;
        }


        [MenuItem("GameObject/UI/Table View", false, 3001)]
        static public void AddText(MenuCommand menuCommand)
        {
            GetStandardResources ();
            GameObject go = new GameObject ("Table View");

            RectTransform rect = go.AddComponent<RectTransform>();
            rect.sizeDelta = new Vector2 (200, 200);

            ScrollRect scrollRect = go.AddComponent<ScrollRect> ();
            scrollRect.horizontal = false;

            Image image = go.AddComponent<Image> ();
            image.sprite = s_StandardResources.background;
            image.color = new Color (255, 255, 255, 100);
            image.type = Image.Type.Sliced;

            Mask mask = go.AddComponent<Mask> ();
            mask.showMaskGraphic = false;

            GameObject content = new GameObject ("Content");

            rect = content.AddComponent<RectTransform> ();
            rect.SetParent (go.GetComponent<RectTransform> (), false);
            rect.anchorMax = new Vector2 (0.5f, 1);
            rect.anchorMin = new Vector2 (0.5f, 1);
            rect.pivot = new Vector2 (0.5f, 1);
            rect.sizeDelta = new Vector2 (200, 200);


            scrollRect.content = content.GetComponent<RectTransform>();

            StarForce.TableView tableView = go.AddComponent<StarForce.TableView>();
            tableView.scrollRect = scrollRect;
            tableView.content = scrollRect.content;


            PlaceUIElementRoot (go, menuCommand);
        }

        private static void PlaceUIElementRoot(GameObject element, MenuCommand menuCommand)
        {
            if (m_PlaceUIElementRoot == null)
            {
                Assembly assembly = Assembly.GetAssembly(typeof(UnityEditor.UI.TextEditor));
                Type type = assembly.GetType("UnityEditor.UI.MenuOptions");
                m_PlaceUIElementRoot = type.GetMethod("PlaceUIElementRoot", BindingFlags.NonPublic | BindingFlags.Static);
            }
            m_PlaceUIElementRoot.Invoke(null, new object[] {element, menuCommand});
        }
    }


}

