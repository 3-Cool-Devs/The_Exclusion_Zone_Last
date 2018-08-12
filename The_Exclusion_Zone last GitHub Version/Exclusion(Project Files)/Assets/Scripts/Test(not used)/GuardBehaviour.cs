﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GuardBehaviour : MonoBehaviour 
{
	public NavMeshAgent agent;

	public float speed = 5;
	public float waitTime = 0.5f;
	public float turnSpeed = 90;

	public Transform pathHolder;

	public Light spotLight;
	public float viewDistance;
	private float viewAngle;
	public LayerMask viewMask;

	private Transform player;
	private Vector3 playerV3;
	private Color originalSpotLightColour;
	private bool patrol;
	void Awake()
	{
		agent = GetComponent<NavMeshAgent>();
		player = GameObject.FindGameObjectWithTag ("Player").transform;
		viewAngle = spotLight.spotAngle;
		originalSpotLightColour = spotLight.color;
	}
	public void Start () // Use this for initialization
	{
		Vector3[] waypoints = new Vector3[pathHolder.childCount];
		for (int i = 0; i < waypoints.Length; i++) 
		{
			waypoints [i] = pathHolder.GetChild (i).position;
			waypoints [i] = new Vector3 (waypoints [i].x, transform.position.y, waypoints [i].z);
		}
		StartCoroutine (FollowPath (waypoints));
	}
	void Update () // Update is called once per frame
	{

	}
	IEnumerator MoveToLocation(Vector3 targetPoint)
	{
		agent.destination = targetPoint;
		agent.isStopped = false;
		Vector3[] waypoints = new Vector3[pathHolder.childCount];
		for (int i = 0; i < waypoints.Length; i++) 
		{
			waypoints [i] = pathHolder.GetChild (i).position;
			waypoints [i] = new Vector3 (waypoints [i].x, transform.position.y, waypoints [i].z);
		}
		yield return StartCoroutine (FollowPath (waypoints));
		yield return false;

	}
	IEnumerator CanSeePlayer()
	{
		if(Vector3.Distance(transform.position,player.position) < viewDistance)
		{
			Vector3 dirToPlayer = (player.position - transform.position).normalized;
			float angleBetweenGuardAndPlayer = Vector3.Angle (transform.forward, dirToPlayer);
			if (angleBetweenGuardAndPlayer < viewAngle / 2f) 
			{
				if(!Physics.Linecast(transform.position,player.position, viewMask))
				{
					spotLight.color = Color.red;
					yield return StartCoroutine (MoveToLocation (dirToPlayer));
				}
			}
		}
		yield return false;
	}
	IEnumerator FollowPath(Vector3[] waypoints)
	{
		spotLight.color = originalSpotLightColour;
		transform.position = waypoints [0];
		int targetWaypointIndex = 1;
		Vector3 targetWaypoint = waypoints [targetWaypointIndex];
		transform.LookAt (targetWaypoint);
		while (true) 
		{
			transform.position = Vector3.MoveTowards (transform.position, targetWaypoint, speed * Time.deltaTime);
			if (transform.position == targetWaypoint) 
			{
				targetWaypointIndex = (targetWaypointIndex + 1) % waypoints.Length;
				targetWaypoint = waypoints [targetWaypointIndex];
				yield return new WaitForSeconds (waitTime);
				yield return StartCoroutine (TurnToFace (targetWaypoint));
			}
            yield return null;
		}
	}
	IEnumerator TurnToFace(Vector3 lookTarget)
	{
		Vector3 dirToLookTarget = (lookTarget - transform.position).normalized;
		float targetAngle = 90 - Mathf.Atan2 (dirToLookTarget.z, dirToLookTarget.x) * Mathf.Rad2Deg;
		while (Mathf.Abs(Mathf.DeltaAngle (transform.eulerAngles.y, targetAngle)) > 0.05f) 
		{
			float angle = Mathf.MoveTowardsAngle (transform.eulerAngles.y, targetAngle, turnSpeed * Time.deltaTime);
			transform.eulerAngles = Vector3.up * angle;
            yield return null;
		}
	}
	void OnDrawGizmos()
	{
		Vector3 startPosition = pathHolder.GetChild (0).position;
		Vector3 previousPosition = startPosition;
		foreach (Transform waypoint in pathHolder) 
		{
			Gizmos.DrawSphere (waypoint.position, .3f);
			Gizmos.DrawLine(previousPosition, waypoint.position);
			previousPosition = waypoint.position;
		}
		Gizmos.DrawLine (previousPosition, startPosition);
		Gizmos.DrawRay (transform.position, transform.forward * viewDistance);
	}
}
