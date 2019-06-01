using Google.Protobuf.Collections;
using Oracle.Packets.Model;
using Proto.Dota;

namespace Oracle.Packets.Entry
{
    public class CombatLogEntry
    {
        private StringTableModel _stringTable;

        private CMsgDOTACombatLogEntry _cle;

        public CombatLogEntry(StringTableModel stringTable, CMsgDOTACombatLogEntry combatLog)
        {
            _stringTable = stringTable;
            _cle = combatLog;
        }

        private string ReadName(uint id)
        {
            if (_stringTable == null)
                return null;

            return _stringTable.HasIndex((int)id) ? _stringTable.GetNameByIndex((int)id) : null;
        }

        public override string ToString() => _cle.ToString();

        public DOTA_COMBATLOG_TYPES Type { get => _cle.Type; }

        public string TargetName { get => ReadName(_cle.TargetName); }

        public uint TargetNameId {get => _cle.TargetName; }

        public string TargetSourceName { get => ReadName(_cle.TargetSourceName); }
        public uint TargetSourceNameId { get => _cle.TargetSourceName; }

        public string AttackerName { get => ReadName(_cle.AttackerName); }
        public uint AttackerNameId {get => _cle.AttackerName; }

        public string DamageSourceName { get => ReadName(_cle.DamageSourceName); }
        public uint DamageSourceNameId { get => _cle.DamageSourceName; }

        public string InflictorName { get => ReadName(_cle.InflictorName); }
        public uint InflictorNameId { get => _cle.InflictorName; }

        public bool IsAttackerIllusion {get => _cle.IsAttackerIllusion; }

        public bool IsAttackerHero { get => _cle.IsAttackerHero; }

        public bool IsTargetIllusion { get => _cle.IsTargetIllusion; }

        public bool IsTargetHero {get => _cle.IsTargetHero; }

        public bool IsTargetBuilding { get => _cle.IsTargetBuilding; }

        public bool IsVisibleRadiant { get => _cle.IsVisibleRadiant; }

        public bool IsVisibleDire { get => _cle.IsVisibleDire; }

        public string ValueName { get => ReadName(_cle.Value); }

        public int Value { get => (int)_cle.Value; }

        public int Health { get => _cle.Health; }

        public float Timestamp { get => _cle.Timestamp; }

        public float TimestampRaw { get => _cle.TimestampRaw; }

        public float StunDuration { get => _cle.StunDuration; }

        public float SlowDuration { get => _cle.SlowDuration; }

        public bool IsAbilityToggleOn { get => _cle.IsAbilityToggleOn; }

        public bool IsAbilityToggleOff { get => _cle.IsAbilityToggleOff; }

        public uint AbilityLevel { get => _cle.AbilityLevel; }

        public float LocationX { get => _cle.LocationX; }

        public float LocationY { get => _cle.LocationY; }

        public uint GoldReason { get => _cle.GoldReason; }

        public float ModifierDuration { get => _cle.ModifierDuration; }

        public uint XpReason { get => _cle.XpReason; }

        public uint LastHits { get => _cle.LastHits; }

        public uint AttackerTeam { get => _cle.AttackerTeam; }

        public uint TargetTeam { get => _cle.TargetTeam; }

        public uint IsObsPlaced { get => _cle.ObsWardsPlaced; }

        public RepeatedField<uint> AssistPlayerCount { get => _cle.AssistPlayers; }

        public uint StackCount { get => _cle.StackCount; }

        public bool HiddenModifier { get => _cle.HiddenModifier; }

        public uint NeutralCampType { get => _cle.NeutralCampType; }

        public uint RuneType { get => _cle.RuneType; }

        public bool IsHealSave { get => _cle.IsHealSave; }

        public bool IsUltimateAbility { get => _cle.IsUltimateAbility; }

        public uint AttackerHeroLevel { get => _cle.AttackerHeroLevel; }

        public uint TargetHeroLevel { get => _cle.TargetHeroLevel; }

        public uint Xpm { get => _cle.Xpm; }

        public uint Gpm { get => _cle.Gpm; }

        public uint EventLocation { get => _cle.EventLocation; }

        public bool TargetSelf { get => _cle.TargetIsSelf; }

        public uint DamageType { get => _cle.DamageType; }

        public uint DamageCategory { get => _cle.DamageCategory; }

        public bool InvisibilityModifier { get => _cle.InvisibilityModifier; }

        public uint Networth { get => _cle.Networth; }

        public uint BuildingType { get => _cle.BuildingType; }

        public float ModifierElapsedDuration { get => _cle.ModifierElapsedDuration; }

        public bool SilenceModifier { get => _cle.SilenceModifier; }

        public bool HealFromLifesteal { get => _cle.HealFromLifesteal; }

        public bool ModifierPurged { get => _cle.ModifierPurged; }

        public bool SpellEvaded { get => _cle.SpellEvaded; }

        public bool MotionControllerModifier { get => _cle.MotionControllerModifier; }

        public bool IsLongRangeKill { get => _cle.LongRangeKill; }

        public uint ModifierPurgeAbility { get => _cle.ModifierPurgeAbility; }

        public uint ModifierPurgeNpc { get => _cle.ModifierPurgeNpc; }

        public bool RootModifier { get => _cle.RootModifier; }

        public uint TotalUintDeathCount { get => _cle.TotalUnitDeathCount; }

        public bool AuraModifier { get => _cle.AuraModifier; }

        public bool ArmorDebuffModifier { get => _cle.ArmorDebuffModifier; }

        public bool NoPhysicalDamageModifiere { get => _cle.NoPhysicalDamageModifier; }

        public uint ModifierAbility { get => _cle.ModifierAbility; }

        public bool ModifierHidden { get => _cle.ModifierHidden; }


    }
}