# GaIMUnityTemplate
Standard template for Unity projects with some basic features (Unity gitignore, GitLFS, etc.)
To use this template, click the "Use this template" button at the top of the page to create a new repository on GitHub.  Then you can clone, pull, or otherwise use the repository as needed. If you are starting a new project, I'd suggest cloning this repository to your computer in your desired location, then creating your new Unity project in that folder.


Modifying Ruby: GDD
DIG: 3480, Lee Eimer
Original Challenge
	When starting work on my final version of Ruby’s Adventure, there were a few initial changes I needed to make in order to fully attempt to meet the original challenge. I needed to fix my music, which played at the wrong times, and include a new particle effect for when the player gained health. Unfortunately, I was unable to make these fixes as I had run out of time and prioritized adding in my new mechanics instead. 
Visual Change
	For visual changes, I included two new NPCS: Rabbit and Turtle. Both of these NPCs introduce new mechanics into their levels–Rabbit introduces the Dash powerup, which allows the Player to move quickly for a short amount of time. Turtle introduces the Slow-Cogs, which slow enemies down to make them easier to hit with a normal cog, but deal no damage. All 4 of these elements include idle animations–both the Slow Cogs and the Dash pickups have a simple up-and-down idle, while Turtle comes out of his shell to look around and Rabbit rises up to look back and forth before settling back down. These NPCs both talk when the player hits X. When the player throws a SlowCog, the cog they throw is the same shade of green as the powerup, signaling that they are using it. 
Audio Change
	For Audio Changes, I carried over the pickup effects for both the Slow Cogs and Dash pickups. I did this to keep pickups and powerups unified across the game. I also attempted to implement quiet themes for each NPC, but was unable to due to time constraints. I was unable to implement any other audio changes, due to time constraints. 

GamePlay Changes
	For my Gameplay Changes, I included two new powerups into the game that work as collectables for the player throughout the two levels. 
The first powerup is the Dash Powerup, introduced by the NPC Rabbit in level 1. This powerup takes the form of a rabbit’s foot, and doubles the player’s speed for 2 seconds, allowing them to move quickly but not giving them invincibility. I included this as an additional option for movement so players could get around enemies or traverse the level quicker. I also made this tied to the number of Dashes that the player collects throughout the level, encouraging players to use the powerup when they need to rather than making it a constant ability. 
The second powerup is the SlowCog Powerup, introduced by the NPC Turtle in level 2. This powerup takes the form of a green cog, and slows the speed of enemies hit with it by half of their movement speed, but deals no damage. I included this as a secondary ammo type for the player, who has limited ammo in the game, in order to help make Hard Enemies easier if they can get the initial hit. By slowing enemies, the player can have an easier time hitting them with one of their limited cogs. I tied this ability to the number of pickups the player collects throughout the level, in order to ration this ability and ensure that it could not be spammed.  
