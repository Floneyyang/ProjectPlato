using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeerAnimationController : MonoBehaviour
{
    public float speed = 0.3f;
    public GameObject Key;
    public Vector3[] targetPosition;
    private Animator deerAni;
    public int PositionIter = 0;
    public bool _eat = true;
    public bool _move = false;
    public bool _run = true;
    public bool _dead = false;
    public bool randomState = false;
    public bool destroyAfterAni = true;
    public bool deactivateAfterAni = false;
    private bool connectToMove = false;
    private float random = 1f;
    private bool resetDead = false;
    private Rigidbody rb;
    private Vector3 originalPos;
    private void Start()
    {
        deerAni = GetComponent<Animator>();
        rb = transform.GetComponent<Rigidbody>();
        if (_eat) connectToMove = true;
        if (_dead) resetDead = true;
        originalPos = transform.position;
    }

    private void Update()
    {
        if (_eat)
        {
            StartCoroutine(DeerEatsKey());
            _eat = false;
        }
        if (_move)
        {
            StartCoroutine(DeerMove());
            Quaternion lookAt = Quaternion.LookRotation(targetPosition[PositionIter] - transform.position);
            transform.rotation = Quaternion.Lerp(transform.rotation, lookAt, Time.smoothDeltaTime);
        }
        if (_run)
        {
            StartCoroutine(DeerRun());
            Quaternion lookAt = Quaternion.LookRotation(targetPosition[PositionIter] - transform.position);
            transform.rotation = Quaternion.Lerp(transform.rotation, lookAt, Time.smoothDeltaTime);
        }
        if (_dead)
        {
            StartCoroutine(DeerDie());
            _dead = false;
        }
    }

    void FixedUpdate()
    {

        if (_move || _run)
        {
            Vector3 movePosition = Vector3.Slerp(transform.position, targetPosition[PositionIter], speed * Time.fixedDeltaTime);
            rb.MovePosition(movePosition);
            //Quaternion deltaRotation = Quaternion.Euler(new Vector3(75,0,0) * Time.deltaTime);
            //rb.MoveRotation(rb.rotation * deltaRotation);
            if(Vector3.Distance(transform.position, targetPosition[PositionIter]) <= 0.8f)
            {
                PositionIter++;
                _run = false;
                StartCoroutine(DeerEatsKey());
                if (PositionIter == targetPosition.Length)
                {
                    _move = false;
                    if (destroyAfterAni) Destroy(this.gameObject);
                    PositionIter = 0;
                    transform.position = targetPosition[PositionIter];
                    if (deactivateAfterAni) this.gameObject.SetActive(false);
                    RandomizeTargetLocations();
                }
            }
        }
    }


    IEnumerator DeerEatsKey()
    {
        deerAni.Play("Eat");
        yield return new WaitForSeconds(1.5f);
        if(Key != null) Destroy(Key);
        yield return new WaitForSeconds(1f);
        if (connectToMove) _move = true;
        else _run = true;
        if (randomState)
        {
            random = Random.Range(0.0f, 1.0f);
        }
    }

    IEnumerator DeerMove()
    {
        deerAni.Play("Locomotion");
        yield return new WaitForSeconds(.5f);
    }

    IEnumerator DeerRun()
    {
        if (random >= 0.5f)
        {
            deerAni.Play("Run");
            speed = 1f;
        }
        else
        {
            deerAni.Play("Locomotion");
            speed = 0.6f;
        }
        yield return new WaitForSeconds(.5f);
    }

    IEnumerator DeerDie()
    {
        deerAni.Play("Idle");
        yield return new WaitForSeconds(.5f);
    }

    void RandomizeTargetLocations()
    {
        for(int i = 0; i < targetPosition.Length; i++)
        {
            targetPosition[i] = new Vector3(targetPosition[i].x + Random.Range(-1.0f, 1.0f), targetPosition[i].y, targetPosition[i].z + Random.Range(-1.0f, 1.0f));
        }
    }

    public void ResetPosition()
    {
        transform.position = targetPosition[0];
        PositionIter = 0;
        if (resetDead) _dead = true;
        _run = true;
    }


}
