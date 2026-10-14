# Game Design — Last Survivor: Warzone

## High concept
A single-player, third-person survival action game in a dangerous post-collapse world. Players scavenge, manage limited supplies, improve their equipment, and decide when to fight or avoid threats.

## Design pillars
1. **Survival choices:** supplies and ammunition are limited; preparation matters.
2. **Exploration rewards:** abandoned places contain resources, stories, and danger.
3. **Readable combat:** enemy intent, hit feedback, and player status should be clear.
4. **Steady progression:** exploration and missions unlock better equipment and harder areas.
5. **Mobile-first usability:** large touch targets, legible HUD, adjustable graphics, and stable frame rate.

## First playable slice (priority)
One small forest-edge outpost with a damaged road, a shelter, a supply cache, and one enemy type. The player can move, look around, aim, fire, take damage, and restart after defeat. Use placeholder assets first.

### Completion criteria
- The player can move without falling through the ground.
- Camera movement is predictable and does not invert unexpectedly.
- The enemy can detect and approach the player in the test arena.
- The weapon only damages valid targets within range.
- Health reaches zero correctly and a clear defeat state appears.
- The scene runs without recurring console errors.
- Android controls and performance are tested before calling the slice mobile-ready.

## Player loop
1. Prepare at a safe point.
2. Explore a nearby location.
3. Search containers and collect supplies.
4. Avoid or engage threats.
5. Return to safety or reach an objective.
6. Use rewards to improve equipment and unlock harder locations.

## Player systems
- Health and stamina first.
- Inventory and loot containers after the combat loop is stable.
- Hunger, thirst, temperature, crafting, and shelter systems later; avoid adding too many meters before the basic game is fun.
- Equipment progression with clear benefits and trade-offs.

## Combat
Initial prototype: a simple raycast weapon with range and damage settings. Later systems may include recoil, reloads, ammunition, weapon switching, hit reactions, melee, armour, and enemy archetypes.

## Enemy design
Begin with one readable enemy that detects the player within a radius, approaches, and attacks at close range. Later add patrols, search behavior, ranged enemies, elite enemies, and bosses. Production AI should use NavMesh or another tested navigation system; the starter AI does not navigate obstacles.

## World regions (long-term)
- **Abandoned city:** dense interiors, streets, scavenging, ambushes.
- **Forest:** natural cover, wildlife, camps, limited visibility.
- **Military base:** high-value equipment and organized hostile patrols.
- **Desert:** long sight lines and limited water.
- **Snowy mountains:** cold exposure, steep terrain, isolated facilities.
- **Underground bunker:** low visibility, close-quarters encounters, environmental storytelling.
- **Industrial zone:** machinery, warehouses, hazards, and rare components.

## Art direction
Aim for grounded, cinematic realism while maintaining Android performance. Use consistent scale, believable materials, controlled color grading, atmospheric lighting, and clear silhouettes. Visual quality must be measured on target devices, not only on a powerful desktop.

## Audio
Prioritize footsteps, weapon reports, reloads, enemy cues, environmental ambience, and clear UI feedback. Use only original or properly licensed assets.

## Accessibility and mobile UX
- Adjustable sensitivity and aim settings.
- Large, customizable touch controls.
- Separate graphics presets.
- Subtitles for important dialogue and cues where applicable.
- Avoid relying on color alone to communicate critical information.

## Monetization principles
No monetization during the first prototype. If the game becomes enjoyable and stable, evaluate optional rewarded ads and cosmetic or convenience purchases. Avoid deceptive prompts, excessive interruptions, and pay-to-win pressure. All integrations must comply with current store and provider rules.