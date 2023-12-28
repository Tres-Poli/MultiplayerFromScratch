using UnityEngine;

namespace UI.Infrastructure
{
    public interface IUiFactory
    {
        ScreenType ScreenType { get; }
        IUiController AddUiScreen(RectTransform canvas, GameObject prefab);
    }
}