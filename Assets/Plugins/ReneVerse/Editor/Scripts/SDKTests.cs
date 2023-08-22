using UnityEditor;
using UnityEngine;

public class SDKTests
{
    [MenuItem("Tools/Reset Welcome Popup")]
    private static void ResetWelcomePopup()
    {
        EditorPrefs.SetBool("WelcomePopupShown", false);
    }

    [MenuItem("Tools/GetPop")]
    private static void GetPop()
    {
        Debug.Log(EditorPrefs.GetBool("WelcomePopupShown"));
    }
    
    [MenuItem("Tools/Clear All Editor Prefs")]
    private static void ClearAllEditorPrefs()
    {
        EditorPrefs.DeleteAll();
    }
}