using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Comment : MonoBehaviour
{
    private string id;
    private string author;
    private string text;
    private string timestamp;

    public Text avtor;
    public Text besedilo;
    public Text datum;

    public void SetData(string id, string author, string text, string timestamp, bool canDelete)
    {
        this.id = id;
        this.author = author;
        this.text = text;
        this.timestamp = timestamp;

        GetComponent<Button>().interactable = canDelete;
        refresh();
    }
    void refresh() {
        avtor.text = author;
        besedilo.text = text;
        datum.text = timestamp;
    }


    public void delete() {
        

        GameObject.Find("ConnectionManager").GetComponent<MysqlManager>().alertComment(int.Parse(id));
        refresh();
    }
}
