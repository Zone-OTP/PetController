using System;


namespace PetController
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IPetDataHandler fileHandler = new FilePetDataHandler();
            PetManager petManager = new PetManager(fileHandler);
            //we never neaded keyword required
            petManager.AddPet(new Dog("Lord Farkwad", 28));
            petManager.AddPet(new Dog("Lord ", 18));
            petManager.AddPet(new Dog("Farkwad", 14));
            //push
            petManager.SavePets();
            petManager.LoadPets();
            petManager.ShowPets();


        }
    }
}
