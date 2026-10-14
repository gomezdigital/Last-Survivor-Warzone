# Technical Architecture

## Project status
This repository is currently a **starter source repository**, not a complete Unity-generated project. Unity project settings, scenes, prefabs, materials, and imported assets still need to be created in the Unity Editor.

## Recommended initial setup
- Create a Unity 3D project using a supported LTS version.
- Start with Built-in Render Pipeline for the simplest prototype, or URP if the target device and workflow support it well. Choose one pipeline early; do not mix pipeline-specific materials.
- Use C# scripts under `Assets/Scripts`.
- Keep scenes and assets under version control once they exist.
- Do not commit `Library`, temporary build output, signing keys, or private credentials.
- Ensure Unity-generated `.meta` files are committed with assets.

## Prototype components
- `ThirdPersonPlayerController`: CharacterController movement using legacy Input Manager axes.
- `ThirdPersonCameraFollow`: simple camera follow and mouse look for Editor testing.
- `SurvivalStats`: health and stamina values with basic damage and stamina methods.
- `SimpleWeapon`: camera-based raycast damage prototype.
- `SimpleEnemyAI`: simple distance-based chase and close-range attack.

## Known prototype limitations
- Legacy Input Manager input is not a complete mobile control system.
- The simple camera is a starter implementation and needs collision handling, sensitivity settings, and mobile look input.
- Enemy chase does not use pathfinding and can get stuck on obstacles.
- The raycast weapon is a prototype; it needs fire-rate control, ammunition, effects, animation, and proper hit layers.
- No save system, inventory, quests, multiplayer, production UI, analytics, or monetization is included.
- Code must be compiled and tested in Unity before release.

## Suggested next architecture steps
1. Create and commit a Unity scene and player prefab after validating scripts in Editor.
2. Add a configurable input abstraction so keyboard/controller and mobile touch input can share gameplay code.
3. Add a game-state manager for playing, paused, and defeated states.
4. Add enemy navigation and configurable enemy data.
5. Add loot/inventory data structures and UI.
6. Add profiling, device testing, and automated validation where practical.

## Quality gates
For each feature: define expected behavior, implement the smallest change, compile in Unity, test normal and edge cases, inspect the Console, and record any known limitations. Do not label a feature complete until it has been tested.