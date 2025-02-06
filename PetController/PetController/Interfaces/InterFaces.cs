using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetController
{
    public interface IPetDataHandlerJson
    {
        void SavePetsJson(List<Pets> pets);
        List<Pets> LoadPetsJson();
    }
    public interface IPetDataHandlerMemory
    {
        void SavePetsInMemory(List<Pets> pets);
        List<Pets> LoadPetsInMemory();
    }
}
