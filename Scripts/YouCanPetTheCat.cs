using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using DaggerfallWorkshop;
using DaggerfallWorkshop.Game;
using DaggerfallWorkshop.Utility;
using DaggerfallWorkshop.Game.Utility.ModSupport;
using YouCanPetTheCat.Compatibility.Faithful;

namespace YouCanPetTheCat
{
    public class YouCanPetTheCat : MonoBehaviour
    {
        // temporary fix for mute animals until my fix goes in base DFU
        const float animalSoundMaxDistance = 768 * MeshReader.GlobalScale;
        
        public static Mod mod;
        public static YouCanPetTheCat instance;

        [Invoke(StateManager.StateTypes.Start, 0)]
        public static void Init(InitParams initParams)
        {
            mod = initParams.Mod;
            var go = new GameObject(mod.Title);
            instance = go.AddComponent<YouCanPetTheCat>();
            var faithful = ModManager.Instance.GetModFromGUID("1424b378-0a50-4bd9-96ae-82162eec9fc4");
            if (faithful == null)
                RegisterActivations();
            else
                LoadFaithfulColliders(go);

            mod.IsReady = true;
        }

        private static void RegisterActivations()
        {
            for (int i = 0; i <= 11; i++)
            {
                PlayerActivate.RegisterCustomActivation(mod, 201, i, PetAnimal);
            }
        }

        private static void LoadFaithfulColliders(GameObject petParent)
        {
            ColliderDB.PopulateColliders(mod);
            StreamingWorld.OnUpdateLocationGameObject += ColliderDB.ApplyColliders;
            PlayerEnterExit.OnRespawnerComplete += ColliderDB.ApplyColliders;
            PlayerEnterExit.OnTransitionDungeonInterior += ColliderDB.ApplyColliders;
            PlayerEnterExit.OnTransitionDungeonExterior += ColliderDB.ApplyColliders;
            PlayerEnterExit.OnTransitionInterior += ColliderDB.ApplyColliders;
            PlayerEnterExit.OnTransitionExterior += ColliderDB.ApplyColliders;
        }

        public static void PetAnimal(RaycastHit hit)
        {
            var idx = InfoHelper.GetAnimalIndex(hit.transform.gameObject);
            if (idx == -1) return;
            var source = hit.transform.gameObject.GetComponentInChildren<DaggerfallWorkshop.DaggerfallAudioSource>();

            var details = InfoHelper.GetAnimalDetails(idx);
            DaggerfallUI.AddHUDText($"You pet the {details.animalName}.");
            if (source == null)
            {
                AddAnimalAudioSource(hit.transform.gameObject, idx);
                source = hit.transform.gameObject.GetComponentInChildren<DaggerfallWorkshop.DaggerfallAudioSource>();
            }
            source.PlayOneShot(details.clip);
        }

        private static void OnDisable()
        {
            StreamingWorld.OnUpdateLocationGameObject -= ColliderDB.ApplyColliders;
            PlayerEnterExit.OnRespawnerComplete -= ColliderDB.ApplyColliders;
            PlayerEnterExit.OnTransitionDungeonInterior -= ColliderDB.ApplyColliders;
            PlayerEnterExit.OnTransitionDungeonExterior -= ColliderDB.ApplyColliders;
            PlayerEnterExit.OnTransitionInterior -= ColliderDB.ApplyColliders;
            PlayerEnterExit.OnTransitionExterior -= ColliderDB.ApplyColliders;
        }

        // temporary fix for mute animals until my fix goes in base DFU
        private static void AddAnimalAudioSource(GameObject go, int record)
        {
            DaggerfallAudioSource source = go.AddComponent<DaggerfallAudioSource>();
            source.AudioSource.maxDistance = animalSoundMaxDistance;

            SoundClips sound;
            switch (record)
            {
                case 0:
                case 1:
                    sound = SoundClips.AnimalHorse;
                    break;
                case 2:
                    sound = SoundClips.EnemyGargoyleBark;
                    break;
                case 3:
                case 4:
                    sound = SoundClips.AnimalCow;
                    break;
                case 5:
                case 6:
                    sound = SoundClips.AnimalPig;
                    break;
                case 7:
                case 8:
                    sound = SoundClips.AnimalCat;
                    break;
                case 9:
                case 10:
                    sound = SoundClips.AnimalDog;
                    break;
                default:
                    sound = SoundClips.None;
                    break;
            }

            source.SetSound(sound, AudioPresets.PlayRandomlyIfPlayerNear);
        }
    }
}