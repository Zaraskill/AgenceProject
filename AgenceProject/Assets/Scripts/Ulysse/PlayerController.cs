using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float throwForce;

    private Rigidbody2D _rb;
    private Vector2 _startPosition, _lastPosition, _currentPosition, _direction;
    private bool _throwAllowed = true;

    // Start is called before the first frame update
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _startPosition = transform.position;
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch t = Input.GetTouch(0);
            _currentPosition = Input.touches[0].position;

            if (t.phase == TouchPhase.Began)
            {
                _lastPosition = Input.touches[0].position;
            }
            else if (t.phase == TouchPhase.Moved)
            {
                _direction = _lastPosition - _currentPosition;
            }
            else if (t.phase == TouchPhase.Ended && _throwAllowed)
            {
                _direction = _lastPosition - _currentPosition;
                print(_direction);

                _lastPosition = Vector2.zero;
                _rb.isKinematic = false;
                _rb.AddForce(_direction.normalized * throwForce, ForceMode2D.Impulse);
                //_throwAllowed = false;
                print(_direction.normalized);
            }
        }
    }
}
