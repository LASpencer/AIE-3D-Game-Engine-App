Computer Graphics Game Engine Assignment
By Andrew Spencer, 2018

CONTROLS
WASD		Movement
LMB		Shoot pistol
E/RMB		Interact with object
Ctrl/MMB	Toggle IR visor
Spacebar	Jump


Turn on IR visor to see through fog and highlight enemies
Shooting removes your IR visor


DESIGN

Several post-processing effects are used in this game. A fog effect using an exponential fog rule
is applied to the main camera. By turning on the visor, an edge detection effect based on depth is
combined with it, allowing objects' outline to be seen through the fog. Also, a distortion effect is
applied to the last camera.

The enemies in the game have two renderers, one with a custom unlit shader on a layer only rendered 
if the visor is on. The material colours them red and yellow, with horizontal rows clipped out in 
screenspace to give the effect of scanlines.

Another custom material is the Highlight material, used when an interactable object is viewed by 
the player. This makes the object pulse with colour.

This scene includes several particle effects. Shooting at most objects will spawn a particle 
effect for sparks or debris coming off it. The wine barrel's particle effect has a trail and 
collides with the environment, with 0 friction. This has the appearance of wine pouring out of
a hole and flowing across the ground.
The broken glass particles also collide with the environment, but with friction.




ATTRIBUTIONS

Several assets from the Unity Asset Store were used:
Scene, props, fire effect and debris and smoke textures from Old Sea Port by cdmpants
Pistol model and texture from [PBR] Makarov by Bluesu
Shell casing from 1911 Pistol Pack by 3D store
Enemy model from Basic Bandit by Will Morillas
Enemy falling animation from Fighting Motions Vol.1 by Magicpot Inc.
Water particle textures from Water FX Pack by Unity
Gun audio from Weapons of Choice by Komposite Sound



Muzzle flash by Julius Krischan Makowa https://opengameart.org/content/muzzle-flash-with-model

