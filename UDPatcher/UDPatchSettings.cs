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
        public string OutputPatchName = string.Empty;
        public HashSet<ModKey> ModsToPatch = new();
        //public ModKey ModToPatch = ModKey.Null;
    }
}
