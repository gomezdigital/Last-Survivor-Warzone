# Development Roadmap

## Stage 0 — Repository foundation
- [x] Create GitHub repository.
- [x] Document vision, design pillars, architecture, and milestones.
- [x] Add Unity-aware .gitignore.
- [ ] Create a Unity project in the Editor and choose a render pipeline.
- [ ] Import scripts and confirm a clean compile.

## Stage 1 — First playable prototype
- [ ] Player movement and camera.
- [ ] Greybox test arena with collision and lighting.
- [ ] Health/stamina feedback.
- [ ] One enemy with navigation and attack behavior.
- [ ] Weapon aiming, hit detection, and damage feedback.
- [ ] Defeat and restart loop.
- [ ] Test and fix Console errors.

**Exit gate:** the entire loop can be played repeatedly without blocking bugs.

## Stage 2 — Survival gameplay
- [ ] Interactable supply containers.
- [ ] Inventory and item definitions.
- [ ] Ammunition and reloads.
- [ ] Health items.
- [ ] One objective/mission.
- [ ] Basic save/load after core data stabilizes.

## Stage 3 — First polished region
- [ ] Build one compact outpost/forest region.
- [ ] Add licensed or original environment assets.
- [ ] Add lighting, sound, environmental storytelling, and optimized materials.
- [ ] Add simple mission guidance and a clear HUD.
- [ ] Test on representative Android devices.

## Stage 4 — Mobile readiness
- [ ] Touch movement and camera controls.
- [ ] On-screen aim, fire, reload, interact, and pause buttons.
- [ ] Graphics quality presets.
- [ ] Frame-rate, memory, heat, and battery profiling.
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