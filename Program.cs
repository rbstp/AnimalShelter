string[] menuOptions = {
    "1. List all pets",
    "2. Add new pet",
    "3. Complete pet info",
    "4. Complete pet nickname/personality",
    "5. Edit age",
    "6. Edit personality",
    "7. Find cats by characteristics",
    "8. Find dogs by characteristics",
    "9. Exit"
};

const string FILE_PATH = "animals.txt";
const int MAX_PETS = 8;
string[,] ourAnimals = LoadAnimals();

bool exit = false;
while (!exit)
{
    Console.Clear();
    Console.WriteLine("Welcome to the Contoso Pets Application!");
    Console.WriteLine();

    foreach (string option in menuOptions)
    {
        Console.WriteLine(option);
    }

    Console.Write("\nEnter your selection number (or type exit): ");
    string selection = Console.ReadLine() ?? "0";

    switch (selection)
    {
        case "1":
            ListAllPets(ourAnimals);
            break;
        case "2":
            AddNewPet(ourAnimals);
            break;
        case "3":
            CompletePetInfo(ourAnimals);
            break;
        case "4":
            CompletePetPersonality(ourAnimals);
            break;
        case "5":
            EditAge(ourAnimals);
            break;
        case "6":
            EditPersonality(ourAnimals);
            break;
        case "7":
            FindCatsByCharacteristics(ourAnimals);
            break;
        case "8":
            FindDogsByCharacteristics(ourAnimals);
            break;
        case "9":
            exit = true;
            break;
        case "exit":
            exit = true;
            break;
        default:
            Console.WriteLine("Invalid selection. Press any key to continue.");
            Console.ReadKey();
            break;
    }
}

static string[,] LoadAnimals()
{
    string[,] animals = new string[MAX_PETS, 6];
    if (File.Exists(FILE_PATH))
    {
        string[] lines = File.ReadAllLines(FILE_PATH);
        for (int i = 0; i < lines.Length && i < MAX_PETS; i++)
        {
            string[] parts = lines[i].Split('|');
            if (parts.Length == 6)
            {
                for (int j = 0; j < 6; j++)
                {
                    animals[i, j] = parts[j];
                }
            }
        }
    }
    // Validate animal data
    ValidateAnimalData(animals);
    return animals;
}

static void ValidateAnimalData(string[,] animals)
{
    for (int i = 0; i < animals.GetLength(0); i++)
    {
        string petID = animals[i, 0];
        if (string.IsNullOrEmpty(petID))
            continue; // Skip default pet IDs

        // Validate Age
        bool validAge = int.TryParse(animals[i, 2], out _);
        while (!validAge)
        {
            Console.Write($"Invalid age for Pet ID {petID}. Enter a valid numeric age: ");
            string input = Console.ReadLine() ?? "";
            if (int.TryParse(input, out _))
            {
                animals[i, 2] = input;
                validAge = true;
            }
            else
            {
                Console.WriteLine("Age must be a numeric value.");
            }
        }

        // Validate Physical Description
        while (string.IsNullOrEmpty(animals[i, 3]) || animals[i, 3].Length == 0)
        {
            Console.Write($"Physical description missing for Pet ID {petID}. Enter a valid description (size, color, gender, weight, housebroken): ");
            string desc = Console.ReadLine() ?? "";
            if (!string.IsNullOrEmpty(desc) && desc.Length > 0)
            {
                animals[i, 3] = desc;
            }
            else
            {
                Console.WriteLine("Physical description cannot be empty.");
            }
        }

        // Validate Personality Description
        while (string.IsNullOrEmpty(animals[i, 4]) || animals[i, 4].Length == 0)
        {
            Console.Write($"Personality description missing for Pet ID {petID}. Enter a valid description: ");
            string personality = Console.ReadLine() ?? "";
            if (!string.IsNullOrEmpty(personality))
            {
                animals[i, 4] = personality;
            }
            else
            {
                Console.WriteLine("Personality description cannot be empty.");
            }
        }

        // Validate Nickname
        while (string.IsNullOrEmpty(animals[i, 5]) || animals[i, 5].Length == 0)
        {
            Console.Write($"Nickname missing for Pet ID {petID}. Enter a valid nickname: ");
            string nickname = Console.ReadLine() ?? "";
            if (!string.IsNullOrEmpty(nickname))
            {
                animals[i, 5] = nickname;
            }
            else
            {
                Console.WriteLine("Nickname cannot be empty.");
            }
        }
    }

    Console.WriteLine("All animal data requirements are met.");
    Console.WriteLine("Press any key to continue.");
    Console.ReadKey();
    SaveAnimals(animals);
}

