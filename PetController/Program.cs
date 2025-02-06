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
<<<<<<< HEAD
            //commit
=======
            //push
            petManager.MemoryOrFile();
>>>>>>> 2160f03dd00330a516960207ba40b8aff2ce8ffc
            petManager.ShowPets();
            petManager.SpesificPet(1);


        }

    }
}
