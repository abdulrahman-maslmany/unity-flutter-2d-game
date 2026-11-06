<div align="center">

# 🎮 2D Action Platformer Mobile Game
### A Production-Ready Hybrid Mobile Architecture (Unity & Flutter)

[![C# Tests](https://img.shields.io/badge/C%23%20Tests-63%20passed%20%7C%20255ms-brightgreen?style=flat-square&logo=dotnet&logoColor=white)](#1-isolated-c-pure-domain-tests-headless-cli)
[![Flutter Tests](https://img.shields.io/badge/Flutter%20Tests-All%20Passed-blue?style=flat-square&logo=flutter&logoColor=white)](#2-flutter-mobile-shell-tests)
[![DeepSource Active Issues](https://app.deepsource.com/gh/abdulrahman-maslmany/unity-flutter-2d-game.svg/?label=active+issues&show_trend=true&token=bILbWHBdLsLN74b_ko_bNaO2)](https://app.deepsource.com/gh/abdulrahman-maslmany/unity-flutter-2d-game/)
[![DeepSource Coverage](https://app.deepsource.com/gh/abdulrahman-maslmany/unity-flutter-2d-game.svg/?label=code+coverage&show_trend=true&token=bILbWHBdLsLN74b_ko_bNaO2)](https://app.deepsource.com/gh/abdulrahman-maslmany/unity-flutter-2d-game/)
[![Codacy Badge](https://app.codacy.com/project/badge/Grade/ca7652e526854e57917c9d09a76b72a9)](https://app.codacy.com?utm_source=gh&utm_medium=referral&utm_content=&utm_campaign=Badge_grade)
[![CI/CD Pipeline](https://github.com/abdulrahman-maslmany/unity-flutter-2d-game/actions/workflows/ci.yml/badge.svg)](https://github.com/abdulrahman-maslmany/unity-flutter-2d-game/actions/workflows/ci.yml)
[![Unity](https://img.shields.io/badge/Unity-2022.3%20LTS-black?logo=unity)](https://unity.com/)
[![Flutter](https://img.shields.io/badge/Flutter-3.19%2B-02569B?logo=flutter)](https://flutter.dev/)

<p align="center">
  A fully functional, decoupled 2D action-platformer mobile game combining the high-performance physics of <b>Unity (C#)</b> with the reactive UI and mobile integration capabilities of <b>Flutter (Dart)</b>.
</p>

</div>

---

## 🏗️ Architecture & Engineering Highlights

This repository is built following **Clean Code** principles and automated DevSecOps pipelines:

* **Decoupled Pure Logic:** Game rules (such as `CombatRules`, `LevelProgressionRules`, and `ConnectivityService`) are isolated as pure C# domain logic independent of Unity's `MonoBehaviour`. This enables instant, sub-second unit test execution (63 tests in ~255ms) without launching the Unity runtime or requiring graphical display buffers.
* **Deterministic Build & Dependency Hygiene:** Fully locked dependency manifests (`packages.lock.json` and `pubspec.lock`) verified in CI with `--locked-mode` to prevent supply-chain drift.
* **Continuous Quality Assurance:** Integrated with **DeepSource** and **Codacy** to enforce Roslyn static analysis, file-scoped namespaces, and eliminate anti-patterns across C# and Dart codebases.
* **Security & Secret Scanning:** Monitored against OWASP Top 10 vulnerabilities, ensuring zero exposed keys or sensitive runtime configurations.
* **Hybrid Bridge Integration:** Clean inter-process communication bridging Flutter application state and Unity engine lifecycle.

---

## 🌟 Key Gameplay Features

* **Dual Combat System:** Melee sword mechanics combined with a physics-based ranged projectile system (Bow & Arrow).
* **Responsive 2D Platforming:** Custom physics-driven character controller handling precise jump curves, variable acceleration, ground checks, and obstacle collision.
* **Collectibles & Progression:** Centralized score tracking, dynamic level unlocking, health containers, and save/load persistence layer (`ISaveStore`).
* **Monetization Architecture:** Clean AdMob integration supporting banner, interstitial, and rewarded ad unit lifecycle events with test-ID fallbacks.

---

## 🛠️ Tech Stack & Specifications

| Layer | Technology | Role / Version |
| :--- | :--- | :--- |
| **Game Engine Core** | Unity Engine (C#) | `2022.3 LTS` / `.NET 8.0` (2D Physics, Tilemaps, NUnit) |
| **Mobile Host Shell** | Flutter / Dart (`my_app`) | `Flutter 3.19+` (State Management, UI Bridge) |
| **Target OS** | Android | API Level 24+ (ARM64 / ARMv7) |
| **Static Analysis** | DeepSource & Codacy | Automated Linting, Anti-pattern detection, Roslyn |
| **CI/CD** | GitHub Actions | Deterministic build, locked restore, lint, and regression testing |

---

## 📁 Repository Structure

```text
├── Assets/                 # Unity Engine Layer
│   ├── Scripts/            # Core domain rules, services, and game entities
│   │   ├── Core/           # Decoupled domain business rules (Combat, Leveling)
│   │   └── Services/       # Persistence (ISaveStore) and connectivity services
│   └── Tests/              # EditMode NUnit test suites (pure C# logic)
│       └── GameTests.csproj# Isolated C# test project runnable via dotnet CLI
├── my_app/                 # Flutter Mobile Shell
│   ├── lib/                # Reactive UI, bridge services, and ads configuration
│   └── test/               # Flutter unit and widget test suites
├── .github/workflows/      # Automated CI pipelines (C# locked build, Flutter test & lint)
├── docker-compose.yml      # Isolated containerized test runner
└── .env.example            # Environment variables & ad placement templates
🚀 Quickstart & Fresh Clone Setup
Prerequisites
.NET 8.0 SDK (Required for running isolated C# domain test suites)
Flutter SDK 3.19+ (Required for mobile shell and Dart tests)
Unity 2022.3 LTS (Optional: only needed for asset editing and game deployment)
🧪 Testing & Verification
Both the Unity C# core domain logic and the Flutter application module can be built and verified independently from CLI without launching the Unity Editor UI or requiring Unity license activation:
1. Isolated C# Pure Domain Tests (Headless CLI)
Runs all 63 unit and regression specifications covering combat mechanics, progression curves, and connectivity logic:
Bash
# Execute isolated NUnit test suite (Runs in ~250ms)
dotnet test Assets/Tests/GameTests.csproj
2. Flutter Mobile Shell Tests
Runs static analysis and Flutter widget/unit suites:
Bash
cd my_app
flutter pub get
flutter analyze --fatal-infos --fatal-warnings
flutter test
cd ..
📄 License
This project is open-source and distributed under the MIT License.
Code
---
