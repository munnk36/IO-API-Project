using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace IO_API_Project
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            UpdateList();
        }

        Pokemon Current = new Pokemon();
        List<Pokemon> Inventory = LoadFromFile();

        private void btnEncounter_Click(object sender, RoutedEventArgs e)
        {
            txtResult.Text = "";
            WebClient Client = new WebClient();
            int Mon = EncounterMon();
            string StringMon = Mon.ToString();
            string URIQuery = string.Format(@"http://pokeapi.co/api/v2/pokemon-species/{0}", Mon);
            string JSONData = Client.DownloadString(URIQuery);
            JObject JMON = JObject.Parse(JSONData);

            Pokemon EncounteredPokemon = new Pokemon()
            {
                CaptureRate = (int)JMON["capture_rate"],
                Name = (string)JMON["name"],
                ID = (int)JMON["id"],
            };

            txtEncounteredPokemon.Text = EncounteredPokemon.Name;
            btnEncounter.IsEnabled = false;
            btnThrow.IsEnabled = true;
            Current = EncounteredPokemon;
        }

        private int EncounterMon()
        {
            Random Gen = new Random();
            //get a random species of pokemon including 1 to 721.
            return Gen.Next(721) + 1;
        }

        private void UpdateList()
        {
            List<string> PokemonNames = new List<string>();
            foreach (Pokemon CurrentMon in Inventory)
            {
                PokemonNames.Add(CurrentMon.Name);
            }
            lbPokemonInventory.ItemsSource = PokemonNames;
        }

        private bool ThrowBall(Pokemon Mon)
        {
            Random Gen = new Random();
            if(Gen.Next(256) < Mon.CaptureRate)
            {
                return true;
            }
            return false;
        }

        private void btnThrow_Click(object sender, RoutedEventArgs e)
        {
            if (ThrowBall(Current))
            {
                Inventory.Add(Current);
                txtResult.Text = "You caught it!";
                UpdateList();
                Save(Inventory);
            } else
            {
                txtResult.Text = "The pokemon broke free! Try again!";
            }
            btnEncounter.IsEnabled = true;
            btnThrow.IsEnabled = false;
            txtEncounteredPokemon.Text = "";
        }

        public static List<Pokemon> LoadFromFile()
        {
			var XML = new XmlSerializer(typeof(List<Pokemon>));
			try {
				using (var stream = new FileStream("Pokemon.xml", FileMode.Open)) {
					return (List<Pokemon>)XML.Deserialize(stream);
				}
			} catch (FileNotFoundException e) {
				Console.WriteLine("{0}. Creating new file.", e.Message);
				var newDex = new List<Pokemon>();
				Save(newDex);
				return newDex;
			}
        }

		public static void Save(List<Pokemon> MyPokemon)
        {
            using (var stream = new FileStream("Pokemon.xml", FileMode.Create))
            {
                var XML = new XmlSerializer(typeof(List<Pokemon>));
                XML.Serialize(stream, MyPokemon);
            }
        }
    }
}
