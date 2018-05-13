namespace Savchin.Services
{
    public interface IColorProvider
    {
        /// <summary>
        /// Gets the color of the price up.
        /// </summary>
        /// <value>
        /// The color of the price up.
        /// </value>
        string PriceUpColor { get; }

        /// <summary>
        /// Gets the color of the price down.
        /// </summary>
        /// <value>
        /// The color of the price down.
        /// </value>
        string PriceDownColor { get; }

    }
}
