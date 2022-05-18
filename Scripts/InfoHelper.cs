using System;
using System.Text.RegularExpressions;
using UnityEngine;
using DaggerfallWorkshop;

namespace YouCanPetTheCat
{
    public class InfoHelper
    {
        public static int GetAnimalIndex(GameObject animal)
        {
            var fullname = animal.name;
            Regex rx = new Regex(@"^DaggerfallBillboard \[TEXTURE.201, Index=(?<idx>\d+)\]", RegexOptions.ExplicitCapture);
            var match = rx.Match(fullname);
            if (match.Groups.Count < 2) return -1;
            int idx = Int32.Parse(match.Groups[1].Value);
            return idx;
        }

        public static AnimalDetails GetAnimalDetails(int record)
        {
            DaggerfallWorkshop.SoundClips clip = SoundClips.None;
            string animalName = "";
            switch (record)
            {
                case 0:
                case 1:
                    clip = DaggerfallWorkshop.SoundClips.AnimalHorse;
                    animalName = "horse";
                    break;
                case 2:
                    clip = DaggerfallWorkshop.SoundClips.EnemyGargoyleBark;
                    animalName = "camel";
                    break;
                case 3:
                case 4:
                    clip = DaggerfallWorkshop.SoundClips.AnimalCow;
                    animalName = "cow";
                    break;
                case 5:
                case 6:
                    clip = DaggerfallWorkshop.SoundClips.AnimalPig;
                    animalName = "pig";
                    break;
                case 7:
                case 8:
                    clip = DaggerfallWorkshop.SoundClips.AnimalCat;
                    animalName = "cat";
                    break;
                case 9:
                case 10:
                    clip = DaggerfallWorkshop.SoundClips.AnimalDog;
                    animalName = "dog";
                    break;
                case 11:
                    var chance = UnityEngine.Random.Range(0, 100);
                    if (chance < 50) clip = DaggerfallWorkshop.SoundClips.BirdCall1;
                    else clip = DaggerfallWorkshop.SoundClips.BirdCall2;
                    animalName = "bird";
                    break;
                default:
                    clip = DaggerfallWorkshop.SoundClips.None;
                    animalName = "unknown";
                    break;
            }
            return new AnimalDetails(clip, animalName);
        }
    }
}