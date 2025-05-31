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
	
	public void Initialize(string name, Vector2 spawnPosition)
	{
		CharacterName = name;
		SpawnPosition = spawnPosition;
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
		collider = new CollisionShape2D { Shape = shape };
		sprite = new ColorRect
		{
			Color = new Color(0.2f, 1f, 0.2f), 
			Size = size,
			Position = -size / 2
		};

		AddChild(collider);
		AddChild(sprite);
		
		// Display the character's name
		var nameLabel = new Label
		{
			Text = CharacterName,
			Position = new Vector2(-size.X + 2, -size.Y / 2 - 25),
			HorizontalAlignment = HorizontalAlignment.Center,
			VerticalAlignment = VerticalAlignment.Center,
		};

		AddChild(nameLabel);

		Spawn();
	}

	public override void _PhysicsProcess(double delta)
	{
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
