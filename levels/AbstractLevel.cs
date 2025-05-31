using Godot;
using System;

public abstract partial class AbstractLevel : Node2D
{
	protected Vector2 PlayerPosition;
	protected Vector2 ExitPosition;
	protected string NextLevelFile;
	
	protected Player Player1;
	protected Door Exit;
	protected Node HUD;

	protected void Initialize(
		Vector2 playerPosition,
		Vector2 exitPosition,
		string nextLevelFile
	)
	{
		PlayerPosition = playerPosition;
		ExitPosition = exitPosition;
		NextLevelFile = nextLevelFile;
	}
	
	public override void _Ready()
	{
		// Créer et ajouter le joueur
		Player1 = new Player();
		Player1.Initialize(PlayerPosition);
		AddChild(Player1);

		// Instancier la sortie
		Exit = new Door();
		Exit.Position = ExitPosition;
		Exit.PlayerEnteredDoor += OnPlayerEnteredDoor;
		AddChild(Exit);
	}
	
	protected void SetupHUD()
	{
		HUD = GD.Load<PackedScene>("res://ui/HUD.tscn").Instantiate();
		AddChild(HUD);

		// Passer le player au HUD (pour afficher des infos)
		if (HUD.HasMethod("SetPlayer"))
		{
			HUD.Call("SetPlayer", Player1);
		}
	}

	private void OnPlayerEnteredDoor()
	{
		GetTree().ChangeSceneToFile(NextLevelFile);
	}
}
