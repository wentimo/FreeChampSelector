using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FreeChampSelector
{
    class Character
    {
        public string name;
        public string[] weapons;
        public override string ToString() => $"{name} ({String.Join(" and ", weapons)})";
    }

    class CharacterSheet
    {
        public List<Character> characters;
    }

    class Program
    {
        static void Main(string[] args)
        {
            var jsonData = File.ReadAllText("Data\\characters.json");
            var data = JsonConvert.DeserializeObject<CharacterSheet>(jsonData);
            data.characters.ForEach(Console.WriteLine);

            var availableWeapons = data.characters.SelectMany(x => x.weapons).Distinct().ToList();

            var charLimit = 6;
            var characters = new List<Character>();

            var first = data.characters[new Random().Next(1, data.characters.Count)];
            characters.Add(first);
            first.weapons.ToList().ForEach(x => availableWeapons.Remove(x));

            var availableCharacters = new List<Character>();
            while (characters.Count < charLimit)
            {
                availableCharacters = availableCharacters.Where(x => availableWeapons.Contains(x.weapons[0]) && 
                                                                      availableWeapons.Contains(x.weapons[1])).ToList();
                var nextCharacter = availableCharacters[new Random().Next(1, data.characters.Count)];
            }

            Console.Read();
        }
    }
}
