using CommandLine;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Plugins.Cache;
using Mutagen.Bethesda.Plugins.Records;

namespace UDPatcher
{
    public class UDConstants
    {
        public ILinkCache IdLinkCache { get; }
        //public string UDName { get; }
        //public string DDIName { get; }

        public ModKey UDMod { get; }
        public ModKey DDIMod { get; }

        /*public IKeywordGetter? UdInvKw { get; init; }
        public IKeywordGetter? ZadInvKw { get; }*/

        private Dictionary<string, IMajorRecordGetter> records;
        //public IKeywordGetter 

        public UDConstants(ILinkCache idLinkCache, string uDName, string dDIName)
        {
            IdLinkCache = idLinkCache;
            UDMod = ModKey.FromFileName(uDName);
            DDIMod = ModKey.FromFileName(dDIName);

            records = new Dictionary<string, IMajorRecordGetter> {};
        }

        public void SetRecord<T>(Func<UDConstants, T> propSelector, uint formID, ModKey mod)
        {
            //propSelector(this) = DumbRecordGetter<T>(mod, formID);
        }

        public T DumbRecordGetter<T>(ModKey mod, uint formId)
        {
            return IdLinkCache.Resolve(new FormKey(mod, formId), typeof(T)).Cast<T>();
        }
        //public static
    }
}
