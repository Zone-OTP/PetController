using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization.Metadata;
using System.Text.Json;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json.Serialization;

namespace PetController
{
    public class FilePetDataHandler : IPetDataHandlerJson
    {
        private readonly static string filePath = "pets.json";
        public void SavePetsJson(List<Pets> pets)
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
                            if (typeInfo.Type == typeof(Pets))
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
        public List<Pets> LoadPetsJson()
        {
            Pets._nextId = 1;
            if (!File.Exists(filePath))
            {
                Console.WriteLine("No Saved Found");
                return new List<Pets>();
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
                        { if(typeInfo.Type == typeof(Pets))
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
                List<Pets>? pets = JsonSerializer.Deserialize<List<Pets>>(json, options);
                Console.WriteLine("Pets loaded successfully");
                return pets ?? new List<Pets>();
            }
            catch (Exception ex) { Console.WriteLine($"Error Loading pets:{ex.Message}"); return new List<Pets>(); }
        }
    }
}
