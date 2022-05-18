using System.Collections.Generic;
using DaggerfallWorkshop.Game;
using DaggerfallWorkshop.Game.Utility.ModSupport;
using UnityEngine;

namespace YouCanPetTheCat.Compatibility.Faithful
{
    public static class ColliderDB
    {
        public static BoxCollider[] colliders = new BoxCollider[12];

        public static void PopulateColliders(Mod mod)
        {
            for (int i = 0; i <= 11; i++)
            {
                string path = $"YCPTC_{i}";
                GameObject prefab = mod.GetAsset<GameObject>(path);
                BoxCollider collider = prefab.GetComponent<BoxCollider>();
                colliders[i] = collider;
            }
        }

        public static void Add3DCollider(GameObject obj, int index)
        {
            if (index < colliders.Length)
            {
                var newCollider = obj.AddComponent<BoxCollider>();
                newCollider.isTrigger = true;
                newCollider.center = colliders[index].center;
                newCollider.size = colliders[index].size;
                obj.name = $"{obj.name} [Pettable]";
            }
        }

        public static void ApplyColliders(GameObject location, bool allowYield) => ApplyColliders();
        public static void ApplyColliders(PlayerEnterExit.TransitionEventArgs args) => ApplyColliders();
        
        public static void ApplyColliders()
        {
            var skinnedRend = UnityEngine.Object.FindObjectsOfType<SkinnedMeshRenderer>();
            List<GameObject> animals = new List<GameObject>();
            foreach (var rend in skinnedRend)
            {
                var rendParent = rend.gameObject.transform.parent;
                if (!animals.Contains(rendParent.gameObject)
                    && rendParent.name.Contains("TEXTURE.201")
                    && rendParent.name.Contains("[Replacement]")
                    && !rendParent.name.Contains("[Pettable]"))
                {
                    GameObject animal = rend.gameObject.transform.parent.gameObject;
                    animals.Add(animal);
                }
            }
            foreach (var animal in animals)
            {
                var idx = InfoHelper.GetAnimalIndex(animal);
                Add3DCollider(animal, idx);
                animal.AddComponent<Pettable>();
            }
        }
    }
}