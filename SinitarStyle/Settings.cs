using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Skyrim;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SinitarStyle
{
    public class Settings
    {
        public List<ModMatch> MergeGroups = new();
    }

    public class ModMatch
    {
        public HashSet<ModKey> Into = new();
        public HashSet<ModKey> From= new();
    }

    public class ModMatchOps : ModMatch
    {
        public HashSet<ModKey> AllMods { get; }
        public ModMatchOps(ModMatch origin) {
            this.Into = origin.Into;
            this.From = origin.From;
            AllMods = new HashSet<ModKey>(Into.Union(From));
        }
    }
}
