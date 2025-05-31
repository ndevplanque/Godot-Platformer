using Godot;
using System;

public partial class Data : Node
{
	private static string SaveGame = "user://save_game.dat";

	public static void Save(string key, string value)
	{
		var file = FileAccess.Open(SaveGame, FileAccess.ModeFlags.Write);
		if (FileAccess.GetOpenError() != Error.Ok)
		{
			GD.PrintErr("Failed to open data file for writing.");
			return;
		}

		// Search for existing value
		var existingValue = Load(key);
		if (!string.IsNullOrEmpty(existingValue))
		{
			// If the key already exists, delete the old line
			Delete(key);
		}

		// Add a new line
		var success = file.StoreLine($"{key}={value}");
		if (success)
		{
			GD.Print("Data saved successfully.");
		}
		else
		{
			GD.PrintErr("Failed to save data.");
		}

		file.Close();
	}

	public static string Load(string key)
	{
		var file = FileAccess.Open(SaveGame, FileAccess.ModeFlags.Read);
		if (FileAccess.GetOpenError() != Error.Ok)
		{
			GD.PrintErr("Failed to open data file for reading.");
			return string.Empty;
		}

		// Find the value related to the given key
		var match = key + "=";
		string value = string.Empty;
		while (!file.EofReached())
		{
			var line = file.GetLine();
			if (line.StartsWith(match))
			{
				value = line.Substring(match.Length);
				break;
			}
		}

		file.Close();
		return value;
	}

	public static void Delete(string key)
	{
		var file = FileAccess.Open(SaveGame, FileAccess.ModeFlags.ReadWrite);
		if (FileAccess.GetOpenError() != Error.Ok)
		{
			GD.PrintErr("Failed to open data file for reading.");
			return;
		}

		// Gather all data that are not from the given key
		var match = key + "=";
		var lines = new System.Collections.Generic.List<string>();
		while (!file.EofReached())
		{
			var line = file.GetLine();
			if (!line.StartsWith(match))
			{
				lines.Add(line);
			}
		}

		// Clear the file
		var error = file.Resize(0);
		if (error != Error.Ok)
		{
			GD.PrintErr("Failed to resize data file.");
			file.Close();
			return;
		}

		// Write back the gathered lines
		foreach (var line in lines)
		{
			file.StoreLine(line);
		}
		GD.Print("Data deleted successfully.");

		file.Close();
	}

	public static void Clear()
	{
		var file = FileAccess.Open(SaveGame, FileAccess.ModeFlags.Write);
		if (FileAccess.GetOpenError() != Error.Ok)
		{
			GD.PrintErr("Failed to open data file for clearing.");
			return;
		}

		// Clear the file
		var error = file.Resize(0);
		if (error == Error.Ok)
		{
			GD.Print("Data cleared successfully.");
		}
		else
		{
			GD.PrintErr("Failed to clear data file.");
		}

		file.Close();
	}
}
