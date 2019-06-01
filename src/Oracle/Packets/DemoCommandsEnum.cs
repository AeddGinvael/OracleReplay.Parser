namespace Oracle.Packets
{
    public enum DemoCommandsEnum: sbyte
    {
        DemError = -1,
        DemStop = 0,
        DemFileHeader = 1,
        DemFileInfo = 2,
        DemSyncTick = 3,
        DemSendTables = 4,
        DemClassInfo = 5,
        DemStringTables = 6,
        DemPacket = 7,
        DemSignonPacket = 8,
        DemConsoleCmd = 9,
        DemCustomData = 10,
        DemCustomDataCallbacks = 11,
        DemUserCmd = 12,
        DemFullPacket = 13,
        DemSaveGame = 14,
        DemSpawnGroups = 15,
        DemMax = 16
    }
}