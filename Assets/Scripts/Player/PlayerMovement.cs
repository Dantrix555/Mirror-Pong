using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

/// <summary>
/// Controls player movement and it's values
/// </summary>
public class PlayerMovement : NetworkBehaviour
{

    #region Fields and properties

    [Header("Physics components")]
    [SerializeField]
    private Rigidbody2D _rigidbody;

    [Space(5)]
    [Header("Player properties")]
    [SerializeField]
    private float _speed;

    #endregion

    private void FixedUpdate()
    {
        if (!isLocalPlayer)
            return;

        _rigidbody.velocity = new Vector2(0, Input.GetAxis("Vertical") * _speed);
    }
}
