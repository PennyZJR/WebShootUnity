using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rd;
    private bool isGround;//是否解除地面
    private Transform gunPos;//开火点
    private Transform groundTransform;//检测是否与陆地有接触的位置
    private float groundCheckDistance ;//向下检测的距离
    [SerializeField]
    private LayerMask groundLayer;//地面的图层
    private void Start()
    {
        rd = GetComponent<Rigidbody2D>();
        gunPos = transform.Find("player").Find("HandGun").Find("Fire").transform;
        groundTransform = transform.Find("player").Find("isGround").transform;
        groundCheckDistance = 0.2f;
        groundLayer=LayerMask.GetMask("Ground");
    }

    private void Update()
    {
        GroundCheck();
        Move();
        Jump();
        
    }

    private void Move()
    {
        float h=Input.GetAxis("Horizontal");
        if(h!=0)
            rd.velocity=new Vector2(h*10,rd.velocity.y);
        else 
            rd.velocity=new Vector2(0,rd.velocity.y);
    }

    private void Jump()
    {
        if (isGround&&Input.GetKeyDown(KeyCode.Space))
            rd.velocity = new Vector2(rd.velocity.x, 15);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(groundTransform.position,groundTransform.position+Vector3.down*groundCheckDistance);
    }

    private void GroundCheck()
    {
        isGround = Physics2D.Raycast(groundTransform.position, Vector3.down, groundCheckDistance, groundLayer);
    }
}
