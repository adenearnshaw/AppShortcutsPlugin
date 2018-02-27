using AppShortcutsSample.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AppShortcutsSample.Data
{
    public class MonkeyStore
    {
        private static MonkeyStore _instance;
        public static MonkeyStore Instance => _instance ?? (_instance = new MonkeyStore());

        private readonly string _filePath;
        private List<Monkey> _monkeys;

        private MonkeyStore()
        {
            var folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            _filePath = Path.Combine(folderPath, "monkeydata.json");

            _monkeys = GetMonkeys();
        }

        public List<Monkey> Monkeys => _monkeys;

        public void UpdateMonkeyShortcutId(string monkeyId, string shortcutId)
        {
            var monkeyToUpdate = _monkeys.FirstOrDefault(m => m.Id.Equals(monkeyId));
            monkeyToUpdate.ShortcutId = shortcutId;
            SaveMonkeyData(_monkeys);
        }

        private List<Monkey> GetMonkeys()
        {
            var monkeys = LoadMonkeyData();

            if (monkeys.Any())
                return monkeys;


            monkeys.Add(new Monkey
            {
                Id = "monkey_001",
                Name = "Baboon",
                Location = "Africa & Asia",
                Details = "Baboons are African and Arabian Old World monkeys belonging to the genus Papio, part of the subfamily Cercopithecinae.",
                Image = "http://upload.wikimedia.org/wikipedia/commons/thumb/f/fc/Papio_anubis_%28Serengeti%2C_2009%29.jpg/200px-Papio_anubis_%28Serengeti%2C_2009%29.jpg"
            });

            monkeys.Add(new Monkey
            {
                Id = "monkey_002",
                Name = "Capuchin Monkey",
                Location = "Central & South America",
                Details = "The capuchin monkeys are New World monkeys of the subfamily Cebinae. Prior to 2011, the subfamily contained only a single genus, Cebus.",
                Image = "http://upload.wikimedia.org/wikipedia/commons/thumb/4/40/Capuchin_Costa_Rica.jpg/200px-Capuchin_Costa_Rica.jpg"
            });

            monkeys.Add(new Monkey
            {
                Id = "monkey_003",
                Name = "Blue Monkey",
                Location = "Central and East Africa",
                Details = "The blue monkey or diademed monkey is a species of Old World monkey native to Central and East Africa, ranging from the upper Congo River basin east to the East African Rift and south to northern Angola and Zambia",
                Image = "http://upload.wikimedia.org/wikipedia/commons/thumb/8/83/BlueMonkey.jpg/220px-BlueMonkey.jpg"
            });


            monkeys.Add(new Monkey
            {
                Id = "monkey_004",
                Name = "Squirrel Monkey",
                Location = "Central & South America",
                Details = "The squirrel monkeys are the New World monkeys of the genus Saimiri. They are the only genus in the subfamily Saimirinae. The name of the genus Saimiri is of Tupi origin, and was also used as an English name by early researchers.",
                Image = "http://upload.wikimedia.org/wikipedia/commons/thumb/2/20/Saimiri_sciureus-1_Luc_Viatour.jpg/220px-Saimiri_sciureus-1_Luc_Viatour.jpg"
            });

            monkeys.Add(new Monkey
            {
                Id = "monkey_005",
                Name = "Golden Lion Tamarin",
                Location = "Brazil",
                Details = "The golden lion tamarin also known as the golden marmoset, is a small New World monkey of the family Callitrichidae.",
                Image = "http://upload.wikimedia.org/wikipedia/commons/thumb/8/87/Golden_lion_tamarin_portrait3.jpg/220px-Golden_lion_tamarin_portrait3.jpg"
            });

            monkeys.Add(new Monkey
            {
                Id = "monkey_006",
                Name = "Howler Monkey",
                Location = "South America",
                Details = "Howler monkeys are among the largest of the New World monkeys. Fifteen species are currently recognised. Previously classified in the family Cebidae, they are now placed in the family Atelidae.",
                Image = "http://upload.wikimedia.org/wikipedia/commons/thumb/0/0d/Alouatta_guariba.jpg/200px-Alouatta_guariba.jpg"
            });

            monkeys.Add(new Monkey
            {
                Id = "monkey_007",
                Name = "Japanese Macaque",
                Location = "Japan",
                Details = "The Japanese macaque, is a terrestrial Old World monkey species native to Japan. They are also sometimes known as the snow monkey because they live in areas where snow covers the ground for months each",
                Image = "http://upload.wikimedia.org/wikipedia/commons/thumb/c/c1/Macaca_fuscata_fuscata1.jpg/220px-Macaca_fuscata_fuscata1.jpg"
            });

            monkeys.Add(new Monkey
            {
                Id = "monkey_008",
                Name = "Mandrill",
                Location = "Southern Cameroon, Gabon, Equatorial Guinea, and Congo",
                Details = "The mandrill is a primate of the Old World monkey family, closely related to the baboons and even more closely to the drill. It is found in southern Cameroon, Gabon, Equatorial Guinea, and Congo.",
                Image = "http://upload.wikimedia.org/wikipedia/commons/thumb/7/75/Mandrill_at_san_francisco_zoo.jpg/220px-Mandrill_at_san_francisco_zoo.jpg"
            });

            monkeys.Add(new Monkey
            {
                Id = "monkey_009",
                Name = "Proboscis Monkey",
                Location = "Borneo",
                Details = "The proboscis monkey or long-nosed monkey, known as the bekantan in Malay, is a reddish-brown arboreal Old World monkey that is endemic to the south-east Asian island of Borneo.",
                Image = "http://upload.wikimedia.org/wikipedia/commons/thumb/e/e5/Proboscis_Monkey_in_Borneo.jpg/250px-Proboscis_Monkey_in_Borneo.jpg"
            });

            SaveMonkeyData(monkeys);
            return monkeys;
        }

        private void SaveMonkeyData(List<Monkey> monkeys)
        {
            try
            {
                var json = JsonConvert.SerializeObject(monkeys);
                File.WriteAllText(_filePath, json);
            }
            catch { }
        }

        private List<Monkey> LoadMonkeyData()
        {
            try
            {
                var json = File.ReadAllText(_filePath);
                var monkeys = JsonConvert.DeserializeObject<List<Monkey>>(json);
                return monkeys;
            }
            catch
            {
                return new List<Monkey>();
            }
        }
    }
}
