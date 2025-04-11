# Changelog

All notable changes to this project will be documented in this file.

## [v1.0.10] - 2025-04-10

### Fixed
- Bug that made the cut control text not show when looking at some cuttable containers
- Bug that caused NREs when cutting fruit

### Removed
- Debug logging that was used for testing

## [v1.0.9] - 2025-04-10

### Added
- Fish move when caught instead of remaining motionless.
- Configuration option to enable/disable fish movement.

## [v1.0.8] - 2025-04-09

### Added
- Control text showing that you can cut a container to turn it into firewood
- Configuration options to enable/disable the crate inventory count text and the ability to turn containers into firewood
- Configuration option to disable the hint text for items. Currently only the knife and fishing hooks

## [v1.0.7] - 2025-04-08

### Added
- The ability to break up an empty crate, tobacco box, candle box, or standard size barrel into firewood with a knife
- Item quantity to crate look text

## [v1.0.6] - 2025-04-04

### Added
- Disable reading text while looking through spyglass.

### Fixed
- NRE during save file load.

### Removed
- MVC files, no longer needed.

## [v1.0.5] - 2025-03-31

### Added
- Option to view compass reading text while holding compass
- Files for MVC support

## [v1.0.3] - 2025-03-30

### Added
- Item descriptions when looked at in a crate

### Fixed
- Save serialization bug

## [v1.0.2] - 2025-03-29

### Added
- Added cardinal direction text option to compass reading text with options for level of cardinal directions

## [v1.0.1] - 2025-03-28

### Updated
- Updated to check for ModSaveBackups dependency.

## [v1.0.0] - 2025-03-28

### Added
- The ability to swap the item held with the one in the selected inventory slot.
- A quick map button which withdraws and stows the leftmost map in your inventory slots.
- Quick slot buttons which withdraws from and stows to the selected inventory slot.
- On clocks, text that shows the time on clocks in global, local, or both global and local times.
- On compasses, text that shows the direction the compass is facing in degrees.
- On quadrants when inspecting, text that shows the angle read.
- Config settings for enabling/disabling: inventory swap, quick map, quick slots, global clock text, local clock text, compass text, quadrant text.
- Config settings for setting the quick map/slots buttons.
- Config settings for the distance at which the compass and clock texts are viewable.
