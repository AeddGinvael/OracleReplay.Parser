using System;
using Google.Protobuf;
using Oracle.Packets.Entry;
using Oracle.Packets.Model;
using Proto.Dota;

namespace Oracle.Events
{

    public abstract class Args<T> : EventArgs
    {
        public Args(T item)
        {
            Item = item;
        }
        public T Item { get; protected set; }
    }

    public class ObjectArgs : Args<object>
    {
        public ObjectArgs(object item) : base(item)
        {
        }
    }
    public class CDemoFileHeaderArgs : Args<CDemoFileHeader>
    {
        public CDemoFileHeaderArgs(CDemoFileHeader item) : base(item)
        {
        }
    }

    public class CDemoFileInfoArgs : EventArgs
    {
        public CDemoFileInfo FileInfo { get; set; }
    }

    public class TipAlertArgs : EventArgs
    {
        public CDOTAUserMsg_TipAlert TipAlert { get; set;}
    }

    public class TeamCaptainChangedArgs : EventArgs
    {
        public CDOTAUserMessage_TeamCaptainChanged TeamCaptainChanged { get; set;}
    }

    public class DotaMatchArgs : EventArgs
    {
        public CMsgDOTAMatch DotaMatch { get; set;}
    }

    public class ChatWheelArgs : EventArgs
    {
        public CDOTAUserMsg_ChatWheel ChatWheel { get; set;}
    }

    public class ItemPurchasedArgs : EventArgs
    {
        public CDOTAUserMsg_ItemPurchased ItemPurchased { get; set;}
    }

    public class LocationPingArgs : EventArgs
    {
        public CDOTAUserMsg_LocationPing LocationPing { get; set;}
    }

    public class ChatEventArgs : EventArgs
    {
        public CDOTAUserMsg_ChatEvent ChatEvent { get; set;}
    }

    public class ParticleManagerArgs : EventArgs
    {
        public CDOTAUserMsg_ParticleManager ParticleManager { get; set;}
    }

    public class SendAudioArgs : EventArgs
    {
        public CUserMessageSendAudio SendAudio { get; set;}
    }

    public class TextMsgArgs : EventArgs
    {
        public CUserMessageTextMsg TextMsg { get; set;}
    }

    public class SayTextArgs : EventArgs
    {
        public CUserMessageSayText2 SayText { get; set;}
    }

    public class CombatLogEntryArgs : EventArgs
    {
        public CombatLogEntry CombatEntry { get; set;}
    }


    public class GameEventArgs : EventArgs
    {
        public GameEventModel GameEvent { get; set;}
    }

    public class EntityArgs : EventArgs
    {
        public Entity GameEntity { get; set; }
    }
}