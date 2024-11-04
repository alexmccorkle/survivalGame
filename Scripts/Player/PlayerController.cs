using System;
using Godot;

public partial class PlayerController : Sprite2D
{
  private float speed = 400.0f;

  public override void _Ready()
  {
    // Center the ColorRect in the viewport
    var viewport = GetViewport();
    var viewportSize = viewport.GetVisibleRect().Size;
    Position = viewportSize / 3;

    // To verify that script is running
    GD.Print($"PlayerController is ready! Position: {Position}");
  }



  public override void _Process(double delta)
  {
    Vector2 velocity = Vector2.Zero;

    // Get input:
    if (Input.IsActionPressed("ui_right"))
      velocity.X += 1;
    if (Input.IsActionPressed("ui_left"))
      velocity.X -= 1;
    if (Input.IsActionPressed("ui_down"))
      velocity.Y += 1;
    if (Input.IsActionPressed("ui_up"))
      velocity.Y -= 1;

    // Normalize velocity to prevent faster diagonal movement
    velocity = velocity.Normalized();

    // Move the sprite:
    Position += velocity * speed * (float)delta;

    // Optional: Print position to output window to verify movement
    if (velocity != Vector2.Zero)
    {
      GD.Print($"Position: {Position}");
    }
  }
}
