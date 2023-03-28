using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Sirenix.OdinInspector.Editor;
using Sirenix.OdinInspector;

public class ToolsSpritesAnchors : OdinEditorWindow
{
    [MenuItem("Tools/AnchorsUpdate")]
    private static void OpenWindow()
    {
        GetWindow<ToolsSpritesAnchors>().Show();
    }

    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private Vector2 size;

    [SerializeField] private float up;
    [SerializeField] private float left;
    [SerializeField] private float right;
    [SerializeField] private float down;


    [Button]
    private void UpdateAnchors()
    {
        var newDown = down / size.x;
        var newUp = (size.x - up) / size.x;

        var newLeft = left / size.y;
        var newRight = (size.y - right) / size.y;

        rectTransform.anchorMin = new Vector2(newLeft, newDown);
        rectTransform.anchorMax = new Vector2(newRight, newUp);
    }
}
