using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private Rigidbody rigidBody;
    [Header("--- ��{���� ---")]
    [SerializeField] public float speed;
    public void PlayerMovement()
    {
        float x = Input.GetAxisRaw("Horizontal"); // x�����̃L�[����
        float z = Input.GetAxisRaw("Vertical"); // z�����̃L�[����
        Vector3 Player_movedir = new Vector3(x, rigidBody.velocity.y, z); // ���K��
        Player_movedir = this.transform.forward * z + this.transform.right * x;
        Player_movedir = Player_movedir.normalized;
        rigidBody.velocity = new Vector3(Player_movedir.x * speed, rigidBody.velocity.y, Player_movedir.z * speed);
    }
}