static void SaveAnimals(string[,] animals)
{
    List<string> lines = new List<string>();
    for (int i = 0; i < animals.GetLength(0); i++)
    {
        if (!string.IsNullOrEmpty(animals[i, 0]))
        {
            lines.Add(string.Join("|", 
                animals[i, 0], 
                animals[i, 1], 
                animals[i, 2], 
                animals[i, 3], 
                animals[i, 4], 
                animals[i, 5]));
        }
    }
    File.WriteAllLines(FILE_PATH, lines);
}

static void ListAllPets(string[,] animals)
{
    Console.Clear();
    for (int i = 0; i < animals.GetLength(0); i++)
    {
        if (!string.IsNullOrEmpty(animals[i, 0]))
        {
            Console.WriteLine($"\nID #{animals[i, 0]}");
            Console.WriteLine($"Species: {animals[i, 1]}");
            Console.WriteLine($"Age: {animals[i, 2]}");
            Console.WriteLine($"Physical description: {animals[i, 3]}");
            Console.WriteLine($"Personality: {animals[i, 4]}");
            Console.WriteLine($"Nickname: {animals[i, 5]}");
        }
    }
    Console.WriteLine("\nPress any key to continue.");
    Console.ReadKey();
}

static void AddNewPet(string[,] animals)
{
    bool addAnother = true;
    
    while (addAnother)
    {
        Console.Clear();
        
        // Check if shelter is at capacity
        int currentPets = 0;
        for (int i = 0; i < animals.GetLength(0); i++)
        {
            if (!string.IsNullOrEmpty(animals[i, 0]))
            {
                currentPets++;
            }
        }

        // Display current pets and remaining capacity
        Console.WriteLine($"We currently have {currentPets} pets that need homes. We can manage {MAX_PETS - currentPets} more.");
        
        if (currentPets >= animals.GetLength(0))
        {
            Console.WriteLine("Sorry, the shelter is at maximum capacity (8 pets).");
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
            return;
        }

        string species = "";
        bool validSpecies = false;

        // Find next available space
        int index = 0;
        for (int i = 0; i < animals.GetLength(0); i++)
        {
            if (string.IsNullOrEmpty(animals[i, 0]))
            {
                index = i;
                break;
            }
        }

        // Get and validate species
        while (!validSpecies)
        {
            Console.Write("Enter pet species (dog/cat): ");
            species = (Console.ReadLine() ?? "").ToLower();
            if (species == "dog" || species == "cat")
                validSpecies = true;
            else
                Console.WriteLine("Invalid species. Please enter 'dog' or 'cat'.");
        }

        // Generate ID
        string id = species[..1] + (index + 1);

        // Add basic info
        animals[index, 0] = id;
        animals[index, 1] = species;
        
        // Prompt user for additional information
        Console.Write("Enter age: ");
        animals[index, 2] = Console.ReadLine() ?? "Unknown";
        
        Console.Write("Enter physical description: ");
        animals[index, 3] = Console.ReadLine() ?? "N/A";
        
        Console.Write("Enter personality description: ");
        animals[index, 4] = Console.ReadLine() ?? "N/A";
        
        Console.Write("Enter nickname: ");
        animals[index, 5] = Console.ReadLine() ?? "N/A";

        SaveAnimals(animals);

        Console.WriteLine($"\nAdded pet with ID: {id}");
        
        // Ask if they want to add another pet
        if (currentPets + 1 >= animals.GetLength(0))
        {
            Console.WriteLine("Shelter is now at maximum capacity.");
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
            return;
        }

        Console.Write("\nWould you like to add another pet? (y/n): ");
        string response = (Console.ReadLine() ?? "n").ToLower();
        addAnother = (response == "y" || response == "yes");
    }
}

