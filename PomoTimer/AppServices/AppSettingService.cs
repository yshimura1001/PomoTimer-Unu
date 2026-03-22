namespace PomoTimer.AppServices
{
    public class AppSettingService
    {
        public string appName { get; private set; } = "PomoTimer";
        public string codeName { get; private set; } = "Unu";
        public string updateDateStr { get; private set; } = "260314";
        public string revisionStr { get; private set; } = "1";
        public string targetStr { get; private set; } = ".NET 9";
        public AppSettingService() { }

        public string GetAppName() => appName;
        public string GetAppTitleShort() => $"{GetAppName()} {codeName}";
        public string GetAppTitleLong() => $"{GetAppTitleShort()} {updateDateStr}.{revisionStr} [{targetStr}]";
        public string GetUpdateDateStr() => updateDateStr;
        public string GetRevisionStr() => revisionStr;
    }
}
