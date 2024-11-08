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
    public Color Color { get; set; } // Add color property
  }

  public static Dictionary<string, FoodData> Foods = new()
  {
    ["Apple"] = new FoodData
    {
      Name = "Apple",
      HungerRestoration = 15f,
      Description = "A fresh apple. Restores some hunger.",
      Color = Colors.Red
    },
    ["Berry"] = new FoodData
    {
      Name = "Berry",
      HungerRestoration = 10f,
      Description = "Yummies!",
      Color = Colors.BlueViolet
    },
    ["Bread"] = new FoodData
    {
      Name = "Bread",
      HungerRestoration = 30f,
      Description = "A loaf of bread. Good for filling up.",
      Color = new Color (0.82f, 0.65f, 0.40f) // Tan/brown color
    },
    ["Meat"] = new FoodData
    {
      Name = "Meat",
      HungerRestoration = 45f,
      Description = "Cooked meat. Highly nutritious.",
      Color = new Color(0.55f, 0.27f, 0.27f) // Brownish red
    }
  };
}