using System;
using Godot;

public partial class PlayerController : Sprite2D
{
  private float speed = 400.0f;
  private PlayerStats stats;
  private const float SPRINT_SPEED_MULTIPLIER = 1.5f;
  private const float SPRINT_STAMINA_COST = 20f; // Stamina cost per second

  public override void _Ready()
  {
    // Create and add PlayerStats as child node
    stats = new PlayerStats();
    AddChild(stats);


    // Center the ColorRect in the viewport
    var viewport = GetViewport();
    var viewportSize = viewport.GetVisibleRect().Size;
    Position = viewportSize / 2;

    // Connect to the death signal
    stats.PlayerDied += OnPlayerDied;

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

    // Handle Sprinting:
    float currentSpeed = speed;
    if (Input.IsActionPressed("sprint") && velocity != Vector2.Zero)
    {
      if (stats.UseStamina(SPRINT_STAMINA_COST * (float)delta))
      {
        currentSpeed *= SPRINT_SPEED_MULTIPLIER;
      } 
    }

    // Move the sprite:
    Position += velocity * speed * (float)delta;
  }

  private void OnPlayerDied()
  {
    // Handle Player Death
    GD.Print("Player has died!");
    // Can implement game over screen here, restart level etc.
  }

  public void TakeDamage(float amount)
  {
    stats.TakeDamage(amount);
  }

  public void EatFood(float nutritionValue)
  {
    stats.ConsumeFood(nutritionValue);
  }
}
