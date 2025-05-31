using Godot;

public partial class Player : CharacterBody2D
{
	const float Speed = 200f;
	const float JumpForce = -400f;
	const float Gravity = 900f;
	const float DeathY = 800f;
	
	public readonly Vector2 size = new Vector2(32, 64);

	private Vector2 spawnPosition = new Vector2(100, 200); // Position de spawn fixe (x = 100, y = 200 par exemple)
	private int jumpsLeft = 2; // max 2 sauts
	private bool jumpButtonPressed = false;

	public void Respawn()
	{
		Position = spawnPosition;
		Velocity = Vector2.Zero;
	}

	public override void _Ready()
	{
		Position = spawnPosition;

		// Collider
		var shape = new RectangleShape2D { Size = size };
		var collider = new CollisionShape2D { Shape = shape };
		AddChild(collider);

		// Visuel
		var sprite = new ColorRect
		{
			Color = new Color(0f, 0.4f, 1f),
			Size = size,
			Position = -size / 2
		};
		AddChild(sprite);
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 input = Vector2.Zero;

		input.X = Input.GetJoyAxis(0, JoyAxis.LeftX);
		if (Mathf.Abs(input.X) < 0.2f)
		{
			input.X = 0;
		}

		Velocity = new Vector2(input.X * Speed, Velocity.Y);
		
		// Empêcher d'aller à gauche au-delà de x = 0
		if (Position.X < size.X/2 && Velocity.X < 0)
		{
			Velocity = new Vector2(0, Velocity.Y); // Optionnel : couper la vitesse horizontale à gauche
		}

		// Réinitialiser le compteur de saut si au sol
		if (IsOnFloor())
			jumpsLeft = 2;

		// Saut
		if (!Input.IsJoyButtonPressed(0, JoyButton.A))
		{
			jumpButtonPressed = false;
		}
		else if (jumpsLeft > 0 && !jumpButtonPressed)
		{
			jumpButtonPressed = true;
			Velocity = new Vector2(Velocity.X, JumpForce);
			jumpsLeft--;
		}

		// Gravité
		Velocity += new Vector2(0, Gravity * (float)delta);
		MoveAndSlide();

		// Respawn si tombe
		if (Position.Y > DeathY)
		{
			GD.Print("Player mort. Respawn.");
			Position = spawnPosition;
			Velocity = Vector2.Zero;
		}
	}
}
