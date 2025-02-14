using UnityEngine;

namespace CoreGame
{
    public static class GameUtils
    {
        public static void EnableCanvasGroup(this CanvasGroup canvasGroup, bool enable)
        {
            canvasGroup.alpha = enable ? 1 : 0;
            canvasGroup.blocksRaycasts = enable;
            canvasGroup.interactable = enable;
        }
    }
}