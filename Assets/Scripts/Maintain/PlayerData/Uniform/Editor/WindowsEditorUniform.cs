using LazySoccer.User.Uniform;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WindowsEditorUniform : OdinEditorWindow
{
    [MenuItem("Tools/Editor/Editor Uniform")]
    private static void OpenWindow()
    {
        GetWindow<WindowsEditorUniform>().Show();
    }

    [EnumToggleButtons, BoxGroup("Settings"), HideLabel]
    public EditorUniform TypeUniform;

    [SerializeField, ShowIf("TypeUniform", EditorUniform.SelectDb)] private DbUniformPlayer uniformPlayer;

    [ShowIf("TypeUniform", EditorUniform.PlayerUniform)]
    public UniformPlayerDictionaty InputTextures;

    [ShowIf("TypeUniform", EditorUniform.PlayerUniform)]
    [Button]
    private void UpdatePlayerUniform()
    {
        
    }

}

public enum EditorUniform
{
    SelectDb,
    PlayerUniform,
    AllUniform,
    PrototypeUniform
}
