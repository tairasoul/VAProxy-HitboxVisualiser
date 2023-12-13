using UnityEngine;
using BepInEx;
using Invector.vMelee;

namespace HitboxVisualiser
{
    internal class Visualiser: MonoBehaviour
    {
        bool F8Pressed = false;
        public static bool active = false;

        internal void Update()
        {
            foreach (vHitBox hitbox in GameObject.FindObjectsOfType<vHitBox>())
            {
                if (!hitbox.gameObject.GetComponent<VisualiserComponent>())
                {
                    Plugin.Log.LogInfo($"Adding VisualiserComponent to vHitBox {hitbox.name}");
                    hitbox.gameObject.AddComponent<VisualiserComponent>();
                }
            }
        }
    }
}
