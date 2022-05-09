using UnityEngine;
using System.Collections.Generic;
using DaggerfallWorkshop;
using DaggerfallWorkshop.Game;
using DaggerfallWorkshop.Game.Utility.ModSupport;

namespace YouCanPetTheCat
{
    public class YouCanPetTheCat : MonoBehaviour
    {
        static Mod mod;
        static YouCanPetTheCat instance;

        [Invoke(StateManager.StateTypes.Start, 0)]
        public static void Init(InitParams initParams)
        {
            mod = initParams.Mod;       
            var go = new GameObject(mod.Title);
            instance = go.AddComponent<YouCanPetTheCat>();
            
            for (int i = 0; i <= 11; i++)
            {
                PlayerActivate.RegisterCustomActivation(mod, 201, i, PetAnimal2D);
            }
            var faithful = ModManager.Instance.GetModFromGUID("1424b378-0a50-4bd9-96ae-82162eec9fc4");
            if (faithful != null)
            {
                Debug.Log("!!! Faithful 3D Animals found. !!!");
                StreamingWorld.OnUpdateLocationGameObject += instance.FindBillboards;
            }
            mod.IsReady = true;
        }
        
        public void FindBillboards(GameObject location, bool allowYield)
        {
            var skinnedRend = FindObjectsOfType<SkinnedMeshRenderer>();
            Debug.Log($"Renderers found: {skinnedRend.Length}");
            var started = Time.time;
            List<GameObject> animals = new List<GameObject>();
            foreach (var rend in skinnedRend)
            {
                if (rend.gameObject.transform.parent.name.Contains("TEXTURE.201"))
                {
                    animals.Add(rend.gameObject.transform.parent.gameObject);
                }
            }
            var ended = Time.time;
            Debug.Log($"List created in { ended - started } seconds and contains {animals.Count} items.");
            foreach (var anim in animals)
            {
                anim.AddComponent<Pettable>();
            }
        }

        public static void PetAnimal(RaycastHit hit, int archive, int record)
        {
            var source = hit.transform.gameObject.GetComponentInChildren<DaggerfallWorkshop.DaggerfallAudioSource>();
            if (source == null) return;
            if (archive != 201) return;
            string animalName = "animal";
            switch (record)
            {
                case int n when n < 2:
                    source.PlayOneShot(DaggerfallWorkshop.SoundClips.AnimalHorse);
                    animalName = "horse";
                    break;
                case int n when n < 3:
                    source.PlayOneShot(DaggerfallWorkshop.SoundClips.EnemyGargoyleBark);
                    animalName = "camel";
                    break;
                case int n when n < 5:
                    source.PlayOneShot(DaggerfallWorkshop.SoundClips.AnimalCow);
                    animalName = "cow";
                    break;
                case int n when n < 7:
                    source.PlayOneShot(DaggerfallWorkshop.SoundClips.AnimalPig);
                    animalName = "pig";
                    break;
                case int n when n < 9:
                    source.PlayOneShot(DaggerfallWorkshop.SoundClips.AnimalCat);
                    animalName = "cat";
                    break;
                case int n when n < 11:
                    source.PlayOneShot(DaggerfallWorkshop.SoundClips.AnimalDog);
                    animalName = "dog";
                    break;
                default:
                    var chance = Random.Range(0,100);
                    if (chance < 50) source.PlayOneShot(DaggerfallWorkshop.SoundClips.BirdCall1);
                    else source.PlayOneShot(DaggerfallWorkshop.SoundClips.BirdCall2);
                    animalName = "bird";
                    break;
            }
            DaggerfallUI.AddHUDText($"You pet the {animalName}.");
        }
        
        public static void PetAnimal2D(RaycastHit hit)
        {
            var billboard = hit.transform.gameObject.GetComponent<DaggerfallWorkshop.DaggerfallBillboard>();
            var archive = billboard.Summary.Archive;
            var record = billboard.Summary.Record;
            PetAnimal(hit, archive, record);
        }
        
        public static void PetAnimal3D(RaycastHit hit)
        {
            var fullname = hit.transform.gameObject.name;
            if (!fullname.Contains("TEXTURE.201")) return;
            var idx = fullname.Substring(41,2);
            if (idx.Contains("]"))
            {
                idx = idx.Substring(0,1);
            }
            Debug.Log($"Index for {fullname} is {idx}");
        }
    }
}