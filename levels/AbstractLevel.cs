using Godot;
using System;

public abstract partial class AbstractLevel : Node2D
{
	protected Vector2 PlayerPosition;
	protected Vector2 ExitPosition;
	protected string NextLevelFile;

	protected void Initialize(Vector2 playerPosition, Vector2 exitPosition, string nextLevelFile)
	{
		PlayerPosition = playerPosition;
		ExitPosition = exitPosition;
		NextLevelFile = nextLevelFile;
	}
	
	public override void _Ready()
	{
		// Créer et ajouter le joueur
		var player = new Player();
		player.Initialize(PlayerPosition);
		AddChild(player);

		// Instancier la sortie
		var exit = new Door();
		exit.Position = ExitPosition;
		exit.PlayerEnteredDoor += OnPlayerEnteredDoor;
		AddChild(exit);
	}

	private void OnPlayerEnteredDoor()
	{
		GetTree().ChangeSceneToFile(NextLevelFile);
	}
}
