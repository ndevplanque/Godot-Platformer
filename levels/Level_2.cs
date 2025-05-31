using Godot;

public partial class Level_2 : AbstractLevel
{
		public override void _Ready()
	{
		Initialize(
			playerPosition: new Vector2(100, 200),
			exitPosition: new Vector2(250, 250 - 48),
			nextLevelFile: "res://Main.tscn"
		);

		base._Ready();

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
}
