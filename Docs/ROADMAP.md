# Development Roadmap

This roadmap distinguishes source-code work from features that have been verified in a running Unity build. A checked script task means the code is in the repository, not that it has compiled or passed gameplay testing.

## Stage 0 — Repository foundation
- [x] Create GitHub repository.
- [x] Document vision, design pillars, architecture, and milestones.
- [x] Add Unity-aware .gitignore.
- [ ] Create a Unity project in the Editor and choose a render pipeline.
- [ ] Import scripts and confirm a clean compile.

## Stage 1 — First playable prototype
- [x] Add player movement and camera scripts.
- [ ] Create greybox test arena with collision and lighting.
- [x] Add health/stamina system and HUD script.
- [x] Add one basic enemy with chase and attack behavior.
- [x] Add raycast weapon, damage interface, magazine ammo, fire-rate limit, and reload logic.
- [x] Add a defeat/restart controller.
- [ ] Wire all scene objects, UI, and buttons in Unity, including mission progress/victory UI.
- [ ] Compile all scripts in Unity and fix Console errors.
- [ ] Verify the complete gameplay loop in a running Unity build.

**Exit gate:** the entire loop can be played repeatedly without blocking bugs. This gate is not yet passed.

## Stage 2 — Combat feedback and survival gameplay
- [x] Add mobile input support for fire and reload requests.
- [x] Add ammo label and centre-screen crosshair HUD controller.
- [x] Add a brief crosshair hit marker when a damageable target is hit.
- [x] Add a nearby-enemy health HUD script.
- [ ] Add interactable supply containers.
- [ ] Add inventory and item definitions.
- [x] Add trigger-based health and ammunition supply pickup script.
- [x] Add a starter enemy-count mission objective and victory panel controller (Unity wiring/testing still pending).
- [ ] Add basic save/load after core data stabilizes.

## Stage 3 — First polished region
- [ ] Build one compact outpost/forest region.
- [ ] Add licensed or original environment assets.
- [ ] Add lighting, sound, environmental storytelling, and optimized materials.
- [ ] Add simple mission guidance and a clear HUD.
- [ ] Test on representative Android devices.

## Stage 4 — Mobile readiness
- [x] Add starter touch movement and camera-control scripts.
- [x] Add starter on-screen fire, reload, jump, and sprint input support.
- [ ] Create and wire the actual touchscreen UI in Unity.
- [ ] Add graphics quality presets.
- [ ] Profile frame rate, memory, heat, and battery.
- [ ] Handle app pause/resume, low memory, and different screen sizes.
- [ ] Verify build, install, launch, and play on real devices.

## Stage 5 — Expansion
- [ ] More enemy archetypes and AI.
- [ ] Additional weapons and equipment.
- [ ] Missions, rewards, and progression.
- [ ] Add regions one at a time: city, military base, industrial zone, desert, snow, bunker.
- [ ] Add weather and day/night only after performance is stable.

## Stage 6 — Publishing preparation
- [ ] Confirm name, branding, ownership, and asset licenses.
- [ ] Create privacy policy and accurate data-safety disclosures as required.
- [ ] Review Google Play target API, testing, account, and content requirements.
- [ ] Prepare store listing, screenshots, icon, and age/content rating.
- [ ] Integrate and test ads or purchases only if suitable.
- [ ] Run a closed test and fix reported problems.
- [ ] Publish only after release criteria are met.

## Release gate
Do not call the game launch-ready until it builds reliably, runs on target Android devices, has no known critical bugs, handles data and permissions responsibly, uses licensed assets, and complies with the current requirements of every platform and monetization provider.

## Scope rule
A polished, fun small region is more valuable than a huge empty map. Expand only when the current gameplay loop is stable and enjoyable.
