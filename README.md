# Tower Defense Demo (Unity 3D)

⚠️ **Note:** This is a demo project created for educational and portfolio purposes.  

---

## About the Project

A cyberpunk-themed 3D tower defense prototype for PC, built in Unity.  

> **Important:** This repository contains only the core gameplay classes. UI elements, visual assets, and full playable scenes are **not** included.

---

## 🎮 Gameplay Video

[Gameplay](https://drive.google.com/drive/folders/1oVvfF5dT5BwLYawppOQTPN9X5cb33wfN?usp=sharing) 

---

## Technologies & Patterns

- **Unity 3D** – game engine, PC build
- **C#** – all game logic
- **Event Bus (Publisher-Subscriber)**  
  Towers, enemies, hero, UI and services exchange signals without direct references, keeping coupling low.
- **Object Pool**  
  Projectiles are reused via `AmmoReservoir<T>` to avoid frequent allocations and reduce garbage collection.
- **State Pattern**  
  Enemies and the hero switch cleanly between states (moving, attacking, idle, etc.), each encapsulated in its own class.
- **Strategy Pattern**  
  Towers delegate attack logic to interchangeable `IAttackStrategy` objects, allowing easy changes and upgrades.
- **Unweighted Graph Pathfinding**  
  Enemy routes are built from a node graph; minimal-hop paths are computed so each enemy follows a sequence of waypoints without heavy pathfinding at runtime.
- **Scriptable Objects**  
  All balance data (health, damage, speed, etc.) is stored as assets, editable in the Unity Editor without touching code.

---
