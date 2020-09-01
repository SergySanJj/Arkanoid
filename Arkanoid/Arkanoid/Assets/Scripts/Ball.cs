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


    private Vector3 _startPosition;
    
    private Vector3 _direction= Vector3.zero;
    private float _currentSpeed = 0;

    private Vector3 xAxis = new Vector3(1f, 0f, 0f);
    private Vector3 yAxis = new Vector3(0f, 1f, 0f);

    private int _stuckCount = 0;


    void Start()
    {
        GameEvents.self.OnStartGame += StartGame;
        GameEvents.self.OnReturnToStart += ReturnToStart;
        _startPosition = transform.position;
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
                _direction.z = 0;
                FixBadAngle();
                transform.position = SceneBoundaries.self.LimitAll(newPos, offset);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        _direction = Vector3.Reflect(_direction, collision.GetContact(0).normal).normalized;
        _direction.z = 0;
        FixBadAngle();

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
            Debug.Log("Stuck");
            _stuckCount++;
            if (_stuckCount > 5)
            {
                _direction = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0f).normalized;
                _stuckCount = 0;
                _currentSpeed = baseSpeed;
            } if (_stuckCount > 3)
            {
                _currentSpeed = baseSpeed * 1.2f;
            }
        }
    }

    public void StartGame()
    {
        _direction = new Vector3(0f, 1f, 0).normalized;
        _currentSpeed = baseSpeed;
    }
    public void ReturnToStart()
    {
        transform.position = _startPosition;
        _direction = new Vector3(0f, 1f, 0).normalized;
        _currentSpeed = baseSpeed;
    }
}
