using System;

namespace Savchin.Services.Localization
{
    public interface ILocalizationDepended
    {
        void OnLocalizationChangedEvent(EventArgs obj);
    }
}