using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PetController
{
    [JsonPolymorphic]
    [JsonDerivedType(typeof(Dog))]
    [JsonDerivedType(typeof(Cat))]
    [JsonDerivedType(typeof(Bird))]

    public abstract class Pet
    {
        internal static int _nextId = 1;

        public int PetId { get; }
        public string Name { get; set; }
        public uint Age { get; set; }

        public abstract void MakeNoise();
        public abstract void PetMove();

        protected Pet(string name, uint age)
        {
            PetId = _nextId++;
            Name = name;
            Age = age;
        }


    }

    public class Dog : Pet
    {
        public Dog(string name, uint age) : base(name, age) { }

        public override void MakeNoise() => Console.WriteLine($"{Name} Says woof Woof WOOOF!");
        public override void PetMove() => Console.WriteLine("RUN");
    }

    public class Cat : Pet
    {
        public Cat(string Name, uint Age) : base(Name, Age) { }
        public override void MakeNoise() => Console.WriteLine($"{Name}Says Meow meow...");
        public override void PetMove() => Console.WriteLine("Hardcore Parkour");
    }

    public class Bird : Pet
    {
        public Bird(string name, uint age) : base(name, age) { }
        public override void MakeNoise() => Console.WriteLine($"{Name} says Tweet TWEET!");
        public override void PetMove() => Console.WriteLine("Fly");
    }
    public class PetManager
    {
        private readonly IPetDataHandlerJson dataHandlerJson;
        private readonly IPetDataHandlerMemory dataHandlermemory;
        public PetManager(IPetDataHandlerJson HandlerJson, IPetDataHandlerMemory HandlerMemory)
        {
            dataHandlerJson = HandlerJson;
            dataHandlermemory = HandlerMemory;
        }
        private static List<Pet> PetList { get; set; } = new List<Pet>();
        public void AddPet(Pet pet)
        {
            PetList.Add(pet);
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
        public void LoadPetsJson()
        {
            PetList = dataHandlerJson.LoadPetsJson();
        }
        public void LoadPetsInMemory()
        {
            PetList = dataHandlermemory.LoadPetsInMemory();
        }
        public void MemoryOrFile()
        {
            Console.WriteLine("are you working In Memory or would you like to save to a File" +
                "\nWrite 1 or 2 in the next line" +
                "\n#1 Memory" +
                "\n#2 File");
            while (true)
            {
                try
                {
                    int choice = Convert.ToInt32(Console.ReadLine());
                    if (choice == 2)
                    {
                        Console.WriteLine("your choice is to work in file");
                        SavePetsJson();
                        LoadPetsJson();
                        break;

                    }
                    else if (choice == 1)
                    {
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
    public interface IPetDataHandlerJson
    {
         void SavePetsJson(List<Pet> pets);
         List<Pet> LoadPetsJson();
    }

    public interface IPetDataHandlerMemory
    {
        void SavePetsInMemory(List<Pet> pets);
        List<Pet> LoadPetsInMemory();
    }

    public class FilePetDataHandler : IPetDataHandlerJson
    {
        private readonly static string filePath = "pets.json";
        public  void SavePetsJson(List<Pet> pets)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                IncludeFields = true,
                TypeInfoResolver = new DefaultJsonTypeInfoResolver
                {
                    Modifiers = 
                    { (typeInfo) =>
                        {
                            if (typeInfo.Type == typeof(Pet))
                            {
                                typeInfo.PolymorphismOptions = new JsonPolymorphismOptions
                                {
                                        TypeDiscriminatorPropertyName = "$type",

                                        DerivedTypes =
                                        {
                                        new JsonDerivedType(typeof(Dog), "Dog"),
                                        new JsonDerivedType(typeof(Cat), "Cat"),
                                        new JsonDerivedType(typeof(Bird), "Bird")
                                        }
                                };
                            }
                        }
                    }
                }
            };

            string json = JsonSerializer.Serialize(pets, options);
            File.WriteAllText(filePath, json);
            Console.WriteLine("Pets Have been saved");
        }
        public  List<Pet> LoadPetsJson() {
            Pet._nextId = 1;
            if (!File.Exists(filePath))
            {
                Console.WriteLine("No Saved Found");
                return new List<Pet>();
            }

            string json = File.ReadAllText(filePath);
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNameCaseInsensitive = true,
                IncludeFields = true,
                TypeInfoResolver = new DefaultJsonTypeInfoResolver
                {
                    Modifiers =
                    { (typeInfo) =>
                        { if(typeInfo.Type == typeof(Pet))
                          {
                             typeInfo.PolymorphismOptions = new JsonPolymorphismOptions
                             {
                                TypeDiscriminatorPropertyName = "$type",
                                IgnoreUnrecognizedTypeDiscriminators = true,
                                DerivedTypes =
                                {

                                    new JsonDerivedType(typeof(Dog), "Dog"),
                                    new JsonDerivedType(typeof(Cat), "Cat"),
                                    new JsonDerivedType(typeof(Bird), "Bird")

                                }
                             };
                          }
                        }

                    }
                }

            };
            try
            {
                List<Pet>? pets = JsonSerializer.Deserialize<List<Pet>>(json, options);
                Console.WriteLine("Pets loaded successfully");
                return pets ?? new List<Pet>();
            }
            catch (Exception ex) { Console.WriteLine($"Error Loading pets:{ex.Message}"); return new List<Pet>(); }
        }
    }
    public class InMemoryPetDataHandler : IPetDataHandlerMemory
    {
        private List<Pet> _inMemoryPets = new List<Pet>();
        public void SavePetsInMemory(List<Pet> pets)
        {
            _inMemoryPets = new List<Pet>(pets);
            Console.WriteLine("Pets Have been saved in memory");
        }

        public List<Pet> LoadPetsInMemory()
        {
                Pet._nextId = 1;
                if (_inMemoryPets != null && _inMemoryPets.Any())
                {
                    Pet._nextId = _inMemoryPets.Max(p => p.PetId) + 1;
                }
                Console.WriteLine("Pets loaded from memory");
                return new List<Pet>(_inMemoryPets);
           
        }
        
    }
}



