using Godot;

public partial class Player : CharacterBody2D
{
	// Physique
	const float Speed = 200f;
	const float JumpForce = -400f;
	const float DeathY = 800f;
	const float DefaultGravity = 900f;
	const float SpawnGravity = 4000f;
	float Gravity = DefaultGravity;

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

	// Nombre de vies du joueur
	public int Lives { get; private set; }


	public void Initialize(Vector2 spawnPosition)
	{
		SpawnPosition = spawnPosition;
		Lives = Data.Load("lives") != string.Empty ? int.Parse(Data.Load("lives")) : 3;
	}

	private void Spawn()
	{
		Position = SpawnPosition;
		Gravity = SpawnGravity;
		Velocity = Vector2.Zero;
	}

	public void Respawn()
	{
		Lives--;
		Data.Save("lives", Lives.ToString());
		GD.Print("Life lost, remaining: " + Lives);
		if (Lives <= 0)
		{
			Data.Delete("lives");
			GD.Print("Game Over");
			GetTree().ChangeSceneToFile("res://Main.tscn");
			return;
		}
		GD.Print("Respawning...");
		Spawn();
	}

	public override void _Ready()
	{
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

		SetCollisionLayerValue(2, true); // Player est sur layer 2
		SetCollisionMaskValue(1, true);  // Collisionne uniquement avec le sol et les plateformes
		SetCollisionMaskValue(2, false); // Ignore les autres joueurs
		SetCollisionMaskValue(3, false); // Ignore les NPCs

		Spawn();
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
			Gravity = DefaultGravity;
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
