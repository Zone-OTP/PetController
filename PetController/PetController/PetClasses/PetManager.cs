using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetController;

namespace PetController
{
    public class PetManager
    {
        private readonly IPetDataHandlerJson dataHandlerJson;
        private readonly IPetDataHandlerMemory dataHandlermemory;
        public PetManager(IPetDataHandlerJson HandlerJson, IPetDataHandlerMemory HandlerMemory)
        {
            dataHandlerJson = HandlerJson;
            dataHandlermemory = HandlerMemory;
        }
        private static List<Pets> PetList { get; set; } = new List<Pets>();
        public void AddPet(Pets pet)
        {
            while (true)
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(pet.Name) && !pet.Name.Contains('"') && !pet.Name.Contains("'"))
                    {
                        PetList.Add(pet);
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Name was Not Accaptable");
                        Console.WriteLine("Enter the name");
                        pet.Name = Console.ReadLine();

                    }
                }
                catch (Exception e)
                {

                    Console.WriteLine($"{e.Message}");
                }
            }
        }
        public void ShowPets()
        {
            foreach (var pet in PetList)
            {
                Console.WriteLine($"Pet ID: {pet.PetId}, Name: {pet.Name}, Age: {pet.Age}");
                pet.MakeNoise();
                pet.PetMove();
            }
        }
        public void SpesificPet(int targetPetId)
        {
            var SpesificPet = PetList.Where(pet => pet.PetId == targetPetId).FirstOrDefault();
            if (SpesificPet == null) { Console.WriteLine("That pet ID doesn not happen to exist"); }
            else { Console.WriteLine($"Pet ID: {SpesificPet.PetId}, Name: {SpesificPet.Name}, Age: {SpesificPet.Age}"); }
        }
        public void SavePetsJson() => dataHandlerJson.SavePetsJson(PetList);
        public void SavePetsInMemory() => dataHandlermemory.SavePetsInMemory(PetList);
        public void LoadPetsJson() => PetList = dataHandlerJson.LoadPetsJson();
        public void LoadPetsInMemory() => PetList = dataHandlermemory.LoadPetsInMemory();

        public void MemoryOrFile()
        {
            Console.WriteLine(
                "are you working In Memory or would you like to save to a File" +
                "\nWrite 1 or 2" +
                "\n#1 Memory" +
                "\n#2 File");
            while (true)
            {
                try
                {
                    int choice = Convert.ToInt32(Console.ReadLine());
                    if (choice == 2)
                    {
                        Console.Clear();
                        Console.WriteLine("your choice is to work in file");
                        SavePetsJson();
                        LoadPetsJson();
                        break;

                    }
                    else if (choice == 1)
                    {
                        Console.Clear();
                        Console.WriteLine("your choice is to work in Memory");
                        SavePetsInMemory();
                        LoadPetsInMemory();
                        break;
                    }
                    else { Console.WriteLine($"{choice} is not a choice, plese input 1 or 2 "); }
                }
                catch (Exception ex) { Console.WriteLine($"{ex.Message}, input 1 or 2"); }
            }
        }
    }
}