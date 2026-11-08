# System Architecture: Unity & Flutter Hybrid Mobile Game

This document outlines the architectural boundaries and design patterns utilized in this hybrid 2D mobile game.

## 1. Domain Separation Overview

The project is structured into three strictly decoupled layers:

- **Flutter Mobile Shell (`my_app/`):** Handles native UI overlays, menus, AdMob lifecycle, and platform bridge communication.
- **Unity Game Engine Layer (`Assets/`):** Physics2D simulation, sprite rendering, and MonoBehaviour lifecycle orchestrators.
- **Isolated C# Core Domain Layer (`Assets/Scripts/Core/`):** Pure C# business rules (`CombatRules`, `LevelProgressionRules`, `ISaveStore`) with zero UnityEngine dependency for instant headless testing.

## 2. Core Domain Layer
* **Headless Execution:** Contains zero references to `UnityEngine.MonoBehaviour`.
* **Deterministic Testability:** Tested via `dotnet test Assets/Tests/GameTests.csproj` in sub-second CLI runs.
