using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DaggerfallWorkshop.Game;

namespace YouCanPetTheCat
{
    public class Pettable : MonoBehaviour, IPlayerActivable
    {
        void Start()
        {
            Debug.Log($"I am {gameObject.name}");
        }

        public void Activate(RaycastHit hit)
        {
            YouCanPetTheCat.PetAnimal3D(hit);
        }
    }
}