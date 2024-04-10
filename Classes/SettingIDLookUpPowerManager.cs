using System;
using System.Collections.Generic;

namespace Handheld_Hardware_Tools.Classes
{
    public enum ErrorCode : uint
    {
        SUCCESS = 0x000,
        FILE_NOT_FOUND = 0x002,
        ERROR_INVALID_PARAMETER = 0x057,
        ERROR_ALREADY_EXISTS = 0x0B7,
        MORE_DATA = 0x0EA,
        NO_MORE_ITEMS = 0x103
    }

    public enum AccessFlags : uint
    {
        ACCESS_SCHEME = 16,
        ACCESS_SUBGROUP = 17,
        ACCESS_INDIVIDUAL_SETTING = 18
    }

    public enum SettingSubgroup
    {
        NO_SUBGROUP,
        DISK_SUBGROUP,
        SYSTEM_BUTTON_SUBGROUP,
        PROCESSOR_SETTINGS_SUBGROUP,
        VIDEO_SUBGROUP,
        BATTERY_SUBGROUP,
        SLEEP_SUBGROUP,
        PCIEXPRESS_SETTINGS_SUBGROUP
    }

    public enum Setting
    {
        BATACTIONCRIT,
        BATACTIONLOW,
        BATFLAGSLOW,
        BATLEVELCRIT,
        BATLEVELLOW,
        LIDACTION,
        PBUTTONACTION,
        SBUTTONACTION,
        UIBUTTON_ACTION,
        DISKIDLE,
        ASPM,
        PROCFREQMAX,
        PROCTHROTTLEMAX,
        PROCTHROTTLEMIN,
        SYSCOOLPOL,
        HIBERNATEIDLE,
        HYBRIDSLEEP,
        RTCWAKE,
        STANDBYIDLE,
        ADAPTBRIGHT,
        VIDEOIDLE
    }

    public static class SettingIdLookup
    {
        public static Dictionary<SettingSubgroup, Guid> SettingSubgroupGuids = new Dictionary<SettingSubgroup, Guid>
        {
            { SettingSubgroup.NO_SUBGROUP,                  new Guid("fea3413e-7e05-4911-9a71-700331f1c294") },
            { SettingSubgroup.DISK_SUBGROUP,                new Guid("0012ee47-9041-4b5d-9b77-535fba8b1442") },
            { SettingSubgroup.SYSTEM_BUTTON_SUBGROUP,       new Guid("4f971e89-eebd-4455-a8de-9e59040e7347") },
            { SettingSubgroup.PROCESSOR_SETTINGS_SUBGROUP,  new Guid("54533251-82be-4824-96c1-47b60b740d00") },
            { SettingSubgroup.VIDEO_SUBGROUP,               new Guid("7516b95f-f776-4464-8c53-06167f40cc99") },
            { SettingSubgroup.BATTERY_SUBGROUP,             new Guid("e73a048d-bf27-4f12-9731-8b2076e8891f") },
            { SettingSubgroup.SLEEP_SUBGROUP,               new Guid("238C9FA8-0AAD-41ED-83F4-97BE242C8F20") },
            { SettingSubgroup.PCIEXPRESS_SETTINGS_SUBGROUP, new Guid("501a4d13-42af-4429-9fd1-a8218c268e20") }
        };

        public static Dictionary<Setting, Guid> SettingGuids = new Dictionary<Setting, Guid>
        {
            { Setting.BATACTIONCRIT,    new Guid("637ea02f-bbcb-4015-8e2c-a1c7b9c0b546") },
            { Setting.BATACTIONLOW,     new Guid("d8742dcb-3e6a-4b3c-b3fe-374623cdcf06") },
            { Setting.BATFLAGSLOW,      new Guid("bcded951-187b-4d05-bccc-f7e51960c258") },
            { Setting.BATLEVELCRIT,     new Guid("9a66d8d7-4ff7-4ef9-b5a2-5a326ca2a469") },
            { Setting.BATLEVELLOW,      new Guid("8183ba9a-e910-48da-8769-14ae6dc1170a") },
            { Setting.LIDACTION,        new Guid("5ca83367-6e45-459f-a27b-476b1d01c936") },
            { Setting.PBUTTONACTION,    new Guid("7648efa3-dd9c-4e3e-b566-50f929386280") },
            { Setting.SBUTTONACTION,    new Guid("96996bc0-ad50-47ec-923b-6f41874dd9eb") },
            { Setting.UIBUTTON_ACTION,  new Guid("a7066653-8d6c-40a8-910e-a1f54b84c7e5") },
            { Setting.DISKIDLE,         new Guid("6738e2c4-e8a5-4a42-b16a-e040e769756e") },
            { Setting.ASPM,             new Guid("ee12f906-d277-404b-b6da-e5fa1a576df5") },
            { Setting.PROCFREQMAX,      new Guid("75b0ae3f-bce0-45a7-8c89-c9611c25e100") },
            { Setting.PROCTHROTTLEMAX,  new Guid("bc5038f7-23e0-4960-96da-33abaf5935ec") },
            { Setting.PROCTHROTTLEMIN,  new Guid("893dee8e-2bef-41e0-89c6-b55d0929964c") },
            { Setting.SYSCOOLPOL,       new Guid("94d3a615-a899-4ac5-ae2b-e4d8f634367f") },
            { Setting.HIBERNATEIDLE,    new Guid("9d7815a6-7ee4-497e-8888-515a05f02364") },
            { Setting.HYBRIDSLEEP,      new Guid("94ac6d29-73ce-41a6-809f-6363ba21b47e") },
            { Setting.RTCWAKE,          new Guid("bd3b718a-0680-4d9d-8ab2-e1d2b4ac806d") },
            { Setting.STANDBYIDLE,      new Guid("29f6c1db-86da-48c5-9fdb-f2b67b1f44da") },
            { Setting.ADAPTBRIGHT,      new Guid("fbd9aa66-9553-4097-ba44-ed6e9d65eab8") },
            { Setting.VIDEOIDLE,        new Guid("3c0bc021-c8a8-4e07-a973-6b14cbcb2b7e") }
        };
    }
}