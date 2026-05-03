# 1.1.2
* Fixed Lemurian Eggs never appearing with Artifact of Devotion enabled
* Repositioned a tree branch that wasn't fully connected to its tree's trunk
* Attempted to fix a few particularly egregious texture seams. They still exist but should be much less noticeable now
  * I didn't really know what I was doing when modeling this map (still don't) so it's kind of a mess. Seams will probably never be completely fixed but I'll probably continue trying to clean up new ones that I find

# 1.1.1
* Fixed one of the Newt Altar spots being fully submerged underground

# 1.1.0
* Art pass:
  * The inside of the tree trunks now uses the inner wood texture instead of the bark texture (only just now realized it was weird that the insides of the trees also had a layer of bark..)
  * Increased the tree bark texture's contrast and fixed the bark texture being scaled differently on most of the tree trunks
  * Updated the inner wood and wood/bark transition textures and gave them each a unique normal map
  * Gave the wind spirals a new material and texture
  * The distant clouds' material is now subtly animated
* Added crunch compression to most of the map's textures. Shouldn't affect visuals much if at all, but should decrease the mod's file size
* Changed the stage's subtitle to something more evocative ("Deserted Encampment" -> "Grafted Encampment")
* Added a red light to the logbook diorama
* Fixed a spot where you could get caught on nothing while walking up the big ramp near the bottom of the map
* Fixed a spot where you could get stuck between two pipes for the rest of your life

# 1.0.2
* Fixed the Simulacrum variant's scene def having "Valid for Random Selection" enabled
* Fixed a missing face on the central log (tall style)

# 1.0.1
* Removed Shrines of Combat from the map's interactable selection
  * Originally, this map had fans instead of the Aphelian Sanctuary jump pads, so it had both Combat and Blood shrines in an attempt to make it easier to get the funds needed to activate them. I had to remove the fans since they weren't working properly in multiplayer and I had forgotten to remove one of the shrines. Now there are only Blood shrines
* Fixed a typo in the map's logbook entry
* Fixed a couple invisible colliders staying active when they shouldn't have been
* Added a link to the mod's GitHub repo in its manifest.json file 

# 1.0.0
* Initial Release

