namespace GoToApps.Debug.Data
{
    [System.Serializable]
    public struct DebugRecord
    {
        public string color;

        public string moduleName;
        public string className;
        public string methodName;
        public string message;

        public RecordType type;
    }
}