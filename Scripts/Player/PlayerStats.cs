using Godot;
using System;

public partial class PlayerStats : Node
{
  // Base Stats:
  private float _maxHealth = 100f;
  private float _currentHealth;
  private float _maxHunger = 100f;
  private float _currentHunger;
  private float _maxStamina = 100f;
  private float _currentStamina;

  // States(?):
  private bool _isSprinting = false;

  // Stat Decay Rates: (Per Second)
  private const float HUNGER_DECAY_RATE = 1f;
  private const float STAMINA_REGEN_RATE = 10f;

  public override void _Ready()
  {
    // Initialize Stats at Maximum:
    _currentHealth = _maxHealth;
    _currentHunger = _maxHunger;
    _currentStamina = _maxStamina;
  }

  public override void _Process(double delta)
  {
    // Update stats over time
    UpdateHunger(delta);
    RegenerateStamina(delta);
    UpdateHealthBasedOnHunger(delta);
  }

  private void UpdateHunger(double delta)
  {
    _currentHunger = Mathf.Max(0, _currentHunger - (HUNGER_DECAY_RATE * (float)delta));
    EmitSignal(SignalName.HungerChanged, _currentHunger);
  }

  private void RegenerateStamina(double delta)
  {
    // Make sure we don't regen during sprinting
    if (!_isSprinting && _currentStamina < _maxStamina)
    {
      _currentStamina = Mathf.Min(_maxStamina, _currentStamina + (STAMINA_REGEN_RATE * (float)delta));
      EmitSignal(SignalName.StaminaChanged, _currentStamina);
    }
    // Reset sprinting flag each frame - if we're still sprinting, UseStamina will set it again
    _isSprinting = false;
  }

  private void UpdateHealthBasedOnHunger(double delta)
  {
    // If hunger is at 0, start taking damage
    if (_currentHunger <= 0)
    {
      TakeDamage(1f * (float)delta); // 1 damage per second when starving
    }
  }

  public void TakeDamage(float amount)
  {
    _currentHealth = Mathf.Max(0, _currentHealth - amount);
    EmitSignal(SignalName.HealthChanged, _currentHealth);

    if (_currentHealth <= 0)
    {
      EmitSignal(SignalName.PlayerDied);
    }
  }

  public void Heal(float amount)
  {
    _currentHealth = Mathf.Min(_maxHealth, _currentHealth + amount);
    EmitSignal(SignalName.HealthChanged, _currentHealth);
  }

  public void ConsumeFood(float nutritionValue)
  {
    _currentHunger = Mathf.Min(_maxHunger, _currentHunger + nutritionValue);
    EmitSignal(SignalName.HungerChanged, _currentHunger);
  }

  public bool UseStamina(float amount)
  {
    if (_currentStamina >= amount)
    {
      _currentStamina -= amount;
      _isSprinting = true;
      EmitSignal(SignalName.StaminaChanged, _currentStamina);
      return true;
    }
    GD.Print("Not enough stamina!");
    return false;
  }

  // Signals for UI Updates and Game Events:
  [Signal]
  public delegate void HealthChangedEventHandler(float newHealth);

  [Signal]
  public delegate void HungerChangedEventHandler(float newHunger);

  [Signal]
  public delegate void StaminaChangedEventHandler(float newStamina);

  [Signal]
  public delegate void PlayerDiedEventHandler();
}