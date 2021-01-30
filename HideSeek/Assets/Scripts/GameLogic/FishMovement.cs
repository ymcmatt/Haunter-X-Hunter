using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class FishMovement : MonoBehaviour
{
    public NavMeshAgent navAgent;
    public float moveSpeed;
    public float rotateSpeed;
    public Transform[] targets;
    private Vector3 curTarget;
    // Start is called before the first frame update
    void Start()
    {
        curTarget = targets[Random.Range(0, targets.Length)].position;
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Magnitude(transform.position - curTarget) < 0.1)
        {
            print("stwitch target");
            curTarget = targets[Random.Range(0, targets.Length)].position;
        }
        moveToLocation(curTarget);
    }

    void moveToLocation(Vector3 target)
    {
        // translation
        float step = moveSpeed * Time.deltaTime; // calculate distance to move
        transform.position = Vector3.MoveTowards(transform.position, target, step);

        // rotation
        Vector3 targetDirection = target - transform.position;
        float singleStep = rotateSpeed * Time.deltaTime;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDirection);
    }
}
