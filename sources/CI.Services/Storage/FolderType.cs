namespace Savchin.Services.Storage
{
    /// <summary>
    /// Implements types of folders where application settings are stored
    /// </summary>
    public enum FolderType
    {
        Root,
        ScheduledPackage,
        Settings,
        Workspaces,
        WorkspaceBackups,
        Alerts,
        ChartHistory,
        ChartTemplates,
        Logs,
        CodeRepository,
        CodePublish,
        Data,
        StartupBackups,
        AssemblyCache,
        CustomIndicators,
        CustomStrategies,
        Common,
        Cache,
        Diagnostics
    }
}
