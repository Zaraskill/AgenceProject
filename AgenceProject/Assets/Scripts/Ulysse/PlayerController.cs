using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float throwForce;

    private Rigidbody2D _rb;
    private Vector2  _startPosition, _currentPosition, _direction;
    private float _currentForce;
    private bool _throwAllowed = true;

    // Start is called before the first frame update
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch t = Input.GetTouch(0);
            _currentPosition = Input.touches[0].position;

            if (t.phase == TouchPhase.Began)
            {
                _startPosition = Input.touches[0].position;
            }
            else if (t.phase == TouchPhase.Moved)
            {
                _direction = (_startPosition - _currentPosition).normalized;
                print((_startPosition - _currentPosition).magnitude);
                _currentForce = Mathf.Clamp((_startPosition - _currentPosition).magnitude/10, 0, 10);
            }
            else if (t.phase == TouchPhase.Ended && _throwAllowed)
            {
                _rb.isKinematic = false;
                //_rb.AddForce(_direction.normalized * throwForce, ForceMode2D.Impulse);
                _rb.AddForce(_direction * _currentForce * throwForce, ForceMode2D.Impulse);
                //_throwAllowed = false;
                _startPosition = Vector2.zero;
                //print("Dir " + _direction);
                //print("Force " + _currentForce);
            }
        }
    }
}
