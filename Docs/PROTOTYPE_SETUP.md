# Prototype Setup — First Test Scene

This guide turns the starter scripts into a small Editor test scene. It does not require art assets.

## Before starting
You need access to a computer that can run a supported Unity Editor version. GitHub can store and edit the source, but it cannot compile or run a Unity game by itself. Unity Editor is not supported as a full game-development environment on an Android phone.

## 1. Create the Unity project
1. Install/open Unity Hub on a suitable computer.
2. Create a **3D** project using a supported Unity LTS version.
3. Use the default Built-in Render Pipeline for this simple first test, unless you already have a URP project.
4. Copy the repository's `Assets/Scripts` folder into the new Unity project's `Assets` folder.
5. Let Unity finish importing scripts and inspect the Console for compile errors.

The starter scripts use the legacy Input Manager APIs (`Input.GetAxis`, `Input.GetButtonDown`). If the project is configured for only the new Input System, enable **Both** input handling in Player Settings or adapt the scripts to the new Input System.

## 2. Make a basic test arena
1. Create a Plane at position (0, 0, 0), scale (5, 1, 5).
2. Create a Capsule named `Player` at (0, 1, 0).
3. On Player, add a **CharacterController**, **SurvivalStats**, and **ThirdPersonPlayerController**. Remove or disable the Capsule Collider if it causes unwanted collisions; the CharacterController should handle player movement.
4. Create a Camera and add **ThirdPersonCameraFollow**. Drag Player into its Target field.
5. If the camera is the active scene camera, tag it `MainCamera`. Drag its Transform into the player's Camera Transform field if automatic assignment does not work.
6. Create a Cube named `Enemy` at (0, 1, 8). Add **SimpleEnemyAI**. Keep a non-trigger collider on the enemy. Drag Player into its Player field.
7. Add **SimpleWeapon** to Player or an empty child object. Assign the scene camera to Aim Camera.
8. Add a few cubes as obstacles and test movement, jumping, stamina, aiming, shooting, and enemy behavior.

## 3. Test controls (Editor)
- **W/A/S/D** or arrow keys: move.
- **Left Shift**: sprint while stamina remains.
- **Space**: jump.
- **Mouse movement** while cursor is locked: rotate camera.
- **Left mouse button**: fire the prototype raycast weapon.
- **Escape**: unlock cursor; right-click to lock it again.

## 4. Expected limitations
- There is no animation controller, character model, weapon model, crosshair, HUD, ammo system, sound, or mobile touch UI yet.
- The enemy moves in a straight line and may walk through or get stuck on obstacles.
- The weapon fires on every click; it has no reload or fire-rate system.
- Player death raises an event but does not yet show a defeat screen or stop movement automatically.
- The camera has no obstacle collision, so it may clip through walls.
- These are prototype limitations, not evidence of a release-ready game.

## 5. Report bugs accurately
If Unity shows a Console error, copy the full first red error and its file/line number. Fix compile errors before adding new features. After the first scene compiles and runs, the next priorities are camera collision, a game-state/death loop, a proper enemy navigation system, and mobile controls.
