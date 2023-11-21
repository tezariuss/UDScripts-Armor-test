using Mutagen.Bethesda;
using Mutagen.Bethesda.Synthesis;
using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Plugins.Cache;
using Mutagen.Bethesda.Plugins.Order;
using Mutagen.Bethesda.Environments;
using CommandLine;
using Noggog;
using System.Xml.Linq;
using System.Runtime.CompilerServices;

namespace UDPatcher
{
    public class Program
    {
        public static async Task<int> Main(string[] args)
        {
            return await SynthesisPipeline.Instance
                .AddPatch<ISkyrimMod, ISkyrimModGetter>(RunPatch)
                .SetTypicalOpen(GameRelease.SkyrimSE, "UDPatched" +
                ".esp")
                .Run(args);
        }

        public static IKeywordGetter GetZadInventoryKeyword(ILinkCache linkCache)
        {
            const string DDI_NAME = "Devious Devices - Integration.esm";
            const int ZADINV_ID = 0x02b5f0;
            if (ModKey.TryFromFileName(DDI_NAME, out var modKey))
            {
                var zadInvKwFormKey = new FormKey(modKey, ZADINV_ID);
                if (linkCache.TryResolve(zadInvKwFormKey, typeof(IKeywordGetter), out var zadInvKw))
                {
                    return zadInvKw.Cast<IKeywordGetter>();
                } else
                {
                    throw new Exception($"Could not find zad_Inventory record");
                }
            }
            else
            {
                throw new Exception($"Could not find {DDI_NAME}");
            }
        }

        public static IKeywordGetter GetUDInventoryKeyword(ILinkCache linkCache)
        {
            const string UD_NAME = "UnforgivingDevices.esp";
            const int UDINV_ID = 0x1553dd;
            ModKey udMod;
            if (ModKey.TryFromFileName(UD_NAME, out var modKey))
            {
                udMod = modKey;
            } else
            {
                throw new Exception($"Could not find ${UD_NAME}");
            }
            //var UDInvKwFormKey = ;
            var UDInvKwFormLink = new FormKey(udMod, UDINV_ID).ToLinkGetter<IKeywordGetter>();
            if (UDInvKwFormLink.TryResolve(linkCache, out var resolvedUDKw))
            {
                return resolvedUDKw;
            } else { 
                throw new Exception($"Could not find {UDInvKwFormLink.FormKey} in {udMod}");
            }
        }

        //public static IQuestGetter GetUDCDMain(ILinkCache linkCache)
        //{
        //    const string UD_NAME = "UnforgivingDevices.esp";
        //    const int UDCD_MAIN_NAME = "UD_CustomDevice_Quest";


        //}

        public static IScriptEntryGetter? FindArmorScript(IEnumerable<IScriptEntryGetter> armorScripts, IDictionary<string, IScriptEntryGetter> searchScripts)
        {
            
            foreach (var armorScript in armorScripts) {
                //IScriptEntryGetter outScript;
                if (searchScripts.TryGetValue(armorScript.Name, out var outScript))
                {
                    return outScript;
                }
            }
            return null;
        }

        public static T DumbRecordGetter<T>(ILinkCache linkCache, ModKey mod, uint formId)
        {
            return linkCache.Resolve(new FormKey(mod, formId), typeof(T)).Cast<T>();
        }

