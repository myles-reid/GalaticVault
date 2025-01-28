using System.Text.RegularExpressions;

namespace GalaticVault {
	internal class Program {
		public static Artifact[] vault = new Artifact[5];
		public static int count = 0;
		public static bool sorted = false;
		public static bool running = true;
		public static bool vaultInitilized = false;
		public static void SortVault() {
			Sort.MergeSort(vault, 0, count - 1);
			sorted = true;
		}
		public static void InitilizeVault() {
			try {
				AddNewArtifact();
			} catch (Exception e) {
				Console.WriteLine(e.Message);
				Console.WriteLine("Terminating Program");
				Environment.Exit(0);
			}
			SortVault();
			vaultInitilized = true;
		}
		static void Main(string[] args) {
			InitilizeVault();
			Console.WriteLine("Welcome to the Galatic Vault Manager");
			do { MainMenu(); } while (running);
		}

		public static void MainMenu() {
			Console.WriteLine("\n<<<<< Main Menu >>>>>");
			Console.WriteLine("Please select an option");
			Console.WriteLine("1. Add Journey Log");
			Console.WriteLine("2. View and Search Inventory");
			Console.WriteLine("3. Save and Exit");
			string choice = Console.ReadLine();
			switch (choice) {
				case "1":
					bool adding = true;
					do {
						bool addAnother = RunAddNew();
						if (addAnother) {
							Console.WriteLine("\nWould you like to add another? (Y/N)");
							char input = Console.ReadKey().KeyChar;
							switch (input) {
								case 'y':
								case 'Y':
									break;
								case 'n':
								case 'N':
									adding = false;
									break;
								default:
									Console.WriteLine("\nInvalid response, returning to Main Menu");
									adding = false;
									break;
							}
						} else { adding = false; }
					} while (adding);
					break;
				case "2":
					RunViewAndSearch();
					break;
				case "3":
					SaveAndExit();
					break;
				default:
					Console.WriteLine("Invalid Selection. Options are 1, 2 or 3");
					break;
			}
		}

		public static void SaveAndExit() {
			Console.WriteLine("Are you sure you would like to Exit? (Y/N)");
			char input = Console.ReadKey().KeyChar;
			switch (input) {
				case 'y':
				case 'Y':
					SaveCollection();
					Console.WriteLine("\nExiting System");
					running = false;
					break;
				case 'n':
				case 'N':
					Console.WriteLine("\nReturning to Main Menu");
					break;
				default:
					Console.WriteLine("\nInvalid response, Returning to Main Menu");
					break;
			}
		}

		public static void RunViewAndSearch() {
			Console.WriteLine("<<<<<< View and Search >>>>>>");
			bool running = true;
			do {
				Console.WriteLine("\nPlease select one of the following options");
				Console.WriteLine("1. View Collection");
				Console.WriteLine("2. Search Collection");
				Console.WriteLine("3. Main Menu");
				string choice = Console.ReadLine();
				switch (choice) {
					case "1":
						Console.WriteLine("\nHere is the current Vault collection:");
						PrintVault();
						break;
					case "2":
						bool artifactFound = true;
						do {
							artifactFound = RunSearch();
						} while (!artifactFound);
						break;
					case "3":
						Console.WriteLine("Returning to Main Menu");
						running = false;
						break;
					default:
						Console.WriteLine("Invalid Selection. Options are 1, 2 or 3");
						break;
				}
			} while (running);
		}

		public static bool RunSearch() {
			Console.WriteLine("Please enter the full name of the artifact you would like to find");
			string input = Console.ReadLine();
			return SearchCollection(input, true);
		}

		public static bool RunAddNew() {
			bool adding = true;
			do {
				Console.WriteLine("\nPlease enter the full name of the journey log you wish to process.");
				Console.WriteLine("If you would like to return to the main menu please enter 'Exit' or 'Quit' ");
				string input = Console.ReadLine();
				if (input.ToLower().Equals("exit") || input.ToLower().Equals("quit")) { return false; }
				try {
					adding = AddNewArtifact(input);
				} catch (FileNotFoundException e) {
					Console.WriteLine(e.Message);
				}
			} while (adding);
			return true;
		}

		public static bool AddNewArtifact(string artifactName = "galactic_vault") {
			string[] data = RetrieveFileData(artifactName);
			if (data == null) { return true; }
			foreach (string artifact in data) {
				try {
					ParseArtifactData(artifact, out string encodedName, out string planet, out string discovery, out string storage, out string desc);
					AddArtifactToVault(encodedName, planet, discovery, storage, desc);
				} catch (FormatException e) {
					Console.WriteLine(e.Message);
					Console.WriteLine("Please edit the file and try again");
					return true;
				}
			}
			return false;
		}

