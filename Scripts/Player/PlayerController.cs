using System;
using Godot;

public partial class PlayerController : CharacterBody2D
{
  private float speed = 400.0f;
  private PlayerStats stats;
  private const float SPRINT_SPEED_MULTIPLIER = 1.5f;
  private const float SPRINT_STAMINA_COST = 40f; // Stamina cost per second

  public override void _Ready()
  {
    // Get existing PlayerStats node
    stats = GetNode<PlayerStats>("PlayerStats");

    if (stats == null)
    {
      GD.PrintErr("PlayerStats node not found");
      return;
    }

    // Center the ColorRect in the viewport
    var viewport = GetViewport();
    var viewportSize = viewport.GetVisibleRect().Size;
    Position = viewportSize / 2;

    // Connect to the death signal
    stats.PlayerDied += OnPlayerDied;

    // To verify that script is running
    GD.Print($"PlayerController is ready! Position: {Position}");
  }


  public override void _Input(InputEvent @event)
  {
    // Add food spawning test
    if (@event.IsActionPressed("spawn_food"))
    {
      Vector2 spawnOffset = new Vector2(50, 0); // Spawn food 50 pixels to the right
      GameManager.Instance.SpawnRandomFood(Position + spawnOffset);
      GD.Print("Spawned food at: " + (Position + spawnOffset));
    }
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
        GD.Print($"Sprinting! Speed: {currentSpeed}"); // Debug print
      }
    }

    // Move the sprite:
    Velocity = velocity * currentSpeed;
    MoveAndSlide();
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
