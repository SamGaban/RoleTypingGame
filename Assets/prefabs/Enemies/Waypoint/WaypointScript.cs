using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a Waypoint in a game for enemies pathfinding.
/// </summary>
public class WaypointScript : MonoBehaviour
{
    [Range(0, 12)]
    public int index;

    /// <summary>
    /// Makes gizmos the size of the detection range of waypoints
    /// </summary>
    /// <remarks>
    /// This method sets the Gizmos color to red and draws a wire sphere at the current transform position.
    /// </remarks>
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 500);
    }
}
