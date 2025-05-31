using Godot;
using System;

public partial class NPC : CharacterBody2D
{
	const float Speed = 200f;
	const float JumpForce = -400f;
	const float DeathY = 800f;
	const float Gravity = 900f;

	// Visuel
	public readonly Vector2 size = new Vector2(32, 64);
	protected RectangleShape2D shape;
	protected CollisionShape2D collider;
	protected ColorRect sprite;

	// Position de spawn fixe
	public Vector2 SpawnPosition { get; private set; }

	public string CharacterName { get; private set; }

	// Interactions
	private ColorRect pressButtonIndicator;
	private Area2D interactionArea;
	private CollisionShape2D interactionShape;
	private bool playerInRange = false;
	
	private DialogBox DialogBox;

	public void Initialize(string name, Vector2 spawnPosition, DialogBox dialogBox)
	{
		CharacterName = name;
		SpawnPosition = spawnPosition;
		DialogBox = dialogBox;
	}

	private void Spawn()
	{
		Position = SpawnPosition;
		Velocity = Vector2.Zero;
	}

	public void Respawn()
	{
		GD.Print("Respawning...");
		Spawn();
	}

	public override void _Ready()
	{
		shape = new RectangleShape2D { Size = size };
		collider = new CollisionShape2D { Shape = shape,  };
		sprite = new ColorRect
		{
			Color = new Color(0.2f, 1f, 0.2f),
			Size = size,
			Position = -size / 2
		};

		AddChild(collider);
		AddChild(sprite);

		SetCollisionLayerValue(3, true); // NPC est sur layer 3
		SetCollisionMaskValue(1, true);  // Collisionne avec le sol
		SetCollisionMaskValue(2, false); // Ignore les joueurs
		SetCollisionMaskValue(3, false); // Ignore les autres NPCs
		
		// Display the character's name
		var nameLabel = new Label
		{
			Text = CharacterName,
			Position = new Vector2(-size.X + 2, -size.Y / 2 - 25),
			HorizontalAlignment = HorizontalAlignment.Center,
			VerticalAlignment = VerticalAlignment.Center,
		};
		AddChild(nameLabel);

		// Affiche un cercle bleu avec un X dedans en entrant dans la zone d'interaction
		pressButtonIndicator = new ColorRect
		{
			Color = new Color(0f, 0f, 1f), // Blue
			Size = new Vector2(32, 32),
			Position = new Vector2(-16, -96), // Centered around the NPC
			Visible = false // Initially hidden,
		};
		pressButtonIndicator.AddChild(new Label
		{
			Text = "X",
			Position = new Vector2(11, 4), // Center the X in the circle
			HorizontalAlignment = HorizontalAlignment.Center,
			VerticalAlignment = VerticalAlignment.Center,
			Modulate = new Color(1f, 1f, 1f) // White color for the X
		});
		AddChild(pressButtonIndicator);

		// Interaction area
		interactionArea = new Area2D();
		interactionArea.BodyEntered += OnBodyEntered;
		interactionArea.BodyExited += OnBodyExited;
		interactionShape = new CollisionShape2D
		{
			Shape = new CircleShape2D { Radius = 56 } // 56 pixels radius for the interaction area
		};
		interactionArea.AddChild(interactionShape);
		AddChild(interactionArea);

		Spawn();
	}

	private void OnBodyEntered(Node body)
	{
		if (body is Player)
		{
			playerInRange = true;
			pressButtonIndicator.Visible = true; // Show the indicator when player is in range
			GD.Print($"Player entered interaction area with {CharacterName}.");
		}
	}

	private void OnBodyExited(Node body)
	{
		if (body is Player)
		{
			playerInRange = false;
			pressButtonIndicator.Visible = false; // Hide the indicator when player is out of range
			GD.Print($"Player exited interaction area with {CharacterName}.");
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		// Déclencher un dialogue si le joueur est dans la zone
		if (playerInRange && Input.IsJoyButtonPressed(0, JoyButton.X))
		{
			GD.Print($"Dialogue avec {CharacterName} !");
			DialogBox.ShowDialog("Bonjour, héros ! Bienvenue dans ce monde mystérieux.");
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
