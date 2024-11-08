using Godot;
using System;

public enum ResourceType
{
  Tree,
  Bush,
  // Others can be added later here (e.g. Ores?)
}

// Organizing drops
public class ResourceDrop
{
  public string ItemId { get; set; } // "Apple", "Berry", etc.
  public float Probability { get; set; } // 0.0 - 1.0
  public int MinAmount { get; set; }
  public int MaxAmount { get; set; }
  public Label Label;
}