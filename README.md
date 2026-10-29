# Chiikawa Defense

A 2D side-scrolling tower defense game inspired by Battle Cats, starring the characters from Chiikawa. Built in Unity 6 for a class project.

## How to Play

Protect your house and destroy the enemy cave. Coins build up over time. Spend them to send out units who walk toward the cave and fight any enemies in their way. When the cave drops to half health, the boss arrives and wrecks havoc.

- You win when the enemy cave is destroyed
- You lose when your house is destroyed

## Controls

- A / D: Move the camera left and right
- Mouse click: Spawn units, buy upgrades, mute sound

## Units

- Chiikawa (50 coins): Basic unit
- Usagi (75 coins): Fast attacker, low health
- Hachiware (100 coins): Slow tank with lots of health

## Enemies

- Bug: Basic enemy
- BigBug: Tougher enemy that shows up later in the round
- Red-Eyed Boss: Spawns when the enemy cave reaches half health

## Features

- Coins earned every second, with an upgrade to earn them faster
- Enemy waves that speed up and get stronger over time
- Boss fight triggered at half tower health
- Simple code-based animations (walking waddle, attack stretch, hit flash)
- Sound effects, background music, and a mute button
- Win / lose screen with restart

## Opening the Project

1. Clone this repo
2. Open the folder in Unity 6 (6000.6.2f1) through Unity Hub
3. Open Assets/Scenes/SampleScene and press Play

## Scripts

- Unit.cs: Movement, targeting, attacking, and health for every unit and enemy
- UnitAnimator.cs: Waddle, attack, and hit flash animations
- Tower.cs: Base health and win / lose trigger
- EnemySpawner.cs: Enemy waves and boss spawn
- CoinManager.cs: Coins and the coin upgrade
- UnitButton.cs: Spawn buttons
- CameraController.cs: A / D camera scrolling
- GameManager.cs: Win / lose screen and restart
- AudioManager.cs: Music, sound effects, and mute

## Credits

Characters and Art
- Chiikawa characters: nagano 
- Enemy, background, and building art: Chiikawa anime and the Chiikawa Pocket game 
- UI: Cozy UI Pack (Demo) by dobo_ui (https://dobo-ui.itch.io/)

Sound Effects (from Pixabay)
- Hit: "Thud Impact Sound" by virtual_vibes
- Click: "Button Click Sound" by DenielCZ
- Death: "Cartoon Impact 02" by Universfield
- Win: "gewonnen2" by freesound_community
- Lose: "Game Over Orchestral Stinger" by coghezzi

Music
- Battle Theme: Chiikawa Cave Theme
