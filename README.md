# 2D Action Platformer Mobile Game (Unity & Flutter) 🎮⚔️

A fully functional, production-ready 2D action-platformer mobile game developed using **Unity Engine (C#)** and integrated with a **Flutter** mobile wrapper application.

---

## 🌟 Key Gameplay Features

* **Advanced Combat System:** Dual-combat mechanics featuring **melee attacks (Sword)** and **ranged projectile combat (Bow & Arrow System)**.
* **Responsive 2D Platforming:** Physics-driven character controller handling acceleration, jump curves, ground detection, and obstacle collision.
* **Collectibles & Progression:** Score tracking, currency/coin pick-ups, health containers, and dynamic level unlocking logic.
* **Monetization & Ads Ready:** Structured AdMob integration for banner, interstitial, and rewarded ad placements.
* **Automated Quality Pipeline:** Continuous Integration (CI) suite covering Flutter code analysis and automated regression testing.

---

## 🛠️ Tech Stack & Requirements

| Component | Technology | Recommended Version |
| :--- | :--- | :--- |
| **Game Engine** | Unity Engine (C#, 2D Physics, Tilemaps) | `2022.3 LTS` or newer |
| **Mobile App** | Flutter / Dart (`my_app`) | `Flutter 3.19+` (Dart 3.3+) |
| **Target OS** | Android (ARM64 / ARMv7) | API Level 24+ (Android 7.0+) |
| **CI/CD** | GitHub Actions | Automated Linting, Build & Unit Tests |

---

## 📁 Repository Structure

```text
├── Assets/                 # Unity C# Scripts, Sprites, Tilemaps, Prefabs & Audio
├── ProjectSettings/        # Unity project engine configurations
├── my_app/                 # Flutter mobile shell and integration layer
│   ├── lib/                # Flutter UI and bridge services
│   ├── test/               # Automated unit and widget tests
│   └── pubspec.yaml        # Flutter dependency manifest & lockfile
├── .github/workflows/      # CI/CD pipelines (Flutter test, static analysis)
└── .env.example            # Environment & ad configuration template
