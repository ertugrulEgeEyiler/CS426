using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 2f;           // Speed of NPC movement
    public Transform[] waypoints;         // Points to move between
    public float waitTime = 1f;           // Time to wait at each waypoint

    private int currentWaypointIndex = 0; // Index of the current waypoint
    private bool isWaiting = false;       // Flag to check if NPC is waiting

    private void Update()
    {
        if (!isWaiting)
        {
            MoveToNextWaypoint();
        }
    }

    private void MoveToNextWaypoint()
    {
        if (waypoints.Length == 0) return;

        // Move towards the current waypoint
        Transform targetWaypoint = waypoints[currentWaypointIndex];
        Vector3 direction = (targetWaypoint.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;

        // Check if NPC has reached the waypoint
        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
        {
            StartCoroutine(WaitAtWaypoint());
        }
    }

    private IEnumerator WaitAtWaypoint()
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitTime);

        // Move to the next waypoint
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        isWaiting = false;
    }
}
