using UnityEngine;
using DaggerfallWorkshop.Game;

namespace YouCanPetTheCat.Compatibility.Faithful
{
    public class Pettable : MonoBehaviour, IPlayerActivable
    {
        public void Activate(RaycastHit hit)
        {
            if (hit.distance > 1.75f)
            {
                DaggerfallUI.AddHUDText("You are too far away...");
                return;
            }
            YouCanPetTheCat.PetAnimal(hit);
        }
    }
}