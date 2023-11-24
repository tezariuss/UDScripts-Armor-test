# UDScripts

Various script for [this mod](https://github.com/IHateMyKite/UnforgivingDevices). Currently functional parts include the Patcher and Flexible Keywords.

## Installation

These are Synthesis scripts, so the installation is beyond simple. Instructions can be found [here](https://github.com/Mutagen-Modding/Synthesis/wiki/Installation).

## Reinstallation

There may come a time when you need to erase your own configs and replace them with the defaults. The instructions for this may be found [here](https://github.com/Mutagen-Modding/Synthesis/wiki/Installation).

## Projects

There are several Projects in this repository. All will appear as separate patchers in the Synthesis project.

### UDPatcher

Synthesis patcher to patch various DD items to work with UD devices. Functionality is identical to [the FOMOD](https://github.com/IHateMyKite/UnforgivingDevices_FOMOD/tree/main), but with far greater configurability.

#### How-To (Basic)

1. Select the mods you wish to patch in the Settings (as well as any other mods these mods pull relevant records from - not necessarily the masters)
1. Set the other Settings as desired (the Default settings should account for all DD devices)
1. Run the Synthesis patcher with only this patcher selected
1. Rename the resulting patch in xEdit to an appropriate name

Alternatively, you can simply run the patcher along other Synthesis scripts and leave everything in the one `Synthesis.esp`.

#### How-To (Advanced)

To be continued

### FlexibleKeywords

Synthesis patcher for adding or removing keywords from Armors based on various settings.

#### How-To (Basic)

1. Select the mods you wish to patch
1. Set the appropriate rules
1. Run the Synthesis patcher - FlexibleKeywords can be run among other patchers

#### How-To (Advanced)

Aside from the manual armor selector, the patcher matches via three main properties:

 - Keywords Name
	- Display Name
	- Editor ID

All of these are Regex strings (you can find the documentation for that [here](https://learn.microsoft.com/en-us/dotnet/standard/base-types/regular-expression-language-quick-reference)). Some common examples of how this may be used:

 - `word` matches on any occurrence of "word" (e.g. "word", "words", "word salad" but not "wo rd")
	- `(word|gamer)` matches on any occurrences of "word" or "gamer"
	- `word(ga|mer)` matches on "wordga" or "wordmer"
	- `word*` matches on "wor", "word", "wordd", etc. (any number of `d`s)
	- '.' and '.*' match on any character and any amount of any characters respectively

If `AND` is checked, it will only apply Keywords to the Armor if every non-empty condition is satisfied. Otherwise, only one of the conditions needs to produce a match.

After an Armor satisfies the chosen set of rules, the Keywords To Add are added then the Keywords To Remove are removed. It won't add Keywords that are already on an Armor, and it won't remove a Keyword if the Armor doesn't have it. The order in which Keywords from various rules get applied depends on the Priority setting - the highest Priority rules get applied last.