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

		string value = string.Empty;
		while (!file.EofReached())
		{
			var line = file.GetLine();
			if (line.StartsWith(key + "="))
			{
				value = line.Substring(key.Length + 1);
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

		var error = file.Resize(0); // Clear the file before writing
		if (error != Error.Ok)
		{
			GD.PrintErr("Failed to resize data file.");
			file.Close();
			return;
		}

		// Write back the remaining lines
		foreach (var line in lines)
		{
			file.StoreLine(line);
		}
		file.Close();
		GD.Print("Data deleted successfully.");
	}

	public static void Clear()
	{
		var file = FileAccess.Open(SaveGame, FileAccess.ModeFlags.Write);
		if (FileAccess.GetOpenError() != Error.Ok)
		{
			GD.PrintErr("Failed to open data file for clearing.");
			return;
		}

		var error = file.Resize(0); // Clear the file
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
