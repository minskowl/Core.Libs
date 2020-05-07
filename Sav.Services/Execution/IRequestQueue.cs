namespace Savchin.Services.Execution
{
    public enum RequestPriority
    {
        Low = 0,
        Normal = 1,
        High = 2
    }
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix")]
    public interface ITaskQueue : IRequestQueue
    {

    }


    /// <summary>
    /// Defines interface for message queue
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix")]
    public interface IRequestQueue
    {
        void Clear();
        /// <summary>
        /// Adds the specified request to the end of queue
        /// </summary>
        /// <param name="request">The request to add</param>
        /// <param name="priority">The priority.</param>
        void Enqueue(IRequest request, RequestPriority priority = RequestPriority.Normal);

        /// <summary>
        /// Removes request from queue and returns to caller
        /// </summary>
        /// <returns>Request instance or null</returns>
        IRequest Dequeue();

        /// <summary>
        /// Wakeup.
        /// </summary>
        void Wakeup();
    }
}
