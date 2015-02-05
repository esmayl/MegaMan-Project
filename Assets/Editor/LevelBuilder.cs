using UnityEngine;
using UnityEditor;
using System.Collections;


public class LevelBuilder : EditorWindow  {

    public static GameObject[] platforms;
    public static GameObject[] hazards;
    public static GameObject[] doors;
    public static GameObject[] enemies;
    public static GameObject[] bosses;
    GameObject objToPlace;
    float depth = -0.5f;
    Vector3 location;
    int buttonWidth =110;


    Camera sceneCam;
    Vector3 spawnPos;
    GameObject tempObject;

    Vector2 platformScroll;
    Vector2 hazardScroll;
    Vector2 doorScroll;
    Vector2 enemyScroll;
    Vector2 bossScroll;

    [MenuItem("File/LevelBuilder")]
	static void Init () {
        Object[] tempObjArray=Resources.LoadAll("Hazards");
        hazards = new GameObject[tempObjArray.Length];
        for(int i = 0;i<tempObjArray.Length;i++){
            hazards[i] = (GameObject)tempObjArray[i];
        }

        tempObjArray = Resources.LoadAll("Doors");
        doors = new GameObject[tempObjArray.Length];
        for(int i = 0;i<tempObjArray.Length;i++){
            doors[i] = (GameObject)tempObjArray[i];
        }

        tempObjArray = Resources.LoadAll("Platforms");
        platforms = new GameObject[tempObjArray.Length];
        for (int i = 0; i < tempObjArray.Length; i++)
        {
            platforms[i] = (GameObject)tempObjArray[i];
        }
        tempObjArray = Resources.LoadAll("Enemies");
        enemies = new GameObject[tempObjArray.Length];
        for (int i = 0; i < tempObjArray.Length; i++)
        {
            enemies[i] = (GameObject)tempObjArray[i];
        }
        tempObjArray = Resources.LoadAll("Bosses");
        bosses = new GameObject[tempObjArray.Length];
        for (int i = 0; i < tempObjArray.Length; i++)
        {
            bosses[i] = (GameObject)tempObjArray[i];
        }

        LevelBuilder window = (LevelBuilder)EditorWindow.GetWindow(typeof(LevelBuilder));
        window.minSize = new Vector2(410, 300);
        window.maxSize = new Vector2(410, 300);
        window.title = "LevelBuilder";
	}

    void OnGUI()
    {
        Debug.Log(""+platforms.Length);
        EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical();
                
                        EditorGUILayout.Space();
                        GUILayout.Label("Platforms");
                        platformScroll = EditorGUILayout.BeginScrollView(platformScroll, GUILayout.Width(130), GUILayout.Height(100));
                        for (int i = 0; i < platforms.Length;i++ )
                        {

                            if (GUILayout.Button(platforms[i].name, GUILayout.Width(buttonWidth), GUILayout.Height(20)))
                            {
                                objToPlace = platforms[i];
                                Selection.activeObject = SceneView.currentDrawingSceneView;
                                sceneCam = SceneView.currentDrawingSceneView.camera;
                                spawnPos = sceneCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 10f));
                                spawnPos.x = depth;
                                tempObject = PrefabUtility.InstantiatePrefab(objToPlace) as GameObject;
                                tempObject.transform.position = spawnPos;

                                Selection.activeGameObject = tempObject;
                                SceneView.FrameLastActiveSceneViewWithLock();
                            }
                        }
                        EditorGUILayout.EndScrollView();

                        EditorGUILayout.Space();
                        GUILayout.Label("Hazards");
                        hazardScroll = EditorGUILayout.BeginScrollView(hazardScroll, GUILayout.Width(130), GUILayout.Height(100));
                        for (int i = 0; i < hazards.Length; i++)
                        {

                            if (GUILayout.Button(hazards[i].name, GUILayout.Width(buttonWidth), GUILayout.Height(20)))
                            {

                                objToPlace = hazards[i];
                                Selection.activeObject = SceneView.currentDrawingSceneView;
                                sceneCam = SceneView.currentDrawingSceneView.camera;
                                spawnPos = sceneCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 10f));
                                spawnPos.x = depth;
                                tempObject = PrefabUtility.InstantiatePrefab(objToPlace) as GameObject;
                                tempObject.transform.position = spawnPos;

                                Selection.activeGameObject = tempObject;
                                SceneView.FrameLastActiveSceneViewWithLock();
                            }
                        }
                        EditorGUILayout.EndScrollView();

            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical();
                        EditorGUILayout.Space();
                        GUILayout.Label("Enemies");
                        enemyScroll = EditorGUILayout.BeginScrollView(enemyScroll, GUILayout.Width(130), GUILayout.Height(100));
                        for (int i = 0; i < enemies.Length; i++)
                        {

                            if (GUILayout.Button(enemies[i].name, GUILayout.Width(buttonWidth), GUILayout.Height(20)))
                            {

                                objToPlace = enemies[i];
                                Selection.activeObject = SceneView.currentDrawingSceneView;
                                sceneCam = SceneView.currentDrawingSceneView.camera;
                                spawnPos = sceneCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 10f));
                                spawnPos.x = depth;
                                tempObject = PrefabUtility.InstantiatePrefab(objToPlace) as GameObject;
                                tempObject.transform.position = spawnPos;

                                Selection.activeGameObject = tempObject;
                                SceneView.FrameLastActiveSceneViewWithLock();
                            }
                        }
                        EditorGUILayout.EndScrollView();

                        EditorGUILayout.Space();
                        GUILayout.Label("Bosses");
                        bossScroll = EditorGUILayout.BeginScrollView(bossScroll, GUILayout.Width(130), GUILayout.Height(100));
                        for (int i = 0; i < bosses.Length; i++)
                        {

                            if (GUILayout.Button(bosses[i].name, GUILayout.Width(buttonWidth), GUILayout.Height(20)))
                            {

                                objToPlace = bosses[i];
                                Selection.activeObject = SceneView.currentDrawingSceneView;
                                sceneCam = SceneView.currentDrawingSceneView.camera;
                                spawnPos = sceneCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 10f));
                                spawnPos.x = depth;
                                tempObject = PrefabUtility.InstantiatePrefab(objToPlace) as GameObject;
                                tempObject.transform.position = spawnPos;

                                Selection.activeGameObject = tempObject;
                                SceneView.FrameLastActiveSceneViewWithLock();
                            }
                        }
                        EditorGUILayout.EndScrollView();
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical();
                        GUILayout.Label("Doors");
                        doorScroll = EditorGUILayout.BeginScrollView(doorScroll, GUILayout.Width(130), GUILayout.Height(100));
                        for (int i = 0; i < doors.Length; i++)
                        {

                            if (GUILayout.Button(doors[i].name, GUILayout.Width(buttonWidth), GUILayout.Height(20)))
                            {

                                objToPlace = doors[i];
                                Selection.activeObject = SceneView.currentDrawingSceneView;
                                sceneCam = SceneView.currentDrawingSceneView.camera;
                                spawnPos = sceneCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 10f));
                                spawnPos.x = depth;
                                tempObject = PrefabUtility.InstantiatePrefab(objToPlace) as GameObject;
                                tempObject.transform.position = spawnPos;

                                Selection.activeGameObject = tempObject;
                                SceneView.FrameLastActiveSceneViewWithLock();
                            }
                        }
                        EditorGUILayout.EndScrollView();
                EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();

                 
    }
}
