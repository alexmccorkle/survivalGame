using Godot;
using System;
using System.Linq;

public partial class GameManager : Node // Singleton
{
  private static GameManager _instance;
  public static GameManager Instance
  {
    get
    {
      if (_instance == null)
        GD.PushError("GameManager instance not found!");
      return _instance;
    }
  }

  public override void _Ready()
  {
    // If an instance already exists, get rid of this one
    if (_instance != null)
    {
      QueueFree();
      return;
    }
    // Set this one as THE instance
    _instance = this;
  }

  // Food spawning methods
  public void SpawnFood(string foodType, Vector2 position)
  {
    var foodItemScene = GD.Load<PackedScene>("res://Scenes/Items/Food/FoodItem.tscn");
    var foodItem = foodItemScene.Instantiate<FoodItem>();

    if (!FoodDatabase.Foods.ContainsKey(foodType))
    {
      GD.PushError($"Food type '{foodType}' not found in database!");
      return;
    }

    var foodData = FoodDatabase.Foods[foodType];
    foodItem.ItemName = foodData.Name;
    foodItem.HungerRestoration = foodData.HungerRestoration;
    foodItem.Description = foodData.Description;
    // foodItem.ItemTexture = GD.Load<Texture2D>(foodData.TexturePath);

    foodItem.Position = position;
    AddChild(foodItem); // Simplified - food items will be children of GameManager  
  }

  // Debug/testing method
  public void SpawnRandomFood(Vector2 position)
  {
    var foodTypes = FoodDatabase.Foods.Keys.ToArray();
    var randomIndex = new Random().Next(0, foodTypes.Length);
    SpawnFood(foodTypes[randomIndex], position);
  }
}