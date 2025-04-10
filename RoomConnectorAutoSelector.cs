#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;

[InitializeOnLoad]
public static class RoomConnectorAutoSelector
{
    static RoomConnectorAutoSelector()
    {
        Selection.selectionChanged += OnSelectionChanged;
        EditorApplication.update += CheckKeyPress;
    }

    private static void OnSelectionChanged()
    {
        if (Selection.activeGameObject != null && Selection.activeGameObject.GetComponent<RoomEmpty>())
        {
            ToolManager.SetActiveTool<RoomConnector>();
        }
    }

    private static void CheckKeyPress()
    {
        Event e = Event.current;
        if (e != null && e.type == EventType.KeyDown && e.keyCode == KeyCode.LeftAlt)
        {
            ToolManager.SetActiveTool<RoomConnector>();
            Event.current.Use();
        }
    }
}
#endif