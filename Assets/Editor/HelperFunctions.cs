using UnityEditor;
using UnityEngine;

public class HelperFunctions : MonoBehaviour
{
    [MenuItem("Tools/Clear PlayerPrefs")]
    static void ClearPrefs()
    {
        Debug.Log("Clearing PlayerPrefs");
        PlayerPrefs.DeleteAll();
    }
}
