using UnityEngine;
using BepInEx;
using Invector.vMelee;
using Invector;

namespace HitboxVisualiser
{
    internal class Visualiser: MonoBehaviour
    {
        //bool F8Pressed = false;
        public static bool active = false;

        internal void Update()
        {
            foreach (vHitBox hitbox in GameObject.FindObjectsOfType<vHitBox>())
            {
                if (!hitbox.gameObject.GetComponent<VisualiserComponent>())
                {
                    hitbox.gameObject.AddComponent<VisualiserComponent>();
                }
            }
            foreach (vObjectDamage hitbox in GameObject.FindObjectsOfType<vObjectDamage>())
            {
                if (!hitbox.gameObject.GetComponent<VisualiserComponent>())
                {
                    hitbox.gameObject.AddComponent<VisualiserComponent>();
                }
            }
        }
    }
}
