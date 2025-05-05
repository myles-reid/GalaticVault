# GalaticVault

**GalaticVault** is a C# console application designed to assist in tracking galactic artifacts discovered during space expeditions. It provides functionalities to record, sort, and manage artifact data efficiently.

## Features

- Record details of newly discovered galactic artifacts.
- Sort artifacts based on various criteria.
- Decode artifacts based on a recursive algorithm
- Manage and update existing artifact records.

## Prerequisites

- .NET SDK (version 6.0 or later) installed on your machine. You can download it from the [.NET official website](https://dotnet.microsoft.com/download).

## Getting Started

### Clone the Repository

To obtain a local copy of the project, open your terminal or command prompt and execute:

```bash
git clone https://github.com/myles-reid/GalaticVault.git
cd GalaticVault
```

### Build the Application

Ensure you're in the project directory, then build the application using the .NET CLI:

```bash
dotnet build
```

### Run the Application

After a successful build, you can run the application with:

```bash
dotnet run
```

This will launch the console interface, allowing you to interact with the GalaticVault system.

## Project Structure

- `Program.cs` - Entry point of the application.
- `Artifact.cs` - Defines the `Artifact` class representing galactic artifacts.
- `Sort.cs` - Contains sorting logic for organizing artifacts.
- `GalaticVault.csproj` - Project file containing metadata and project configurations.
- `GalaticVault.sln` - Solution file for the project.
- `SampleInput/` - Directory containing sample input files for testing purposes.

## Encoding/Decoding Information
Each artifact name is encoded, and you must create a recursive function to decode it. The encoding scheme is as follows: 
  **Original array**: {'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'}
  **Mapped array**: {'H', 'Z', 'A', 'U', 'Y', 'E', 'K', 'G', 'O', 'T', 'I', 'R', 'J', 'V', 'W', 'N', 'M', 'F', 'Q', 'S', 'D', 'B', 'X', 'L', 'C', 'P'}
#### Encoding Details:
- Each encoded name contains characters followed by numbers indicating encoding levels (e.g., "A3" means character 'A' with encoding level 3).
- When the encoding level reaches 1 and goes to 0, the function directly maps the character using a reverse alphabet mapping. For example, 'A' becomes 'Z', 'B' becomes 'Y', and so on. 
*Example Decodings*:
A3:
Layer 3 → 2: 'A' maps to 'H' and 3→2.
Layer 2 → 1: 'H' maps to 'G' and 2→1.
Base case mirrors 'G' to 'T'.
B1: Directly mirrors to 'Y'.
C2:
Layer 2 → 1: 'C' maps to 'A' and 2→1.
Base case mirrors 'A' to 'Z'.

---

<p float="left">
  <img
  src="https://img.shields.io/badge/c%23-%23239120.svg?style=for-the-badge&logo=csharp&logoColor=white"
  />
  <img 
  src="https://img.shields.io/badge/.NET-512BD4?style=flat&logo=net&logoColor=white" height="28"
  />
</p>