using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UDPatcher
{
    public class UDPatchSettings
    {
        public bool UseModes = false;
        public HashSet<ModKey> ModsToPatch = new();
        public Dictionary<string, HashSet<string>> ScriptMatches = new();
        public List<UDOtherSettings> OtherMatches = new();
    }

    public class UDOtherSettings
    {
        public HashSet<string> InputScripts = new HashSet<string>();
        public List<UDKwSettings> KeywordMatch = new();
        public List<UDNameSearchSettings> NameMatch = new();
    }

    public class UDKwSettings : UDOtherSetting
    {
        public HashSet<IFormLinkGetter<IKeywordGetter>> Keywords = new();
    }

    public class UDNameSearchSettings : UDOtherSetting
    {
        public string SearchText = string.Empty;
    }

    public abstract class UDOtherSetting
    {
        public string OutputScript = string.Empty;
    }
}
