using UnityEngine;
using UnityEditor;
using System.IO;

/// <summary>
/// A Unity Editor window that replaces a specified color with another color
/// </summary>

public class ColorReplacer : EditorWindow
{
    //Stores the texture
    Object image;

    Color orignialColor = new Color(0,0,0,1);
    Color replacementColor = new Color (0,0,0,1);

    string pathOfConvertedFile;

    [MenuItem("Window/PowTool/Color Replacer")]
    public static void ShowWindow()
    {
        GetWindow<ColorReplacer>("Color Replacer");
    }

    public void OnGUI()
    {
        // Credits to Me!
        GUILayout.Label("Created by Powxjason. Discord: JimmyJohn's#7242", EditorStyles.boldLabel);
        image = EditorGUILayout.ObjectField("Image to Convert", image, typeof(Texture2D), false);

        // A button that opens a window to select the output location
        if (GUILayout.Button("Set File Path"))
        {
            pathOfConvertedFile = EditorUtility.SaveFilePanel("Save new image as PNG", "", ".png", "png");
        }

        EditorGUILayout.HelpBox("Every pixel that is the orignial color is changed into the replacement color", MessageType.Info);
        orignialColor = EditorGUILayout.ColorField("Original Color", orignialColor);
        replacementColor = EditorGUILayout.ColorField("Replacement Color", replacementColor);

        if (GUILayout.Button("Convert Image"))
        {
            ((Texture2D)image).filterMode = FilterMode.Point;
            Texture2D editedTexture = ReplaceColor((Texture2D)image, orignialColor, replacementColor);
            editedTexture.filterMode = FilterMode.Point;

            byte[] bytes = editedTexture.EncodeToPNG();
            File.WriteAllBytes(pathOfConvertedFile, bytes);

            AssetDatabase.Refresh();
        }
    }

    static Texture2D ReplaceColor(Texture2D localTexture, Color oldColor, Color newColor)
    {
        int pixelCounter = 0;        
        Color[] pixelArray = localTexture.GetPixels(0);

        for(int i = 0; i <= pixelArray.Length - 1; i++)
        {
            if(pixelArray[i] == oldColor)
            {
                pixelArray[i] = newColor;
                pixelCounter++;
            }
        }

        if(pixelCounter == 0)
        {
            Debug.LogWarning("No pixels were changed");
        }

        Texture2D newTexture = new Texture2D(localTexture.width, localTexture.height);
        newTexture.SetPixels(pixelArray, 0);
        return newTexture;
    } 
}
