using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] float baseSpeed = 1.0f;

    // Damage per hit
    [SerializeField] int power = 1;

    private Rigidbody rb;
    private Vector3 _startPosition;
    private float _currentSpeed = 0.0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        GameEvents.self.OnStartGame += StartGame;
        GameEvents.self.OnReturnToStart += ReturnToStart;
        GameEvents.self.OnIncreaceDifficulty += IncreaceDifficulty;
        _startPosition = transform.position;
    }

    void Update()
    {
        if (SceneBoundaries.self.EscapedBottom(transform.position))
        {
            Die();
        }
    }

    void FixedUpdate()
    {
        float x, y;
        if (rb.velocity.x > 0.0f) x = 1.0f;
        else x = -1.0f;

        if (rb.velocity.y > 0.0f) y = 1.0f;
        else y = -1.0f;
        rb.velocity = new Vector3(x, y, 0.0f).normalized * _currentSpeed;
    }


    private void OnCollisionEnter(Collision collision)
    {
        var brick = collision.gameObject.GetComponent<Brick>();
        brick?.ReceiveHit(power);
    }

    private void Die()
    {
        Debug.Log("Dead");
        GameEvents.self.ShowDeathScreen(true);
        Destroy(this.gameObject);
    }


    public void StartGame()
    {
        _currentSpeed = baseSpeed;
        rb.velocity = new Vector3(1f, 1f, 0).normalized * _currentSpeed;
    }

    public void ReturnToStart()
    {
        transform.position = _startPosition;
        rb.velocity = new Vector3(1f, 1f, 0).normalized *  _currentSpeed;
    }
    public void IncreaceDifficulty()
    {
        _currentSpeed *= 1.2f;
    }
}
