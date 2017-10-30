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

    class FreeChampSelector
    {
        static void Main(string[] args)
        {
            var jsonData = File.ReadAllText("Data\\characters.json");
            var data = JsonConvert.DeserializeObject<CharacterSheet>(jsonData);

            var charLimit = 6;
            string input = "";
            while (input != "Q")
            {
                var (characters, availableWeapons) = CreateCharacterList(data, charLimit);
                while (availableWeapons.Count > 0)
                {
                    var (newCharacters, newAvailableWeapons) = CreateCharacterList(data, charLimit);
                    availableWeapons = newAvailableWeapons;
                    characters = newCharacters;
                }
                Console.WriteLine("List of characters chosen:");
                characters.ForEach(Console.WriteLine);
                if (availableWeapons.Count > 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("List of weapons not represented:");
                    availableWeapons.ForEach(Console.WriteLine);
                    Console.ForegroundColor = ConsoleColor.White;
                }
                input = Console.ReadLine();
                Console.Clear();
            }
        }

        private static (List<Character>, List<string>) CreateCharacterList(CharacterSheet data, int charLimit)
        {
            var characters = new List<Character>();
            var availableWeapons = data.characters.SelectMany(x => x.weapons).Distinct().ToList();
            var availableCharacters = new List<Character>(data.characters);
            while (characters.Count < charLimit)
            {
                var choices = availableCharacters.Where(x => availableWeapons.Contains(x.weapons[0]) && availableWeapons.Contains(x.weapons[1])).ToList();
                if (!choices.Any())
                {
                    choices = availableCharacters.Where(x => availableWeapons.Contains(x.weapons[0]) || availableWeapons.Contains(x.weapons[1])).ToList();
                }
                var nextCharacter = choices[new Random().Next(0, choices.Count - 1)];
                availableCharacters.Remove(nextCharacter);
                characters.Add(nextCharacter);
                nextCharacter.weapons.ToList().ForEach(x => availableWeapons.Remove(x));
            }

            return (characters, availableWeapons);
        }
    }
}
