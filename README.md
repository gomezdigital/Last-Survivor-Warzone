# LAST SURVIVOR: WARZONE
**A third-person open-world survival action game for Android, built with Unity and C#.**

> Survive the collapse. Explore the unknown. Become the last survivor.

Last Survivor: Warzone is an ambitious survival-action project focused on exploration, scavenging, tactical combat, environmental danger, and meaningful player progression. The long-term vision is a detailed world with abandoned cities, forests, military installations, deserts, snowy mountains, underground bunkers, and industrial zones.

## Current status
**Pre-production / prototype foundation.** This repository contains the game design, technical plan, and starter Unity C# scripts. It is not yet a complete Unity project or a playable Android release. The scripts must be imported into a Unity project and tested in the Unity Editor before they can be considered working game features.

## Development principles
- Build a small, playable vertical slice before expanding the world.
- Target Android performance from the beginning.
- Prefer readable, maintainable code and test each system as it is added.
- Use original or properly licensed models, textures, audio, animations, and other assets.
- Never promise a release date or revenue before testing and publishing.
- Keep monetization optional and fair; prioritize a good player experience.

## Planned first playable slice
1. Third-person player movement and camera.
2. One small, explorable environment.
3. Basic health and stamina.
4. One hostile enemy with simple chase and attack behavior.
5. A basic hitscan weapon prototype.
6. Clear objectives and a restart/death loop.
7. Android touch controls and performance testing.

## Planned world regions
- Abandoned city
- Forest and wilderness
- Military base
- Desert
- Snowy mountains
- Underground bunker
- Industrial zone

These are long-term goals, not features that are already implemented.

## Technology
- **Engine:** Unity (version to be selected after checking the target device and available development hardware)
- **Language:** C#
- **Initial platform:** Android
- **Version control:** Git and GitHub
- **Potential monetization:** optional rewarded ads, carefully placed ads, and optional in-app purchases, subject to platform policies and eligibility.

## Repository map
- `Assets/Scripts/Player/` — player movement and camera prototype
- `Assets/Scripts/Survival/` — health and stamina prototype
- `Assets/Scripts/Combat/` — basic weapon prototype
- `Assets/Scripts/Enemies/` — simple enemy behavior prototype
- `Docs/` — design, architecture, and roadmap

## How to use the starter scripts
1. Create a 3D Unity project using a supported Unity LTS release.
2. Copy the `Assets/Scripts` directory from this repository into the Unity project's `Assets` folder.
3. Use a GameObject with a `CharacterController` for the player and add the player controller script.
4. Add a camera and the camera-follow script; assign the player as its target.
5. Add the survival component to the player.
6. Create an enemy GameObject with a collider and the enemy AI script. The simple prototype moves toward the player without pathfinding.
7. Add the weapon script to the player or a weapon object, assign a camera, and configure the weapon's range and damage.
8. Read the script comments and test in the Editor. The initial movement prototype uses Unity's legacy Input Manager; mobile touch UI is a later task and must be implemented before a phone release.

## Important limitations
The current scripts are learning/prototype foundations, not production-ready systems. They do not provide realistic graphics, a complete open world, animations, save data, robust navigation, multiplayer, anti-cheat, touch controls, or a Play Store-ready build. These require additional development and device testing.

## Revenue and publishing
Downloads and playtime do not automatically pay a developer. Revenue depends on eligible ad impressions, purchases, store eligibility, policy compliance, player retention, and other factors. Monetization will be considered only after the game is playable and tested. Check current Google Play and advertising-provider requirements before publishing.

## Contributing and issue reporting
Use GitHub Issues to track bugs, decisions, and future features. Keep changes small enough to review and test.

See [Game Design](Docs/GAME_DESIGN.md), [Technical Architecture](Docs/TECHNICAL_ARCHITECTURE.md), and [Roadmap](Docs/ROADMAP.md).