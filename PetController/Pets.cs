using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PetController
{
    public abstract class Pet
    {
        public Guid PetId { get; } = Guid.NewGuid();
        public string Name { get; set; }
        public uint Age { get; set; } 

        public abstract void MakeNoise();
        public abstract void PetMove();

        protected Pet(string name, uint age)
        {
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

        public List<Pet> PetList { get; set; } = new List<Pet>();
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



    }
}
