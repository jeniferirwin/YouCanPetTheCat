using System.Collections;
using System.Collections.Generic;
using DaggerfallWorkshop;
using DaggerfallWorkshop.Game;
using UnityEngine;
using UnityEditor;

namespace YouCanPetTheCat.Compatibility.Faithful
{
    public class ColliderDB : MonoBehaviour
    {
        public BoxCollider[] colliders;
        public static ColliderDB instance;

        private void Start()
        {
            instance = this;
        }

        public static void Add3DCollider(GameObject obj, int index)
        {
            Debug.Log($"Index: {index}");
            if (index < instance.colliders.Length)
            {
                var newCollider = obj.AddComponent<BoxCollider>();
                newCollider.isTrigger = true;
                newCollider.center = instance.colliders[index].center;
                newCollider.size = instance.colliders[index].size;
                obj.name = $"{obj.name} [Pettable]";
            }
        }

        public static void ApplyColliders(GameObject location, bool allowYield)
        {
            ApplyColliders();
        }
        
        public static void ApplyColliders(PlayerEnterExit.TransitionEventArgs args)
        {
            ApplyColliders();
        } 
        
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