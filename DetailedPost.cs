using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DetailedPost : MonoBehaviour
{
    public string id;
    public string title;
    public string text;
    public string imageURL;
    public string ticketURL;
    public string likeRate;
    public string author;
    public string admin;

    [Header("UI variables")]
    public Text naslov;
    public Text avtor;
    public Image slika;
    public Text besedilo;

    void Start() {
        
    }
    

    public void AssignValues(string id,string title, string text, string imageURL, string ticketURL, string likeRate, string author, string admin)
    {
        this.id = id;
        this.title = title;
        this.text = text;
        this.imageURL = imageURL;
        this.ticketURL = ticketURL;
        this.likeRate = likeRate;
        this.author = author;
        this.admin = admin;

        refreshView();
    }

    public void refreshView() {
        naslov.text = title;
        avtor.text = author;
        StartCoroutine(downloadImage(slika));
        besedilo.text = text;
    }

    IEnumerator downloadImage(Image wSlika) {
        using (WWW www = new WWW(imageURL)) {
            yield return www;

            wSlika.sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0));

        }
    }



    public void loadComments()
    {
        if (admin == "1")
        {
            GameObject.Find("ConnectionManager").GetComponent<MysqlManager>().getAllAdminComments(this);
            GameObject.Find("ConnectionManager").GetComponent<ScreenChanger>().switchAdminPostToComments();
        }
        else {
            GameObject.Find("ConnectionManager").GetComponent<MysqlManager>().getAllComments(this);
            GameObject.Find("ConnectionManager").GetComponent<ScreenChanger>().switchUserPostToComments();
        }
        

    }

    public void vstopnice()
    {
        Application.OpenURL(this.ticketURL);
    }

    public void addCommentBtn() {
        if (admin == "1")
        {
            GameObject.Find("ConnectionManager").GetComponent<MysqlManager>().addCommentAdmin(int.Parse(id));

        }
        else {
            GameObject.Find("ConnectionManager").GetComponent<MysqlManager>().addComment(int.Parse(id));

        }
    }


}
