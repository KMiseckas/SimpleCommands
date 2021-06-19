namespace SimpleCommands.Runtime.Base
{
    /// <summary>
    /// The build target for which the command should be available to use within.<br></br><br></br>
    /// <b>DEVELOPMENT_ONLY</b> - Command should only be usable for development builds.<br></br>
    /// <b>PRODUCTION_ONLY</b> - Command should only be usable for production/delivery builds.<br></br>
    /// <b>PRODUCTION_AND_DEVELOPMENT</b> - Command should be usable both within development and production builds.
    /// </summary>
    public enum BuildTarget
    {
        DEVELOPMENT_ONLY,
        PRODUCTION_ONLY,
        PRODUCTION_AND_DEVELOPMENT
    }
}