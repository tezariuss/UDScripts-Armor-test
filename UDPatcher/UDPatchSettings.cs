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
        //public ModKey ModToPatch = ModKey.Null;
    }
}
