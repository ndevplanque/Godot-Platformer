using Godot;

public partial class Platform : StaticBody2D
{
	public readonly Vector2 size = new Vector2(32, 32);
	protected RectangleShape2D shape;
	protected CollisionShape2D collider;
	protected ColorRect sprite;

	public override void _Ready()
	{
		shape = new RectangleShape2D { Size = size };
		collider = new CollisionShape2D { Shape = shape };
		sprite = new ColorRect
		{
			Color = new Color(0.2f, 0.2f, 0.2f),
			Size = size,
			Position = -size / 2
		};

		AddChild(collider);
		AddChild(sprite);
	}
}
