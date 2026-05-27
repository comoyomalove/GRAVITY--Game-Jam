---
title: "Designer_Friendly_Tools_Inspector_Guidelines_v1_0"
type: "workflow_guidelines"
version: "1.0"
project: "G-Hell - Roguelite Bullet Heaven"
created: "2025-12-15"
description: >
  Guidelines for creating designer-friendly, Inspector-editable parameters
  in Unity. Covers tooltips, organization, naming conventions, and best
  practices for enabling non-programmers to tweak gameplay values safely.
related_documents:
  - "Unity_CSharp_Coding_Standards.md"
  - "Unity_Specific_Best_Practices.md"
  - "Character_Controller_Development_Specifications.md"
  - "ScriptableObject_Workflow_Guide.md"
---

# 🎨 Designer-Friendly Tools & Inspector Guidelines v1.0

## Purpose

This document ensures that **non-programmers (game designers, level designers, QA)** can safely and effectively tweak gameplay parameters in Unity without touching code. By following these guidelines, programmers create intuitive, well-documented Inspector interfaces that empower the entire team.

**Core Principle**: If a designer needs to change a value to tune gameplay, it should be **visible, understandable, and safe to modify** in the Inspector.

---

## 🎯 Why Designer-Friendly Inspectors Matter

### **Benefits**

|
 Benefit 
|
 Impact 
|
|
---------
|
--------
|
|
**
Faster Iteration
**
|
 Designers tweak values in seconds, not hours 
|
|
**
Reduced Programmer Bottleneck
**
|
 Designers don't wait for code changes 
|
|
**
Better Game Feel
**
|
 Designers experiment freely with parameters 
|
|
**
Fewer Bugs
**
|
 Designers can't break code, only adjust values 
|
|
**
Clearer Communication
**
|
 Tooltips explain what each value does 
|
|
**
Version Control Friendly
**
|
 Scene/prefab changes are easy to review 
|
### **The Goal**

A designer should be able to:
1. ✅ **Find** the parameter they need quickly
2. ✅ **Understand** what it does without asking a programmer
3. ✅ **Adjust** it safely within reasonable bounds
4. ✅ **Test** the change immediately in Play mode
5. ✅ **Revert** easily if the change doesn't work

---

## 🏗️ Core Inspector Attributes

### **1. [SerializeField] - Expose Private Fields**

**Purpose**: Make private fields visible in the Inspector.

