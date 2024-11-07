using Godot;
using System.Collections.Generic;

public static class FoodDatabase
{
  public class FoodData
  {
    public string Name { get; set; }
    public float HungerRestoration { get; set; }
    public string Description { get; set; }
    public string TexturePath { get; set; }
  }

  public static Dictionary<string, FoodData> Foods = new()
  {
    ["Apple"] = new FoodData
    {
      Name = "Apple",
      HungerRestoration = 15f,
      Description = "A fresh apple. Restores some hunger.",
      TexturePath = "res://Assets/Sprites/Items/Foods/apple.png"
    },
    ["Bread"] = new FoodData
    {
      Name = "Bread",
      HungerRestoration = 30f,
      Description = "A loaf of bread. Good for filling up.",
      TexturePath = "res://Assets/Sprites/Items/Foods/bread.png"
    },
    ["Meat"] = new FoodData
    {
      Name = "Meat",
      HungerRestoration = 45f,
      Description = "Cooked meat. Highly nutritious.",
      TexturePath = "res://Assets/Sprites/Items/Foods/meat.png"
    }
  };
}