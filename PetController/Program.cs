using System;
using static PetController.FilePetDataHandler;


namespace PetController
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IPetDataHandlerJson jsonFileHandler = new FilePetDataHandler();
            IPetDataHandlerMemory memoryFileHandler = new InMemoryPetDataHandler();

            PetManager petManager = new PetManager(jsonFileHandler, memoryFileHandler);
            //we never neaded keyword required
            petManager.AddPet(new Dog("Lord Farkwad", 28));
            petManager.AddPet(new Dog("Lord ", 18));
            petManager.AddPet(new Dog("Farkwad", 14));
            //push
            petManager.MemoryOrFile();
            petManager.ShowPets();
            petManager.SpesificPet(1);


        }

    }
}
