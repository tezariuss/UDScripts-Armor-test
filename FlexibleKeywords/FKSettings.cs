using Mutagen.Bethesda;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Plugins.Cache;
using Mutagen.Bethesda.Synthesis;
using Mutagen.Bethesda.Skyrim;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FlexibleKeywords
{
    public class FKSettings
    {
        public HashSet<ModKey> ModsToPatch = new();
        public List <KeywordRules> Rules = new List<KeywordRules>();
    }

    public class KeywordRules
    {
        public int Priority = 0;
        public ArmorMatcher MatchingRules = new ArmorMatcher();
        public HashSet<IFormLinkGetter<IKeywordGetter>> KeywordsToAdd = new();
        public HashSet<IFormLinkGetter<IKeywordGetter>> KeywordsToRemove = new();
    }

    public class ArmorMatcher
    {
        public bool AND = false;
        public HashSet<IFormLink<IArmorGetter>> ManualSelection = new();
        public string KeywordRegex = string.Empty;
        public string EditorIdRegex = string.Empty;
        public string DisplayNameRegex = string.Empty;
    }

    public class ArmorMatcherOperations : ArmorMatcher
    {
        public Regex Keyword { get; }
        public Regex EditorId { get; }
        public Regex DisplayName { get; }
        public ILinkCache LinkCache { get; }

        public ArmorMatcherOperations(ArmorMatcher parent, ILinkCache linkCache)
        {
            KeywordRegex = parent.KeywordRegex;
            EditorIdRegex = parent.EditorIdRegex;
            DisplayNameRegex = parent.DisplayNameRegex;
            ManualSelection = parent.ManualSelection;
            AND = parent.AND;

            Keyword = new Regex(KeywordRegex);
            EditorId = new Regex(EditorIdRegex);
            DisplayName = new Regex(DisplayNameRegex);
            LinkCache = linkCache;
        }

        public bool? MatchKeywords(IArmorGetter armor)
        {
            if (Keyword.ToString() == string.Empty) return null;
            var keywords = armor.Keywords;
            if (keywords != null)
            {
                foreach (var keywordLink in keywords)
                {
                    if (keywordLink.TryResolve(LinkCache, out var keyword))
                    {
                        if (keyword.EditorID != null && Keyword.IsMatch(keyword.EditorID))
                        {
                            return true;
                        }
                    } else
                    {
                        throw new KeyNotFoundException($"Keyword {keywordLink.FormKey} of " +
                            $"Armor {armor.EditorID} could not be resolved");
                    }
                }
            }
            return AND ? false : null;
        }

        public bool? MatchName(Func<ArmorMatcherOperations, Regex> property, string? armorName)
        {
            if (property(this).ToString() != string.Empty
                )
            {
                return armorName != null && property(this).IsMatch(armorName);
            } else
            {
                return AND ? false : null;
            }
        }

        public bool? MatchManual(IArmorGetter armor)
        {
            if (ManualSelection.Contains(armor))
            {
                return true;
            }
            else if (ManualSelection.Any() && AND)
                return false;
            return null;
        }

        public bool MatchArmor(IArmorGetter armor)
        {
            return MatchManual(armor) 
                ?? MatchName((ops => ops.DisplayName), armor.Name?.String) 
                ?? MatchName((ops => ops.EditorId), armor.EditorID) 
                ?? MatchKeywords(armor) 
                ?? false;
        }
    }
}
