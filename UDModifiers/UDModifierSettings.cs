using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UDModifiers
{
    public class UDModifierSettings
    {
        //public string test = string.Empty;
        public List<ModifierArmorSelection> ModifierMatches = new();
    }

    public class ModifierArmorSelection
    {
        public HashSet<string> Modifiers = new HashSet<string>();
        public HashSet<IFormLinkGetter<IArmorGetter>> Armors = new HashSet<IFormLinkGetter<IArmorGetter>>();
    }
}
