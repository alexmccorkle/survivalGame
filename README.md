# Survival Game Project

A 2D survival game built on Godot 4 and C#, featuring resource gathering, hunger/stamina management, and basic survival mechanics.
No Sprites or anything right now. Moving squares, will add the visuals last. All about the game logic right now.

## To Add:

- Random Spawner for Resources etc.
- Item Storage/Bag
- Tools (Axe, Pickaxe, Sword)
  - Crafting(?)
- Enemies/Animals
- XP? or Levels

## Current Features

### Player Systems

- Character movement with sprint capability
- Stamina system for sprinting
- Health and hunger mechanics
  - Health decreases when hunger is depleted
  - Stamina regenerates over time
  - Hunger gradually decreases

### Resource Gathering

- Harvestable resources (trees and bushes)
  - Will be adding Ores(?) and potentially other stuff
- Resource types drop different items:
  - Trees drop apples
  - Bushes drop berries
- Resources respawn after a timer
- Interaction system (press E when in range)

### World

- Bounded game world (10000x8000)
- Camera follows player
- UI elements stay fixed on screen

### Food System

- Food items can be spawned and consumed
- Eating food restores hunger
- Basic food database implementation

## Project Structure

```
Scripts/
├── Core/
│   └── GameManager.cs (singleton for spawning food)
├── Harvestable/
│   ├── ResourceTypes.cs (enums and ResourceDrop class)
│   └── HarvestableResource.cs (main resource logic)
├── Items/Food/
│   ├── FoodItem.cs
│   └── FoodDatabase.cs
└── Player/
    └── PlayerController.cs (CharacterBody2D implementation)

Scenes/
├── Harvestable/
│   └── HarvestableResource.tscn
└── Items/Food/
    └── FoodItem.tscn
```

## Controls

- WASD/Arrow Keys: Movement
- Shift: Sprint
- E: Interact with resources
- F: Debug food spawn

## Technical Notes

- Uses collision layers for different interactions:
  - Layer 1: Player
  - Layer 2: Harvestable Resources
  - Layer 3: World Boundaries
