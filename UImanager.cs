using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UImanager : MonoBehaviour
{

    public Transform SideMenu;
    public Vector2 targetPosX;
    public bool closed;
    public float speed;

    public void Start()
    {
        closed = false;
    }

    public void Update()
    {
        float tgt;
        if (!closed)
            tgt = targetPosX.x;
        else
            tgt = targetPosX.y;

        SideMenu.localPosition = new Vector3(Mathf.Lerp(SideMenu.localPosition.x,tgt,speed), SideMenu.localPosition.y, SideMenu.localPosition.z);

    }


    public void Menu()
    {
        closed = true;
    }

    public void CloseMenu()
    {
        closed = false;

    }


}
