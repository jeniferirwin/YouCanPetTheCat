using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using DaggerfallWorkshop;
using DaggerfallWorkshop.Game;
using DaggerfallWorkshop.Game.Utility.ModSupport;
using YouCanPetTheCat.Compatibility.Faithful;

namespace YouCanPetTheCat
{
    public class YouCanPetTheCat : MonoBehaviour
    {
        public static Mod mod;
        public static YouCanPetTheCat instance;
        public static ColliderDB colliderDB;

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
                GameObject db = Instantiate(Resources.Load("YCPTC_ColliderDB", typeof(GameObject))) as GameObject;
                StreamingWorld.OnUpdateLocationGameObject += ColliderDB.ApplyColliders;
                PlayerEnterExit.OnRespawnerComplete += ColliderDB.ApplyColliders;
                PlayerEnterExit.OnTransitionDungeonInterior += ColliderDB.ApplyColliders;
                PlayerEnterExit.OnTransitionDungeonExterior += ColliderDB.ApplyColliders;
                /*
                PlayerEnterExit.OnTransitionInterior += ColliderDB.ApplyColliders;
                PlayerEnterExit.OnTransitionExterior += ColliderDB.ApplyColliders;
                */
        }

        public static void PetAnimal(RaycastHit hit)
        {
            var idx = InfoHelper.GetAnimalIndex(hit.transform.gameObject);
            if (idx == -1) return;
            var source = hit.transform.gameObject.GetComponentInChildren<DaggerfallWorkshop.DaggerfallAudioSource>();

            var details = InfoHelper.GetAnimalDetails(idx);
            DaggerfallUI.AddHUDText($"You pet the {details.animalName}.");
            if (source == null || details.clip == SoundClips.None) return;
            source.PlayOneShot(details.clip);
        }

        private static void OnDisable()
        {
            StreamingWorld.OnUpdateLocationGameObject -= ColliderDB.ApplyColliders;
            PlayerEnterExit.OnRespawnerComplete -= ColliderDB.ApplyColliders;
            PlayerEnterExit.OnTransitionDungeonInterior -= ColliderDB.ApplyColliders;
            PlayerEnterExit.OnTransitionDungeonExterior -= ColliderDB.ApplyColliders;
            /*
            PlayerEnterExit.OnTransitionExterior -= ColliderDB.ApplyColliders;
            PlayerEnterExit.OnTransitionExterior -= ColliderDB.ApplyColliders;
            */
        }
    }
}