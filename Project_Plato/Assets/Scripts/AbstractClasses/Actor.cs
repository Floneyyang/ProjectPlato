using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Actor : MonoBehaviour
{
    public enum ActorState
    {
        Static,
        Patrol,
        Detect,
        Attack,
        Null
    }

    public enum ActorType
    {
        Angle,
        Switch
    }

    [Header("Actor Features")]
    public float speed;
    [Range(0, 5)]
    public float health;

    public Rigidbody ActorRd { get; set; }

    private void Start()
    {
        ActorRd = this.gameObject.GetComponent<Rigidbody>();
    }
}
