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
        //public ModKey OutputPatchMod = ModKey.Null;
        public HashSet<ModKey> ModsToPatch = new();
        public Dictionary<string, HashSet<string>> ScriptMatches = new();
        public List<UDOtherSettings> OtherMatches = new();
        //public 
        //public UDKwSettings test = new();
        //public IFormLinkGetter<IKeywordGetter> UDKeyword = FormLink<IKeywordGetter>.Null;
        //public FormLink<IArmorGetter> testArmor = new();
        //public ModKey ModToPatch = ModKey.Null;
    }

    public class UDOtherSettings
    {
        /*public FormLink<IKeywordGetter> UDKeyword = new();
        public HashSet<FormLink<IKeywordGetter>> KeywordsToMatch = new();*/
        public HashSet<string> InputScripts = new HashSet<string>();
        public UDKwSettings KeywordMatch = new();
        public List<UDNameSearchSettings> NameMatch = new();
    }

    public class UDKwSettings
    {
        public HashSet<IFormLinkGetter<IKeywordGetter>> Keywords = new();
        public string OutputScript = string.Empty;
    }
/*
    public class UDNameSettings
    {
        public HashSet<string> InputScripts = new HashSet<string>();
        public List<UDNameSearchSettings> SearchResults = new();
    }*/

    public class UDNameSearchSettings
    {
        public string SearchText = string.Empty;
        public string OutputScript = string.Empty;
    }
}
