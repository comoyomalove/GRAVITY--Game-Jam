---
title: "Unity_Architecture_Design_Patterns_v1_0"
type: "technical_guidelines"
version: "1.0"
project: "G-Hell - Roguelite Bullet Heaven"
created: "2025-12-11"
description: >
  Architectural principles and design patterns for building modular,
  stat-driven systems in Unity. Establishes foundational patterns for
  character controllers, combat systems, inventory management, and
  extensible game mechanics.
related_documents:
  - "Unity_CSharp_Coding_Standards.md"
  - "Character_Controller_Development_Specifications.md"
  - "Stats_Modifier_System_Architecture.md"
---

# 🏗️ Unity Architecture & Design Patterns Guide v1.0

## Purpose

This document establishes the architectural foundation for G-Hell's modular, stat-driven systems. All code—whether written by human developers or AI agents—must follow these patterns to ensure scalability, maintainability, and team collaboration.

**Core Philosophy**: Build systems that are **modular**, **data-driven**, and **extensible** without requiring rewrites when new features are added.

---

## 🎯 Architectural Principles

### 1. **Separation of Concerns**

Each system should have a single, well-defined responsibility.

**✅ Good Example:**
```csharp
// PlayerMovement handles ONLY movement logic
public class PlayerMovement : MonoBehaviour
{
    private IStatsProvider statsProvider;
    
    private void Move()
    {
        float speed = statsProvider.GetStat(StatType.MoveSpeed);
        // Movement logic using stat
    }
}

// PlayerCombat handles ONLY combat logic
public class PlayerCombat : MonoBehaviour
{
    private IStatsProvider statsProvider;
    
    private void Attack()
    {
        float damage = statsProvider.GetStat(StatType.Damage);
        // Combat logic using stat
    }
}
❌ Bad Example:

csharp

Copy
// God class doing everything
public class Player : MonoBehaviour
{
    public float moveSpeed;
    public float damage;
    public int health;
    
    private void Update()
    {
        // Movement
        // Combat
        // Inventory
        // UI updates
        // Everything mixed together
    }
}
2. Data-Driven Design
Game behavior should be driven by data (ScriptableObjects, configs) rather than hardcoded values.

Why?: Designers can tweak values without touching code; AI agents can generate consistent data structures.

✅ Good Example:

csharp

Copy
[CreateAssetMenu(fileName = "NewItem", menuName = "Items/Weapon")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public Vector2Int gridSize;
    public List<StatModifier> statModifiers;
    public float cooldown;
}
❌ Bad Example:

csharp

Copy
public class Sword : MonoBehaviour
{
    // Hardcoded values scattered across different scripts
    private float damage = 10f;
    private float cooldown = 1.5f;
}
3. Dependency Injection via Interfaces
Systems should depend on abstractions (interfaces), not concrete implementations.

Benefits:

Easy to swap implementations
Testable code
Loose coupling between systems
✅ Good Example:

csharp

Copy
public interface IStatsProvider
{
    float GetStat(StatType statType);
    void ModifyStat(StatType statType, float value, ModifierType type);
}

public class PlayerMovement : MonoBehaviour
{
    private IStatsProvider statsProvider;
    
    public void Initialize(IStatsProvider provider)
    {
        statsProvider = provider;
    }
}
❌ Bad Example:

csharp

Copy
public class PlayerMovement : MonoBehaviour
{
    public PlayerStats playerStats; // Tightly coupled to concrete class
    
    private void Move()
    {
        float speed = playerStats.moveSpeed; // Direct access
    }
}
4. Event-Driven Communication
Use events/delegates for cross-system communication to avoid tight coupling.

✅ Good Example:

csharp

Copy
// Event channel (ScriptableObject)
[CreateAssetMenu(menuName = "Events/Item Equipped Event")]
public class ItemEquippedEvent : ScriptableObject
{
    private event System.Action<ItemData> OnItemEquipped;
    
    public void Raise(ItemData item)
    {
        OnItemEquipped?.Invoke(item);
    }
    
    public void Register(System.Action<ItemData> listener)
    {
        OnItemEquipped += listener;
    }
    
    public void Unregister(System.Action<ItemData> listener)
    {
        OnItemEquipped -= listener;
    }
}

// Usage
public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private ItemEquippedEvent itemEquippedEvent;
    
    private void OnEnable()
    {
        itemEquippedEvent.Register(OnItemEquipped);
    }
    
    private void OnDisable()
    {
        itemEquippedEvent.Unregister(OnItemEquipped);
    }
    
    private void OnItemEquipped(ItemData item)
    {
        // React to item being equipped
    }
}
❌ Bad Example:

csharp

Copy
public class Inventory : MonoBehaviour
{
    public PlayerCombat playerCombat; // Direct reference
    
    private void EquipItem(ItemData item)
    {
        playerCombat.UpdateWeapon(item); // Tight coupling
    }
}
🧩 Required Design Patterns
1. Component Pattern
Break complex entities into smaller, reusable components.

Application in G-Hell:

Player = Movement + Combat + Stats + Inventory components
Each component is independent and composable
Implementation:

csharp

Copy
public class PlayerController : MonoBehaviour
{
    private PlayerMovement movement;
    private PlayerCombat combat;
    private PlayerStats stats;
    
    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();
        combat = GetComponent<PlayerCombat>();
        stats = GetComponent<PlayerStats>();
        
        // Inject dependencies
        movement.Initialize(stats);
        combat.Initialize(stats);
    }
}
2. Strategy Pattern
Define a family of interchangeable algorithms (e.g., different attack patterns, movement modifiers).

Application in G-Hell:

Different item attack behaviors
Movement modifiers (dash, jump, slam)
Implementation:

csharp

Copy
public interface IAttackStrategy
{
    void Execute(Transform origin, IStatsProvider stats);
}

public class ProjectileAttack : IAttackStrategy
{
    public void Execute(Transform origin, IStatsProvider stats)
    {
        // Spawn projectile logic
    }
}

public class MeleeAttack : IAttackStrategy
{
    public void Execute(Transform origin, IStatsProvider stats)
    {
        // Melee swing logic
    }
}

// Item holds strategy
[CreateAssetMenu(menuName = "Items/Attack Item")]
public class AttackItemData : ItemData
{
    public AttackType attackType;
    
    public IAttackStrategy GetAttackStrategy()
    {
        return attackType switch
        {
            AttackType.Projectile => new ProjectileAttack(),
            AttackType.Melee => new MeleeAttack(),
            _ => null
        };
    }
}
3. Observer Pattern
Allow objects to subscribe to and react to events without tight coupling.

Application in G-Hell:

Stat changes notify UI, combat system, movement system
Item equipped/unequipped events
Enemy death triggers XP gain
Implementation:

csharp

Copy
public class PlayerStats : MonoBehaviour, IStatsProvider
{
    public event System.Action<StatType, float> OnStatChanged;
    
    public void ModifyStat(StatType statType, float value, ModifierType type)
    {
        // Apply modification
        OnStatChanged?.Invoke(statType, GetStat(statType));
    }
}

// Listeners
public class PlayerMovement : MonoBehaviour
{
    private void OnEnable()
    {
        statsProvider.OnStatChanged += HandleStatChange;
    }
    
    private void HandleStatChange(StatType type, float newValue)
    {
        if (type == StatType.MoveSpeed)
        {
            // Update movement speed
        }
    }
}
4. Object Pool Pattern
Reuse objects instead of instantiating/destroying repeatedly (critical for performance).

Application in G-Hell:

Projectiles
Enemy spawns
VFX particles
Damage numbers
Implementation:

csharp

Copy
public class ObjectPool<T> where T : Component
{
    private Queue<T> pool = new Queue<T>();
    private T prefab;
    private Transform parent;
    
    public ObjectPool(T prefab, int initialSize, Transform parent = null)
    {
        this.prefab = prefab;
        this.parent = parent;
        
        for (int i = 0; i < initialSize; i++)
        {
            T obj = Object.Instantiate(prefab, parent);
            obj.gameObject.SetActive(false);
            pool.Enqueue(obj);
        }
    }
    
    public T Get()
    {
        if (pool.Count > 0)
        {
            T obj = pool.Dequeue();
            obj.gameObject.SetActive(true);
            return obj;
        }
        
        return Object.Instantiate(prefab, parent);
    }
    
    public void Return(T obj)
    {
        obj.gameObject.SetActive(false);
        pool.Enqueue(obj);
    }
}
📊 Stats System Architecture
Core Principle: Single Source of Truth
All gameplay values (speed, damage, health, cooldowns) must come from a central stats system.

Why?:

Items can modify stats without changing code
Environmental effects stack properly
Easy to debug (one place to check)
Future-proof for buffs/debuffs
Stats Provider Interface
csharp

Copy
public interface IStatsProvider
{
    float GetStat(StatType statType);
    void AddModifier(StatModifier modifier);
    void RemoveModifier(StatModifier modifier);
    float GetBaseValue(StatType statType);
}

public enum StatType
{
    MoveSpeed,
    JumpForce,
    Acceleration,
    Damage,
    AttackSpeed,
    CritChance,
    Health,
    // Add more as needed
}
Stat Modifier System
csharp

Copy
[System.Serializable]
public class StatModifier
{
    public StatType statType;
    public ModifierType modifierType;
    public float value;
    public object source; // Track what created this modifier
}

public enum ModifierType
{
    Flat,          // +10 damage
    PercentAdd,    // +20% damage (additive with other %)
    PercentMult    // x1.2 damage (multiplicative)
}
Calculation Order
csharp

Copy
public float GetStat(StatType statType)
{
    float baseValue = GetBaseValue(statType);
    float flatBonus = 0f;
    float percentAdd = 0f;
    float percentMult = 1f;
    
    foreach (StatModifier mod in GetModifiersForStat(statType))
    {
        switch (mod.modifierType)
        {
            case ModifierType.Flat:
                flatBonus += mod.value;
                break;
            case ModifierType.PercentAdd:
                percentAdd += mod.value;
                break;
            case ModifierType.PercentMult:
                percentMult *= (1f + mod.value);
                break;
        }
    }
    
    // Order: (Base + Flat) * (1 + PercentAdd) * PercentMult
    return (baseValue + flatBonus) * (1f + percentAdd) * percentMult;
}
🎮 Character Controller Architecture
Modular Component Structure
PlayerController (Root)
├── PlayerStats (IStatsProvider)
├── PlayerMovement
│   └── Depends on: IStatsProvider
├── PlayerCombat
│   └── Depends on: IStatsProvider, InventorySystem
├── PlayerInventory
│   └── Raises: ItemEquipped/Unequipped events
└── PlayerInput
    └── Sends commands to Movement & Combat
Communication Flow
Input → Movement/Combat Components
           ↓
    Query IStatsProvider for values
           ↓
    Stats calculated from Base + Modifiers
           ↓
    Modifiers come from Items/Buffs/Environment
🎒 Inventory & Item Integration
Item Data Structure
csharp

Copy
[CreateAssetMenu(menuName = "Items/Base Item")]
public class ItemData : ScriptableObject
{
    [Header("Identity")]
    public string itemName;
    public Sprite icon;
    
    [Header("Grid Properties")]
    public Vector2Int gridSize;
    public bool[] customShape; // For non-rectangular items
    
    [Header("Stat Modifiers")]
    public List<StatModifier> passiveModifiers; // Always active when equipped
    
    [Header("Active Ability")]
    public bool hasActiveAbility;
    public float abilityCooldown;
    public AbilityData abilityData;
    
    [Header("Synergy")]
    public List<SynergyPoint> synergyPoints;
}
Synergy System
csharp

Copy
[System.Serializable]
public class SynergyPoint
{
    public Vector2Int localPosition; // Position relative to item
    public SynergyType synergyType;
}

public enum SynergyType
{
    Weapon,
    Armor,
    Magic,
    Support
}

// Synergy bonus applied when matching types are adjacent
public class SynergyBonus
{
    public SynergyType typeA;
    public SynergyType typeB;
    public List<StatModifier> bonusModifiers;
}
⚔️ Combat System Architecture
Automated Combat Flow
1. PlayerCombat tracks equipped items with active abilities
2. Each item has internal cooldown timer
3. When cooldown ready → Execute attack strategy
4. Attack queries stats for damage/range/etc.
5. Apply damage to enemies
6. Restart cooldown
Combat Component Structure
csharp

Copy
public class PlayerCombat : MonoBehaviour
{
    private IStatsProvider statsProvider;
    private List<EquippedAbility> equippedAbilities = new List<EquippedAbility>();
    
    public void Initialize(IStatsProvider provider)
    {
        statsProvider = provider;
    }
    
    private void Update()
    {
        foreach (EquippedAbility ability in equippedAbilities)
        {
            ability.cooldownTimer -= Time.deltaTime;
            
            if (ability.cooldownTimer <= 0f)
            {
                ExecuteAbility(ability);
                ability.cooldownTimer = ability.GetCooldown(statsProvider);
            }
        }
    }
    
    private void ExecuteAbility(EquippedAbility ability)
    {
        ability.attackStrategy.Execute(transform, statsProvider);
    }
}

[System.Serializable]
public class EquippedAbility
{
    public ItemData sourceItem;
    public IAttackStrategy attackStrategy;
    public float cooldownTimer;
    
    public float GetCooldown(IStatsProvider stats)
    {
        float baseCooldown = sourceItem.abilityCooldown;
        float attackSpeed = stats.GetStat(StatType.AttackSpeed);
        return baseCooldown / attackSpeed;
    }
}
🔧 Extensibility Guidelines
Adding New Features Without Breaking Existing Code
✅ DO:
Add new stat types to StatType enum
Create new IAttackStrategy implementations
Add new modifier types if calculation needs change
Use events to notify systems of new behaviors
Create new ScriptableObject data types
❌ DON'T:
Modify existing component responsibilities
Add hardcoded values instead of using stats
Create tight coupling between systems
Break existing interfaces
Example: Adding Dash Ability
Step 1: Add stat type

csharp

Copy
public enum StatType
{
    // Existing...
    DashForce,      // New
    DashCooldown    // New
}
Step 2: Create movement modifier interface

csharp

Copy
public interface IMovementModifier
{
    void Apply(PlayerMovement movement, IStatsProvider stats);
}

public class DashModifier : IMovementModifier
{
    public void Apply(PlayerMovement movement, IStatsProvider stats)
    {
        float dashForce = stats.GetStat(StatType.DashForce);
        movement.ApplyForce(movement.transform.forward * dashForce);
    }
}
Step 3: Integrate into movement system

csharp

Copy
public class PlayerMovement : MonoBehaviour
{
    private List<IMovementModifier> activeModifiers = new List<IMovementModifier>();
    
    public void AddModifier(IMovementModifier modifier)
    {
        activeModifiers.Add(modifier);
    }
    
    public void ApplyForce(Vector3 force)
    {
        // Apply force to rigidbody/character controller
    }
}
Step 4: Item grants dash ability

csharp

Copy
[CreateAssetMenu(menuName = "Items/Dash Boots")]
public class DashBootsItem : ItemData
{
    public override void OnEquip(PlayerMovement movement, IStatsProvider stats)
    {
        movement.AddModifier(new DashModifier());
        stats.AddModifier(new StatModifier 
        { 
            statType = StatType.DashForce, 
            value = 500f, 
            modifierType = ModifierType.Flat 
        });
    }
}
No existing code was modified—only extended!

🧪 Testing & Debugging
Inspector Debug Tools
Every major system should expose debug information in the Inspector.

csharp

Copy
public class PlayerStats : MonoBehaviour, IStatsProvider
{
    [Header("Debug Info")]
    [SerializeField] private bool showDebugInfo = true;
    
    #if UNITY_EDITOR
    [Header("Current Stats (Runtime)")]
    [SerializeField] private List<DebugStatDisplay> currentStats;
    
    private void OnValidate()
    {
        if (showDebugInfo && Application.isPlaying)
        {
            UpdateDebugDisplay();
        }
    }
    
    private void UpdateDebugDisplay()
    {
        currentStats.Clear();
        foreach (StatType type in System.Enum.GetValues(typeof(StatType)))
        {
            currentStats.Add(new DebugStatDisplay 
            { 
                statType = type, 
                value = GetStat(type) 
            });
        }
    }
    #endif
}

[System.Serializable]
public class DebugStatDisplay
{
    public StatType statType;
    public float value;
}
📚 ScriptableObject Workflow
When to Use ScriptableObjects
Table


Use Case    Example
Data Definitions    Item data, enemy data, ability data
Configuration    Game settings, difficulty curves
Event Channels    Cross-scene communication
Shared State    Current inventory, player stats
Folder Structure
Assets/
└── ScriptableObjects/
    ├── Items/
    │   ├── Weapons/
    │   ├── Armor/
    │   └── Consumables/
    ├── Enemies/
    ├── Events/
    └── Config/
✅ AI Agent Checklist
When generating code for this project, verify:

Does this component have a single, clear responsibility?
Are all gameplay values driven by the stats system?
Am I using interfaces instead of concrete classes for dependencies?
Is communication event-driven rather than direct references?
Can new features be added without modifying existing code?
Are objects pooled if they spawn/despawn frequently?
Is data stored in ScriptableObjects rather than hardcoded?
Does the Inspector show useful debug information?
Am I following the naming conventions from Unity_CSharp_Coding_Standards.md?
🎯 Quick Reference: Pattern Selection
Table


Need    Pattern    Example
Break complex entity into parts    Component    Player = Movement + Combat + Stats
Swap behaviors at runtime    Strategy    Different attack types per item
React to changes elsewhere    Observer    UI updates when stats change
Reuse expensive objects    Object Pool    Projectile pooling
Decouple systems    Event Channel    Item equipped event
Data-driven design    ScriptableObject    Item definitions
<metadata>
  <version>1.0</version>
  <author>Programming Lead & AI Collaboration</author>
  <created>2025-12-11</created>
  <last_updated>2025-12-11</last_updated>
  <project>G-Hell Roguelite Prototype</project>
  <related_documents>
    <document>Unity_CSharp_Coding_Standards.md</document>
    <document>Character_Controller_Development_Specifications.md</document>
    <document>Stats_Modifier_System_Architecture.md</document>
  </related_documents>
</metadata>
```
