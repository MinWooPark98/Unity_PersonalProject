using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogoMove : MonoBehaviour
{
    public float startPosYScreenRatio;
    public float endPosYScreenRatio;
    public float movingTime;

    private float xPos = Screen.width * 0.5f;
    private float startYPos;
    private float endYPos;
    private float deltaY;
    private float timer;
    private bool isMoving;

    void Start()
    {
        startYPos = Screen.height * startPosYScreenRatio;
        endYPos = Screen.height * endPosYScreenRatio;
        deltaY = (endYPos - startYPos) / movingTime;
        transform.position = new Vector3(xPos, startYPos, 0f);
        isMoving = false;
        timer = 0f;
    }

    void Update()
    {
        if (!isMoving)
            return;
        timer += Time.deltaTime;
        if (timer > movingTime)
        {
            transform.position = new Vector3(xPos, endYPos, 0f);
            isMoving = false;
            return;
        }
        transform.position = new Vector3(xPos, startYPos + deltaY * timer, 0f);
    }

    public void Move() => isMoving = true;
}
