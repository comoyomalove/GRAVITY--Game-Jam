---
title: "Unity C# Coding Standards"
type: "coding_guidelines"
version: "1.0"
project: "Unity Game Development"
target_audience: "AI Coding Agents & Human Developers"
created: "2025-12-11"
---

# Unity C# Coding Standards & Conventions

## Purpose

This document defines the coding standards for our Unity game development project. All code—whether written by human developers or AI agents—must strictly adhere to these conventions to ensure consistency, readability, and maintainability across the codebase.

---

## 📋 Naming Conventions

### Fields

| Type | Convention | Example |
|------|------------|---------|
| **Private Field** | camelCase | `private int privateField;` |
| | | `int privateField;` *(omitting `private` keyword is acceptable)* |
| **Protected Field** | camelCase | `protected int protectedField;` |
| **Public Field** | camelCase | `public int publicField;` |

### Variables & Properties

| Type | Convention | Example |
|------|------------|---------|
| **Local Variable** | camelCase | `int localVariable;` |
| **Property** | PascalCase | `public int NewProperty { get; private set; }` |

### Methods & Classes

| Type | Convention | Example |
|------|------------|---------|
| **Method** | PascalCase | `void NewMethod()` |
| **Class** | PascalCase | `public class NewClass` |
| **Interface** | PascalCase with `I` prefix | `public interface INewInterface` |

### Enums

| Type | Convention | Example |
|------|------------|---------|
| **Enum Value** | PascalCase | `EnumValue` |

---

## 🏷️ Unity Inspector Attributes

**Rule**: Write all attributes on a single line above the variable declaration, unless the line becomes excessively long.

### ✅ Correct Format
```csharp
[SerializeField] [Tooltip("This is a new variable")]
private int newVariable;
