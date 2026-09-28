# 1.2.0
* Added a unique music track - 'The Treehouse that Time Forgot' by Cane B! You can find it along with his other work on [YouTube](https://www.youtube.com/@caneb4) and [SoundCloud](https://soundcloud.com/vgmcb)! Thanks to Cane B for letting me use it for the stage!
    * Also added dependency on R2API Sound
* **Art pass 2:**
  * Remodeled the trees and branches: they're higher poly and generally look a bit better. There are also far, far fewer noticeable texture seams (ideally none)
  * Remodeled metal scaffolding to look more scrappy and cobbled together; it also actually has metal beams holding it up, rather than just floating in the air
  * All materials/textures in the map were updated to look a bit nicer. The moss coating on metal objects in particular was significantly improved, I think
  * Replaced the Aphelian Sanctuary launch pads with new ones unique to this map
  * Updated the stage's fog to make transitions between fog levels less abrupt. Also, distant fog is thicker and creates a silhouette on background objects which, I think, looks 'Cool' ,
  * Added sharp splintered wood to the edges of chopped logs and holes
* **Layout changes:**
  * Added boxes and platforms to some of the tree interiors that were previously mostly empty
  * Added shelves to the chopped tree which previously had a weird arch made of crates on it. You can jump on boxes to reach higher shelves in some spots
  * Added a fourth Newt Altar location atop a shelf
  * Simulacrum: Raised the lowest tree upward. The center tree can never be short. Added a couple platforms on one corner of the satellite dish
    * This was an attempt to make the Void Focus crab move in such a way that it's easier to follow but it still sometimes just floats upward in a weird unpredictable way. Maybe it'll do that less now, at least,,,
  * Simulacrum: Added a couple jump pads below the satellite dish to try and make it easier to get back on without trudging through void fog if you fall off
* **Other changes:**
  * Increased Jellyfish spawn distance (Standard -> Far)
  * Added Solus Control Units and Solus Transporters (both after looping)
  * Starstorm 2: Security Chests (Mimics) can now appear in the stage,! (also added a config option to toggle them)

# 1.1.4
* Attempted to fix an issue where you could get stuck inside a rescue ship

# 1.1.3
* Accidentally forgot to include the language files in the mod when I uploaded the previous version. Sorry

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