static void CompletePetInfo(string[,] animals)
{
    Console.Clear();
    Console.Write("Enter pet ID to update: ");
    string id = Console.ReadLine() ?? "";

    int index = FindPetIndex(animals, id);
    if (index == -1)
    {
        Console.WriteLine("Pet not found.");
        Console.ReadKey();
        return;
    }

    Console.WriteLine($"Current age: {animals[index, 2]}");
    Console.Write("Enter new age (or press Enter to keep current): ");
    string ageInput = Console.ReadLine() ?? "";
    string age = string.IsNullOrEmpty(ageInput) ? animals[index, 2] : ageInput;

    Console.WriteLine($"Current physical description: {animals[index, 3]}");
    Console.Write("Enter new physical description (or press Enter to keep current): ");
    string descInput = Console.ReadLine() ?? "";
    string description = string.IsNullOrEmpty(descInput) ? animals[index, 3] : descInput;

    animals[index, 2] = age;
    animals[index, 3] = description;

    SaveAnimals(animals);

    Console.WriteLine("Information updated successfully.");
    Console.ReadKey();
}

static void CompletePetPersonality(string[,] animals)
{
    Console.Clear();
    Console.Write("Enter pet ID to update: ");
    string id = Console.ReadLine() ?? "";

    int index = FindPetIndex(animals, id);
    if (index == -1)
    {
        Console.WriteLine("Pet not found.");
        Console.ReadKey();
        return;
    }

    Console.WriteLine($"Current nickname: {animals[index, 5]}");
    Console.Write("Enter new nickname (or press Enter to keep current): ");
    string nicknameInput = Console.ReadLine() ?? "";
    string nickname = string.IsNullOrEmpty(nicknameInput) ? animals[index, 5] : nicknameInput;

    Console.WriteLine($"Current personality description: {animals[index, 4]}");
    Console.Write("Enter new personality description (or press Enter to keep current): ");
    string personalityInput = Console.ReadLine() ?? "";
    string personality = string.IsNullOrEmpty(personalityInput) ? animals[index, 4] : personalityInput;

    animals[index, 4] = personality;
    animals[index, 5] = nickname;

    SaveAnimals(animals);

    Console.WriteLine("Information updated successfully.");
    Console.ReadKey();
}

static void EditAge(string[,] animals)
{
    Console.Clear();
    Console.Write("Enter pet ID to update: ");
    string id = Console.ReadLine() ?? "";

    int index = FindPetIndex(animals, id);
    if (index == -1)
    {
        Console.WriteLine("Pet not found.");
        Console.ReadKey();
        return;
    }

    Console.WriteLine($"Current age: {animals[index, 2]}");
    Console.Write("Enter new age (or press Enter to keep current): ");
    string ageInput = Console.ReadLine() ?? "";
    string age = string.IsNullOrEmpty(ageInput) ? animals[index, 2] : ageInput;

    animals[index, 2] = age;
    SaveAnimals(animals);

    Console.WriteLine("Age updated successfully.");
    Console.ReadKey();
}

static void EditPersonality(string[,] animals)
{
    Console.Clear();
    Console.Write("Enter pet ID to update: ");
    string id = Console.ReadLine() ?? "";

    int index = FindPetIndex(animals, id);
    if (index == -1)
    {
        Console.WriteLine("Pet not found.");
        Console.ReadKey();
        return;
    }

    Console.WriteLine($"Current personality description: {animals[index, 4]}");
    Console.Write("Enter new personality description (or press Enter to keep current): ");
    string personalityInput = Console.ReadLine() ?? "";
    string personality = string.IsNullOrEmpty(personalityInput) ? animals[index, 4] : personalityInput;

    animals[index, 4] = personality;
    SaveAnimals(animals);

    Console.WriteLine("Personality updated successfully.");
    Console.ReadKey();
}