		public static bool SearchCollection(string key, bool searching) {
			string searchTerm = key;
			key = key.ToLower().Trim();
			int hi = vault.Length - 1;
			int lo = 0;
			while (hi >= lo) {
				int mid = (hi + lo) / 2;
				if (vault[mid] == null) { hi = mid - 1; continue; }
				string current = vault[mid].GetName().ToLower().Trim();
				if (hi == lo && String.Compare(current, key) == 0) {
					if (searching) { vault[lo].PrintArtifact(); }
					return true;
				}
				if (String.Compare(current, key) == 0) {
					if (searching) { vault[mid].PrintArtifact(); }
					return true;
				}
				if (String.Compare(current, key) < 0) { lo = mid + 1; continue; }
				if (String.Compare(current, key) > 0) { hi = mid - 1; continue; }
			}
			if (searching) { Console.WriteLine($"\nAn Artifact with the name \"{searchTerm}\" not found in the vault."); }
			return false;
		}


		public static void IncreaseVaultSize() {
			Artifact[] newVault = new Artifact[vault.Length * 2];
			for (int i = 0; i < vault.Length; i++) {
				newVault[i] = vault[i];
			}
			vault = newVault;
		}

		public static bool ArtifactExists(Artifact name) {
			return SearchCollection(name.GetName(), false);
		}


		public static void AddArtifactToVault(string encodedName, string planet, string discovery, string storage, string desc) {
			Artifact newArtifact = new Artifact(encodedName, planet, discovery, storage, desc);
			if (count == vault.Length) { IncreaseVaultSize(); }
			if (ArtifactExists(newArtifact)) {
				Console.WriteLine($"\nAn artifact named {newArtifact.GetName()} has already been added to the vault.");
				return;
			}

			if (sorted) {
				Insert(newArtifact);
			} else {
				vault[count] = newArtifact;
			}
			count++;

		}

		public static void Insert(Artifact artifact) {
			string newArtifact = artifact.GetName();
			string current = null;
			for (int i = 0; i < vault.Length; i++) {
				if (vault[i] != null) {
					current = vault[i].GetName();
					if (String.Compare(current, newArtifact) > 0) {
						for (int j = vault.Length - 1; j > i; j--) {
							vault[j] = vault[j - 1];
						}
						vault[i] = artifact;
						Console.WriteLine($"\n{newArtifact}has been added to the vault");
						return;
					}
				} else {
					vault[i] = artifact;
					Console.WriteLine($"\n{newArtifact}has been added to the vault");
					return;
				}
			}
		}

		public static void PrintVault() {
			foreach (Artifact artifact in vault) {
				if (artifact != null) {
					Console.WriteLine(artifact.GetName());
				}
			}
		}


		//System.IO File Handling
		public static void ParseArtifactData(string data, out string encodedName, out string planet, out string discovery, out string storage, out string desc) {
			string[] splitData = data.Split(",");
			// Regex to find the proper encoded name format, to try and catch any wrong encoded names
			string validName = @"/^([A-Za-z][0-9]\|)*[A-Za-z][0-9]( ([A-Za-z][0-9]\|)*[A-Za-z][0-9])*$/gm";
			encodedName = splitData[0];
			if (Regex.IsMatch(splitData[0], validName)) {
				throw new FormatException("Invalid name format in file.");
			}
			planet = splitData[1];
			discovery = splitData[2];
			storage = splitData[3];
			desc = splitData[4];
		}

		public static string[] RetrieveFileData(string fileInput) {
			// Ensure that this gets edited and changed to the correct local path
			// Leave the interpolated string as it is as the filename.
			string path = @$"C:\Users\mrmyl\OneDrive\Desktop\Development\school\Back-end\Algorihms and Data Structure\Assignments\Final Project\SampleInput\{fileInput}.txt";
			if (Path.Exists(path)) {
				string[] readText = File.ReadAllLines(path);
				return readText;
			} else {
				if (vaultInitilized) {
					throw new FileNotFoundException($"File with the name <{fileInput}.txt> not found, please try again.");
				} else {
					throw new FileNotFoundException("Vault not initilized, file was not found.");
				}
			}
		}


		public static void SaveCollection() {
			// Please change this file path to one of your own
			string location = @$"C:\Users\mrmyl\OneDrive\Desktop\Development\school\Back-end\Algorihms and Data Structure\Assignments\Final Project\SampleInput\expedition_summary.txt"; ;
			string[] vaultData = new string[count];
			for (int i = 0; i < vault.Length; i++) {
				if (vault[i] != null) {
					vaultData[i] = vault[i].ToString();
				}
			}
			File.WriteAllLines(location, vaultData);
			Console.WriteLine($"\nYour collection has been saved as expedition_summary.txt");
		}
	}
}
