using UnityEngine;
using DaggerfallWorkshop.Game;

namespace YouCanPetTheCat.Compatibility.Faithful
{
    public class Pettable : MonoBehaviour, IPlayerActivable
    {
        public void Activate(RaycastHit hit)
        {
            YouCanPetTheCat.PetAnimal(hit);
        }
    }
}