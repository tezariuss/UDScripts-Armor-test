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

There are several moving parts which determine how an item gets patched. Let's start with the simplest one, and just go down the list.

 - **Inventory Script Settings**
 - *Script Matches*: Each `zad` inventory script will be replaced by the `UD` inventory script it's in. In the default settings, for example, all `zad` scripts will be replaced by `UD_CustomDevice_EquipScript`.
- **Render Script Settings**
 - *Script Matches*: Similar to the above, each `zad` inventory script will be allocated the `UD` render script it's in. Every inventory script in the mods you plan to patch must be under a `UD` script.
 - *Script Values*: These determine which script properties make the transition from the original inventory script to the `UD` render script. The text field determines the new name of the property (e.g. `zad_DeviousDevice` will be `UD_DeviceKeyword` on the `UD` render script)
 - **Other Matches**: Far more granular rules to apply `UD` scripts by
  - *Input Scripts*: It will begin the match if the current `UD` script (after it's been passed through *Script Matches* above) is in this list
  - **Keyword Match**
   - *Output Script*: If the match is successful, the new `UD` script will be this one
   - *Priority*: If multiple Keyword matches apply, the highest priority *Output Script* takes precendence. No guarantees about what happens if there are two matches with the same priority.
   - *Keywords*: We change to the *Output Script* only if the Armor has one of the chosen Keywords
  - **Name Match**
   - *Output Script*: If we match, this will be the new `UD` script
   - *Priority*: Same as with Keywords. Note that **Name Match**es and **Keyword Match**es are separate; **Keyword Match**es will always be applied first.
   - *Search Text*: Regex-based text search. It will search the Armor's `EditorID`.
 
One thing to note: if the `UD` script gets changed by any **Other Match**, it gets fed back into these matches until it comes out unchanged.

Take, for example, a sample Armor with `UD` script `UD_test` and two keywords: `test1` and `test2`. Suppose there are two **Other Matches**, one of which takes `UD_test` and changes the `UD` script to `UD_test1` if an Armor has the `test1` keyword, and the other takes `UD_test1` and does the same with `test2`.

The script would then first change `UD_test` to `UD_test1`, then change `UD_test1` to `UD_test2`. It would put our Armor through the hoops again, and when it finds that nothing has changed, we'll get our final script `UD_test2`.

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
- `.` and `.*` match on any character and any amount of any characters respectively

If `AND` is checked, it will only apply Keywords to the Armor if every non-empty condition is satisfied. Otherwise, only one of the conditions needs to produce a match.

After an Armor satisfies the chosen set of rules, the Keywords To Add are added before the Keywords To Remove are removed. It won't add Keywords that are already on an Armor, and it won't remove a Keyword if the Armor doesn't have it. The order in which Keywords from various rules get applied depends on the Priority setting - the highest Priority rules get applied last.