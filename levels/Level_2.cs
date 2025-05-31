using Godot;

public partial class Level_2 : AbstractLevel
{
	public override void _Ready()
	{
		Initialize(
			playerPosition: new Vector2(100, 0),
			exitPosition: new Vector2(250, 450 - 48),
			nextLevelFile: "res://Main.tscn"
		);

		base._Ready();

		SetupHUD();

		// Sol de base
		for (int i = 0; i < 20; i++)
		{
			var tile = new Platform();
			tile.Position = new Vector2(0 + i * 32, 600);
			AddChild(tile);
		}

		// Plateforme en hauteur
		var highPlat = new Platform();
		highPlat.Position = new Vector2(250, 450);
		AddChild(highPlat);
	}
}
