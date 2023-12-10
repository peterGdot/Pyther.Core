namespace Pyther.Core.Logging
{
    /// <summary>
    /// Enumeration of Log Levels.
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// Temporary logging should only be used during development and should completely be removed on final releases.
        /// This kind of logging will never be skipped.
        /// </summary>
        Temp = 0,
        /// <summary>
        /// Debug logging can be part of the final release, but should only be enabled when required to find problems.
        /// </summary>
        Debug = 1,
        /// <summary>
        /// Process logging should be used to notify about important processes/steps.
        /// </summary>
        Process = 2,
        /// <summary>
        /// Infos should be enabled to display optional, relevant informations.
        /// </summary>
        Info = 3,
        /// <summary>
        /// Warnings should inform that something went wrong but can be handled by the system.
        /// </summary>
        Warning = 4,
        /// <summary>
        /// Errors should inform about critical problems that can't be resolved by the system. 
        /// </summary>
        Error = 5
    }
}