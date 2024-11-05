using Godot;
using System;

public partial class PlayerStatsUI : Control
{
  private ProgressBar _healthBar;
  private ProgressBar _hungerBar;
  private ProgressBar _staminaBar;
  private PlayerStats _playerStats;

  // Add fields to store label references
  private Label _healthLabel;
  private Label _hungerLabel;
  private Label _staminaLabel;

  public override void _Ready()
  {
    // Get references to the ProgressBars
    _healthBar = GetNode<ProgressBar>("MarginContainer/VBoxContainer/HealthBar");
    _hungerBar = GetNode<ProgressBar>("MarginContainer/VBoxContainer/HungerBar");
    _staminaBar = GetNode<ProgressBar>("MarginContainer/VBoxContainer/StaminaBar");

    // Configure the progress bars and store label references
    _healthLabel = ConfigureProgressBar(_healthBar, Colors.DarkRed, "Health");
    _hungerLabel = ConfigureProgressBar(_hungerBar, Colors.SaddleBrown, "Hunger");
    _staminaLabel = ConfigureProgressBar(_staminaBar, Colors.DarkGoldenrod, "Stamina");

    // Find the PlayerStats node
    _playerStats = GetNode<PlayerStats>("/root/MainScene/Player/PlayerStats");

    if (_playerStats != null)
    {
      // Connect to signals
      _playerStats.HealthChanged += UpdateHealthBar;
      _playerStats.HungerChanged += UpdateHungerBar;
      _playerStats.StaminaChanged += UpdateStaminaBar;
      GD.Print("Successfully connected to PlayerStats signals!");
    }
    else
    {
      GD.PrintErr("PlayerStatsUI: Could not find PlayerStats node!");
    }
  }

  private Label ConfigureProgressBar(ProgressBar bar, Color fillColor, string label)
  {
    if (bar != null)
    {
      bar.MinValue = 0;
      bar.MaxValue = 100;
      bar.Value = 100;

      // Set the progress bar's style
      var styleBox = new StyleBoxFlat();
      styleBox.BgColor = fillColor;
      styleBox.CornerRadiusTopLeft = 4;
      styleBox.CornerRadiusTopRight = 4;
      styleBox.CornerRadiusBottomLeft = 4;
      styleBox.CornerRadiusBottomRight = 4;

      bar.AddThemeStyleboxOverride("fill", styleBox);

      // Add the value label directly to the progress bar
      var valueLabel = new Label();
      valueLabel.Name = "ValueLabel"; // Changed the name to avoid confusion
      valueLabel.Text = $"{label}: 100%";
      valueLabel.Position = new Vector2(10, 0);
      bar.AddChild(valueLabel);

      return valueLabel;
    }
    return null;
  }

  private void UpdateHealthBar(float value)
  {
    if (_healthBar != null && _healthLabel != null)
    {
      _healthBar.Value = value;
      _healthLabel.Text = $"Health: {Mathf.Round(value)}%";
    }
  }

  private void UpdateHungerBar(float value)
  {
    if (_hungerBar != null && _hungerLabel != null)
    {
      _hungerBar.Value = value;
      _hungerLabel.Text = $"Hunger: {Mathf.Round(value)}%";
    }
  }

  private void UpdateStaminaBar(float value)
  {
    if (_staminaBar != null && _staminaLabel != null)
    {
      _staminaBar.Value = value;
      _staminaLabel.Text = $"Stamina: {Mathf.Round(value)}%";
    }
  }
}