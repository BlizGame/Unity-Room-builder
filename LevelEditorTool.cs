#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class LevelEditorTool : EditorWindow
{
    private GameObject objectToPlace0;
    private GameObject objectToPlace1;
    private bool canPlaceObject = true;
    private float placementCooldown = 1f;
    private float timeSinceLastPlacement = 0f;
    private Vector3 objectRotation = Vector3.zero;
    private float placementdistance = 0f;
    private byte index = 0;
    RaycastHit hit;

    [MenuItem("Tools/Level Editor")]
    public static void ShowWindow()
    {
        GetWindow<LevelEditorTool>("Level Editor");
    }

    private void OnGUI()
    {
        GUILayout.Label("Level Editor Tool", EditorStyles.boldLabel);

        objectToPlace0 = (GameObject)EditorGUILayout.ObjectField("Object 0 to Place:", objectToPlace0, typeof(GameObject), false);
        objectToPlace1 = (GameObject)EditorGUILayout.ObjectField("Object 1 to Place:", objectToPlace1, typeof(GameObject), false);

        if (GUILayout.Button("Place On"))
        {
            canPlaceObject = true;
        }

        if (GUILayout.Button("Place Off"))
        {
            canPlaceObject = false;
        }

        GUILayout.Label("Rotation");
        objectRotation = EditorGUILayout.Vector3Field("Object Rotation:", objectRotation);

        if (objectToPlace0 || objectToPlace1)
        {
            SceneView.duringSceneGui += OnSceneGUI;
        }
        else
        {
            SceneView.duringSceneGui -= OnSceneGUI;
        }
    }

    private void OnSceneGUI(SceneView sceneView)
    {
        if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && canPlaceObject)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                PlaceObject();
                canPlaceObject = false;
                timeSinceLastPlacement = 0f;
                EditorApplication.update += UpdatePlacementCooldown;
            }

            sceneView.Repaint();
        }
    }

    private void UpdatePlacementCooldown()
    {
        timeSinceLastPlacement += Time.deltaTime;

        if (timeSinceLastPlacement >= placementCooldown)
        {
            canPlaceObject = true;
            EditorApplication.update -= UpdatePlacementCooldown;
        }
    }

    private void OnDestroy()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
        EditorApplication.update -= UpdatePlacementCooldown;
    }

    private void PlaceObject()
    {
        Quaternion alignrotation = Quaternion.LookRotation(-hit.normal);
        Quaternion rotation = Quaternion.Euler(objectRotation);
        GameObject objectToPlace;

        if (index == 0)
        {
            objectToPlace = objectToPlace0;
            index++;
        }
        else
        {
            objectToPlace = objectToPlace1;
            index--;
        }
        Instantiate(objectToPlace, hit.point, rotation);
    }
}
#endif