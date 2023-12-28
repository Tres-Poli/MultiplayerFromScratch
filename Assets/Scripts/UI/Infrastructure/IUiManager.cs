using Cysharp.Threading.Tasks;
using UnityEngine;

namespace UI.Infrastructure
{
    public interface IUiManager
    {
        UniTask Initialize(RectTransform canvas);
        T AddScreen<T>(ScreenType type) where T : IUiController;
    }
}