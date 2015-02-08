using UnityEngine;
using UnityEditor;
using System.Collections;
using System;


public class LevelBuilder : EditorWindow  {

    public static GameObject[] platforms;
    public static GameObject[] hazards;
    public static GameObject[] doors;
    public static GameObject[] enemies;
    public static GameObject[] bosses;
    public static Material[] materials;
    public static Camera sceneCam;

    bool preview = false;
    GameObject previewObject;
    GameObject objToPlace;
    Material selectedMaterial = new Material(Shader.Find("Diffuse"));

    Vector3 spawnPos;
    float depth = -0.5f;
    Vector3 mousePos;

    int buttonWidth =110;
    string groupName ="";
    bool canGroup = false;


    Vector2 platformScroll;
    Vector2 hazardScroll;
    Vector2 doorScroll;
    Vector2 enemyScroll;
    Vector2 bossScroll;
    Vector2 textureScroll;

    void OnEnable() 
    { 
        SceneView.onSceneGUIDelegate += OnSceneGUI;        
        SceneView.lastActiveSceneView.orthographic = true;
        SceneView.lastActiveSceneView.LookAt(SceneView.lastActiveSceneView.pivot, Quaternion.LookRotation(-Vector3.right));
    }

    void OnDisable() { SceneView.onSceneGUIDelegate -= OnSceneGUI; SceneView.lastActiveSceneView.orthographic = false; }

    [MenuItem("File/LevelBuilder")]
	static void Init () 
    {
        UnityEngine.Object[] tempObjArray=Resources.LoadAll("Hazards");
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
        tempObjArray = Resources.LoadAll("Materials");
        materials = new Material[tempObjArray.Length];
        for (int i = 0; i < tempObjArray.Length; i++)
        {
            materials[i] = tempObjArray[i] as Material;
        }

        LevelBuilder window = (LevelBuilder)EditorWindow.GetWindow(typeof(LevelBuilder));
        window.minSize = new Vector2(410, 350);
        window.maxSize = new Vector2(410, 350);
        window.title = "LevelBuilder";

        sceneCam = GameObject.Find("SceneCamera").camera;

	}

