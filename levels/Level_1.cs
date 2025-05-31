using Godot;
using System.Collections.Generic;

public partial class Level_1 : Node2D
{
	public override void _Ready()
	{
		// Créer et ajouter le joueur
		var player = new Player();
		AddChild(player);

		// Instancier la porte
		var door = new Door();
		door.Position = new Vector2(500, 400 - 48);
		AddChild(door);
		
		// Connecter le signal
		door.PlayerEnteredDoor += OnPlayerEnteredDoor;
		
		// Sol de base
		for (int i = 0; i < 20; i++)
		{
			var tile = new Platform();
			tile.Position = new Vector2(100 + i * 32, 400);
			AddChild(tile);
		}
		
		// Plateforme en hauteur
		var highPlat = new Platform();
		highPlat.Position = new Vector2(250, 250);
		AddChild(highPlat);
	}
	
	private void OnPlayerEnteredDoor()
	{
		GetTree().ChangeSceneToFile("res://levels/Level_2.tscn");
	}
}
