using Godot;
using System;
using System.Collections.Generic;

public partial class HarvestableResource : Node2D
{
  [Export]
  public ResourceType Type { get; set; }
  [Export]
  public bool IsHarvestable { get; set; } = true;
  [Export]
  public float RespawnTime { get; set; } = 15.0f; // 15 seconds (can change but less for now for testing)
  [Export]
  public float InteractionRange { get; set; } = 100.0f; // 100 pixels default

  public List<ResourceDrop> PossibleDrops { get; private set; }

  private Timer _respawnTimer;
  private ColorRect _visual;
  private Area2D _interactionArea;
  private bool _playerInRange = false; // To show a "Press E" or something
  private Label _interactLabel;
  private Label _resourceLabel;

  private readonly Dictionary<ResourceType, Color> _resourceColors = new() // Set the colors for each resource here:
  {
    { ResourceType.Bush, Colors.DarkGreen },
    { ResourceType.Tree, Colors.ForestGreen }
  };

  private readonly RandomNumberGenerator _rng = new RandomNumberGenerator();

  public override void _Ready()
  {
    // Initialize visual representation
    _visual = new ColorRect
    {
      Size = new Vector2(48, 48), // Size of resource
      Position = new Vector2(-24, -24), // Center
      Color = _resourceColors[Type],
    };
    AddChild(_visual);

    // Add resource label
    _resourceLabel = new Label
    {
      Text = Type.ToString(),
      Position = new Vector2(-20, 30), // Position below the resource
      HorizontalAlignment = HorizontalAlignment.Center,
    };
    AddChild(_resourceLabel);

    // Initialize respawn timer
    _respawnTimer = new Timer
    {
      OneShot = true,
      WaitTime = RespawnTime
    };
    AddChild(_respawnTimer);
    _respawnTimer.Timeout += OnRespawnTimerTimeout;

    // Initialize drops based on type
    InitializeDrops();

    // Get reference to Area2D
    _interactionArea = GetNode<Area2D>("Area2D");
    _interactionArea.BodyEntered += OnBodyEntered;
    _interactionArea.BodyExited += OnBodyExited;

    // Create interaction label:
    _interactLabel = new Label
    {
      Text = "Press E to harvest!",
      Position = new Vector2(-50, -60), // Above the resource
      Visible = false // Hidden by default
    };
    AddChild(_interactLabel);
  }


  private void UpdateVisualState()
  {
    Color baseColor = _resourceColors[Type];
    _visual.Color = IsHarvestable ? baseColor : baseColor.Darkened(0.5f);
  }

  private void InitializeDrops()
  {
    PossibleDrops = new List<ResourceDrop>();
    switch (Type)
    {
      case ResourceType.Tree:
        PossibleDrops.Add(new ResourceDrop
        {
          ItemId = "Apple",
          Probability = 0.8f,
          MinAmount = 1,
          MaxAmount = 3,
        });
        break;

      case ResourceType.Bush:
        PossibleDrops.Add(new ResourceDrop
        {
          ItemId = "Berry",
          Probability = 0.8f,
          MinAmount = 2,
          MaxAmount = 4,
        });
        break;
    }
  }

  public void OnInteract()
  {
    if (IsHarvestable && _playerInRange)
    {
      Harvest();
    }
  }

  private void Harvest()
  {
    if (!IsHarvestable) return;

    GD.Print($"Harvesting {Type}");

    // Generate and spawn drops on harvest
    foreach (var possibleDrop in PossibleDrops)
    {
      // Check if item should drop or not based on probability
      if (_rng.Randf() <= possibleDrop.Probability)
      {
        // Determine amount to drop
        int amount = _rng.RandiRange(possibleDrop.MinAmount, possibleDrop.MaxAmount);

        // Spawn each item in a small radius
        for (int i = 0; i < amount; i++)
        {
          // Calculate random positions in a circle around the resource
          float angle = _rng.RandfRange(0, Mathf.Tau); // Random angle in radians (think of tau fra midten)
          float distance = _rng.RandfRange(30, 60); // Random distance between 30-60
          Vector2 offset = new Vector2(
            Mathf.Cos(angle) * distance,
            Mathf.Sin(angle) * distance
          );

          // Spawn the food at calculated position
          GameManager.Instance.SpawnFood(possibleDrop.ItemId, GlobalPosition + offset);
        }

        GD.Print($"Dropped {amount} {possibleDrop.ItemId}(s)");
      }
    }

    // Make resource non-harvestable and start respawn timer like before
    IsHarvestable = false;
    UpdateVisualState();
    _respawnTimer.Start();

  }


  private void OnRespawnTimerTimeout()
  {
    IsHarvestable = true;
    _visual.Color = Colors.Green;
    GD.Print($"{Type} has respawned!");
  }

  private void OnBodyEntered(Node2D body)
  {
    if (body is PlayerController)
    {
      _playerInRange = true;
      UpdatePrompt();
    }
  }

  private void OnBodyExited(Node2D body)
  {
    if (body is PlayerController)
    {
      _playerInRange = false;
      UpdatePrompt();
    }
  }

  private void UpdatePrompt()
  {
    _interactLabel.Visible = _playerInRange && IsHarvestable;
  }

  private bool IsPlayerInRange()
  {
    return _playerInRange;
  }

  public override void _Input(InputEvent @event)
  {
    if (@event.IsActionPressed("interact") && _playerInRange && IsHarvestable)
    {
      Harvest();
    }
  }
}