    void OnGUI()
    {
        if (preview)
        {
            GUILayout.Label("Press Left-Mouse to place object, press Escape to leave\n To group select multiple objects and select this window", GUILayout.Width(400));
        }

        EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical(GUILayout.Height(220));
                
                        EditorGUILayout.Space();
                        GUILayout.Label("Platforms");
                        platformScroll = EditorGUILayout.BeginScrollView(platformScroll, GUILayout.Width(130), GUILayout.Height(100));
                        for (int i = 0; i < platforms.Length;i++ )
                        {
                            if (GUILayout.Button(platforms[i].name, GUILayout.Width(buttonWidth), GUILayout.Height(20)))
                            {
                                if(previewObject != null){DestroyImmediate(previewObject);}

                                objToPlace = platforms[i];
                                Selection.activeObject = SceneView.currentDrawingSceneView;
                                spawnPos = sceneCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 10f));
                                spawnPos.x = depth;
                                spawnPos.y = Mathf.Round(spawnPos.y);
                                spawnPos.z = Mathf.Round(spawnPos.z);
                                previewObject = PrefabUtility.InstantiatePrefab(objToPlace) as GameObject;
                                previewObject.transform.position = spawnPos;
                                previewObject.renderer.material = selectedMaterial;

                                Selection.activeGameObject = previewObject;                
                                preview = true;
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
                                if (previewObject != null) { DestroyImmediate(previewObject); }
                                objToPlace = hazards[i];
                                Selection.activeObject = SceneView.currentDrawingSceneView;
                                spawnPos = sceneCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 10f));
                                spawnPos.x = depth;
                                spawnPos.y = Mathf.Round(spawnPos.y);
                                spawnPos.z = Mathf.Round(spawnPos.z);
                                previewObject = PrefabUtility.InstantiatePrefab(objToPlace) as GameObject;
                                previewObject.transform.position = spawnPos;
                                previewObject.renderer.material = selectedMaterial;

                                Selection.activeGameObject = previewObject;
                                preview = true;
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
                                if (previewObject != null) { DestroyImmediate(previewObject); }
                                objToPlace = enemies[i];
                                Selection.activeObject = SceneView.currentDrawingSceneView;
                                spawnPos = sceneCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 10f));
                                spawnPos.x = depth;
                                spawnPos.y = Mathf.Round(spawnPos.y);
                                spawnPos.z = Mathf.Round(spawnPos.z);
                                previewObject = PrefabUtility.InstantiatePrefab(objToPlace) as GameObject;
                                previewObject.transform.position = spawnPos;
                                previewObject.renderer.material = selectedMaterial;

                                Selection.activeGameObject = previewObject;
                                preview = true;
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
                                if (previewObject != null) { DestroyImmediate(previewObject); }
                                objToPlace = bosses[i];
                                Selection.activeObject = SceneView.currentDrawingSceneView;
                                spawnPos = sceneCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 10f));
                                spawnPos.x = depth;
                                spawnPos.y = Mathf.Round(spawnPos.y);
                                spawnPos.z = Mathf.Round(spawnPos.z);
                                previewObject = PrefabUtility.InstantiatePrefab(objToPlace) as GameObject;
                                previewObject.transform.position = spawnPos;
                                previewObject.renderer.material = selectedMaterial;

                                Selection.activeGameObject = previewObject;
                                preview = true;
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
                                if (previewObject != null) { DestroyImmediate(previewObject); }
                                objToPlace = doors[i];
                                Selection.activeObject = SceneView.currentDrawingSceneView;
                                spawnPos = sceneCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 10f));
                                spawnPos.x = depth;
                                spawnPos.y = Mathf.Round(spawnPos.y);
                                spawnPos.z = Mathf.Round(spawnPos.z);
                                previewObject = PrefabUtility.InstantiatePrefab(objToPlace) as GameObject;
                                previewObject.transform.position = spawnPos;
                                previewObject.renderer.material = selectedMaterial;

                                Selection.activeGameObject = previewObject;
                                preview = true;
                            }
                        }
                        EditorGUILayout.EndScrollView();

                        GUILayout.Label("Textures");
                            textureScroll = EditorGUILayout.BeginScrollView(textureScroll, GUILayout.Width(130), GUILayout.Height(100));
                            for (int i = 0; i < materials.Length; i++)
                            {
                                if (GUILayout.Button(materials[i].name, GUILayout.Width(buttonWidth), GUILayout.Height(20)))
                                {
                                    selectedMaterial= materials[i];
                                    if (previewObject != null)
                                    {
                                        previewObject.renderer.material = selectedMaterial;
                                    }
                                    if (Selection.gameObjects.Length > 0)
                                    {
                                        GameObject[] gas = Selection.gameObjects;
                                        foreach (GameObject ga in gas)
                                        {
                                            ga.renderer.material = selectedMaterial;
                                        }
                                    }
                                }
                            }
                            EditorGUILayout.EndScrollView();
                EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginVertical(GUILayout.Height(150));
        if (canGroup)
        {
            GUILayout.Label("Group name:");
            groupName = EditorGUILayout.TextField(groupName, GUILayout.Width(100));
            if (GUILayout.Button("Group"))
            {
                GameObject group = new GameObject();
                group.name = groupName;
                foreach (GameObject ga in Selection.gameObjects)
                {
                    ga.transform.parent = group.transform;
                    ga.isStatic = true;
                }
                group = null;
            }
        }

        EditorGUILayout.EndVertical();
    }

    void OnSceneGUI(SceneView sceneView)
    {
        Event e = Event.current;

        if (e.keyCode == KeyCode.Escape && preview)
        {
            DestroyImmediate(previewObject);
            preview = false;
        }
        
        if (previewObject != null)
        {
            Selection.activeGameObject = previewObject;
            mousePos = sceneCam.ScreenToWorldPoint(new Vector2(sceneCam.pixelRect.width -(sceneCam.pixelRect.width-e.mousePosition.x), sceneCam.pixelRect.height - e.mousePosition.y));
            mousePos.x = depth;
            mousePos.y = (float)Math.Round(mousePos.y,1);
            mousePos.z = (float)Math.Round(mousePos.z,1);
            previewObject.transform.position = mousePos;
        }

        if (Selection.gameObjects.Length > 1) { canGroup = true; }
        if (Selection.gameObjects.Length <= 1) { canGroup = false; }

        if (e.isMouse && previewObject != null)
        {
            if (e.type == EventType.mouseDown)
            {
                mousePos = sceneCam.ScreenToWorldPoint(new Vector2(sceneCam.pixelRect.width - (sceneCam.pixelRect.width - e.mousePosition.x), sceneCam.pixelRect.height - e.mousePosition.y));
                mousePos.x = depth;
                mousePos.y = (float)Math.Round(mousePos.y, 1);
                mousePos.z = (float)Math.Round(mousePos.z, 1);
                GameObject tempObj = PrefabUtility.InstantiatePrefab(objToPlace) as GameObject;
                tempObj.transform.position = mousePos;
                tempObj.renderer.material = selectedMaterial;
                tempObj = null;
            }
        }

    }
}
