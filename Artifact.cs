namespace GalaticVault {
	public class Artifact {
		public static string[] alphabet = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
		public static string[] mapped = new string[] { "H", "Z", "A", "U", "Y", "E", "K", "G", "O", "T", "I", "R", "J", "V", "W", "N", "M", "F", "Q", "S", "D", "B", "X", "L", "C", "P" };
		private bool IsNameEncoded = true;
		public string EncodedName { get; set; }
		private string DecodedName;
		public string Planet { get; set; }
		public string Discovery { get; set; }
		private string StorageLocation { get; set; }
		public string Description { get; set; }

		public void PrintArtifact() {
			Console.WriteLine($"\n{DecodedName.Trim()} entry information:");
			Console.WriteLine($"\nFound {Discovery} on planet {Planet}");
			Console.WriteLine($"\nIs it stored in {StorageLocation}");
			Console.WriteLine("\nDescription:");
			Console.WriteLine($"{Description}");
		}

		public override string ToString() {
			return $"{EncodedName},{Planet},{Discovery},{StorageLocation},{Description}";
		}

		public string GetName() {
			string[] name = GetDecodedName().Trim().Split(' ');
			foreach (string word in name) {
				string proper = word.Substring(0, 1) + word.Substring(1).ToLower();
				DecodedName = DecodedName.Replace(word, proper);
			}
			return DecodedName;
		}

		public Artifact(string name, string planet, string discovery, string storageLocation, string description) {
			EncodedName = name;
			Planet = planet;
			Discovery = discovery;
			StorageLocation = storageLocation;
			Description = description;
		}
		private string GetDecodedName() {
			if (IsNameEncoded == true) {
				string[] splitName = EncodedName.Trim().Split(' ');
				string[][] isolatedNames = new string[splitName.Length][];
				for (int i = 0; i < splitName.Length; i++) {
					isolatedNames[i] = splitName[i].Split('|');
				}

				foreach (string[] name in isolatedNames) {
					foreach (string pair in name) {
						string letter = pair[0].ToString();
						int level = int.Parse(pair.Substring(1));
						DecodedName += Decode(letter, level);
					}
					DecodedName += " ";
				}
				IsNameEncoded = false;
			}
			return DecodedName;
		}
		private static string Decode(string letter, int level) {
			if (level == 1) {
				int index = Array.IndexOf(alphabet, letter);
				string decodedLetter = alphabet[alphabet.Length - (index + 1)];
				return decodedLetter;
			} else {
				int index = Array.IndexOf(alphabet, letter);
				string newLetter = mapped[index];
				return Decode(newLetter, level - 1);
			}
		}


		// Although the encoding was not a required feature, I added it to help with testing and to ensure that the program was working as intended.
		// I also pulled quite a few hairs out due to it...
		private static string Encode(string letter, int level, int start = 0) {
			if (start == 0) {
				int index = Array.IndexOf(alphabet, letter);
				string newLetter = alphabet[alphabet.Length - (index + 1)];
				return Encode(newLetter, level, start + 1);
			} else if (start == level) {
				return letter;
			} else {
				int index = Array.IndexOf(mapped, letter);
				string newLetter = alphabet[index];
				return Encode(newLetter, level, start + 1);
			}
		}

		public static string EncodeName(string name) {
			string encodedName = "";
			Random random = new Random();
			string[] splitName = name.Split(" ");
			string[][] isolatedNames = new string[splitName.Length][];
			for (int i = 0; i < splitName.Length; i++) {
				isolatedNames[i] = new string[splitName[i].Length];
				for (int j = 0; j < splitName[i].Length; j++) {
					isolatedNames[i][j] = splitName[i][j].ToString();
				}
			}

			foreach (string[] word in isolatedNames) {
				for (int i = 0; i < word.Length; i++) {
					int level = random.Next(1, 5);
					string letterChar = word[i].ToUpper();
					encodedName += Encode(letterChar, level) + $"{level}";
					if (i != word.Length - 1) {
						encodedName += "|";
					}
				}
				encodedName += " ";
			}
			return encodedName;
		}

	}
}
