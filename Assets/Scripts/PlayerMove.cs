/* 
PlayerMove.cs
By: Jake Schott
*/

using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    //CLASS CONSTANTS
    public float MOVE_SPEED = 5.0f;

    //[SerializeField]
    [SerializeField]
    private Vector2 moveDir = new Vector2();
    [SerializeField]
    private Rigidbody playerRB = null;

    private void Update()
    {
        moveDir.x = Input.GetAxis("Horizontal");
        moveDir.y = Input.GetAxis("Vertical");
        Debug.DrawLine(transform.position, transform.position + transform.forward * 1.25f);
        if (moveDir.magnitude > 1)
        {
            moveDir.Normalize();
        }
        Move();

        //teleport back if you fall
        if (transform.localPosition.y < -10)
        {
            transform.localPosition = Vector3.zero;
            playerRB.linearVelocity = Vector3.zero;
        }
    }

    void Move()
    {
        Vector3 movement; //= Vector3.zero;
        movement = transform.TransformDirection(new Vector3(moveDir.x, 0, moveDir.y)) * MOVE_SPEED * Time.deltaTime;
        transform.position += movement;

        if (moveDir == Vector2.zero)
        {
            playerRB.angularVelocity = Vector3.zero;
            playerRB.linearVelocity = Vector3.zero;
        }
    }
}
