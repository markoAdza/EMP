using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Klubi : MonoBehaviour
{
    public string id;
    public string ImeKluba;
    public string followers;
    public bool isFollowed;
    public string StSledilcev;

    public Text t1;
    public Text t2;

    
    void Start()
    {
        
    }

    public void AssignValues(string id, string ime, bool followed,bool canInteract,string followers) {
        this.id = id;
        this.ImeKluba = ime;
        this.isFollowed = followed;
        this.StSledilcev = followers;
        GetComponent<Button>().interactable = canInteract;

        refresh();
    }

    public void refresh() {
        t1.text = ImeKluba;
        t2.text = StSledilcev;
        if (isFollowed)
            GetComponent<Image>().color =new Color32(255, 69, 0,255);
        else
            GetComponent<Image>().color =new Color32(206, 227, 248, 200);


    }

    public void followKlub() {
        if(isFollowed)
            GameObject.Find("ConnectionManager").GetComponent<MysqlManager>().neSlediKlub(Int16.Parse(id));
        else
            GameObject.Find("ConnectionManager").GetComponent<MysqlManager>().slediKlub(Int16.Parse(id));
    }
    
}
