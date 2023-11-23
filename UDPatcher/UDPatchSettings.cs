using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mutagen.Bethesda.WPF.Reflection.Attributes;

namespace UDPatcher
{
    public class UDPatchSettings
    {
        [MaintainOrder]
        public bool UseModes = false;
        [Tooltip("Mods from which to read and patch Armors")]
        [MaintainOrder]
        public HashSet<ModKey> ModsToPatch = new();
        [Tooltip("Settings for Render scripts")]
        [MaintainOrder]
        public UDRenderSettings RenderScriptSettings = new();
        [Tooltip("Settings for Inventory scripts")]
        [MaintainOrder]
        public UDInventorySettings InventoryScriptSettings = new();
    }

    public class UDRenderSettings
    {
        [Tooltip("The DD Inventory scripts which match to a single UD render script")]
        [MaintainOrder]
        public Dictionary<string, HashSet<string>> ScriptMatches = new();
        [Tooltip("Other rules for finding appropriate UD scripts. Applied after initially matching to a UD script.")]
        [MaintainOrder]
        public List<UDOtherSettings> OtherMatches = new();
        [Tooltip("Inventory Script values to transfer to render script, and their modified name (leave blank to keep as-is)")]
        [MaintainOrder]
        public Dictionary<string, string?> ScriptValues = new();
    }

    public class UDInventorySettings
    {
        [Tooltip("The DD inventory scripts which match to a single UD inventory script")]
        public Dictionary<string, HashSet<string>> ScriptMatches = new();
    }

    public class UDOtherSettings
    {
        [Tooltip("This rule will only apply when the matched UD script is one of the following")]
        [MaintainOrder]
        public HashSet<string> InputScripts = new HashSet<string>();
        [Tooltip("Change UD script based on keywords")]
        public List<UDKwSettings> KeywordMatch = new();
        [Tooltip("Change UD script based on item name")]
        public List<UDNameSearchSettings> NameMatch = new();
    }

    public class UDKwSettings : UDOtherSetting
    {
        [Tooltip("Will change UD script only if Armor contains one of the chosen Keywords")]
        public HashSet<IFormLinkGetter<IKeywordGetter>> Keywords = new();
    }

    public class UDNameSearchSettings : UDOtherSetting
    {
        [Tooltip("Will change UD script only if the Armor's name contains the chosen text")]
        public string SearchText = string.Empty;
    }

    public abstract class UDOtherSetting
    {
        [Tooltip("The new UD script in case of a match")]
        public string OutputScript = string.Empty;
        [Tooltip("Priority of the rule being applied (if several rules apply, the highest priority overrules)")]
        public int Priority = 0;
    }
}
