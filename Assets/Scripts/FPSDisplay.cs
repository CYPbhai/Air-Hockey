using UnityEngine;

public class FPSDisplay : MonoBehaviour
{
    private float deltaTime = 0.0f;

    void Update()
    {
        // Calculate the time between frames
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
    }

    void OnGUI()
    {
        // Calculate FPS
        float fps = 1.0f / deltaTime;
        string text = string.Format("{0:0.} FPS", fps);

        // Set the style for the text
        GUIStyle style = new GUIStyle();
        style.fontSize = 24;
        style.normal.textColor = Color.white;

        // Display the FPS at the top-left corner of the screen
        GUI.Label(new Rect(10, 10, 200, 50), text, style);
    }
}

