using System;


namespace PetController
{
    internal class Program
    {
        static void Main(string[] args)
        {

            PetManager petManager = new PetManager();

            petManager.AddPet(new Dog( "Lord Farkwad", 28) );
            petManager.ShowPets();

        }
    }
}
