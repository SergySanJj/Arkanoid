using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] float baseSpeed = 1.0f;
    [SerializeField] float offset = 0.1f;
    [SerializeField] private float angleChangeEps = 0.1f;

    // Damage per hit
    [SerializeField] int power = 1;
    
    private Vector3 _direction= Vector3.zero;
    private float _currentSpeed = 0;

    private Vector3 xAxis = new Vector3(1f, 0f, 0f);
    private Vector3 yAxis = new Vector3(0f, 1f, 0f);


    void Start()
    {
        GameEvents.self.OnStartGame += StartGame;
    }

    void Update()
    {
        Move();
    }

    private void Move()
    {
        Vector3 newPos = transform.position + _direction * _currentSpeed * Time.deltaTime;
        if (SceneBoundaries.self.IsInsideAllBoundaries(newPos, offset))
        {
            transform.position = newPos;
        } else
        {
            if (SceneBoundaries.self.EscapedBottom(newPos, offset))
            {
                Die();
            } else
            {
                _direction = SceneBoundaries.self.Reflect(newPos, _direction, offset).normalized;
                transform.position = SceneBoundaries.self.LimitAll(newPos, offset);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        _direction = Vector3.Reflect(_direction, collision.GetContact(0).normal).normalized;
        _direction.z = 0;

        var brick = collision.gameObject.GetComponent<Brick>();
        brick?.ReceiveHit(power);
    }

    private void Die()
    {
        Debug.Log("Dead");
        GameEvents.self.ShowDeathScreen(true);
        Destroy(this.gameObject);
    }


    private void FixBadAngle()
    {
        if (Mathf.Abs(Vector3.Angle(_direction, xAxis)) < angleChangeEps || Mathf.Abs(Vector3.Angle(_direction, yAxis)) < angleChangeEps)
        {
            _direction = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0f).normalized;
        }
    }

    public void StartGame()
    {
        _direction = new Vector3(0.9f, 0.9f, 0).normalized;
        _currentSpeed = baseSpeed;
    }
}
