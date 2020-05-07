namespace Savchin.Services.Execution
{
    /// <summary>
    /// Defines interface for entities which are executed asynchrously
    /// </summary>
    public interface IRunnable 
    {
        /// <summary>
        /// Starts this instance. Invokes in non UI threads
        /// </summary>
        void Start();

        /// <summary>
        /// Stops this instance.
        /// </summary>
        void Stop();
    }
}
