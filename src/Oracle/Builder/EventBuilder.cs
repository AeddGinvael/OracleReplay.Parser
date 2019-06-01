using System;
using System.Collections.Generic;
using Oracle.Events;
using Oracle.Packets.Entry;
using Oracle.Packets.Model;
using Proto.Dota;

namespace Oracle.Utilities
{
    public class EventBuilder
    {
        private Dictionary<Type, Action<Args<object>>> _events { get; set;} = new Dictionary<Type, Action<Args<object>>>();
        public Observer Build()
        {
            return new Observer(_events);
        }

        public EventBuilder OnHeader(Action<Args<object>> e)
        {
            _events[typeof(CDemoFileHeader)] = e;
            return this;
        }

        /// <summary>
        /// CDemoFileInfo
        /// </summary>
        /// <returns></returns>
        public EventBuilder OnDemoFileInfo(Action<Args<object>> e)
        {
            _events[typeof(CDemoFileInfo)] = e;
            return this;
        }

        /// <summary>
        /// CMsgDOTAMatch
        /// </summary>
        /// <returns></returns>
        public EventBuilder OnDotaMatch(Action<Args<object>> e)
        {
            _events[typeof(CMsgDOTAMatch)] = e;
            return this;
        }

        /// <summary>
        /// CDOTAUserMsg_SpectratorPlayerUnitOrder
        /// </summary>
        /// <returns></returns>
        public EventBuilder OnSpectratorPlayerUnitOrder(Action<Args<object>> e)
        {
            _events[typeof(CDOTAUserMsg_SpectatorPlayerUnitOrders)] = e;
            return this;
        }

        /// <summary>
        /// CDOTAUserMsg_LocationPing
        /// </summary>
        /// <returns></returns>
        public EventBuilder OnLocationPing(Action<Args<object>> e)
        {
            _events[typeof(CDOTAUserMsg_LocationPing)] = e;
            return this;
        }

        /// <summary>
        /// CDOTAUserMsg_ChatEvent
        /// </summary>
        /// <returns></returns>
        public EventBuilder OnChat(Action<Args<object>> e)
        {
            _events[typeof(CDOTAUserMsg_ChatEvent)] = e;
            return this;
        }

        /// <summary>
        /// CDOTAUserMsg_ChatWheel
        /// </summary>
        /// <returns></returns>
        public EventBuilder OnChatWheel(Action<Args<object>> e)
        {
            _events[typeof(CDOTAUserMsg_ChatWheel)] = e;
            return this;
        }

        /// <summary>
        /// CUserMessageSayText2
        /// </summary>
        /// <returns></returns>
        public EventBuilder OnSayText(Action<Args<object>> e)
        {
            _events[typeof(CUserMessageSayText2)] = e;
            return this;
        }



        /// <summary>
        /// CombatLogEntry
        /// </summary>
        /// <returns></returns>
        public EventBuilder OnCombatLog(Action<Args<object>> e)
        {
            _events[typeof(CombatLogEntry)] = e;
            return this;
        }

        /// <summary>
        /// OnEntityCreated
        /// </summary>
        /// <returns></returns>
        public EventBuilder OnEntityEntered(Action<Args<object>> e)
        {
            _events[typeof(Entity)] = e;
            return this;
        }

        public EventBuilder OnTipAlert(Action<Args<object>> e)
        {
            _events[typeof(CDOTAUserMsg_TipAlert)] = e;
            return this;
        }

        public EventBuilder OnTeamCaptainChanged(Action<Args<object>> e)
        {
            _events[typeof(CDOTAUserMessage_TeamCaptainChanged)] = e;
            return this;
        }

        public EventBuilder OnItemPurchased(Action<Args<object>> e)
        {
            _events[typeof(CDOTAUserMsg_ItemPurchased)] = e;
            return this;
        }

        public EventBuilder OnPartialManager(Action<Args<object>> e)
        {
            _events[typeof(CDOTAUserMsg_ParticleManager)] = e;
            return this;
        }

        public EventBuilder OnSendAudio(Action<Args<object>> e)
        {
            _events[typeof(CUserMessageSendAudio)] = e;
            return this;
        }


        public EventBuilder OnTextMsg(Action<Args<object>> e)
        {
            _events[typeof(CUserMessageTextMsg)] = e;
            return this;
        }
    }
}