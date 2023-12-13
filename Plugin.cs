using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using SettingsAPI;
using UnityEngine;
using UnityEngine.UI;
using Settings = SettingsAPI.Plugin;

namespace HitboxVisualiser
{
    [BepInPlugin("vaproxy.hitbox.visualiser", "HitboxVisualiser", "1.1.0")]
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
                        GameObject toggle = ComponentUtils.CreateToggle("Visualize Hitboxes", "hitbox.visualizer.toggle");
                        toggle.SetParent(Page, false);
                        toggle.GetComponent<RectTransform>().anchoredPosition = new Vector2(-449.6534f, 158.6981f);
                        GameObject label = toggle.Find("Label");
                        label.GetComponent<RectTransform>().anchoredPosition = new Vector2(52.16f, -1);
                        Image checkmark = toggle.Find("Background/Checkmark").GetComponent<Image>();
                        Toggle toggleComp = toggle.GetComponent<Toggle>();
                        toggleComp.onValueChanged.AddListener((bool toggled) =>
                        {
                            checkmark.enabled = toggled;
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
