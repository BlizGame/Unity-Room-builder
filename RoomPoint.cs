using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomPoint : MonoBehaviour
{
    public enum ConnectingType
    {
        Room,
        Corridor,
        Other
    }
    public ConnectingType type;
    private void OnDrawGizmos()
    {
        switch (type)
        {
            case ConnectingType.Room:
                Gizmos.color = Color.red;
                break;
            case ConnectingType.Corridor:
                Gizmos.color = Color.green;
                break;
            case ConnectingType.Other:
                Gizmos.color = Color.cyan;
                break;
            default:
                break;
        }
        Gizmos.DrawSphere(transform.position, 0.1f);
    }
}