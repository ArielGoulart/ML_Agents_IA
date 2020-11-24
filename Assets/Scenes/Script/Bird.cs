using System;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class Bird : Agent
{
    [Header("References")]
    [SerializeField] private Rigidbody rb = null;
    [SerializeField] private Spawn_Canos pipeHandler = null;
    [SerializeField] private Transform bodyTrasform = null;

    [Header("Settings")]
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float maxVelocityMagnitude = 5f;

    private Vector3 startingPos;

    public override void Initialize()
    {
        startingPos = transform.position;
    }

    public override void OnEpisodeBegin()
    {
        transform.position = startingPos;
        rb.velocity = Vector3.zero;

        pipeHandler.ResetPipes();
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        AddReward(0.1f);

        if(Mathf.FloorToInt(vectorAction[0]) != 1)
        {
            return;
        }

        Jump();
    }

    private void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);

        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxVelocityMagnitude);
    }

    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[0] = 0;

        if(Input.GetKey(KeyCode.Space))
        {
            return;
        }

        actionsOut[0] = 1;
    }

    private void OnTriggerEnter(Collider other)
    {
        AddReward(-10.0f);

        EndEpisode();
    }

    private void Update()
    {
        bodyTrasform.rotation = Quaternion.LookRotation(rb.velocity + new Vector3(10f, 0f, 0f), transform.up);
    }
}
