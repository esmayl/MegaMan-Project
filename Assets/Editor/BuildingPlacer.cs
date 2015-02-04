using UnityEngine;
using UnityEditor;
using System.IO;
using System.Net;
using System.Xml;
using System.Xml.Serialization;
using System.Collections;
using System.Collections.Generic;

public enum ImportType
{
    Woning = 0,
    Natuur = 1,
    Wegen =2
}


/// <summary>
/// Tool to place buildings in the scene.
/// </summary>
public class BuildingPlacerWindow : EditorWindow
{

    #region Doxygen declarations
    /**
     * @var path
     * The path to the obj to import
     * 
     * @var dest
     * The destination within the project to import the obj to 
     * 
     * @var id
     * The id that the user puts in the textfield
     * 
     * @var script
     * The script to attach to the placed building
     * 
     * @var ids
     * All of the possible id's in the XML (not possible to put in a selection box)
     * 
     * @var dict
     * The dict that holds the id's and the corrosponding information
     * 
     * @var loadedObjects
     * The array of buildings in the "Huizen" directory
     * 
     * @var loadedNames
     * The array of names of the loadedObjects to display to the user
     * 
     * @var stuk
     * The parent of the building (used for positioning relative to the parent), found by "Huizen " +posibilities[B] where B is a variable
     * 
     * @var i
     * The counter used to scroll through the ids
     * 
     * @var indexX
     * The index of the comma value in the XML element "X"
     * 
     * @var indexY
     * The index of the comma value in the XML element "Y"
     * 
     * @var loaded
     * The variable that shows if the scripts is done downloading
     * 
     * @var posibilities
     * All the possible ints used to find the parent
     * 
     * @var names
     * All the names of the possibilities
     * 
     * @var previewObject
     * The reference to the object the moment it's placed in the scene but the user hasn't saved it yet
     * 
     * @var tempCam
     * The scene camera
     * 
     * @var preview
     * The boolean that shows if the player has chosen to place it in the scene or not
     * 
     * @var scroll
     * The value used to scroll through all the buildings found
     * 
     * @var selected
     * The selected int corrosponding with a element in loadedObjects
     * 
     * @var selectedArea
     * The parent int corrosponding with a element in names
     * 
     * @var root
     * The root directory of the project
     * 
     * @var xmlPath
     * The path that points to the XML
     * 
     */
    #endregion
    #region Public variables
        public static string path = "Selecteer een pad alstublieft.";
        public static string dest = "Selecteer een pad alstublieft.";
        public string id="0080200000490690";

        public static string[] ids;

        public static GameObject[] loadedObjects;
        public static string[] loadedNames;
        public Transform stuk;
    #endregion
    #region Private variables
            private static string xmlPath = "table_export_adressen_Leeuwarden.xml";
            private static int i = 0;
            private static int indexX;
            private static int indexY;
            private static bool loaded = false;

            private int[] posibilities = {2,3,4,5,6};
            private string[] names = {"Gebied 2","Gebied 3","Gebied 4","Gebied 5","Gebied 6"};
            private GameObject previewObject;
            private GameObject tempCam;
            private bool preview =false;
            private Vector2 scroll = new Vector2();
            private int selected = 0;
            private int selectedArea = 2;
            private string root = Application.dataPath;
        #endregion

    static void Init()
    {
        BuildingPlacerWindow window = (BuildingPlacerWindow)EditorWindow.GetWindow(typeof(BuildingPlacerWindow));
        window.minSize = new Vector2(340f,300f);
        window.title = "Gebouwen Plaatsen";
        
        loadedObjects = Resources.LoadAll<GameObject>("Huizen");

        loadedNames = new string[loadedObjects.Length];
        for (int p = 0; p < loadedObjects.Length; p++)
        {
            loadedNames[p] = loadedObjects[p].name;
        }
    }

    void OnGUI()
    {
        Event e = Event.current;

        if (e.type == EventType.KeyDown && e.control)
        {
            id = EditorGUIUtility.systemCopyBuffer;
        }

        GUILayout.BeginVertical();

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Object selecteren"))
        {
            string temp = EditorUtility.OpenFilePanel("Object selecteren", root, "fbx");
            Debug.Log(temp);
            path = temp;
        }

        if (GUILayout.Button("Pad selecteren"))
        {
            string temp = EditorUtility.OpenFolderPanel("Pad selecteren", root, "");
            Debug.Log(temp);
            dest = temp;
        }

        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Object locatie: "+path);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Importeer locatie: " + dest);
        GUILayout.EndHorizontal();

