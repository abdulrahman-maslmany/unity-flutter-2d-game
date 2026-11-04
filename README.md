<div align="center">

# 🎮 2D Action Platformer Mobile Game
### A Production-Ready Hybrid Mobile Architecture (Unity & Flutter)

[![DeepSource Active Issues](https://app.deepsource.com/gh/abdulrahman-maslmany/unity-flutter-2d-game.svg/?label=active+issues&show_trend=true&token=bILbWHBdLsLN74b_ko_bNaO2)](https://app.deepsource.com/gh/abdulrahman-maslmany/unity-flutter-2d-game/)
[![DeepSource Coverage](https://app.deepsource.com/gh/abdulrahman-maslmany/unity-flutter-2d-game.svg/?label=code+coverage&show_trend=true&token=bILbWHBdLsLN74b_ko_bNaO2)](https://app.deepsource.com/gh/abdulrahman-maslmany/unity-flutter-2d-game/)
[![Codacy Badge](https://app.codacy.com/project/badge/Grade/ca7652e526854e57917c9d09a76b72a9)](https://app.codacy.com?utm_source=gh&utm_medium=referral&utm_content=&utm_campaign=Badge_grade)
[![GitHub Workflow Status](https://img.shields.io/github/actions/workflow/status/abdulrahman-maslmany/unity-flutter-2d-game/ci.yml?branch=main&label=CI%2FCD&logo=githubactions&logoColor=white)](https://github.com/abdulrahman-maslmany/unity-flutter-2d-game/actions)
[![Unity](https://img.shields.io/badge/Unity-2022.3%20LTS-black?logo=unity)](https://unity.com/)
[![Flutter](https://img.shields.io/badge/Flutter-3.19%2B-02569B?logo=flutter)](https://flutter.dev/)

<p align="center">
  A fully functional, decoupled 2D action-platformer mobile game combining the high-performance physics of <b>Unity (C#)</b> with the reactive UI and mobile integration capabilities of <b>Flutter (Dart)</b>.
</p>

</div>

---

## 🏗️ Architecture & Engineering Highlights

This repository is built following **Clean Code** principles and automated DevSecOps pipelines:

* **Decoupled Pure Logic:** Game rules (such as `CombatRules` and `LevelProgressionRules`) are isolated as pure C# domain logic independent of Unity's `MonoBehaviour`, allowing instant sub-second unit test execution without launching the Unity runtime.
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
| **Game Engine** | Unity Engine (C#) | `2022.3 LTS` (2D Physics, Tilemaps, NUnit) |
| **Mobile Host** | Flutter / Dart (`my_app`) | `Flutter 3.19+` (State Management, UI Bridge) |
| **Target OS** | Android | API Level 24+ (ARM64 / ARMv7) |
| **Static Analysis** | DeepSource & Codacy | Automated Linting, Anti-pattern detection |
| **CI/CD** | GitHub Actions & Docker | Multi-stage build, lint, and regression testing |

---

## 📁 Repository Structure

```text
├── Assets/                 # Unity Engine Layer
│   ├── Scripts/            # Core domain rules, services, and game entities
│   │   ├── Core/           # Decoupled domain business rules (Combat, Leveling)
│   │   └── Services/       # Persistence (ISaveStore) and connectivity services
│   └── Tests/              # EditMode NUnit test suites (pure C# logic)
├── my_app/                 # Flutter Mobile Shell
│   ├── lib/                # Reactive UI, bridge services, and ads configuration
│   └── test/               # Flutter unit and widget test suites
├── .github/workflows/      # Automated CI pipelines (C# build, Flutter test & lint)
├── docker-compose.yml      # Isolated containerized test runner
└── .env.example            # Environment variables & ad placement templates
🧪 Testing & Verification
Both the Unity core and Flutter modules can be verified independently via CLI:
1. Unity C# Engine Tests
Run unit tests in batch mode (or via dotnet test if configured):
Bash
# Executed automatically in CI via NUnit test runner
dotnet test Assets/Tests/EditMode/
2. Flutter Mobile Tests & Analysis
Bash
cd my_app
flutter analyze
flutter test
📄 License
This project is licensed under the MIT License.
