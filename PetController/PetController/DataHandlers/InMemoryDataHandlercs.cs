using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetController
{
    public class InMemoryPetDataHandler : IPetDataHandlerMemory
    {
        private List<Pets> _inMemoryPets = new List<Pets>();
        public void SavePetsInMemory(List<Pets> pets)
        {
            _inMemoryPets = new List<Pets>(pets);
            Console.WriteLine("Pets Have been saved in memory");
        }
        public List<Pets> LoadPetsInMemory()
        {
            Pets._nextId = 1;
            if (_inMemoryPets != null && _inMemoryPets.Any())
            {
                Pets._nextId = _inMemoryPets.Max(p => p.PetId) + 1;
            }
            Console.WriteLine("Pets loaded from memory");
            return new List<Pets>(_inMemoryPets);
        }
    }
}
