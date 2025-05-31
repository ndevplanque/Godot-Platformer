using Godot;

public partial class Main : Control
{
	public override void _Ready()
	{	
		base._Ready();
		
		var startButton = GetNode<Button>("VBoxContainer/StartButton");
		var quitButton = GetNode<Button>("VBoxContainer/QuitButton");

		startButton.GrabFocus();

		startButton.Pressed += OnStartPressed;
		quitButton.Pressed += OnQuitPressed;
	}

	private void OnStartPressed()
	{
		GetTree().ChangeSceneToFile("res://levels/Level_1.tscn");
	}

	private void OnQuitPressed()
	{
		GetTree().Quit();
	}
}
