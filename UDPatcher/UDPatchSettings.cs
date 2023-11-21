using Mutagen.Bethesda.Fallout4;
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
        public ModKey OutputPatchMod = ModKey.Null;
        public HashSet<ModKey> ModsToPatch = new();
        public List<UDKwSettings> KeywordMatches = new();
        //public ModKey ModToPatch = ModKey.Null;
    }

    public class UDKwSettings
    {
        public IFormLinkGetter<IKeywordGetter> UDKeyword = FormLink<IKeywordGetter>.Null;
        public HashSet<IFormLinkGetter<IKeywordGetter>> KeywordsToMatch = new();
    }
}
