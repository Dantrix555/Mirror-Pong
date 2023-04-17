using System.Collections;
using UnityEngine;
using Mirror;

/// <summary>
/// Controls base ball move and collision behaviours
/// </summary>
public class BallController : NetworkBehaviour
{

    #region Fields and properties

    [Header("Physics components")]
    [SerializeField]
    private Rigidbody2D _rigidbody;

    [Space(5)]
    [Header("Ball properties")]
    [SerializeField]
    private float _speed;
    [SerializeField]
    [Range(0f, 0.35f)]
    private float yAxisDeviation;

    #endregion

    #region Public Methods

    /// <summary>
    /// Init and Setup ball
    /// </summary>
    [ServerCallback]
    public void SetupBall()
    {
        StartCoroutine(StartBallMove());
    }

    #endregion

    #region Inner Methods

    /// <summary>
    /// Calculate new ball direction based on hit position and racket height
    /// </summary>
    /// <param name="ballPosition">Ball actual position</param>
    /// <param name="racketPostion">Racket collision position</param>
    /// <param name="racketHeight">Racket Height</param>
    /// <returns>New ball direction</returns>
    private float HitFactor(Vector2 ballPosition, Vector2 racketPostion, float racketHeight) => ((ballPosition.y - racketPostion.y) / racketHeight) + Random.Range(-yAxisDeviation, yAxisDeviation);

    /// <summary>
    /// Pause ball movement for 2 seconds and then starts ball movement
    /// </summary>
    /// <returns></returns>
    private IEnumerator StartBallMove()
    {
        transform.position = Vector3.zero;

        _rigidbody.simulated = false;
        _rigidbody.velocity = Vector2.zero;
        
        yield return new WaitForSeconds(2f);

        _rigidbody.simulated = true;
        Vector2 initialDirection = Random.Range(0f, 1f) > 0.5f ? Vector2.right : Vector2.left;
        _rigidbody.velocity = initialDirection * _speed;
    }

    #endregion

    #region Unity Methods

    [ServerCallback]
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Racket")
        {
            _rigidbody.velocity = Vector2.Reflect(_rigidbody.velocity, collision.GetContact(0).normal).normalized * _speed;

            float x = collision.relativeVelocity.x > 0f ? 1f : -1f;
            float y = HitFactor(transform.position, collision.transform.position, collision.collider.bounds.size.y);
            Vector3 ballDirection = new Vector2(x, y).normalized;

            _rigidbody.velocity = ballDirection * _speed;
        }
    }

    #endregion

}
