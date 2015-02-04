using UnityEngine;
using UnityEditor;
using System.Collections;


public class LevelBuilder : EditorWindow  {

    public static GameObject[] hazards;
    public static GameObject[] doors;
    public static GameObject[] platforms;
    GameObject objToPlace;
    Vector3 depth = new Vector3(-0.5f, 0, 0);
    Vector3 location;

    [MenuItem("File/LevelBuilder")]
	static void Init () {
        Object[] tempObjArray=Resources.LoadAll("Hazards");
        hazards = new GameObject[tempObjArray.Length];
        for(int i = 0;i<tempObjArray.Length;i++){
            hazards[i] = (GameObject)tempObjArray[i];
            Debug.Log(hazards[i].name);
        }

        tempObjArray = Resources.LoadAll("Doors");
        doors = new GameObject[tempObjArray.Length];
        for(int i = 0;i<tempObjArray.Length;i++){
            doors[i] = (GameObject)tempObjArray[i];
            Debug.Log(doors[i].name);
        }

        tempObjArray = Resources.LoadAll("Platforms");
        platforms = new GameObject[tempObjArray.Length];
        for (int i = 0; i < tempObjArray.Length; i++)
        {
            platforms[i] = (GameObject)tempObjArray[i];
            //Debug.Log(platforms[i].name);
        }

        LevelBuilder window = (LevelBuilder)EditorWindow.GetWindow(typeof(LevelBuilder));
        window.minSize = new Vector2(340, 300);
        window.title = "LevelBuilder";
	}

    void OnGUI()
    {
        Debug.Log(""+platforms.Length);
        for (int i = 0; i < platforms.Length;i++ )
        {

            if (GUILayout.Button(platforms[i].name, GUILayout.Width(100), GUILayout.Height(20)))
            {

            Debug.Log(platforms[i].name);
            }
        }
    }
}