        if (GUILayout.Button("Importeren"))
        {
            int p;

            loadedObjects = new GameObject[Resources.LoadAll<GameObject>("Huizen").Length];
            loadedObjects = Resources.LoadAll<GameObject>("Huizen");

            loadedNames = new string[loadedObjects.Length];
            Dictionary<string,GameObject> dictTemp = new Dictionary<string,GameObject>();
            for (p = 0; p < loadedObjects.Length; p++)
            {
                loadedNames[p] = loadedObjects[p].name;
                if (!dictTemp.ContainsKey(loadedObjects[p].name))
                {
                    dictTemp.Add(loadedObjects[p].name, loadedObjects[p]);
                    MonoBehaviour.print("Unfilterd: " + loadedNames[p]);
                }
            }
            

            loadedNames = new string[dictTemp.Count];

            p = 0;

            foreach (KeyValuePair<string, GameObject> k in dictTemp)
            {
                loadedNames[p] = k.Key;
                MonoBehaviour.print("Filterd: " + loadedNames[p]);
                p++;
            }
           
            
        }

        //select obj from array
        GUILayout.Label("1. Selecteer een gebouw.");
        scroll = GUILayout.BeginScrollView(scroll);

        GUILayout.BeginVertical();
        selected = GUILayout.SelectionGrid(selected, loadedNames,1);
        GUILayout.EndVertical();


        GUILayout.EndScrollView();

            GUILayout.Space(Screen.height / 32);            

            GUILayout.BeginHorizontal();
                GUILayout.Label("2. Vul het verblijfsID in: ");
                id = GUILayout.TextField(id, 16);

                GUILayout.Space(Screen.width / 32);
            GUILayout.EndHorizontal();
            
            selectedArea = EditorGUILayout.IntPopup(selectedArea, names, posibilities);

            
            //instantiate camera looking at obj
            if (GUILayout.Button("3. Plaats in de scene"))
            {
                if(previewObject != null){DestroyImmediate(previewObject);}
                previewObject = PrefabUtility.InstantiatePrefab(loadedObjects[selected]) as GameObject;

                Selection.activeGameObject = previewObject;
                SceneView.FrameLastActiveSceneViewWithLock();
                SceneView.lastActiveSceneView.pivot = previewObject.transform.position - previewObject.transform.forward;
                SceneView.lastActiveSceneView.LookAt(previewObject.transform.position);
                preview = true;
            }

            GUILayout.FlexibleSpace();

            if (preview)
            {
                Vector3 tempPos =previewObject.transform.position;

                GUILayout.BeginHorizontal();
            
                GUILayout.BeginVertical();
                GUILayout.Label("Positie");
                tempPos.x = EditorGUILayout.FloatField("X: ", tempPos.x);
                tempPos.y = EditorGUILayout.FloatField("Hoogte: ", tempPos.y);
                tempPos.z = EditorGUILayout.FloatField("Z: ", tempPos.z);
                GUILayout.EndVertical();
                previewObject.transform.position = tempPos;

                previewObject.transform.rotation = Quaternion.Euler(previewObject.transform.rotation.eulerAngles.x, EditorGUILayout.FloatField("Rotatie: ", previewObject.transform.rotation.eulerAngles.y), previewObject.transform.rotation.eulerAngles.z);
                GUILayout.EndHorizontal();
            }


                if (GUILayout.Button("4. Opslaan"))
                {
                    GameObject parent = GameObject.Find("Huizen Stuk "+selectedArea.ToString());

                    previewObject.transform.parent = parent.transform;
                    previewObject.tag = "Huizen";
                    previewObject = new GameObject();
                }
                if (GUILayout.Button("Scene Opslaan"))
                {
                    EditorApplication.SaveScene();
                }

        GUILayout.Space(Screen.width / 32);
        EditorGUILayout.EndVertical();
        
    }

    /// <summary>
    /// When closing this window, destroy the preview object.
    /// </summary>
    void OnDestroy()
    {
        DestroyImmediate(previewObject);
    }
}
