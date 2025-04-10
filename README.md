# RadRefinements

QOL refinements for Sailwind.

## Inventory Refinements

* The ability to swap the item held with the one in the selected inventory slot.
* A quick map button which withdraws and stows the leftmost map in your inventory slots.
* Quick slot buttons which withdraws from and stows to the selected inventory slot.
* When looking at an item in a crate, see that item's description.

## Instrument Reading Refinements

* On clocks, text that shows the time on clocks in global, local, or both global and local times. Only viewable when the clock face is visible. 
* On compasses, text that shows the direction the compass is facing in cardinal directions and/or degrees.
* On quadrants when inspecting, text that shows the angle read.

## Other Refinements

* You can break an empty crate, tobacco box, candle box, or standard size barrel into firewood with a knife.
* When looking at a crate that has been opened, you can see how many items are in the crate.
* Remove hint text for the knife and fishing hook. This is disabled by default and must be turned on in the config.
* Fish move when caught instead of remaining motionless. 

## Configurable

* Enable or disable: inventory swap, quick map, quick slots, crate item description, clock global time text, clock local time text, compass reading while held, compass cardinal direction text, compass degrees text, quadrant text, crate inventory count text, containers into firewood, hint texts for common items, fish movement.
All but clock local time text and removing the hint texts are defaulted to true.
* Setting the quick map/slots buttons to desired keys. Defaults to 'M' for map and 1, 2, 3, 4, 5 for inventory quick slots.
* Distance at which the compass and clock texts are viewable. Defaults to 3 for the compass and 7 for the clock.
* Number of cardinal directions to give compass reading in (4, 8, 16, or 32). Defaults to 16.

### Requires

* [BepInEx 5.4.23](https://github.com/BepInEx/BepInEx/releases)
* [ModSaveBackups 1.1.1](https://thunderstore.io/c/sailwind/p/RadDude/ModSaveBackups/)

### Installation

If updating, remove RadRefinements folders and/or RadRefinements.dll files from previous installations.  
<br>
Extract the downloaded zip. Inside the extracted RadRefinements-\<version\> folder copy the RadRefinements folder and paste it into the Sailwind/BepInEx/Plugins folder.