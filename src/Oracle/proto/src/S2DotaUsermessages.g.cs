// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: s2_dota_usermessages.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Proto.Dota {

  /// <summary>Holder for reflection information generated from s2_dota_usermessages.proto</summary>
  public static partial class S2DotaUsermessagesReflection {

    #region Descriptor
    /// <summary>File descriptor for s2_dota_usermessages.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static S2DotaUsermessagesReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "ChpzMl9kb3RhX3VzZXJtZXNzYWdlcy5wcm90bxIPZ29vZ2xlLnByb3RvYnVm",
            "KpQbChFFRG90YVVzZXJNZXNzYWdlcxISCg5ET1RBX1VNX1VLTk9XThAAEh8K",
            "GkRPVEFfVU1fQWRkVW5pdFRvU2VsZWN0aW9uENADEhgKE0RPVEFfVU1fQUlE",
            "ZWJ1Z0xpbmUQ0QMSFgoRRE9UQV9VTV9DaGF0RXZlbnQQ0gMSIAobRE9UQV9V",
            "TV9Db21iYXRIZXJvUG9zaXRpb25zENMDEhoKFURPVEFfVU1fQ29tYmF0TG9n",
            "RGF0YRDUAxIeChlET1RBX1VNX0NvbWJhdExvZ0J1bGtEYXRhENYDEiMKHkRP",
            "VEFfVU1fQ3JlYXRlTGluZWFyUHJvamVjdGlsZRDXAxIkCh9ET1RBX1VNX0Rl",
            "c3Ryb3lMaW5lYXJQcm9qZWN0aWxlENgDEiUKIERPVEFfVU1fRG9kZ2VUcmFj",
            "a2luZ1Byb2plY3RpbGVzENkDEh0KGERPVEFfVU1fR2xvYmFsTGlnaHRDb2xv",
            "chDaAxIhChxET1RBX1VNX0dsb2JhbExpZ2h0RGlyZWN0aW9uENsDEhsKFkRP",
            "VEFfVU1fSW52YWxpZENvbW1hbmQQ3AMSGQoURE9UQV9VTV9Mb2NhdGlvblBp",
            "bmcQ3QMSFAoPRE9UQV9VTV9NYXBMaW5lEN4DEhwKF0RPVEFfVU1fTWluaUtp",
            "bGxDYW1JbmZvEN8DEh4KGURPVEFfVU1fTWluaW1hcERlYnVnUG9pbnQQ4AMS",
            "GQoURE9UQV9VTV9NaW5pbWFwRXZlbnQQ4QMSHQoYRE9UQV9VTV9OZXZlcm1v",
            "cmVSZXF1aWVtEOIDEhoKFURPVEFfVU1fT3ZlcmhlYWRFdmVudBDjAxIfChpE",
            "T1RBX1VNX1NldE5leHRBdXRvYnV5SXRlbRDkAxIbChZET1RBX1VNX1NoYXJl",
            "ZENvb2xkb3duEOUDEiEKHERPVEFfVU1fU3BlY3RhdG9yUGxheWVyQ2xpY2sQ",
            "5gMSHAoXRE9UQV9VTV9UdXRvcmlhbFRpcEluZm8Q5wMSFgoRRE9UQV9VTV9V",
            "bml0RXZlbnQQ6AMSHAoXRE9UQV9VTV9QYXJ0aWNsZU1hbmFnZXIQ6QMSFAoP",
            "RE9UQV9VTV9Cb3RDaGF0EOoDEhUKEERPVEFfVU1fSHVkRXJyb3IQ6wMSGgoV",
            "RE9UQV9VTV9JdGVtUHVyY2hhc2VkEOwDEhEKDERPVEFfVU1fUGluZxDtAxIW",
            "ChFET1RBX1VNX0l0ZW1Gb3VuZBDuAxIiCh1ET1RBX1VNX0NoYXJhY3RlclNw",
            "ZWFrQ29uY2VwdBDvAxIXChJET1RBX1VNX1N3YXBWZXJpZnkQ8AMSFgoRRE9U",
            "QV9VTV9Xb3JsZExpbmUQ8QMSGwoWRE9UQV9VTV9Ub3VybmFtZW50RHJvcBDy",
            "AxIWChFET1RBX1VNX0l0ZW1BbGVydBDzAxIbChZET1RBX1VNX0hhbGxvd2Vl",
            "bkRyb3BzEPQDEhYKEURPVEFfVU1fQ2hhdFdoZWVsEPUDEh0KGERPVEFfVU1f",
            "UmVjZWl2ZWRYbWFzR2lmdBD2AxIgChtET1RBX1VNX1VwZGF0ZVNoYXJlZENv",
            "bnRlbnQQ9wMSHwoaRE9UQV9VTV9UdXRvcmlhbFJlcXVlc3RFeHAQ+AMSIAob",
            "RE9UQV9VTV9UdXRvcmlhbFBpbmdNaW5pbWFwEPkDEiIKHURPVEFfVU1fR2Ft",
            "ZXJ1bGVzU3RhdGVDaGFuZ2VkEPoDEhcKEkRPVEFfVU1fU2hvd1N1cnZleRD7",
            "AxIZChRET1RBX1VNX1R1dG9yaWFsRmFkZRD8AxIdChhET1RBX1VNX0FkZFF1",
            "ZXN0TG9nRW50cnkQ/QMSGgoVRE9UQV9VTV9TZW5kU3RhdFBvcHVwEP4DEhsK",
            "FkRPVEFfVU1fVHV0b3JpYWxGaW5pc2gQ/wMSHAoXRE9UQV9VTV9TZW5kUm9z",
            "aGFuUG9wdXAQgAQSHwoaRE9UQV9VTV9TZW5kR2VuZXJpY1Rvb2xUaXAQgQQS",
            "GgoVRE9UQV9VTV9TZW5kRmluYWxHb2xkEIIEEhYKEURPVEFfVU1fQ3VzdG9t",
            "TXNnEIMEEhkKFERPVEFfVU1fQ29hY2hIVURQaW5nEIQEEh4KGURPVEFfVU1f",
            "Q2xpZW50TG9hZEdyaWROYXYQhQQSGgoVRE9UQV9VTV9URV9Qcm9qZWN0aWxl",
            "EIYEEh0KGERPVEFfVU1fVEVfUHJvamVjdGlsZUxvYxCHBBIfChpET1RBX1VN",
            "X1RFX0RvdGFCbG9vZEltcGFjdBCIBBIdChhET1RBX1VNX1RFX1VuaXRBbmlt",
            "YXRpb24QiQQSIAobRE9UQV9VTV9URV9Vbml0QW5pbWF0aW9uRW5kEIoEEhgK",
            "E0RPVEFfVU1fQWJpbGl0eVBpbmcQiwQSHQoYRE9UQV9VTV9TaG93R2VuZXJp",
            "Y1BvcHVwEIwEEhYKEURPVEFfVU1fVm90ZVN0YXJ0EI0EEhcKEkRPVEFfVU1f",
            "Vm90ZVVwZGF0ZRCOBBIUCg9ET1RBX1VNX1ZvdGVFbmQQjwQSGQoURE9UQV9V",
            "TV9Cb29zdGVyU3RhdGUQkAQSHgoZRE9UQV9VTV9XaWxsUHVyY2hhc2VBbGVy",
            "dBCRBBIkCh9ET1RBX1VNX1R1dG9yaWFsTWluaW1hcFBvc2l0aW9uEJIEEhYK",
            "EURPVEFfVU1fUGxheWVyTU1SEJMEEhkKFERPVEFfVU1fQWJpbGl0eVN0ZWFs",
            "EJQEEh8KGkRPVEFfVU1fQ291cmllcktpbGxlZEFsZXJ0EJUEEhsKFkRPVEFf",
            "VU1fRW5lbXlJdGVtQWxlcnQQlgQSHgoZRE9UQV9VTV9TdGF0c01hdGNoRGV0",
            "YWlscxCXBBIWChFET1RBX1VNX01pbmlUYXVudBCYBBIeChlET1RBX1VNX0J1",
            "eUJhY2tTdGF0ZUFsZXJ0EJkEEhkKFERPVEFfVU1fU3BlZWNoQnViYmxlEJoE",
            "EiAKG0RPVEFfVU1fQ3VzdG9tSGVhZGVyTWVzc2FnZRCbBBIaChVET1RBX1VN",
            "X1F1aWNrQnV5QWxlcnQQnAQSHQoYRE9UQV9VTV9TdGF0c0hlcm9EZXRhaWxz",
            "EJ0EEh0KGERPVEFfVU1fUHJlZGljdGlvblJlc3VsdBCeBBIaChVET1RBX1VN",
            "X01vZGlmaWVyQWxlcnQQnwQSGAoTRE9UQV9VTV9IUE1hbmFBbGVydBCgBBIX",
            "ChJET1RBX1VNX0dseXBoQWxlcnQQoQQSFgoRRE9UQV9VTV9CZWFzdENoYXQQ",
            "ogQSJgohRE9UQV9VTV9TcGVjdGF0b3JQbGF5ZXJVbml0T3JkZXJzEKMEEiQK",
            "H0RPVEFfVU1fQ3VzdG9tSHVkRWxlbWVudF9DcmVhdGUQpAQSJAofRE9UQV9V",
            "TV9DdXN0b21IdWRFbGVtZW50X01vZGlmeRClBBIlCiBET1RBX1VNX0N1c3Rv",
            "bUh1ZEVsZW1lbnRfRGVzdHJveRCmBBIcChdET1RBX1VNX0NvbXBlbmRpdW1T",
            "dGF0ZRCnBBIeChlET1RBX1VNX1Byb2plY3Rpb25BYmlsaXR5EKgEEhwKF0RP",
            "VEFfVU1fUHJvamVjdGlvbkV2ZW50EKkEEh4KGURPVEFfVU1fQ29tYmF0TG9n",
            "RGF0YUhMVFYQqgQSFAoPRE9UQV9VTV9YUEFsZXJ0EKsEEiAKG0RPVEFfVU1f",
            "VXBkYXRlUXVlc3RQcm9ncmVzcxCsBBIaChVET1RBX1VNX01hdGNoTWV0YWRh",
            "dGEQrQQSGQoURE9UQV9VTV9NYXRjaERldGFpbHMQrgQSGAoTRE9UQV9VTV9R",
            "dWVzdFN0YXR1cxCvBBIcChdET1RBX1VNX1N1Z2dlc3RIZXJvUGljaxCwBBIc",
            "ChdET1RBX1VNX1N1Z2dlc3RIZXJvUm9sZRCxBBIfChpET1RBX1VNX0tpbGxj",
            "YW1EYW1hZ2VUYWtlbhCyBBIeChlET1RBX1VNX1NlbGVjdFBlbmFsdHlHb2xk",
            "ELMEEhsKFkRPVEFfVU1fUm9sbERpY2VSZXN1bHQQtAQSGwoWRE9UQV9VTV9G",
            "bGlwQ29pblJlc3VsdBC1BBIjCh5ET1RBX1VNX1JlcXVlc3RJdGVtU3VnZ2Vz",
            "dGlvbnMQtgQSHwoaRE9UQV9VTV9UZWFtQ2FwdGFpbkNoYW5nZWQQtwQSJQog",
            "RE9UQV9VTV9TZW5kUm9zaGFuU3BlY3RhdG9yUGhhc2UQuAQSHgoZRE9UQV9V",
            "TV9DaGF0V2hlZWxDb29sZG93bhC5BBIhChxET1RBX1VNX0Rpc21pc3NBbGxT",
            "dGF0UG9wdXBzELoEEiEKHERPVEFfVU1fVEVfRGVzdHJveVByb2plY3RpbGUQ",
            "uwQSHgoZRE9UQV9VTV9IZXJvUmVsaWNQcm9ncmVzcxC8BBInCiJET1RBX1VN",
            "X0FiaWxpdHlEcmFmdFJlcXVlc3RBYmlsaXR5EL0EEhUKEERPVEFfVU1fSXRl",
            "bVNvbGQQvgQSGQoURE9UQV9VTV9EYW1hZ2VSZXBvcnQQvwQSGQoURE9UQV9V",
            "TV9TYWx1dGVQbGF5ZXIQwAQSFQoQRE9UQV9VTV9UaXBBbGVydBDBBBIdChhE",
            "T1RBX1VNX1JlcGxhY2VRdWVyeVVuaXQQwgQSHwoaRE9UQV9VTV9FbXB0eVRl",
            "bGVwb3J0QWxlcnQQwwRCEkgBgAEAqgIKUHJvdG8uRG90YWIGcHJvdG8z"));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { },
          new pbr::GeneratedClrTypeInfo(new[] {typeof(global::Proto.Dota.EDotaUserMessages), }, null));
    }
    #endregion

  }
  #region Enums
  public enum EDotaUserMessages {
    [pbr::OriginalName("DOTA_UM_UKNOWN")] DotaUmUknown = 0,
    [pbr::OriginalName("DOTA_UM_AddUnitToSelection")] DotaUmAddUnitToSelection = 464,
    [pbr::OriginalName("DOTA_UM_AIDebugLine")] DotaUmAidebugLine = 465,
    [pbr::OriginalName("DOTA_UM_ChatEvent")] DotaUmChatEvent = 466,
    [pbr::OriginalName("DOTA_UM_CombatHeroPositions")] DotaUmCombatHeroPositions = 467,
    [pbr::OriginalName("DOTA_UM_CombatLogData")] DotaUmCombatLogData = 468,
    [pbr::OriginalName("DOTA_UM_CombatLogBulkData")] DotaUmCombatLogBulkData = 470,
    [pbr::OriginalName("DOTA_UM_CreateLinearProjectile")] DotaUmCreateLinearProjectile = 471,
    [pbr::OriginalName("DOTA_UM_DestroyLinearProjectile")] DotaUmDestroyLinearProjectile = 472,
    [pbr::OriginalName("DOTA_UM_DodgeTrackingProjectiles")] DotaUmDodgeTrackingProjectiles = 473,
    [pbr::OriginalName("DOTA_UM_GlobalLightColor")] DotaUmGlobalLightColor = 474,
    [pbr::OriginalName("DOTA_UM_GlobalLightDirection")] DotaUmGlobalLightDirection = 475,
    [pbr::OriginalName("DOTA_UM_InvalidCommand")] DotaUmInvalidCommand = 476,
    [pbr::OriginalName("DOTA_UM_LocationPing")] DotaUmLocationPing = 477,
    [pbr::OriginalName("DOTA_UM_MapLine")] DotaUmMapLine = 478,
    [pbr::OriginalName("DOTA_UM_MiniKillCamInfo")] DotaUmMiniKillCamInfo = 479,
    [pbr::OriginalName("DOTA_UM_MinimapDebugPoint")] DotaUmMinimapDebugPoint = 480,
    [pbr::OriginalName("DOTA_UM_MinimapEvent")] DotaUmMinimapEvent = 481,
    [pbr::OriginalName("DOTA_UM_NevermoreRequiem")] DotaUmNevermoreRequiem = 482,
    [pbr::OriginalName("DOTA_UM_OverheadEvent")] DotaUmOverheadEvent = 483,
    [pbr::OriginalName("DOTA_UM_SetNextAutobuyItem")] DotaUmSetNextAutobuyItem = 484,
    [pbr::OriginalName("DOTA_UM_SharedCooldown")] DotaUmSharedCooldown = 485,
    [pbr::OriginalName("DOTA_UM_SpectatorPlayerClick")] DotaUmSpectatorPlayerClick = 486,
    [pbr::OriginalName("DOTA_UM_TutorialTipInfo")] DotaUmTutorialTipInfo = 487,
    [pbr::OriginalName("DOTA_UM_UnitEvent")] DotaUmUnitEvent = 488,
    [pbr::OriginalName("DOTA_UM_ParticleManager")] DotaUmParticleManager = 489,
    [pbr::OriginalName("DOTA_UM_BotChat")] DotaUmBotChat = 490,
    [pbr::OriginalName("DOTA_UM_HudError")] DotaUmHudError = 491,
    [pbr::OriginalName("DOTA_UM_ItemPurchased")] DotaUmItemPurchased = 492,
    [pbr::OriginalName("DOTA_UM_Ping")] DotaUmPing = 493,
    [pbr::OriginalName("DOTA_UM_ItemFound")] DotaUmItemFound = 494,
    [pbr::OriginalName("DOTA_UM_CharacterSpeakConcept")] DotaUmCharacterSpeakConcept = 495,
    [pbr::OriginalName("DOTA_UM_SwapVerify")] DotaUmSwapVerify = 496,
    [pbr::OriginalName("DOTA_UM_WorldLine")] DotaUmWorldLine = 497,
    [pbr::OriginalName("DOTA_UM_TournamentDrop")] DotaUmTournamentDrop = 498,
    [pbr::OriginalName("DOTA_UM_ItemAlert")] DotaUmItemAlert = 499,
    [pbr::OriginalName("DOTA_UM_HalloweenDrops")] DotaUmHalloweenDrops = 500,
    [pbr::OriginalName("DOTA_UM_ChatWheel")] DotaUmChatWheel = 501,
    [pbr::OriginalName("DOTA_UM_ReceivedXmasGift")] DotaUmReceivedXmasGift = 502,
    [pbr::OriginalName("DOTA_UM_UpdateSharedContent")] DotaUmUpdateSharedContent = 503,
    [pbr::OriginalName("DOTA_UM_TutorialRequestExp")] DotaUmTutorialRequestExp = 504,
    [pbr::OriginalName("DOTA_UM_TutorialPingMinimap")] DotaUmTutorialPingMinimap = 505,
    [pbr::OriginalName("DOTA_UM_GamerulesStateChanged")] DotaUmGamerulesStateChanged = 506,
    [pbr::OriginalName("DOTA_UM_ShowSurvey")] DotaUmShowSurvey = 507,
    [pbr::OriginalName("DOTA_UM_TutorialFade")] DotaUmTutorialFade = 508,
    [pbr::OriginalName("DOTA_UM_AddQuestLogEntry")] DotaUmAddQuestLogEntry = 509,
    [pbr::OriginalName("DOTA_UM_SendStatPopup")] DotaUmSendStatPopup = 510,
    [pbr::OriginalName("DOTA_UM_TutorialFinish")] DotaUmTutorialFinish = 511,
    [pbr::OriginalName("DOTA_UM_SendRoshanPopup")] DotaUmSendRoshanPopup = 512,
    [pbr::OriginalName("DOTA_UM_SendGenericToolTip")] DotaUmSendGenericToolTip = 513,
    [pbr::OriginalName("DOTA_UM_SendFinalGold")] DotaUmSendFinalGold = 514,
    [pbr::OriginalName("DOTA_UM_CustomMsg")] DotaUmCustomMsg = 515,
    [pbr::OriginalName("DOTA_UM_CoachHUDPing")] DotaUmCoachHudping = 516,
    [pbr::OriginalName("DOTA_UM_ClientLoadGridNav")] DotaUmClientLoadGridNav = 517,
    [pbr::OriginalName("DOTA_UM_TE_Projectile")] DotaUmTeProjectile = 518,
    [pbr::OriginalName("DOTA_UM_TE_ProjectileLoc")] DotaUmTeProjectileLoc = 519,
    [pbr::OriginalName("DOTA_UM_TE_DotaBloodImpact")] DotaUmTeDotaBloodImpact = 520,
    [pbr::OriginalName("DOTA_UM_TE_UnitAnimation")] DotaUmTeUnitAnimation = 521,
    [pbr::OriginalName("DOTA_UM_TE_UnitAnimationEnd")] DotaUmTeUnitAnimationEnd = 522,
    [pbr::OriginalName("DOTA_UM_AbilityPing")] DotaUmAbilityPing = 523,
    [pbr::OriginalName("DOTA_UM_ShowGenericPopup")] DotaUmShowGenericPopup = 524,
    [pbr::OriginalName("DOTA_UM_VoteStart")] DotaUmVoteStart = 525,
    [pbr::OriginalName("DOTA_UM_VoteUpdate")] DotaUmVoteUpdate = 526,
    [pbr::OriginalName("DOTA_UM_VoteEnd")] DotaUmVoteEnd = 527,
    [pbr::OriginalName("DOTA_UM_BoosterState")] DotaUmBoosterState = 528,
    [pbr::OriginalName("DOTA_UM_WillPurchaseAlert")] DotaUmWillPurchaseAlert = 529,
    [pbr::OriginalName("DOTA_UM_TutorialMinimapPosition")] DotaUmTutorialMinimapPosition = 530,
    [pbr::OriginalName("DOTA_UM_PlayerMMR")] DotaUmPlayerMmr = 531,
    [pbr::OriginalName("DOTA_UM_AbilitySteal")] DotaUmAbilitySteal = 532,
    [pbr::OriginalName("DOTA_UM_CourierKilledAlert")] DotaUmCourierKilledAlert = 533,
    [pbr::OriginalName("DOTA_UM_EnemyItemAlert")] DotaUmEnemyItemAlert = 534,
    [pbr::OriginalName("DOTA_UM_StatsMatchDetails")] DotaUmStatsMatchDetails = 535,
    [pbr::OriginalName("DOTA_UM_MiniTaunt")] DotaUmMiniTaunt = 536,
    [pbr::OriginalName("DOTA_UM_BuyBackStateAlert")] DotaUmBuyBackStateAlert = 537,
    [pbr::OriginalName("DOTA_UM_SpeechBubble")] DotaUmSpeechBubble = 538,
    [pbr::OriginalName("DOTA_UM_CustomHeaderMessage")] DotaUmCustomHeaderMessage = 539,
    [pbr::OriginalName("DOTA_UM_QuickBuyAlert")] DotaUmQuickBuyAlert = 540,
    [pbr::OriginalName("DOTA_UM_StatsHeroDetails")] DotaUmStatsHeroDetails = 541,
    [pbr::OriginalName("DOTA_UM_PredictionResult")] DotaUmPredictionResult = 542,
    [pbr::OriginalName("DOTA_UM_ModifierAlert")] DotaUmModifierAlert = 543,
    [pbr::OriginalName("DOTA_UM_HPManaAlert")] DotaUmHpmanaAlert = 544,
    [pbr::OriginalName("DOTA_UM_GlyphAlert")] DotaUmGlyphAlert = 545,
    [pbr::OriginalName("DOTA_UM_BeastChat")] DotaUmBeastChat = 546,
    [pbr::OriginalName("DOTA_UM_SpectatorPlayerUnitOrders")] DotaUmSpectatorPlayerUnitOrders = 547,
    [pbr::OriginalName("DOTA_UM_CustomHudElement_Create")] DotaUmCustomHudElementCreate = 548,
    [pbr::OriginalName("DOTA_UM_CustomHudElement_Modify")] DotaUmCustomHudElementModify = 549,
    [pbr::OriginalName("DOTA_UM_CustomHudElement_Destroy")] DotaUmCustomHudElementDestroy = 550,
    [pbr::OriginalName("DOTA_UM_CompendiumState")] DotaUmCompendiumState = 551,
    [pbr::OriginalName("DOTA_UM_ProjectionAbility")] DotaUmProjectionAbility = 552,
    [pbr::OriginalName("DOTA_UM_ProjectionEvent")] DotaUmProjectionEvent = 553,
    [pbr::OriginalName("DOTA_UM_CombatLogDataHLTV")] DotaUmCombatLogDataHltv = 554,
    [pbr::OriginalName("DOTA_UM_XPAlert")] DotaUmXpalert = 555,
    [pbr::OriginalName("DOTA_UM_UpdateQuestProgress")] DotaUmUpdateQuestProgress = 556,
    [pbr::OriginalName("DOTA_UM_MatchMetadata")] DotaUmMatchMetadata = 557,
    [pbr::OriginalName("DOTA_UM_MatchDetails")] DotaUmMatchDetails = 558,
    [pbr::OriginalName("DOTA_UM_QuestStatus")] DotaUmQuestStatus = 559,
    [pbr::OriginalName("DOTA_UM_SuggestHeroPick")] DotaUmSuggestHeroPick = 560,
    [pbr::OriginalName("DOTA_UM_SuggestHeroRole")] DotaUmSuggestHeroRole = 561,
    [pbr::OriginalName("DOTA_UM_KillcamDamageTaken")] DotaUmKillcamDamageTaken = 562,
    [pbr::OriginalName("DOTA_UM_SelectPenaltyGold")] DotaUmSelectPenaltyGold = 563,
    [pbr::OriginalName("DOTA_UM_RollDiceResult")] DotaUmRollDiceResult = 564,
    [pbr::OriginalName("DOTA_UM_FlipCoinResult")] DotaUmFlipCoinResult = 565,
    [pbr::OriginalName("DOTA_UM_RequestItemSuggestions")] DotaUmRequestItemSuggestions = 566,
    [pbr::OriginalName("DOTA_UM_TeamCaptainChanged")] DotaUmTeamCaptainChanged = 567,
    [pbr::OriginalName("DOTA_UM_SendRoshanSpectatorPhase")] DotaUmSendRoshanSpectatorPhase = 568,
    [pbr::OriginalName("DOTA_UM_ChatWheelCooldown")] DotaUmChatWheelCooldown = 569,
    [pbr::OriginalName("DOTA_UM_DismissAllStatPopups")] DotaUmDismissAllStatPopups = 570,
    [pbr::OriginalName("DOTA_UM_TE_DestroyProjectile")] DotaUmTeDestroyProjectile = 571,
    [pbr::OriginalName("DOTA_UM_HeroRelicProgress")] DotaUmHeroRelicProgress = 572,
    [pbr::OriginalName("DOTA_UM_AbilityDraftRequestAbility")] DotaUmAbilityDraftRequestAbility = 573,
    [pbr::OriginalName("DOTA_UM_ItemSold")] DotaUmItemSold = 574,
    [pbr::OriginalName("DOTA_UM_DamageReport")] DotaUmDamageReport = 575,
    [pbr::OriginalName("DOTA_UM_SalutePlayer")] DotaUmSalutePlayer = 576,
    [pbr::OriginalName("DOTA_UM_TipAlert")] DotaUmTipAlert = 577,
    [pbr::OriginalName("DOTA_UM_ReplaceQueryUnit")] DotaUmReplaceQueryUnit = 578,
    [pbr::OriginalName("DOTA_UM_EmptyTeleportAlert")] DotaUmEmptyTeleportAlert = 579,
  }

  #endregion

}

#endregion Designer generated code