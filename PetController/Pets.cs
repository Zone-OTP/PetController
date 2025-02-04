using System;
using System.Collections.Generic;
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
        private static int _nextId = 1;

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

    public class Cat:Pet
    {
        public Cat(string Name, uint Age) : base(Name, Age) { }
        public override void MakeNoise() => Console.WriteLine($"{Name}Says Meow meow...");
        public override void PetMove() => Console.WriteLine("Hardcore Parkour");
    }

    public class Bird : Pet
    {
        public Bird(string name, uint age): base(name, age) { }
        public override void MakeNoise() => Console.WriteLine($"{Name} says Tweet TWEET!");
        public override void PetMove() => Console.WriteLine("Fly");
    }
    public class PetManager
    {
        private readonly IPetDataHandler dataHandler;
        public PetManager(IPetDataHandler Handler)
        {

            dataHandler = Handler;
        }
        private List<Pet> PetList { get; set; } = new List<Pet>();

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

        public void SavePets() => dataHandler.SavePets(PetList);
        public void LoadPets()
        {
            PetList = dataHandler.LoadPets();
        }
            

    }
    public interface IPetDataHandler
    {
       void SavePets(List<Pet> pets);
       List<Pet> LoadPets();
    }

    public class FilePetDataHandler : IPetDataHandler
    {

            private readonly string filePath = "pets.json";

        public void SavePets(List<Pet> pets)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                TypeInfoResolver = new DefaultJsonTypeInfoResolver
                {
                    Modifiers = { (typeInfo) =>
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
            }}
                }
            };

            string json = JsonSerializer.Serialize(pets, options);
            File.WriteAllText(filePath, json);
            Console.WriteLine("Pets Have been saved");
        }
        public List<Pet> LoadPets() {
                if (!File.Exists(filePath)) { 
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
                    Modifiers = { (typeInfo) =>
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
                List<Pet>? pets = JsonSerializer.Deserialize<List<Pet>>(json,options);
                Console.WriteLine("Pets loaded successfully");
                return pets ?? new List<Pet>();
            }
            catch (Exception ex) { Console.WriteLine($"Error Loading pets:{ex.Message}"); return new List<Pet>(); }
            }


    }
}
