using System;
using System.Collections.Generic;
using Oracle.Packets.Fields;

namespace Oracle.Packets.Model
{
    public static class TablesField
    {
        public static float MaxValue = 3.4028235E38f;
        public static Dictionary<string, string> Encoders { get; }
        public static List<string> ManaProps { get; }
        public static List<string> SimTime { get; }
        public static Dictionary<BuildNumberRange, Action<SerializerField>> Patches { get; }
        public static HashSet<string> Pointers { get; }
        public static HashSet<string> Fixed { get; }

        static TablesField()
        {
            Encoders = new Dictionary<string, string>();
            ManaProps = new List<string>();
            SimTime = new List<string>();
            Patches = new Dictionary<BuildNumberRange, Action<SerializerField>>();
            Pointers = new HashSet<string>();
            Fixed = new HashSet<string>();
            Encoders.Add("CBodyComponentBaseAnimating.m_angRotation", "QAngle");
            Encoders.Add("CBaseAnimating.m_flElasticity", "coord");
            Encoders.Add("CBaseAttributableItem.m_viewtarget", "coord");
            Encoders.Add("CBodyComponentPoint.m_angRotation", "QAngle");
            Encoders.Add("CPlayerLocalData.dirPrimary", "coord");
            Encoders.Add("CPlayerLocalData.origin", "coord");
            Encoders.Add("CPlayerLocalData.localSound", "coord");
            Encoders.Add("CBasePlayer.m_vecLadderNormal", "normal");
            Encoders.Add("CBeam.m_vecEndPos", "coord");
            Encoders.Add("CBodyComponentBaseAnimatingOverlay.m_angRotation", "qangle_pitch_yaw");
            Encoders.Add("CDOTA_BaseNPC_Barracks.m_angInitialAngles", "QAngle");
            Encoders.Add("CEnvDeferredLight.m_vLightDirection", "QAngle");
            Encoders.Add("CEnvWind.m_location", "coord");
            Encoders.Add("CFish.m_poolOrigin", "coord");
            Encoders.Add("CFogController.dirPrimary", "coord");
            Encoders.Add("CFuncLadder.m_vecLadderDir", "coord");
            Encoders.Add("CFuncLadder.m_vecPlayerMountPositionTop", "coord");
            Encoders.Add("CFuncLadder.m_vecPlayerMountPositionBottom", "coord");
            Encoders.Add("CPropVehicleDriveable.m_vecEyeExitEndpoint", "coord");
            Encoders.Add("CPropVehicleDriveable.m_vecGunCrosshair", "coord");
            Encoders.Add("CRagdollProp.m_ragPos", "coord");
            Encoders.Add("CRagdollProp.m_ragAngles", "QAngle");
            Encoders.Add("CRagdollPropAttached.m_attachmentPointBoneSpace", "coord");
            Encoders.Add("CRagdollPropAttached.m_attachmentPointRagdollSpace", "coord");
            Encoders.Add("CShowcaseSlot.vecLocalOrigin", "coord");
            Encoders.Add("CShowcaseSlot.angLocalAngles", "QAngle");
            Encoders.Add("CShowcaseSlot.vecExtraLocalOrigin", "coord");
            Encoders.Add("CShowcaseSlot.angExtraLocalAngles", "QAngle");
            Encoders.Add("CWorld.m_WorldMins", "coord");
            Encoders.Add("CWorld.m_WorldMaxs", "coord");


            Pointers.Add("CBodyComponent");
            Pointers.Add("CEntityIdentity");
            Pointers.Add("CPhysicsComponent");
            Pointers.Add("CRenderComponent");
            Pointers.Add("CDOTAGamerules");
            Pointers.Add("CDOTAGameManager");
            Pointers.Add("CDOTASpectatorGraphManager");
            Pointers.Add("CPlayerLocalData");
            
            ManaProps.Add("m_flMana");
            ManaProps.Add("m_flMaxMana");
            
            SimTime.Add("m_flSimulationTime");
            SimTime.Add("m_flAnimTime");

            Fixed.Add("m_bWorldTreeState");
            Fixed.Add("m_ulTeamLogo");
            Fixed.Add("m_ulTeamBaseLogo");
            Fixed.Add("m_ulTeamBannerLogo");
            Fixed.Add("m_iPlayerIDsInControl");
            Fixed.Add("m_bItemWhiteLis");
            Fixed.Add("m_iPlayerSteamID");
            
            Patches.Add(new BuildNumberRange(null, 990),
                delegate(SerializerField field)
                {
                    Encoders.TryGetValue(field.Parent + "." + field.VarName, out var type);
                    field.EncoderType = type;
                } );
            Patches.Add(new BuildNumberRange(1016, 1026),
                delegate(SerializerField field)
                {
                    if (Fixed.Contains(field.VarName))
                    {
                        field.EncoderType = "fixed64";
                    }
                });
            Patches.Add(new BuildNumberRange(null, null),
                delegate(SerializerField field)
                {
                    if (SimTime.Contains(field.VarName))
                    {
                        field.SerializerType = "simulationtime";
                    }
                });
            Patches.Add(new BuildNumberRange(null, 954),
                delegate(SerializerField field)
                {
                    if (ManaProps.Contains(field.VarName))
                    {
                        if (field.HighValue == 3.4028235E38f)
                        {
                            field.LowValue = null;
                            field.HighValue = 8192.0f;
                        }
                    }

                });
        }
    }
}