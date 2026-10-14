# Prototype Setup — First Test Scene

This guide explains how to turn the starter scripts into a small test scene and prepare a first touchscreen layout. It does not require art assets.

## Before starting

You need a computer that can run a supported Unity Editor version to compile and test the game. GitHub stores and edits source files, but does not compile or run a Unity game by itself. A full Unity Editor workflow is not available on an Android phone.

## 1. Create the Unity project

1. Install/open Unity Hub on a suitable computer.
2. Create a **3D** project using a supported Unity LTS version.
3. Use the default Built-in Render Pipeline for this simple first test, unless you already have a URP project.
4. Copy the repository's `Assets/Scripts` folder into the new Unity project's `Assets` folder.
5. Let Unity finish importing scripts and inspect the Console for compile errors.

The starter scripts use Unity's legacy Input Manager APIs. If the project is configured for only the new Input System, enable **Both** input handling in Player Settings or adapt the keyboard scripts to the new Input System.

## 2. Make the basic test arena

1. Create a Plane at position (0, 0, 0), scale (5, 1, 5).
2. Create a Capsule named `Player` at (0, 1, 0).
3. Add **CharacterController**, **SurvivalStats**, and **ThirdPersonPlayerController** to Player. The CharacterController handles player movement.
4. Create a Camera and add **ThirdPersonCameraFollow**. Assign Player as its Target.
5. Tag the active camera `MainCamera`. Assign the camera Transform to the player's Camera Transform field if it does not auto-fill.
6. Create a Cube named `Enemy` at (0, 1, 8). Add **SimpleEnemyAI**, keep a non-trigger collider, and assign Player to its Player field.
7. Add **SimpleWeapon** to Player or an empty child object and assign the scene camera to Aim Camera.
8. Add a few cubes as obstacles and test movement, jumping, stamina, aiming, shooting, and enemy behavior.

## 3. Add mobile controls to the Canvas

Create a **Canvas** (Screen Space - Overlay) and make sure the scene has an **EventSystem**. Use UI > Image for touch areas and UI > Slider for bars. Set the Canvas Scaler to **Scale With Screen Size** and use a reference resolution such as 1920 x 1080.

### Left movement joystick
1. Create a large circular Image near the bottom-left; make it roughly 180–220 pixels wide at the reference resolution.
2. Add a smaller circular Image as a child, centered on the background.
3. Attach `MobileJoystickControl` to the background Image and drag the child circle into its **Handle** field.
4. Keep the joystick inside the safe screen area so it is not covered by Android system navigation.

### Right-side camera look
1. Create an Image covering the right half of the screen, anchored to the right half.
2. Give it a fully transparent color but keep **Raycast Target** enabled.
3. Attach `MobileLookAreaControl` to it.
4. Put the look-area Image behind the action buttons in the Canvas hierarchy, so the buttons receive touches first.

### Action buttons
Create three visible UI Images or Buttons near the bottom-right and attach `MobileActionButton` to each:
- Set **Action** to **Fire** for the fire button.
- Set **Action** to **Jump** for the jump button.
- Set **Action** to **Sprint** for the sprint button. Sprint remains active while the finger is held down.

Make the buttons large enough to tap comfortably. Keep their labels/icons readable and leave space around them. The scripts provide input behavior; they do not create or automatically style the UI objects.

### Health and stamina HUD
1. Create two UI Sliders near the top-left.
2. Attach `MobileHUDController` to an empty object on the Canvas.
3. Assign the Player's **SurvivalStats**, the health and stamina sliders, and optional legacy UI Text labels.
4. The controller sets bar values from 0 to 1 and updates optional labels as the values change.

## 4. Test controls in the Editor

- **W/A/S/D** or arrow keys: move.
- **Left Shift**: sprint while stamina remains.
- **Space**: jump.
- **Mouse movement** while the cursor is locked: rotate camera.
- **Left mouse button**: fire the prototype raycast weapon.
- **Escape**: unlock cursor; right-click to lock it again.
- In a touch build, use the on-screen joystick, drag the right-side look area, and tap the action buttons.

## 5. Known prototype limitations

- The HUD and touch-control scripts need UI objects created and wired in the Unity Inspector.
- No character/weapon models, animation controller, ammo/reload, audio, effects, pause menu, defeat screen, or save system yet.
- Enemy movement is simple straight-line chasing and can pass through obstacles.
- The camera has no obstacle collision and may clip through walls.
- The touch controls have not been tested on a physical Android device.
- These are prototype limitations, not evidence of a release-ready game.

## 6. Report bugs accurately

If Unity shows a Console error, copy the full first red error and its file/line number. Fix compile errors before adding new features. After the first scene compiles and runs, priorities include camera collision, a proper game-state/death loop, enemy navigation, weapon feedback, and testing the touch layout on real devices.
