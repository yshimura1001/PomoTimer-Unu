namespace PomoTimer.Services
{
    public class AppSettingService
    {
        public string appName { get; private set; } = "PomoTimer";
        public string codeName { get; private set; } = "Unu";
        public string updateDateStr { get; private set; } = "260306";
        public string revisionStr { get; private set; } = "1";
        public AppSettingService() { }

        public string GetAppName() => appName;
        public string GetAppTitleShort() => $"{GetAppName()} {codeName}";
        public string GetAppTitleLong() => $"{GetAppTitleShort()} {updateDateStr}.{revisionStr}";
        public string GetUpdateDateStr() => updateDateStr;
        public string GetRevisionStr() => revisionStr;
    }
}
