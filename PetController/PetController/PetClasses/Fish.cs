using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetController
{
        public class Fish : Pets
        {
            public Fish(string Name, uint Age) : base(Name, Age) { }
            public override void MakeNoise() => Console.WriteLine($"{Name} Says i cant realy make noise tbh ");
            public override void PetMove() => Console.WriteLine("Swim");
        }
}
