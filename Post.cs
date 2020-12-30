using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Post : MonoBehaviour
{
    public string id;
    public string title;
    public string text;
    public string imageURL;
    public string ticketURL;
    public string likeRate;
    public string author;
    public string stkoment;
    public string imeKluba;
    public string admin;


    [Header("UI variables")]
    public Text naslov;
    public Image slika;
    public Text score;
    public Text comment;
    public Button like;
    public Button dislike;
    public Text klub;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(ShowBigScreen);
        


    }

    void ShowBigScreen() {
        Debug.Log(admin);
        if (admin == "1")
        {
            GameObject.Find("ConnectionManager").GetComponent<ScreenChanger>().switchPostsToPostAdmin();
            GameObject.Find("ConnectionManager").GetComponent<ScreenChanger>().postScreenAdmin.GetComponent<DetailedPost>().AssignValues(id, title, text, imageURL, ticketURL, likeRate, imeKluba, admin);
        }
        else {
            GameObject.Find("ConnectionManager").GetComponent<ScreenChanger>().switchPostsToPost();
            GameObject.Find("ConnectionManager").GetComponent<ScreenChanger>().userPost.GetComponent<DetailedPost>().AssignValues(id, title, text, imageURL, ticketURL, likeRate, imeKluba, admin);
        }
        
    }

    public void AssignValues(string id,string title, string text, string imageURL, string ticketURL, string likeRate, string author, bool shouldBeActive, string stkoment, string imeKluba, string admin)
    {
        this.id = id;
        this.title = title;
        this.text = text;
        this.imageURL = imageURL;
        this.ticketURL = ticketURL;
        this.likeRate = likeRate;
        this.author = author;
        this.stkoment = stkoment;
        this.imeKluba = imeKluba;
        this.admin = admin;
        like.interactable = shouldBeActive;
        dislike.interactable = shouldBeActive;

        refreshView();
    }

    public void refreshView() {
        if (!gameObject.active)
            return;

        naslov.text = title;
        StartCoroutine(downloadImage(slika));
        score.text = likeRate;
        comment.text = stkoment + " KOMENTARJEV";
        klub.text = imeKluba;

    }
    
    IEnumerator downloadImage(Image wSlika) {
        using (WWW www = new WWW(imageURL)) {
            yield return www;
            wSlika.sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0));

        }
    }

    public void hitLike() {
        GameObject.Find("ConnectionManager").GetComponent<MysqlManager>().like(1,int.Parse(id));
        
    }
    public void hitDislike()
    {
        GameObject.Find("ConnectionManager").GetComponent<MysqlManager>().like(-1,int.Parse(id));
    }



    public void deletePost()
    {
        GameObject.Find("ConnectionManager").GetComponent<MysqlManager>().AdminDeletePost(int.Parse(id));
    }

    public void alertPost()
    {

        GameObject.Find("ConnectionManager").GetComponent<MysqlManager>().alertPost(int.Parse(id));
    }



}
