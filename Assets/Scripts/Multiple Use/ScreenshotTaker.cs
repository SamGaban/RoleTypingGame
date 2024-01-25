using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class ScreenshotTaker : MonoBehaviour
{
    public Camera orthographicCamera;
    public int resolutionMultiplier = 20; // You can adjust this for higher resolution
    
    public void TakeScreenshot(int width, int height)
    {
        if (width <= 0 || height <= 0)
        {
            Debug.LogError("Invalid width or height for screenshot");
            return;
        }

        RenderTexture renderTexture = new RenderTexture(width, height, 24);
        orthographicCamera.targetTexture = renderTexture;

        // Render the camera's view.
        orthographicCamera.Render();

        // Create a new texture with the same size as the render texture
        Texture2D screenshot = new Texture2D(width, height, TextureFormat.RGB24, false);

        // Read the pixels from the RenderTexture
        RenderTexture.active = renderTexture;
        screenshot.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        screenshot.Apply();

        // Reset the active RenderTexture
        RenderTexture.active = null;

        // Convert to PNG
        byte[] bytes = screenshot.EncodeToPNG();
        System.IO.File.WriteAllBytes("Screenshot.png", bytes);

        // Clean up
        orthographicCamera.targetTexture = null;
        Destroy(renderTexture);
    }


    private void Start()
    {
        StartCoroutine(ScreenCoroutine());
    }

    IEnumerator ScreenCoroutine()
    {
        yield return new WaitForSeconds(4f);
        TakeScreenshot(Screen.width * resolutionMultiplier, Screen.height * resolutionMultiplier);
        Debug.Log("Screenshot Taken");
    }
}
