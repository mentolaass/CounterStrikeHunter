namespace CounterStrikeHunter.Core.Service.Queue.Work
{
    public class Tasks
    {
        public static readonly TaskMetaInfo BACKGROUND_TASK_META = new TaskMetaInfo(
            App.GetLocalizedString("AsyncBackgroundAppTask_Title"),
            App.GetLocalizedString("AsyncBackgroundAppTask_Description"),
            new Flag[] { Flag.APP_CORE, Flag.NETWORK });

        public static readonly TaskMetaInfo STEAM_ACCOUNT_PARSE_TASK_META = new TaskMetaInfo(
            App.GetLocalizedString("SteamAccountParseTask_Title"),
            App.GetLocalizedString("SteamAccountParseTask_Description"),
            new Flag[] { Flag.NETWORK, Flag.INSPECTION });

        public static readonly TaskMetaInfo BROWSER_READ_DATA_TASK = new TaskMetaInfo(
            App.GetLocalizedString("BrowserReadDataTask_Title"),
            App.GetLocalizedString("BrowserReadDataTask_Description"),
            new Flag[] { Flag.LOCAL, Flag.INSPECTION });

        public static readonly TaskMetaInfo REGISTRY_READ_DATA_TASK = new TaskMetaInfo(
            App.GetLocalizedString("RegistryReadDataTask_Title"),
            App.GetLocalizedString("RegistryReadDataTask_Description"),
            new Flag[] { Flag.LOCAL, Flag.SYSTEM, Flag.INSPECTION });

        public static readonly TaskMetaInfo LOCAL_FILES_READ_DATA_TASK = new TaskMetaInfo(
            App.GetLocalizedString("LocalFilesReadDataTask_Title"),
            App.GetLocalizedString("LocalFilesDataTask_Description"),
            new Flag[] { Flag.LOCAL, Flag.SYSTEM, Flag.INSPECTION });

        public static readonly TaskMetaInfo TEST_TASK = new TaskMetaInfo(
            "This is Test Task",
            "Lorem ipsum odor amet, consectetuer adipiscing elit. Curae fusce consectetur faucibus lobortis malesuada tempor. Aliquet diam bibendum sapien, nibh sapien faucibus augue facilisis.",
            new Flag[] { Flag.NETWORK, Flag.INSPECTION, Flag.APP_CORE, Flag.LOCAL, Flag.SYSTEM });
    }
}
