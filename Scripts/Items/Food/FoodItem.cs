using Godot;
using System;

public partial class FoodItem : Node2D
{
  [Export]
  public string ItemName { get; set; } = "Food";

  [Export]
  public float HungerRestoration { get; set; } = 20f;

  [Export]
  public string Description { get; set; } = "A basic food item";

  private Area2D _pickupArea;
  private Label _label;
  private ColorRect _visual;

  public override void _Ready()
  {
    // Get references to nodes
    _pickupArea = GetNode<Area2D>("PickupArea");
    _label = GetNode<Label>("Label");
    _visual = GetNode<ColorRect>("ColorRect");

    // Set up label
    _label.Text = ItemName;

    // Get color of food from database (later this will be sprites)
    if (FoodDatabase.Foods.TryGetValue(ItemName, out var foodData))
    {
      _visual.Color = foodData.Color;
    }

    // Connect signal
    _pickupArea.BodyEntered += OnBodyEntered;
  }

  private void OnBodyEntered(Node2D body)
  {
    if (body is PlayerController player)
    {
      var playerStats = player.GetNode<PlayerStats>("PlayerStats");
      if (playerStats != null)
      {
        playerStats.RestoreHunger(HungerRestoration);
        GD.Print($"Food collected! Restored {HungerRestoration} hunger");
        QueueFree();
      }
    }
  }
}