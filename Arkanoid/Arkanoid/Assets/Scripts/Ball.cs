using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] float speed = 1.0f;
    [SerializeField] float offset = 0.1f;

    // Damage per hit
    [SerializeField] int power = 1;
    
    private Vector3 _direction;
    



    void Start()
    {
        _direction = new Vector3(1, 1, 0).normalized;
    }

    void Update()
    {
        Move();
    }

    private void Move()
    {
        Vector3 newPos = transform.position + _direction * speed * Time.deltaTime;
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
                _direction = SceneBoundaries.self.Reflect(newPos, _direction, offset);
                transform.position = SceneBoundaries.self.LimitAll(newPos, offset);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Change");
        _direction = Vector3.Reflect(_direction, collision.GetContact(0).normal);
        _direction.z = 0;

        var brick = collision.gameObject.GetComponent<Brick>();
        brick?.ReceiveHit(power);
    }

    private void Die()
    {
        Debug.Log("Dead");
        Destroy(this.gameObject);
    }
}
