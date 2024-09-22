using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMain : MonoBehaviour
{
    [SerializeField] private PlayerMove move;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        move.PlayerMovement();
    }
}