static void FindCatsByCharacteristics(string[,] animals)
{
    Console.Clear();
    Console.Write("Enter physical characteristics to search for: ");
    string searchTerm = (Console.ReadLine() ?? "").ToLower();

    bool found = false;
    for (int i = 0; i < animals.GetLength(0); i++)
    {
        if (!string.IsNullOrEmpty(animals[i, 0]) &&
            animals[i, 1] == "cat" &&
            animals[i, 3].ToLower().Contains(searchTerm))
        {
            DisplayPet(animals, i);
            found = true;
        }
    }

    if (!found)
        Console.WriteLine("No cats found matching those characteristics.");

    Console.WriteLine("\nPress any key to continue.");
    Console.ReadKey();
}

static void FindDogsByCharacteristics(string[,] animals)
{
    Console.Clear();
    Console.WriteLine("Enter dog characteristics to search for separated by commas");
    string input = (Console.ReadLine() ?? "").ToLower();
    string[] searchTerms = [.. input.Split(',')
                                .Select(term => term.Trim())
                                .Where(term => !string.IsNullOrEmpty(term))];

    // Simple ASCII spinning animation
    string[] searchingIcons = ["-", "\\", "|", "/"];
    int iconIndex = 0;
    
    // Search animation with countdown
    int countdown = 3;
    DateTime startTime = DateTime.Now;
    while (countdown > 0)
    {
        string searchAnimation = $"\rSearching {searchingIcons[iconIndex]} {string.Join(", ", searchTerms)} / {countdown}";
        Console.Write(searchAnimation.PadRight(Console.WindowWidth - 1));  // Clear the line
        iconIndex = (iconIndex + 1) % searchingIcons.Length;
        
        if ((DateTime.Now - startTime).TotalSeconds >= 1)
        {
            countdown--;
            startTime = DateTime.Now;
        }
        
        Thread.Sleep(100);
    }
    Console.WriteLine();

    // Dictionary to store dogs and their matching terms
    Dictionary<int, List<string>> matchedDogs = new();

    // Find matches for each search term
    foreach (string term in searchTerms)
    {
        for (int i = 0; i < animals.GetLength(0); i++)
        {
            if (!string.IsNullOrEmpty(animals[i, 0]) &&
                animals[i, 1].ToLower() == "dog" &&
                animals[i, 3].ToLower().Contains(term))
            {
                if (!matchedDogs.ContainsKey(i))
                {
                    matchedDogs[i] = new List<string>();
                }
                matchedDogs[i].Add(term);
            }
        }
    }

    Console.WriteLine();

    // Display results
    if (matchedDogs.Count == 0)
    {
        Console.WriteLine("None of our dogs are a match for: " + string.Join(", ", searchTerms));
    }
    else
    {
        // Show matches and dog details
        foreach (var dogMatch in matchedDogs)
        {
            int index = dogMatch.Key;
            foreach (string term in dogMatch.Value)
            {
                Console.WriteLine($"Our dog Nickname: {animals[index, 5]} matches your search for {term}");
            }
            Console.WriteLine($"Nickname: {animals[index, 5]} (ID #: {animals[index, 0]})");
            Console.WriteLine($"Physical description: {animals[index, 3]}");
            Console.WriteLine($"Personality: {animals[index, 4]}");
            Console.WriteLine();
        }
    }

    Console.WriteLine("Press the Enter key to continue");
    Console.ReadLine();
}

static int FindPetIndex(string[,] animals, string id)
{
    for (int i = 0; i < animals.GetLength(0); i++)
    {
        if (animals[i, 0] == id)
            return i;
    }
    return -1;
}

static void DisplayPet(string[,] animals, int index)
{
    Console.WriteLine($"\nID #{animals[index, 0]}");
    Console.WriteLine($"Species: {animals[index, 1]}");
    Console.WriteLine($"Age: {animals[index, 2]}");
    Console.WriteLine($"Physical description: {animals[index, 3]}");
    Console.WriteLine($"Personality: {animals[index, 4]}");
    Console.WriteLine($"Nickname: {animals[index, 5]}");
}