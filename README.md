# Auto-Battler RPG (Lesta Academy Test Assignment Refactor)

A minimalist RPG auto-battler originally created as a test assignment for **Lesta Academy**, now completely refactored and modernized to demonstrate clean architecture, scalable patterns, and AI-assisted workflow. 

It is a tactical RPG experiment where the gameplay focuses entirely on hero building: customizing stats, selecting classes, and choosing the best loot to survive a gauntlet of 5 automated, turn-based battles.

## Tech Stack

- **DI Container:** VContainer
- **Async Flow:** UniTask
- **Messaging:** MessagePipe
- **Tweening:** DOTween
- **UI & Text:** TextMeshPro
- **Development Tools:** JetBrains Rider + Junie AI Chat (for workflow automation and refactoring)

## Architecture & Patterns

- **Dependency Injection:** Full inversion of control via `VContainer` for clean service wiring, replacing singletons and expensive runtime lookups (`FindObjectOfType`).
- **Message-Driven Communication:** Pub/sub event bus powered by `MessagePipe` to completely decouple core domains (UI, Sound, Combat Loop).
- **Asynchronous Gameplay Flow:** Entire combat logic is written in a clean, linear style using `UniTask` with robust resource safety via `CancellationToken` tracking.
- **Data-Driven & Strategy Design:** Core balance, character stats, and the bonus system (`BonusBase`) are isolated into `ScriptableObjects`, acting as interchangeable strategies configurable directly in the Unity Inspector.
- **State Management & Optimization:** 
  - **Snapshot (Memento):** Dedicated mechanism to capture and restore the player's exact state before entering combat.
  - **Object Pooling:** Smart resource recycling for audio clips using the built-in `UnityEngine.Pool` API to minimize Garbage Collector pressure.

## Core Modular Systems
- **Combat Loop:** Fully asynchronous, DI-containerized battle engine independent of specific scene objects.
- **UI Architecture:** Decoupled interface system that communicates with game logic strictly through the central message bus.
- **Audio Management:** Efficient `SoundManager` utilizing pooled audio emitters for dynamic sound effects.
- **Progression & Stats:** Systems handling character classes, stat modifications, and loot evaluations.

## Project Structure

This repository contains core architecture, gameplay, and systems code only. 
Art assets, environment scenes, and commercial third-party content are excluded.

## Links
- [Play WebGL](https://b4kilin.itch.io/auto-battler)