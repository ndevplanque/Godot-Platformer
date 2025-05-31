using Godot;

public partial class HUD : CanvasLayer
{
	private Player Player1;

	public void SetPlayer(Player player1)
	{
		this.Player1 = player1;
	}

	public override void _Process(double delta)
	{
		if (Player1 != null)
		{
			// Exemple : update un label avec la vie
			// GetNode<Label>("LifeLabel").Text = player.Life.ToString();
		}
	}
}
