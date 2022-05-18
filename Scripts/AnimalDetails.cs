namespace YouCanPetTheCat
{
    public class AnimalDetails
    {
        public DaggerfallWorkshop.SoundClips clip;
        public string animalName;
        
        public AnimalDetails(DaggerfallWorkshop.SoundClips sound, string name)
        {
            clip = sound;
            animalName = name;
        }
    }
}