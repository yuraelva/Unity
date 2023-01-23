using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    public GameObject playerObj;
    PlayerController player;
    
    Transform playerTransform;
    public int stageMaxX=1000,stageMaxY=1000;
    public bool MoveFlag = true;
    int xflag, yflag;
    void Start()
    {
        
        player = playerObj.GetComponent<PlayerController>();
        playerTransform = playerObj.transform;
    }
    void LateUpdate()
    {

        MoveCamera();
        //MoveFlag = false; 
    }
    void MoveCamera()
    {
        /*”­•\‚Å‚Í¡‚Ì‚Æ‚±‚ë“®‚©‚³‚È‚¢—\’è*/
        
        Vector2 vec = new Vector2(playerObj.transform.position.x, playerObj.transform.position.y);
        xflag = 0; yflag = 0;
       
        if (vec.x > -50 && vec.x < stageMaxX) xflag = 1;
        if (vec.y > 0 && vec.y < stageMaxY) yflag = 1;
        //‰¡•ûŒü‚¾‚¯’Ç]
        if (MoveFlag)transform.position = new Vector3(playerTransform.position.x * xflag + 4, playerTransform.position.y * yflag + 3, transform.position.z);
    }
}