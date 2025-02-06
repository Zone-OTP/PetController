using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PetController
{
    [JsonPolymorphic]
    [JsonDerivedType(typeof(Dog))]
    [JsonDerivedType(typeof(Cat))]
    [JsonDerivedType(typeof(Bird))]

    public abstract class Pets
    {
        internal static int _nextId = 1;
        public int PetId { get; }
        public string Name { get; set; }
        public uint Age { get; set; }

        public abstract void MakeNoise();
        public abstract void PetMove();

        protected Pets(string name, uint age)
        {
            PetId = _nextId++;
            Name = name;
            Age = age;
        }
    }

    public class Dog : Pets
    {
        public Dog(string name, uint age) : base(name, age) { }

        public override void MakeNoise() => Console.WriteLine($"{Name} Says woof Woof WOOOF!");
        public override void PetMove() => Console.WriteLine("RUNS");
    }

    public class Cat : Pets
    {
        public Cat(string Name, uint Age) : base(Name, Age) { }
        public override void MakeNoise() => Console.WriteLine($"{Name}Says Meow meow...");
        public override void PetMove() => Console.WriteLine("Hardcore Parkours");
    }

    public class Bird : Pets
    {
        public Bird(string name, uint age) : base(name, age) { }
        public override void MakeNoise() => Console.WriteLine($"{Name} says Tweet TWEET!");
        public override void PetMove() => Console.WriteLine("Flys");
    }
}
