
using System;
using UnityEngine;

public class UpdatePos:MonoBehaviour
{
    private UpdatePosRequest updatePosRequest;
    private Transform gunTransform;
    private SpriteRenderer sr;
    private void Start()
    {
        updatePosRequest = gameObject.GetComponent<UpdatePosRequest>();
        gunTransform = transform.Find("player/HandGun");
        sr = gunTransform.gameObject.GetComponent<SpriteRenderer>();
        InvokeRepeating("UpdatePosFun",1f,1f/60f);//1s60帧
    }

    private void UpdatePosFun()
    {
        Vector2 pos=transform.position;
        float characterRot=gunTransform.eulerAngles.z;
        float gunRot=gunTransform.eulerAngles.z;
        bool flipY = sr.flipY;
        updatePosRequest.SendRequest(pos,characterRot, gunRot,flipY);
    }
}
