using System;
using Oracle;
using Oracle.Events;

namespace Examples
{
    public class Parser
    {
        public void Runner()
        {
            var parser = new ReplayParser("s2.dem");

            parser.ConfigureObserver(b => b
                .OnHeader(HeaderParse)
                .OnDemoFileInfo(FileInfoParsed)
                .OnLocationPing(LocationPingParsed)
                .OnChat(ChatParsed)
                .OnSayText(SayTextParsed)
                .OnDotaMatch(OnDataMatchParsed)
                .OnChatWheel(ChatWheelParsed)
                .OnCombatLog(CombatLogParsed)
            );

            parser.ParseDemReplay();
        }

        private void EntityParsed(Args<object> obj)
        {
            var e = obj;
            Console.WriteLine(e.Item);
        }

        private void TipAlertParsed(Args<object> obj)
        {
            var tip = obj;
            Console.WriteLine(tip.Item);
        }

        private void LocationPingParsed(Args<object> obj)
        {
            var ping = obj;
            Console.WriteLine(ping.Item);
        }

        private void PartialManagerParsed(Args<object> obj)
        {
            var partialManager = obj;
            Console.WriteLine(partialManager.Item);
        }

        private void OnDataMatchParsed(Args<object> obj)
        {
            var dotaMatch = obj;
            Console.WriteLine(dotaMatch.Item);
        }

        private void SayTextParsed(Args<object> obj)
        {
            var text = obj;
            Console.WriteLine(text.Item);
        }

        private void CombatLogParsed(Args<object> args)
        {
            var log = args;
            Console.WriteLine(log.Item);
        }

        private void ChatWheelParsed(Args<object> args)
        {
            var chatWheel = args;
            Console.WriteLine(chatWheel.Item);
        }

        private void ChatParsed(Args<object> args)
        {
            var chat = args;
            Console.WriteLine(chat.Item);
        }

        private void FileInfoParsed(Args<object> args)
        {
            Console.WriteLine(args.Item);
        }

        public void HeaderParse(Args<object> args)
        {
            Console.WriteLine(args.Item);
        }
    }
}