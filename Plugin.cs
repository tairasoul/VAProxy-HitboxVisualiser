using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using SettingsAPI;
using UnityEngine;
using UnityEngine.UI;
using Settings = SettingsAPI.Plugin;

namespace HitboxVisualiser
{
    [BepInPlugin("vaproxy.hitbox.visualiser", "HitboxVisualiser", "1.0.0")]
    public class Plugin: BaseUnityPlugin
    {
        Harmony harmony = new Harmony("vaproxy.hitbox.visualiser");
        bool initialized = false;
        public static ManualLogSource Log;
        internal void Awake()
        {
            Log = Logger;
            Logger.LogInfo("HitboxVisualiser awake.");
        }

        internal void Start()
        {
            Init();
        }

        internal void OnDestroy()
        {
            Init();
        }

        internal void Init()
        {
            if (!initialized)
            {
                initialized = true;
                GameObject game = new GameObject("HitboxVisualiser");
                DontDestroyOnLoad(game);
                game.AddComponent<Visualiser>();
                Logger.LogInfo("Visualiser initialized.");
                Option Toggle = new()
                {
                    Create = (GameObject Page) =>
                    {
                        Log.LogInfo("Creating HitboxVisualiser page init.");
                        GameObject HitboxToggle = Page.AddObject("HitboxToggle");
                        HitboxToggle.AddComponent<RectTransform>().anchoredPosition = new Vector2(-449.6534f, 158.6981f);
                        HitboxToggle.transform.localScale = new Vector3(2.1268f, 2.1268f, 2.1268f);
                        Toggle toggle = HitboxToggle.AddComponent<Toggle>();
                        GameObject Label = HitboxToggle.AddObject("Label");
                        Label.AddComponent<CanvasRenderer>();
                        Label.AddComponent<RectTransform>().anchoredPosition = new Vector2(52.16f, -1);
                        Label.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
                        Text text = Label.AddComponent<Text>();
                        text.alignment = TextAnchor.MiddleLeft;
                        text.fontSize = 100;
                        GameObject original = GameObject.Find("MAINMENU/Canvas/Pages/Setting/Resolution/VSync/Background");
                        text.font = original.transform.parent.gameObject.Find("Label").GetComponent<Text>().font;
                        text.text = "Visualize Hitboxes";
                        text.horizontalOverflow = HorizontalWrapMode.Overflow;
                        text.color = new Color(0.9843f, 0.6902f, 0.2314f);
                        GameObject background = HitboxToggle.AddObject("Background");
                        background.AddComponent<RectTransform>().anchoredPosition = new Vector2(10, 0);
                        background.AddComponent<CanvasRenderer>();
                        background.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
                        Image image = background.AddComponent<Image>();
                        image.sprite = original.GetComponent<Image>().sprite;
                        GameObject checkmark = background.AddObject("Checkmark");
                        checkmark.AddComponent<RectTransform>();
                        checkmark.AddComponent<CanvasRenderer>();
                        Image checkmarkImage = checkmark.AddComponent<Image>();
                        checkmarkImage.enabled = false;
                        toggle.image = checkmarkImage;
                        checkmarkImage.sprite = original.Find("Checkmark").GetComponent<Image>().sprite;
                        toggle.onValueChanged.AddListener((bool toggled) =>
                        {
                            checkmarkImage.enabled = toggled;
                            Visualiser.active = toggled;
                        });
                    }
                };
                Option[] Options = [Toggle];
                Settings.API.RegisterMod("tairasoul.HitboxVisualiser", "HitboxVisualiser", Options, (GameObject obj) => { obj.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f); }) ;
            }
        }
    }
}
