using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace YouCanPetTheCat
{
    public class ColliderConverter : MonoBehaviour
    {
        // Helper class to convert prefabs into code... there's
        // gotta be a better way to make use of the collider prefabs,
        // and I think it involves Addressables, but I don't know how
        // to use those yet.
        
        // To use this class, make a temporary GameObject with this
        // script on it, add everything in the Prefabs/ folder to
        // the prefabs array in the Inspector, and then run the game.

        // These strings can then be used to generate colliders for the
        // Faithful 3D Animals from script alone.

        public GameObject[] prefabs;
        string text = "";

        void Start()
        {
            foreach (var prefab in prefabs)
            {
                var coll = prefab.GetComponent<BoxCollider>();
                text += $"{prefab.name}:\n\r\n\rvar center = new Vector3({coll.center.x}, {coll.center.y}, {coll.center.z});\n\rvar size = new Vector3({coll.size.x}, {coll.size.y}, {coll.size.z});\n\r\n\r";
            }
            Debug.Log(text);
        }
    }
}