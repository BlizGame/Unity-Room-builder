#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEditor.SceneManagement;
using UnityEngine;

[EditorTool("Room Connector & Rotate Tool", typeof(RoomEmpty))]
public class RoomConnector : EditorTool
{
    [SerializeField] private Texture2D toolIcon;

    private Transform oldTarget;
    private RoomPoint[] allPoints;
    private RoomPoint[] targetPoints;

    private void OnEnable()
    {
        if (!SessionState.GetBool("RoomConnector_ToolTipShown", false))
        {
            SceneView.lastActiveSceneView.ShowNotification(
                new GUIContent("Room Connector: Q - âëĺâî, E - âďđŕâî, LAlt - âűáđŕňü číńňđóěĺíň"),
                3f
            );

            SessionState.SetBool("RoomConnector_ToolTipShown", true);
        }
    }

    public override GUIContent toolbarIcon
    {
        get
        {
            return new GUIContent
            {
                image = toolIcon,
                text = "Room Connector & Rotate",
                tooltip = "This tool helps you connect rooms and rotate RoomEmpty objects. Press Q to rotate left, E to rotate right."
            };
        }
    }

    public override void OnToolGUI(EditorWindow window)
    {
        Transform targetTransform = ((RoomEmpty)target).transform;

        if (targetTransform != oldTarget)
        {
            PrefabStage prefabStage = PrefabStageUtility.GetPrefabStage(targetTransform.gameObject);

            if (prefabStage != null)
                allPoints = prefabStage.prefabContentsRoot.GetComponentsInChildren<RoomPoint>();
            else
                allPoints = FindObjectsOfType<RoomPoint>();

            targetPoints = targetTransform.GetComponentsInChildren<RoomPoint>();
            oldTarget = targetTransform;
        }

        EditorGUI.BeginChangeCheck();
        Vector3 newPosition = Handles.PositionHandle(targetTransform.position, Quaternion.identity);

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(targetTransform, "Move with custom snap tool");
            if (((RoomEmpty)target).isGround) newPosition.y = 0;
            MoveWithSnapping(targetTransform, newPosition);
        }

        Event e = Event.current;
        if (e.type == EventType.KeyDown)
        {
            if (e.keyCode == KeyCode.Q)
            {
                RotateObject(targetTransform, -90f);
                e.Use();
            }
            else if (e.keyCode == KeyCode.E)
            {
                RotateObject(targetTransform, 90f);
                e.Use();
            }
        }
    }

    private void MoveWithSnapping(Transform targetTransform, Vector3 newPosition)
    {
        Vector3 bestPosition = newPosition;
        float closestDistance = float.PositiveInfinity;

        foreach (RoomPoint point in allPoints)
        {
            if (point.transform.parent == targetTransform) continue;

            foreach (RoomPoint ownPoint in targetPoints)
            {
                if (ownPoint.type != point.type) continue;

                Vector3 targetPos = point.transform.position - (ownPoint.transform.position - targetTransform.position);
                float distance = Vector3.Distance(targetPos, newPosition);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    bestPosition = targetPos;
                }
            }
        }

        if (closestDistance < 5.5f)
        {
            targetTransform.position = bestPosition;
        }
        else
        {
            targetTransform.position = newPosition;
        }
    }

    private void RotateObject(Transform targetTransform, float angle)
    {
        Undo.RecordObject(targetTransform, "Rotate RoomEmpty");
        targetTransform.rotation *= Quaternion.Euler(0, angle, 0);
    }
}
#endif