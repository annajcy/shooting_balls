using UnityEngine;

namespace Framework.UI
{
    public class FPSDisplay : MonoBehaviour
    {
        private void OnGUI()
        {
            float fps = 1.0f / Time.unscaledDeltaTime;
            GUI.Label(new Rect(10, 10, 100, 20), "FPS: " + Mathf.Ceil(fps));
        }

    }
}