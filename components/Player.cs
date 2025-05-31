using Godot;

public partial class Player : CharacterBody2D
{
	// Physique
	const float Speed = 200f;
	const float JumpForce = -400f;
	const float Gravity = 900f;
	const float DeathY = 800f;
	
	// Visuel
	public readonly Vector2 size = new Vector2(32, 64);
	protected RectangleShape2D shape;
	protected CollisionShape2D collider;
	protected ColorRect sprite;
	
	// Position de spawn fixe
	public Vector2 SpawnPosition { get; private set; }
	
	// Max 2 sauts
	private int jumpsLeft = 2; 
	private bool jumpButtonPressed = false;

 	public void Initialize(Vector2 spawnPosition)
	{
		SpawnPosition = spawnPosition;
		Position = SpawnPosition;
	}
	
	public void Respawn()
	{
		GD.Print("Player mort. Respawn.");
		Position = SpawnPosition;
		Velocity = Vector2.Zero;
	}

	public override void _Ready()
	{
		Position = SpawnPosition;

		shape = new RectangleShape2D { Size = size };
		collider = new CollisionShape2D { Shape = shape };
		sprite = new ColorRect
		{
			Color = new Color(0f, 0.4f, 1f),
			Size = size,
			Position = -size / 2
		};
		
		AddChild(collider);
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
		if (Position.X < size.X / 2 && Velocity.X < 0)
		{
			Velocity = new Vector2(0, Velocity.Y);
		}

		// Réinitialiser le compteur de saut si au sol
		if (IsOnFloor())
		{
			jumpsLeft = 2;
		}

		// Verouiller l'écoute du bouton de saut tant qu'il n'est pas lâché
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
		
		// Appliquer
		MoveAndSlide();

		// Respawn si tombé
		if (Position.Y > DeathY)
		{
			Respawn();
		}
	}
}