        public static void RunPatch(IPatcherState<ISkyrimMod, ISkyrimModGetter> state)
        {
            //var env = GameEnvironment.Typical.Skyrim(SkyrimRelease.SkyrimSE);
            //var test = env.LinkCache;
            var UDScripts = new Dictionary<string, IScriptEntryGetter>();
            var zadScripts = new Dictionary<string, IScriptEntryGetter>();

            const bool USE_MODES = false;

            const string DDI_NAME = "Devious Devices - Integration.esm";
            ModKey ddiMod = ModKey.FromFileName(DDI_NAME);

            const int ZADINVKW_ID = 0x02b5f0;

            const string UD_NAME = "UnforgivingDevices.esp";
            ModKey udMod = ModKey.FromFileName(UD_NAME);

            const int UDINVKW_ID = 0x1553dd;
            const int UDPATCHKW_ID = 0x13A977;
            const int UDKW_ID = 0x11a352;
            const int UDPATCHNOMODEKW_ID = 0x1579be;

            const int UDCDMAINQST_ID = 0x15e73c;

            var idLinkCache = state.LoadOrder.PriorityOrder.ToImmutableLinkCache<ISkyrimMod, ISkyrimModGetter>(LinkCachePreferences.OnlyIdentifiers());
            //var zadInvKeyword = GetZadInventoryKeyword(idLinkCache);
            IKeywordGetter zadInvKeyword = DumbRecordGetter<IKeywordGetter>(idLinkCache, ddiMod, ZADINVKW_ID);
                //idLinkCache.Resolve(new FormKey(ModKey.FromFileName(DDI_NAME), ZADINV_ID), typeof(IKeywordGetter)).Cast<IKeywordGetter>();
            IKeywordGetter udInvKeyword = DumbRecordGetter<IKeywordGetter>(idLinkCache, udMod, UDINVKW_ID);
            IKeywordGetter udPatchKw = DumbRecordGetter<IKeywordGetter>(idLinkCache, udMod, UDPATCHKW_ID);
            IKeywordGetter udKw = DumbRecordGetter<IKeywordGetter>(idLinkCache, udMod, UDKW_ID);
            IKeywordGetter udPatchNoModeKw = DumbRecordGetter<IKeywordGetter>(idLinkCache, udMod, UDPATCHNOMODEKW_ID);

            IQuestGetter udMainQst = DumbRecordGetter<IQuestGetter>(idLinkCache, udMod, UDCDMAINQST_ID);

            void addKeywords(Armor armor)
            {
                var keywords = new ExtendedList<IKeywordGetter>() { udKw, udPatchKw };
                if (USE_MODES)
                {
                    keywords.Add(udPatchNoModeKw);
                }
                if (armor.Keywords == null)
                {
                    armor.Keywords = new ExtendedList<IFormLinkGetter<IKeywordGetter>>();
                }
                foreach (var keyword in new List<IKeywordGetter>())
                {
                    var kwLink = keyword.ToLinkGetter();
                    if (!armor.Keywords.Contains(kwLink))
                    {
                        armor.Keywords.Add(kwLink);
                    }
                }
            }
                //idLinkCache.Resolve(new FormKey(ModKey.FromFileName(UD_NAME), UDCDMAIN_ID), typeof(IQuestGetter)).Cast<IQuestGetter>();
            int totalPatched = 0;
            int newDevices = 0;
            foreach (var invArmorGetter in state.LoadOrder.PriorityOrder.Armor().WinningOverrides())
            {
                if (invArmorGetter.Keywords!.Contains(zadInvKeyword))
                {
                    // find the script the armour's using
                    var invCurrentScripts = invArmorGetter.VirtualMachineAdapter!.Scripts;//.Select(script => script.Name);
                    var invUDScript = FindArmorScript(invCurrentScripts, UDScripts);
                    var invZadScript = FindArmorScript(invCurrentScripts, zadScripts);
                    if (invZadScript == null && invUDScript == null)
                    {
                        Console.WriteLine("penigs");
                        continue;
                    }
                    var renderDevice = (invZadScript != null ? invZadScript : invUDScript)!
                        .Properties
                        .Where(prop => prop.Name == "deviceRendered")
                        .FirstOrDefault()!
                        .Cast<IScriptObjectPropertyGetter>()
                        .Object;
                    IArmorGetter renderArmor;
                    if (renderDevice.TryResolve<IArmorGetter>(idLinkCache, out var foundArmor))
                    {
                        renderArmor = foundArmor;
                    } else
                    {
                        throw new Exception($"Invalid render target {renderDevice.FormKey} for inventory item {invArmorGetter.EditorID} ({invArmorGetter.FormKey})");
                    }
                    var renderUDScript = FindArmorScript(renderArmor.VirtualMachineAdapter!.Scripts, UDScripts);
                    if (renderUDScript == null && invUDScript == null)
                    {
                        Console.WriteLine("pegnis");
                        continue;
                    }
                    var renderArmorOverride = state.PatchMod.Armors.GetOrAddAsOverride(renderArmor);
                    if (renderArmorOverride == null)
                    {
                        Console.WriteLine("video peningns");
                        continue;
                    }
                    if (renderArmorOverride.Keywords == null)
                    {
                        renderArmorOverride.Keywords = new ExtendedList<IFormLinkGetter<IKeywordGetter>>();
                    }
                    if (renderArmorOverride.VirtualMachineAdapter == null)
                    {
                        renderArmorOverride.VirtualMachineAdapter = new VirtualMachineAdapter();
                    }
                    renderArmorOverride.Keywords.Add(udInvKeyword);

                    var newRenderScriptName = UDScripts[invZadScript!.Name].Name;
                    var newRenderScript = invZadScript.DeepCopy();
                    newRenderScript.Name = newRenderScriptName;

                    if (invUDScript == null)
                    {
                        var invArmorOverride = state.PatchMod.Armors.GetOrAddAsOverride(invArmorGetter);
                        if (invArmorOverride.VirtualMachineAdapter == null)
                        {
                            throw new Exception("wtf???");
                        }
                        var invScript = invArmorOverride.VirtualMachineAdapter.Scripts.Where(script => script.Name == invZadScript!.Name).Single();
                        //invScript.Name = "UD_CustomDevice_EquipScript";
                        
                        var UDCDProp = new ScriptObjectProperty();
                        UDCDProp.Name = "UDCDmain";
                        UDCDProp.Flags = ScriptProperty.Flag.Edited;
                        UDCDProp.Object = udMainQst.ToLink();
                        //invScript.Properties = new ExtendedList<ScriptProperty>(UDCDProp);
                        
                        invScript.Name = "UD_CustomDevice_EquipScript";
                        invScript.Properties.Add(UDCDProp);

                        
                        if (renderUDScript == null)
                        {
                            //var newRenderScript = invScript.DeepCopy();
                            
                            /*if (renderArmorOverride.VirtualMachineAdapter == null)
                            {
                                renderArmorOverride.VirtualMachineAdapter = new VirtualMachineAdapter();
                            }*/
                            renderArmorOverride.VirtualMachineAdapter.Scripts.Append(newRenderScript);
                            addKeywords(renderArmorOverride);
                            Console.WriteLine($"Device {renderArmorOverride} patched!");
                            totalPatched++;
                            // add keywords
                            // 1. add epatchkw
                            // 2. add eudkw
                            // 3. add patchnomode if not using modes
                        } else
                        {
                            Console.WriteLine($"WARNING: Render device {renderArmor} already has UD script! Creating new render device!");
                            newDevices++;
                            var newRenderArmor = state.PatchMod.Armors.DuplicateInAsNewRecord(renderArmor);
                            newRenderArmor.EditorID = newRenderArmor.EditorID + "_AddedRenderDevice";
                            var newRenderArmorScripts = newRenderArmor.VirtualMachineAdapter!.Scripts;
                            newRenderArmorScripts[newRenderArmorScripts.FindIndex(script => script == renderUDScript)] = newRenderScript;
                            invScript.Properties[invScript.Properties.FindIndex(prop => prop.Name == "devideRendered")].Cast<ScriptObjectProperty>().Object = newRenderArmor.ToLink();
                            Console.WriteLine($"---NEW DEVICE {newRenderArmor} CREATED!---");
                            //newRenderArmor.VirtualMachineAdapter!.Scripts.FindIndex(x => x == renderUDScript) = newRenderScript;
                        }
                        //newInvScript.Properties.
                        //newInvScript.Name = "UD_CustomDevice_EquipScript";
                        //newInvScript.Properties = UDCDProp;
                        // Up to element edit values
                        //UDCDProp.
                    } else if (renderUDScript == null)
                    {
                        Console.WriteLine($"Device with patched INV but not patched REND detected. Patching renderDevice {renderArmor}.");

                        renderArmorOverride.VirtualMachineAdapter.Scripts.Add(newRenderScript);
                        addKeywords(renderArmorOverride);
                        Console.WriteLine($"Repatched RenderDevice {renderArmor} of InventoryDevice {invArmorGetter}");
                    }
                }
            }
        }
    }
}
