using Godot;
using Godot.Collections;

// todo: refacto
public partial class Obstacle : Platform
{
	public override void _Ready()
	{
		base._Ready();
		sprite.Color = new Color(1f, 0f, 0f); // rouge
	}

	public override void _PhysicsProcess(double delta)
	{
		var bodies = GetOverlappingBodies();
		GD.Print("Bodies detected: ", bodies.Count);
		foreach (Node2D body in bodies)
		{
			GD.Print("Body: ", body.Name);
			if (body is Player player)
			{
				player.Respawn();
			}
		}
	}

	public Godot.Collections.Array GetOverlappingBodies()
	{
		var intersectQuery = new PhysicsShapeQueryParameters2D
		{
			ShapeRid = collider.Shape.GetRid(),
			Transform = GlobalTransform,
			CollideWithAreas = false,
			CollideWithBodies = true,
			Exclude = new Godot.Collections.Array<Rid> { this.GetRid() }
		};

		var result = GetWorld2D().DirectSpaceState.IntersectShape(intersectQuery);

		var bodies = new Godot.Collections.Array();

		foreach (Dictionary dict in result)
		{
			GD.Print("Hit: ", dict);

			if (dict.ContainsKey("collider"))
			{
				var colliderVariant = dict["collider"];
				var colliderNode = colliderVariant.As<Node2D>();
				if (colliderNode != null)
				{
					GD.Print("Collider node found: ", colliderNode.Name);
					bodies.Add(colliderNode);
				}
			}
		}

		return bodies;
	}
}
