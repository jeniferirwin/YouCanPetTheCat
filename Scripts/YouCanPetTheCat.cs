using UnityEngine;
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
            
            for (int i = 0; i < 11; i++)
            {
                PlayerActivate.RegisterCustomActivation(mod, 201, i, PetAnimal);
            }
            mod.IsReady = true;
        }

        public static void PetAnimal(RaycastHit hit)
        {
            var source = hit.transform.gameObject.GetComponent<DaggerfallWorkshop.DaggerfallAudioSource>();
            var billboard = hit.transform.gameObject.GetComponent<DaggerfallWorkshop.DaggerfallBillboard>();
            var archive = billboard.Summary.Archive;
            var record = billboard.Summary.Record;
            if (archive != 201) return;
            string animalName = "animal";
            switch (record)
            {
                case int n when n < 2:
                    source.PlayOneShot(DaggerfallWorkshop.SoundClips.AnimalHorse);
                    animalName = "horse";
                    break;
                case int n when n < 3:
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
            }
            DaggerfallUI.AddHUDText($"You pet the {animalName}.");
        }
    }
}