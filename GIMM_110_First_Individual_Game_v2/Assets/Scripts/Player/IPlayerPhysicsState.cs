using UnityEngine;

public interface IPlayerPhysicsState
{
    void FixedUpdate();
    void OnCollisionEnter2D(Collision2D collision);
}
