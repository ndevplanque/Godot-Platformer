using Godot;

public partial class Door : Area2D
{
	public readonly Vector2 size = new Vector2(64, 64);
	protected RectangleShape2D shape;
	protected CollisionShape2D collider;
	protected ColorRect sprite;
	
	[Signal]
	public delegate void PlayerEnteredDoorEventHandler();

	public override void _Ready()
	{
		shape = new RectangleShape2D { Size = size };
		collider = new CollisionShape2D { Shape = shape };
		sprite = new ColorRect
		{
			Color = Colors.Black,
			Size = size,
			Position = -size / 2
		};
		
		AddChild(sprite);
		AddChild(collider);

		// Connecter le signal pour détecter quand un corps entre dans la porte
		this.BodyEntered += OnBodyEntered;
	}

	private void OnBodyEntered(Node body)
	{
		if (body is Player)
		{
			EmitSignal(nameof(PlayerEnteredDoor));
		}
	}
}