✅ **Good (encapsulated but editable):**
```csharp
[SerializeField] private float walkSpeed = 5f;
❌ Bad (breaks encapsulation):

csharp

Copy
public float walkSpeed = 5f; // Other scripts can modify this!
Why [SerializeField] is better:

✅ Designers can edit it in the Inspector
✅ Other scripts can't accidentally modify it
✅ You control access via properties if needed
2. [Tooltip] - Explain What It Does
Purpose: Provide hover text that explains the parameter.

✅ Good:

csharp

Copy
[Tooltip("Maximum speed the player walks in units per second.")]
[SerializeField] private float walkSpeed = 5f;
❌ Bad (no explanation):

csharp

Copy
[SerializeField] private float walkSpeed = 5f; // Designer has to guess!
Tooltip Writing Guidelines:

Table


✅ Good Tooltip    ❌ Bad Tooltip
"How fast the player walks in units per second."    "Walk speed."
"Grace period after leaving a ledge where jump still works (in seconds)."    "Coyote time."
"Movement control while airborne (0 = none, 1 = full ground control)."    "Air control value."
"Number of times the player can jump before landing (1 = single jump)."    "Max jumps."
Formula for Good Tooltips:

[What it does] + [Units/Scale] + [Example values if helpful]
Examples:

csharp

Copy
[Tooltip("How high the player jumps in world units. (Default: 2.0)")]
[SerializeField] private float jumpHeight = 2f;

[Tooltip("Multiplier for sprint speed. 1.5 means 50% faster than walk speed.")]
[SerializeField] private float sprintMultiplier = 1.5f;

[Tooltip("Downward acceleration in units per second². Negative value pulls player down.")]
[SerializeField] private float gravity = -20f;
3. [Header] - Organize Sections
Purpose: Group related parameters into labeled sections.

✅ Good (organized):

csharp

Copy
[Header("Movement Speed")]
[Tooltip("How fast the player walks.")]
[SerializeField] private float walkSpeed = 5f;

[Tooltip("How fast the player sprints.")]
[SerializeField] private float sprintSpeed = 8f;

[Header("Jumping")]
[Tooltip("How high the player jumps.")]
[SerializeField] private float jumpHeight = 2f;

[Tooltip("How many jumps allowed before landing.")]
[SerializeField] private int maxJumps = 1;
❌ Bad (no organization):

csharp

Copy
[SerializeField] private float walkSpeed = 5f;
[SerializeField] private float jumpHeight = 2f;
[SerializeField] private float sprintSpeed = 8f;
[SerializeField] private int maxJumps = 1;
// All mixed together, hard to scan!
Header Naming Guidelines:

Table


✅ Good Header Names    ❌ Bad Header Names
"Movement Speed"    "Speed Stuff"
"Jump Settings"    "Jumping" (too vague)
"Physics Parameters"    "Physics"
"Debug Visualization"    "Debug"
4. [Range] - Constrain Values with Sliders
Purpose: Provide sliders with min/max bounds to prevent invalid values.

✅ Good (bounded slider):

csharp

Copy
[Tooltip("Movement control while airborne (0 = none, 1 = full).")]
[Range(0f, 1f)]
[SerializeField] private float airControl = 0.5f;
❌ Bad (unbounded field):

csharp

Copy
[Tooltip("Movement control while airborne.")]
[SerializeField] private float airControl = 0.5f;
// Designer could accidentally set to -100 or 9999!
When to Use [Range]:

Table


Use Case    Example
Normalized values (0-1)    Air control, volume, opacity
Percentages    Crit chance (0-100), damage reduction
Bounded gameplay values    Jump count (1-5), difficulty level (1-10)
Angles    Rotation limits (0-360), slope limits (0-90)
Examples:

csharp

Copy
[Tooltip("Critical hit chance as a percentage (0-100).")]
[Range(0f, 100f)]
[SerializeField] private float critChance = 5f;

[Tooltip("Maximum slope angle the character can climb in degrees.")]
[Range(0f, 90f)]
[SerializeField] private float slopeLimit = 45f;

[Tooltip("Number of jumps allowed (1 = single, 2 = double, etc.).")]
[Range(1, 5)]
[SerializeField] private int maxJumps = 1;
5. [Space] - Add Visual Breathing Room
Purpose: Add vertical spacing between groups of fields.

✅ Good (readable):

csharp

Copy
[Header("Movement")]
[SerializeField] private float walkSpeed = 5f;
[SerializeField] private float sprintSpeed = 8f;

[Space(10)]

[Header("Jumping")]
[SerializeField] private float jumpHeight = 2f;
❌ Bad (cramped):

csharp

Copy
[Header("Movement")]
[SerializeField] private float walkSpeed = 5f;
[SerializeField] private float sprintSpeed = 8f;
[Header("Jumping")] // No space, hard to read
[SerializeField] private float jumpHeight = 2f;
6. [TextArea] - Multi-Line Text Input
Purpose: Allow designers to write longer text (descriptions, dialogue, etc.).

csharp

Copy
[Tooltip("Description shown to the player when this item is picked up.")]
[TextArea(3, 5)] // Min 3 lines, max 5 lines
[SerializeField] private string itemDescription = "A mysterious sword...";
7. [Min] - Enforce Minimum Values
Purpose: Prevent values from going below a threshold (no slider, just validation).

csharp

Copy
[Tooltip("Player's maximum health. Must be at least 1.")]
[Min(1f)]
[SerializeField] private float maxHealth = 100f;
🎮 Character Controller: Designer-Adjustable Parameters
Complete Example for PlayerMovement
csharp

Copy
using UnityEngine;

/// <summary>
/// Handles player movement and locomotion.
/// All parameters are exposed for designer tweaking.
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    // ========================================
    // MOVEMENT SPEED
    // ========================================
    
    [Header("Movement Speed")]
    
    [Tooltip("How fast the player walks in units per second.")]
    [Range(1f, 10f)]
    [SerializeField] private float walkSpeed = 5f;
    
    [Tooltip("Multiplier applied when sprinting (e.g., 1.5 = 50% faster).")]
    [Range(1f, 3f)]
    [SerializeField] private float sprintMultiplier = 1.5f;
    
    [Tooltip("How quickly the player accelerates to max speed (units/sec²).")]
    [Range(5f, 50f)]
    [SerializeField] private float acceleration = 20f;
    
    [Tooltip("How quickly the player decelerates to a stop (units/sec²).")]
    [Range(5f, 50f)]
    [SerializeField] private float deceleration = 15f;
    
    [Space(10)]
    
    // ========================================
    // ROTATION
    // ========================================
    
    [Header("Rotation")]
    
    [Tooltip("How fast the character rotates toward movement direction (degrees/sec).")]
    [Range(180f, 1440f)]
    [SerializeField] private float rotationSpeed = 720f;
    
    [Tooltip("If true, character rotates instantly toward movement direction.")]
    [SerializeField] private bool instantRotation = false;
    
    [Space(10)]
    
    // ========================================
    // JUMPING
    // ========================================
    
    [Header("Jumping")]
    
    [Tooltip("How high the player jumps in world units.")]
    [Range(0.5f, 5f)]
    [SerializeField] private float jumpHeight = 2f;
    
    [Tooltip("Number of times the player can jump before landing (1 = single jump).")]
    [Range(1, 5)]
    [SerializeField] private int maxJumps = 1;
    
    [Tooltip("Grace period after leaving a ledge where jump still works (seconds).")]
    [Range(0f, 0.5f)]
    [SerializeField] private float coyoteTime = 0.1f;
    
    [Tooltip("How early the player can press jump before landing (seconds).")]
    [Range(0f, 0.5f)]
    [SerializeField] private float jumpBufferTime = 0.1f;
    
    [Space(10)]
    
    // ========================================
    // AIR CONTROL
    // ========================================
    
    [Header("Air Control")]
    
    [Tooltip("Movement control while airborne (0 = no control, 1 = full ground control).")]
    [Range(0f, 1f)]
    [SerializeField] private float airControl = 0.5f;
    
    [Tooltip("Downward acceleration in units/sec². Negative value pulls player down.")]
    [Range(-50f, -5f)]
    [SerializeField] private float gravity = -20f;
    
    [Tooltip("Maximum fall speed in units per second.")]
    [Range(10f, 100f)]
    [SerializeField] private float maxFallSpeed = 50f;
    
    [Space(10)]
    
    // ========================================
    // GROUND DETECTION
    // ========================================
    
    [Header("Ground Detection")]
    
    [Tooltip("Distance to check below the player for ground (units).")]
    [Range(0.01f, 1f)]
    [SerializeField] private float groundCheckDistance = 0.1f;
    
    [Tooltip("Layer mask for what counts as ground.")]
    [SerializeField] private LayerMask groundLayer = -1;
    
    [Space(10)]
    
    // ========================================
    // CHARACTER CONTROLLER SETTINGS
    // ========================================
    
    [Header("CharacterController Settings")]
    
    [Tooltip("Height of the character capsule collider.")]
    [Range(0.5f, 3f)]
    [SerializeField] private float controllerHeight = 2f;
    
    [Tooltip("Radius of the character capsule collider.")]
    [Range(0.1f, 1f)]
    [SerializeField] private float controllerRadius = 0.5f;
    
    [Tooltip("Maximum slope angle the character can climb (degrees).")]
    [Range(0f, 90f)]
    [SerializeField] private float slopeLimit = 45f;
    
    [Tooltip("Maximum step height the character can climb (units).")]
    [Range(0f, 1f)]
    [SerializeField] private float stepOffset = 0.3f;
    
    [Space(10)]
    
    // ========================================
    // DEBUG VISUALIZATION
    // ========================================
    
    [Header("Debug Visualization")]
    
    [Tooltip("Show movement direction and velocity in Scene view.")]
    [SerializeField] private bool showDebugGizmos = true;
    
    [Tooltip("Show ground check raycast in Scene view.")]
    [SerializeField] private bool showGroundCheck = true;
    
    [Tooltip("Log movement events to Console (jump, land, etc.).")]
    [SerializeField] private bool logMovementEvents = false;
    
    // ========================================
    // PRIVATE RUNTIME STATE (NOT SERIALIZED)
    // ========================================
    
    private CharacterController characterController;
    private Vector3 velocity;
    private int currentJumps;
    private bool isGrounded;
    
    // ... (rest of implementation)
}
📋 Complete List of Designer-Adjustable Parameters
Movement Parameters
Table


Parameter    Type    Range    Tooltip Example
Walk Speed    float    1-10    "How fast the player walks in units per second."
Sprint Multiplier    float    1-3    "Multiplier for sprint speed (1.5 = 50% faster)."
Acceleration    float    5-50    "How quickly player reaches max speed (units/sec²)."
Deceleration    float    5-50    "How quickly player stops moving (units/sec²)."
Rotation Speed    float    180-1440    "Character rotation speed in degrees per second."
Instant Rotation    bool    -    "If true, character snaps to movement direction instantly."
Jump Parameters
Table


Parameter    Type    Range    Tooltip Example
Jump Height    float    0.5-5    "How high the player jumps in world units."
Max Jumps    int    1-5    "Number of jumps allowed (1 = single, 2 = double)."
Coyote Time    float    0-0.5    "Grace period after leaving ledge where jump works (sec)."
Jump Buffer    float    0-0.5    "How early player can press jump before landing (sec)."
Air Control    float    0-1    "Movement control while airborne (0 = none, 1 = full)."
Gravity    float    -50 to -5    "Downward acceleration (negative pulls player down)."
Max Fall Speed    float    10-100    "Terminal velocity in units per second."
Physics Parameters
Table


Parameter    Type    Range    Tooltip Example
Controller Height    float    0.5-3    "Height of the character capsule collider."
Controller Radius    float    0.1-1    "Radius of the character capsule collider."
Slope Limit    float    0-90    "Maximum slope angle character can climb (degrees)."
Step Offset    float    0-1    "Maximum step height character can climb (units)."
Ground Check Distance    float    0.01-1    "Distance to check below player for ground."
Ground Layer    LayerMask    -    "What layers count as ground for landing."
Debug Parameters
Table


Parameter    Type    Range    Tooltip Example
Show Debug Gizmos    bool    -    "Draw movement vectors in Scene view."
Show Ground Check    bool    -    "Draw ground detection raycast in Scene view."
Log Events    bool    -    "Print movement events (jump, land) to Console."
🎨 Inspector Organization Best Practices
Hierarchy of Organization
1. [Header] - Major section
   ├── [Tooltip] + [Range/Min] + [SerializeField] - Parameter 1
   ├── [Tooltip] + [Range/Min] + [SerializeField] - Parameter 2
   └── [Tooltip] + [Range/Min] + [SerializeField] - Parameter 3

2. [Space(10)] - Visual separator

3. [Header] - Next major section
   └── ...
Recommended Section Order
For character controllers:

csharp

Copy
1. [Header("Dependencies")] // Required components/references
2. [Header("Movement Speed")]
3. [Header("Rotation")]
4. [Header("Jumping")]
5. [Header("Air Control")]
6. [Header("Ground Detection")]
7. [Header("CharacterController Settings")]
8. [Header("Debug Visualization")]
Principle: Most-frequently-tweaked parameters at the top, technical/debug at the bottom.

Visual Example
Good Inspector Layout:

╔═══════════════════════════════════════╗
║ Player Movement (Script)              ║
╠═══════════════════════════════════════╣
║ Movement Speed                        ║
║ ├─ Walk Speed         [====|====] 5   ║ ← Slider with value
║ ├─ Sprint Multiplier  [===|=====] 1.5 ║
║ ├─ Acceleration       [======|==] 20  ║
║                                       ║
║ Jumping                               ║
║ ├─ Jump Height        [===|=====] 2   ║
║ ├─ Max Jumps          [=|=======] 1   ║
║                                       ║
║ Debug Visualization                   ║
║ ├─ Show Debug Gizmos  ☑               ║
║ └─ Log Events         ☐               ║
╚═══════════════════════════════════════╝
Bad Inspector Layout:

╔═══════════════════════════════════════╗
║ Player Movement (Script)              ║
╠═══════════════════════════════════════╣
║ walkSpeed             5               ║ ← No tooltip, no slider
║ jumpHeight            2               ║
║ showDebugGizmos       ☑               ║
║ sprintMultiplier      1.5             ║ ← Random order
║ maxJumps              1               ║
║ gravity               -20             ║ ← Can set to +9999!
╚═══════════════════════════════════════╝
✍️ Tooltip Writing Guidelines
The Formula
[Action/Effect] + [Units/Scale] + [Context/Example]
Examples
Table


✅ Excellent Tooltip    ❌ Poor Tooltip
"How fast the player walks in units per second. (Default: 5)"    "Walk speed"
"Multiplier for sprint speed. 1.5 means 50% faster than walk."    "Sprint mult"
"Grace period after leaving a ledge where jump still works (in seconds)."    "Coyote time"
"Movement control while airborne. 0 = no control, 1 = full ground control."    "Air control value"
"Downward acceleration in units/sec². Negative value pulls player down."    "Gravity"
Common Tooltip Patterns
For Speed/Distance Values
csharp

Copy
[Tooltip("How [fast/far] the [object] [action] in [units] per [time].")]
Examples:

"How fast the player walks in units per second."
"How far the projectile travels in world units."
For Multipliers
csharp

Copy
[Tooltip("[What it multiplies]. [Example value] = [result].")]
Examples:

"Multiplier for sprint speed. 1.5 = 50% faster than walk."
"Damage multiplier on critical hits. 2.0 = double damage."
For Normalized Values (0-1)
csharp

Copy
[Tooltip("[What it controls]. 0 = [min behavior], 1 = [max behavior].")]
Examples:

"Movement control while airborne. 0 = no control, 1 = full ground control."
"Master volume. 0 = muted, 1 = full volume."
For Counts/Limits
csharp

Copy
[Tooltip("[What it limits]. [Example value] = [meaning].")]
Examples:

"Number of jumps allowed. 1 = single jump, 2 = double jump."
"Maximum enemies that can spawn. 0 = unlimited."
For Time Values
csharp

Copy
[Tooltip("[What happens] for [duration] in [units].")]
Examples:

"Grace period after leaving a ledge where jump still works (in seconds)."
"Cooldown before the player can dash again (in seconds)."
🚫 Common Mistakes to Avoid
Mistake 1: No Tooltips
❌ Bad:

csharp

Copy
[SerializeField] private float coyoteTime = 0.1f;
Problem: Designer has no idea what "coyote time" means.

✅ Good:

csharp

Copy
[Tooltip("Grace period after leaving a ledge where jump still works (seconds).")]
[SerializeField] private float coyoteTime = 0.1f;
Mistake 2: Vague Tooltips
❌ Bad:

csharp

Copy
[Tooltip("Jump value.")]
[SerializeField] private float jumpHeight = 2f;
Problem: "Jump value" doesn't explain what it does or what units it uses.

✅ Good:

csharp

Copy
[Tooltip("How high the player jumps in world units. (Default: 2.0)")]
[SerializeField] private float jumpHeight = 2f;
Mistake 3: No Range Constraints
❌ Bad:

csharp

Copy
[Tooltip("Movement control while airborne.")]
[SerializeField] private float airControl = 0.5f;
Problem: Designer could accidentally set to -100 or 9999.

✅ Good:

csharp

Copy
[Tooltip("Movement control while airborne (0 = none, 1 = full).")]
[Range(0f, 1f)]
[SerializeField] private float airControl = 0.5f;
Mistake 4: Poor Organization
❌ Bad:

csharp

Copy
[SerializeField] private float walkSpeed = 5f;
[SerializeField] private bool showDebugGizmos = true;
[SerializeField] private float jumpHeight = 2f;
[SerializeField] private float gravity = -20f;
Problem: Random order, no grouping, hard to scan.

✅ Good:

csharp

Copy
[Header("Movement")]
[SerializeField] private float walkSpeed = 5f;

[Header("Jumping")]
[SerializeField] private float jumpHeight = 2f;
[SerializeField] private float gravity = -20f;

[Header("Debug")]
[SerializeField] private bool showDebugGizmos = true;
Mistake 5: Using Technical Jargon
❌ Bad:

csharp

Copy
[Tooltip("Lerp factor for velocity interpolation.")]
[SerializeField] private float smoothing = 0.1f;
Problem: "Lerp factor" and "interpolation" are programmer terms.

✅ Good:

csharp

Copy
[Tooltip("How smoothly the character accelerates (lower = smoother, higher = snappier).")]
[Range(0.01f, 1f)]
[SerializeField] private float smoothing = 0.1f;
🛠️ Advanced Techniques
Custom Property Drawers
For more complex UI, you can create custom Inspector layouts:

csharp

Copy
// Custom attribute
public class MinMaxRangeAttribute : PropertyAttribute
{
    public float min;
    public float max;
    
    public MinMaxRangeAttribute(float min, float max)
    {
        this.min = min;
        this.max = max;
    }
}

// Usage
[MinMaxRange(0f, 10f)]
public Vector2 speedRange = new Vector2(3f, 8f);
Result: Dual slider for min/max values.

Conditional Fields (with Plugins)
Hide/show fields based on other values:

csharp

Copy
// Requires Odin Inspector or similar plugin
[ShowIf("useCustomGravity")]
[SerializeField] private float customGravity = -20f;
Read-Only Fields in Inspector
Show runtime values without allowing edits:

csharp

Copy
[Header("Runtime Info (Read-Only)")]
[SerializeField] [ReadOnly] private float currentSpeed;
[SerializeField] [ReadOnly] private bool isGrounded;

// Requires custom ReadOnly attribute or plugin
✅ AI Agent Checklist
When generating designer-facing code:

Use [SerializeField] for all designer-adjustable values
Add [Tooltip] to EVERY serialized field
Write clear, jargon-free tooltips with units and examples
Use [Header] to organize related parameters
Use [Range] for bounded values (0-1, angles, counts)
Use [Min] for values that must be positive
Add [Space] between major sections for readability
Order fields logically (most-used at top, debug at bottom)
Provide sensible default values
Test that all values work at min/max range boundaries
Document any non-obvious interactions between parameters
📚 Quick Reference
Essential Attributes
csharp

Copy
[Header("Section Name")]           // Organize into sections
[Tooltip("Explanation here")]      // Hover text
[SerializeField]                   // Expose private field
[Range(min, max)]                  // Slider with bounds
[Min(value)]                       // Minimum value constraint
[Space(pixels)]                    // Vertical spacing
[TextArea(minLines, maxLines)]     // Multi-line text
Tooltip Template
csharp

Copy
[Tooltip("[What it does] in [units]. [Scale explanation or example].")]
Example:

csharp

Copy
[Tooltip("How fast the player walks in units per second. (Default: 5)")]
Complete Field Example
csharp

Copy
[Header("Movement Speed")]
[Tooltip("How fast the player walks in units per second.")]
[Range(1f, 10f)]
[SerializeField] private float walkSpeed = 5f;
🎯 Summary
The Golden Rules:

✅ Every designer-facing field needs a tooltip
✅ Organize with headers and spacing
✅ Constrain with [Range] or [Min]
✅ Write tooltips for humans, not programmers
✅ Test that designers can understand without asking
If a designer has to ask what a parameter does, your tooltip failed.

<metadata>
  <version>1.0</version>
  <author>Programming Lead & AI Collaboration</author>
  <created>2025-12-15</created>
  <last_updated>2025-12-15</last_updated>
  <project>G-Hell Roguelite Prototype</project>
  <scope>All Unity development - designer collaboration</scope>
  <audience>Programmers creating designer-friendly tools</audience>
  <related_documents>
    <document>Unity_CSharp_Coding_Standards.md</document>
    <document>Unity_Specific_Best_Practices.md</document>
    <document>Character_Controller_Development_Specifications.md</document>
    <document>ScriptableObject_Workflow_Guide.md</document>
  </related_documents>
</metadata>
```
