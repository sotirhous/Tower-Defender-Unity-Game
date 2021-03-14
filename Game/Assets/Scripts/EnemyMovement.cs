using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    [SerializeField] float enemySpeed = 10f;
    [SerializeField] ParticleSystem goalParticle;

    void Start()
    {
        PathFinder pathfinder = FindObjectOfType<PathFinder>();
        var path = pathfinder.GetPath();
        StartCoroutine(FollowPath(path));
    }

    IEnumerator FollowPath(List<Waypoint> path)
    {
        foreach (Waypoint waypoint in path)
        {
            yield return StartCoroutine(SmoothMove(waypoint));
        }
        SelfDestruct();
    }

    IEnumerator SmoothMove(Waypoint waypoint)
    {
        while (Vector3.Distance(this.transform.position, waypoint.transform.position) > enemySpeed * Time.deltaTime)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, waypoint.transform.position, enemySpeed * Time.deltaTime);
            yield return null;
        }
        this.transform.position = waypoint.transform.position;
    }

    void SelfDestruct()
    {
        var vfx = Instantiate(goalParticle, transform.position, Quaternion.identity);
        vfx.Play();
        Destroy(vfx.gameObject, vfx.main.duration);

        Destroy(gameObject);//the enemy
    }



}
