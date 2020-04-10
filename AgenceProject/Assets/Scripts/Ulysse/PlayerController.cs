using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float throwForce, magnitudeMin, magnitudeMax;
    public PlayerState playerState;

    private Rigidbody2D _rb;
    private Vector2  _startPosition, _currentPosition, _direction;
    private float _magnitude;
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
                ResetValues();
            }
            else if (t.phase == TouchPhase.Moved)
            {
                _direction = (_startPosition - _currentPosition).normalized;
                GetCurrentMagnitude();
            }
            else if (t.phase == TouchPhase.Ended && _throwAllowed)
            {
                if (_magnitude > 0)
                {
                    _rb.AddForce(_direction * _magnitude * throwForce, ForceMode2D.Impulse);
                }
                //_throwAllowed = false;
                print("Dir " + _direction);
                print("Magnitude " + _magnitude);
            }
        }
    }

    private void ResetValues()
    {
        _magnitude = 0;
        _direction = Vector2.zero;
    }

    void GetCurrentMagnitude()
    {
        float rawMagnitude = (_startPosition - _currentPosition).magnitude / 10;
        if (rawMagnitude > magnitudeMin)
        {
            _magnitude = (Mathf.Clamp(rawMagnitude, 0, magnitudeMax) / magnitudeMax);
            playerState = PlayerState.charging;
        }
        else
            _magnitude = 0;
    }

    #region Debug
    void OnGUI()
    {
        GUILayout.Label("Force : " + (_magnitude * 100) + "%");
    }
    #endregion
}

public enum PlayerState
{
    idle,
    charging,
    moving,
}
