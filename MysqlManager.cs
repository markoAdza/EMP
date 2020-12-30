using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MySql.Data;
using MySql.Data.MySqlClient;
using System;

public class MysqlManager : MonoBehaviour
{
    
    public string host, database, user, password;
    private int userId;
    private int adminKlubId;
    private int klubID;

    private DetailedPost savePost;

    private string connectionString;
    private MySqlConnection conn;
    private MySqlCommand cmd;
    private MySqlDataReader rdr;


    [Header("Alert")]
    public int alertId = 0;
    public int alertCommentId = 0;

    [Header("Login spremenljivke")]
    public InputField loginUsername;
    public InputField loginPassword;

    [Header("Register spremenljivke")]
    public InputField registerIme;
    public InputField registerPriimek;
    public InputField registerUsername;
    public InputField registerPassword;
    public InputField registerStarost;


    [Header("Dodaj objavo spremenljivke")]
    public InputField naslov;
    public InputField besedilo;
    public InputField slika;
    public InputField karta;

    [Header("Dodaj komentar spremenljivke")]
    public InputField commentUser;
    public InputField commentAdmin;

    [Header("Seznam objav")]
    public GameObject nextPost;
    public GameObject PostContainer;
    public GameObject FollowedPostContainer;
    public GameObject PostContainerAdmin;
    public GameObject TopLikedPostsContainer;
    public GameObject guestPostContainer;
    public GameObject postPrefab;
    public GameObject postAdminPrefab;
    public GameObject IzbraniPostContainer;


    [Header("Seznam klubov")]
    public GameObject ClubContainer;
    public GameObject ClubPrefab;

    [Header("Komentarji")]
    public GameObject CommentPrefab;
    public GameObject CommentContainer;
    public GameObject CommentContainerAdmin;

    [Header("Side menu")]
    public Text imeKluba;
    public Text steviloSledilcev;
    public Text steviloObjav;
    public Text imePriimek;

    [Header("Gumbi")]
    public Button toplike;
    public Button follow;
    public Dropdown DropdownRegija;
    public Dropdown DropdownIzbrani;

    

    void Start()
    {

    }

    public void GuestLogin() {

        userId = -1;
        toplike.interactable = false;
        follow.interactable = false;

        GetComponent<ScreenChanger>().SwitchLoginToUserHome();
        getAllPosts();
        getAllClubs();


    }

    

    private void ConnectToDB() {
        try
        {
            connectionString = "Server=" + host + ";Database=" + database + ";User=" + user + ";Password=" + password;
            conn = new MySqlConnection(connectionString);
            conn.Open();
            Debug.Log("Connection open");
        }
        catch (Exception ex) {
            Debug.Log(ex);
        }
    }

    private void DisconnectFromDB() {

        if (conn != null) {
            if (conn.State.ToString() != "Closed") {
                conn.Close();
                Debug.Log("Connection closed");
            }
            conn.Dispose();
        }

    }

    public string GetConnectionState() {
        return conn.State.ToString();
    }

    public void login()
    {
        ConnectToDB();


        string sqlQuery = "select *, klub.* from user_data join admin_has_klub on user_data_id = user_data.id join klub on klubId = klub_id where username like '" + loginUsername.text + "' and password like md5('" + loginPassword.text +"');";
        cmd = new MySqlCommand(sqlQuery, conn);

        rdr = cmd.ExecuteReader();
        
        while (rdr.Read()) {
            //SUCCESSFULL LOGIN
            SSTools.ShowMessage("Uspešna prijava", SSTools.Position.bottom, SSTools.Time.twoSecond);
            Debug.Log("Logged in with admin permissions = "+ rdr["admin"].ToString());
            userId = Int16.Parse(rdr["id"].ToString());
            

            if (rdr["admin"].ToString() == "0")
            {
                nextPost.SetActive(true);
                GetComponent<ScreenChanger>().SwitchLoginToUserHome();
                getAllPosts();
                getAllClubs();
            }
            else
            {
                nextPost.SetActive(false);
                adminKlubId = Int16.Parse(rdr["admin"].ToString());
                klubID = Int16.Parse(rdr["klubId"].ToString());
                GetComponent<ScreenChanger>().switchLoginToAdminHome();
                getAllAdminPosts();
                getAllClubs();
            }
            DisconnectFromDB();
            loginUsername.text = "";
            loginPassword.text = "";
            return;
        }
        //FAILED LOGINs
        Debug.Log("Failed to login");
        SSTools.ShowMessage("Napačno ime ali geslo", SSTools.Position.bottom, SSTools.Time.twoSecond);
        DisconnectFromDB();
    }



    public void register()
    {
        if (registerIme.text.Length < 3)
        {
            SSTools.ShowMessage("Neustrezno ime", SSTools.Position.bottom, SSTools.Time.twoSecond);            
        }
        else if (registerPriimek.text.Length < 3) 
        {
            SSTools.ShowMessage("Neustrezen priimek", SSTools.Position.bottom, SSTools.Time.twoSecond);            
        }
        else if (registerStarost.text == "")
        {
            SSTools.ShowMessage("Prosimo dodajte še starost", SSTools.Position.bottom, SSTools.Time.twoSecond);            
        }
        else if (registerUsername.text.Length < 6)
        {
            SSTools.ShowMessage("Uporabniško ime je prekratko", SSTools.Position.bottom, SSTools.Time.twoSecond);            
        }
        else if (registerPassword.text.Length < 6)
        {
            SSTools.ShowMessage("Geslo je prekratko", SSTools.Position.bottom, SSTools.Time.twoSecond);            
        }
        else
        {
            ConnectToDB();
            int nula = 0;

            string sqlQuery = "insert into user_data (ime,priimek,starost,username,password,admin) values" +
            " ('" + registerIme.text + "','" + registerPriimek.text + "','" + registerStarost.text + "','" + registerUsername.text + "',md5('" + registerPassword.text + "'),'" + nula + "');";

            cmd = new MySqlCommand();
            cmd.CommandText = sqlQuery;
            cmd.Connection = conn;

            if (cmd.ExecuteNonQuery() > 0)
            {
                registerIme.text = "";
                registerPriimek.text = "";
                registerStarost.text = "";
                registerUsername.text = "";
                registerPassword.text = "";
                SSTools.ShowMessage("Uspešno registrirani", SSTools.Position.bottom, SSTools.Time.twoSecond);
                Debug.Log("Succesfully registered");
                GetComponent<ScreenChanger>().switchFromRegisterToLogin();
            }
            else
            {
                SSTools.ShowMessage("Napaka pri registraciji!", SSTools.Position.bottom, SSTools.Time.twoSecond);
                Debug.Log("Register failed!");
            }

            DisconnectFromDB();

        }
    }

    public void getAllAdminPosts()
    {


        ConnectToDB();

        string sqlQuery = "select posts.*, klub.imeKluba from posts join klub on posts.klub_id = klub.klubId order by id desc;";
        cmd = new MySqlCommand(sqlQuery, conn);

        rdr = cmd.ExecuteReader();

        foreach (Transform obj in PostContainerAdmin.transform)
        {
            Destroy(obj.gameObject);
        }

        bool first = true;

        while (rdr.Read())
        {
            if (first && GameObject.Find("nextEvent") != null) {
                first = !first;
                GameObject.Find("nextEvent").GetComponent<MiniPost>().AssignValues(rdr[0].ToString(), rdr[1].ToString(), rdr[4].ToString(), rdr["imeKluba"].ToString());
            }

            GameObject tmp = Instantiate(postAdminPrefab, PostContainerAdmin.transform);
            tmp.GetComponent<Post>().AssignValues(rdr[0].ToString(), rdr[1].ToString(), rdr[2].ToString(), rdr[3].ToString(), rdr[4].ToString(), rdr[5].ToString(), rdr[6].ToString(), true, rdr[7].ToString(), rdr["imeKluba"].ToString(), adminKlubId.ToString());

        }
        DisconnectFromDB();
    }

    public void getAllPosts() {

        ConnectToDB();

        string sqlQuery = "select posts.*, klub.imeKluba from posts join klub on posts.klub_id = klub.klubId order by id desc;";
        cmd = new MySqlCommand(sqlQuery, conn);

        rdr = cmd.ExecuteReader();

        foreach (Transform obj in PostContainer.transform)
        {
            Destroy(obj.gameObject);
        }

        bool first = true;
        while (rdr.Read())
        {
            if (first && GameObject.Find("nextEvent") != null)
            {
                first = !first;
                GameObject.Find("nextEvent").GetComponent<MiniPost>().AssignValues(rdr[0].ToString(), rdr[1].ToString(), rdr[4].ToString(), rdr["imeKluba"].ToString());
            }

            GameObject tmp = Instantiate(postPrefab, PostContainer.transform);
            tmp.GetComponent<Post>().AssignValues(rdr[0].ToString(), rdr[1].ToString(), rdr[2].ToString(), rdr[3].ToString(), rdr[4].ToString(), rdr[5].ToString(), rdr[6].ToString(), true, rdr[7].ToString(), rdr["imeKluba"].ToString(), adminKlubId.ToString());

        }



        DisconnectFromDB();
    }

    public void getAllClubs()
    {


        ConnectToDB();

        string sqlQuery = "select * from klub;";
        cmd = new MySqlCommand(sqlQuery, conn);

        rdr = cmd.ExecuteReader();

        foreach (Transform obj in ClubContainer.transform)
        {
            Destroy(obj.gameObject); /* izbrise klube ki so se podvojili */
        }

        while (rdr.Read())
        {
            MySqlConnection con2 = new MySqlConnection(connectionString);
            con2.Open();
            string quer2 = "select * from user_data_has_klub where user_data_id = '" + userId + "' and klub_id = '" + rdr["klubId"].ToString() + "';";
            MySqlCommand cmd2 = new MySqlCommand(quer2, con2);
            bool isFollowed = false;

            MySqlDataReader rdr2 = cmd2.ExecuteReader();

            
                
            while (rdr2.Read()){
                isFollowed = true;
            }

            con2.Close();
            GameObject tmp = Instantiate(ClubPrefab, ClubContainer.transform);
            tmp.GetComponent<Klubi>().AssignValues(rdr["klubId"].ToString(), rdr["imeKluba"].ToString(),isFollowed, (userId > -1),rdr["followers"].ToString());

        }
        DisconnectFromDB();
    }

    public void addPost()
    {
     

        if (karta.text.Substring(0, 5) != "https" && slika.text.Substring(0, 5) != "https")
        {
            SSTools.ShowMessage("Neustrezen link povezave", SSTools.Position.bottom, SSTools.Time.twoSecond);
            return;
        }

        ConnectToDB();
        Debug.Log(klubID);

        string sqlQuery = "insert into posts (title,description,imageURL,ticketURL,klub_id) values" +
        " ('" + naslov.text + "','" + besedilo.text + "','" + slika.text + "','" + karta.text + "','" + klubID + "');";

        cmd = new MySqlCommand();
        cmd.CommandText = sqlQuery;
        cmd.Connection = conn;

        if (cmd.ExecuteNonQuery() > 0)
        {
            naslov.text = "";
            besedilo.text = "";
            slika.text = "";
            karta.text = "";

            Debug.Log("Succesfully added post");
            GetComponent<ScreenChanger>().switchAddPostsToAdminHome();
            getAllAdminPosts();
        }
        else
        {
            Debug.Log("Adding failed!");
        }

        DisconnectFromDB();


    }

    public void slediKlub(int klubId)
    {
        ConnectToDB();


        string sqlQuery = "insert into user_data_has_klub (user_data_id,klub_id) values" +
        " ('" + userId + "','" + klubId + "');";

        cmd = new MySqlCommand();
        cmd.CommandText = sqlQuery;
        cmd.Connection = conn;
        if (cmd.ExecuteNonQuery() > 0)
        {
            Debug.Log("Succesfully added follow");
            Firebase.Messaging.FirebaseMessaging.SubscribeAsync(klubId.ToString());
        }
        else
        {
            Debug.Log("Adding failed failed!");
        }

        string sqlQuery1 = "update klub set followers=followers+1 where klubId= '" + klubId + "';";

        cmd = new MySqlCommand();
        cmd.CommandText = sqlQuery1;
        cmd.Connection = conn;
        if (cmd.ExecuteNonQuery() > 0)
        {
            Debug.Log("+1");
        }
        else
        {
            Debug.Log("failed!");
        }

        DisconnectFromDB();
        getAllClubs();
    }

    
    public void neSlediKlub(int klubId)
    {
        ConnectToDB();

        string sqlQuery = "delete from user_data_has_klub WHERE" +" user_data_id = '" + userId + "' AND klub_id = '" + klubId + "';";

        cmd = new MySqlCommand();
        cmd.CommandText = sqlQuery;
        cmd.Connection = conn;
        if (cmd.ExecuteNonQuery() > 0)
        {
            Debug.Log("Succesfully unfollowed club");
            Firebase.Messaging.FirebaseMessaging.UnsubscribeAsync(klubId.ToString());
        }
        else
        {
            Debug.Log("unfollow failed");
        }

        string sqlQuery1 = "update klub set followers=followers-1 where klubId= '" + klubId + "';";

        cmd = new MySqlCommand();
        cmd.CommandText = sqlQuery1;
        cmd.Connection = conn;
        if (cmd.ExecuteNonQuery() > 0)
        {
            Debug.Log("-1");
        }
        else
        {
            Debug.Log("failed!");
        }

        DisconnectFromDB();
        getAllClubs();
    }
    


    public void getAllFollowedClubs()
    {
        ConnectToDB();

        foreach (Transform obj in FollowedPostContainer.transform)
        {
            Destroy(obj.gameObject);
        }

        string sqlQuery = "select posts.*, klub.imeKluba from posts join klub on posts.klub_id = klub.klubId join user_data_has_klub on klub.klubId = user_data_has_klub.klub_id join user_data on user_data_id = user_data.id where user_data.id = '" + userId+"';";
        cmd = new MySqlCommand(sqlQuery, conn);
        rdr = cmd.ExecuteReader();

        foreach (Transform obj in FollowedPostContainer.transform)
        {
            Destroy(obj.gameObject);
        }

        while (rdr.Read())
        {
            GameObject tmp = Instantiate(postPrefab, FollowedPostContainer.transform);
            tmp.GetComponent<Post>().AssignValues(rdr[0].ToString(), rdr[1].ToString(), rdr[2].ToString(), rdr[3].ToString(), rdr[4].ToString(), rdr[5].ToString(), rdr[6].ToString(), true, rdr[7].ToString(), rdr["imeKluba"].ToString(), adminKlubId.ToString());

        }
        DisconnectFromDB();
    }

    public void deleteLike(int postId)
    {
        ConnectToDB();
        string sqlQuery = "DELETE FROM likes WHERE '"+userId+"' LIKE likes.user_data_id AND '"+postId+"' = likes.posts_id;";
        cmd = new MySqlCommand();
        cmd.CommandText = sqlQuery;
        cmd.Connection = conn;
        cmd.ExecuteNonQuery();
        DisconnectFromDB();
    }


    public void like(int vr, int postId)
    {
        deleteLike(postId);

        ConnectToDB();
        string sqlQuery = "insert into likes(addScore,user_data_id,posts_id) SELECT '" + vr + "','" + userId + "','" + postId + "';";
        cmd = new MySqlCommand();
        cmd.CommandText = sqlQuery;
        cmd.Connection = conn;

        if (cmd.ExecuteNonQuery() > 0)
        {
            Debug.Log("Succesfully added like/dislike");

            getTopLikedPosts();
            getAllFollowedClubs();
            getAllPosts();
        }
        else
        {
            Debug.Log("Adding failed!");
        }

        DisconnectFromDB();
    }
    
    public void getTopLikedPosts()
    {
        ConnectToDB();

        foreach (Transform obj in TopLikedPostsContainer.transform)
        {
            Destroy(obj.gameObject);
        }

        string sqlQuery = "select posts.*, klub.imeKluba from posts JOIN likes ON likes.posts_id = posts.id join klub on posts.klub_id = klub.klubId ORDER BY posts.score DESC LIMIT 5;";
        cmd = new MySqlCommand(sqlQuery, conn);
        rdr = cmd.ExecuteReader();

        while (rdr.Read())
        {
            GameObject tmp = Instantiate(postPrefab, TopLikedPostsContainer.transform);
            tmp.GetComponent<Post>().AssignValues(rdr[0].ToString(), rdr[1].ToString(), rdr[2].ToString(), rdr[3].ToString(), rdr[4].ToString(), rdr[5].ToString(), rdr[6].ToString(), true, rdr[7].ToString(), rdr["imeKluba"].ToString(), adminKlubId.ToString());

        }
        DisconnectFromDB();
    }

    public void getAllComments(DetailedPost objava) {

        if (savePost == null)
            savePost = objava;
        
        ConnectToDB();
        string sqlQuery = "select comments.id, text, timestamp, post_id, user_data_id, ime, priimek from comments join user_data on user_data_id = user_data.id where post_id = '"+ savePost.id+"'";
        cmd = new MySqlCommand(sqlQuery, conn);
        rdr = cmd.ExecuteReader();

        foreach (Transform item in CommentContainer.transform)
        {
            Destroy(item.gameObject);
        }

        while (rdr.Read()) {

            GameObject tmp = Instantiate(CommentPrefab, CommentContainer.transform);
            tmp.GetComponent<Comment>().SetData(rdr["id"].ToString(), rdr["ime"].ToString() + " " + rdr["priimek".ToString()], rdr["text"].ToString(), rdr["timestamp"].ToString(), (rdr["user_data_id"].ToString() == userId.ToString()));
        }
        DisconnectFromDB();
    }

    public void getAllAdminComments(DetailedPost objava)
    {

        if (savePost == null)
            savePost = objava;

        ConnectToDB();
        string sqlQuery = "select comments.id, text, timestamp, post_id, user_data_id, ime, priimek from comments join user_data on user_data_id = user_data.id where post_id = '" + savePost.id + "'";
        cmd = new MySqlCommand(sqlQuery, conn);
        rdr = cmd.ExecuteReader();

        foreach (Transform item in CommentContainerAdmin.transform)
        {
            Destroy(item.gameObject);
        }

        while (rdr.Read())
        {

            GameObject tmp = Instantiate(CommentPrefab, CommentContainerAdmin.transform);
            tmp.GetComponent<Comment>().SetData(rdr["id"].ToString(), rdr["ime"].ToString() + " " + rdr["priimek".ToString()], rdr["text"].ToString(), rdr["timestamp"].ToString(), (rdr["user_data_id"].ToString() == userId.ToString()));
        }
        DisconnectFromDB();
    }


    public void alertComment(int id)
    {
        GetComponent<ScreenChanger>().alertComment();

        alertCommentId = id;

    }

    public void deleteAlertComment()
    {
        if (adminKlubId.ToString() == "1")
        {
            GetComponent<ScreenChanger>().closeAlertComment();
            deleteCommentAdmin(alertCommentId);
        }
        else {
            GetComponent<ScreenChanger>().closeAlertComment();
            deleteComment(alertCommentId);
        }
        
    }

    public void deleteComment(int id) {

        foreach (Transform item in CommentContainer.transform)
        {
            Destroy(item.gameObject);
        }
        
        ConnectToDB();
        string sqlQuery = "select user_data.admin from user_data where id = '" + userId + "';";
        cmd = new MySqlCommand(sqlQuery, conn);
        rdr = cmd.ExecuteReader();

        

        while (rdr.Read()) {
            if (rdr[0].ToString() == "1" )
            {
                delete(id);
              
            }
            else {
                deleteUserComment(id);
            }
            
        }
        
        DisconnectFromDB();
        getAllComments(GameObject.Find("postScreenUser").GetComponent<DetailedPost>());
    }

    public void deleteCommentAdmin(int id)
    {

        foreach (Transform item in CommentContainerAdmin.transform)
        {
            Destroy(item.gameObject);
        }

        ConnectToDB();
        string sqlQuery = "select user_data.admin from user_data where id = '" + userId + "';";
        cmd = new MySqlCommand(sqlQuery, conn);
        rdr = cmd.ExecuteReader();



        while (rdr.Read())
        {
            if (rdr[0].ToString() == "1")
            {
                deleteAdmin(id);

            }
            else
            {
                deleteUserComment(id);
            }

        }

        DisconnectFromDB();
        getAllComments(GameObject.Find("postScreenAdmin").GetComponent<DetailedPost>());
    }

    public void deleteUserComment(int id) {
        ConnectToDB();
        string sqlQuery = "select user_data_id from comments where id = '" + id + "';";
        cmd = new MySqlCommand(sqlQuery, conn);
        rdr = cmd.ExecuteReader();

        while (rdr.Read())
        {
            if (rdr[0].ToString() == userId.ToString())
            {
                delete(id);
            }
            else
            {
                SSTools.ShowMessage("To ni vaš komentar", SSTools.Position.bottom, SSTools.Time.twoSecond);
            }
        }
        
        
    }

    public void delete(int id){
        ConnectToDB();

        string sqlQuery2 = "DELETE FROM comments WHERE id = '" + id + "';";
        cmd = new MySqlCommand();
        cmd.CommandText = sqlQuery2;
        cmd.Connection = conn;
        cmd.ExecuteNonQuery();

        string counter = "select count(post_id) from comments where post_id = '" + id + "';";
        string sqlQuery1 = "update posts set stcomment='" + counter + "'where id = '" + id + "';";

        SSTools.ShowMessage("Komentar je izbrisan", SSTools.Position.bottom, SSTools.Time.twoSecond);
        GetComponent<ScreenChanger>().switchUserPostToComments();
        getAllComments(new DetailedPost());

        cmd = new MySqlCommand();
        cmd.CommandText = sqlQuery1;
        cmd.Connection = conn;
        cmd.ExecuteNonQuery();

    }

    public void deleteAdmin(int id)
    {
        ConnectToDB();

        string sqlQuery2 = "DELETE FROM comments WHERE id = '" + id + "';";
        cmd = new MySqlCommand();
        cmd.CommandText = sqlQuery2;
        cmd.Connection = conn;
        cmd.ExecuteNonQuery();

        string counter = "select count(post_id) from comments where post_id = '" + id + "';";
        string sqlQuery1 = "update posts set stcomment='" + counter + "'where id = '" + id + "';";

        SSTools.ShowMessage("Komentar je izbrisan", SSTools.Position.bottom, SSTools.Time.twoSecond);
        GetComponent<ScreenChanger>().switchAdminPostToComments();
        getAllAdminComments(new DetailedPost());

        cmd = new MySqlCommand();
        cmd.CommandText = sqlQuery1;
        cmd.Connection = conn;
        cmd.ExecuteNonQuery();

    }


    public void addComment(int postId)
    {

        foreach (Transform item in CommentContainer.transform)
        {
            Destroy(item.gameObject);
        }

        ConnectToDB();
        string sqlQuery = "insert into comments (text,post_id,user_data_id) values" +
        " ('" + commentUser.text + "','" + postId + "','" + userId + "');";

        cmd = new MySqlCommand();
        cmd.CommandText = sqlQuery;
        cmd.Connection = conn;

        if (cmd.ExecuteNonQuery() > 0)
        {
            commentUser.text = "";
            Debug.Log("Succesfully added comment");
            GetComponent<ScreenChanger>().switchUserPostToComments();
            getAllComments(GameObject.Find("postScreenUser").GetComponent<DetailedPost>());
        }
        else
        {
            Debug.Log("Adding of comment failed!");
        }
        DisconnectFromDB();


    }

    public void addCommentAdmin(int postId)
    {

        foreach (Transform item in CommentContainerAdmin.transform)
        {
            Destroy(item.gameObject);
        }

        ConnectToDB();
        string sqlQuery = "insert into comments (text,post_id,user_data_id) values" +
        " ('" + commentAdmin.text + "','" + postId + "','" + userId + "');";

        cmd = new MySqlCommand();
        cmd.CommandText = sqlQuery;
        cmd.Connection = conn;

        if (cmd.ExecuteNonQuery() > 0)
        {
            commentAdmin.text = "";
            Debug.Log("Succesfully added comment");
            GetComponent<ScreenChanger>().switchAdminPostToComments();
            getAllAdminComments(GameObject.Find("postScreenAdmin").GetComponent<DetailedPost>());
        }
        else
        {
            Debug.Log("Adding of comment failed!");
        }
        DisconnectFromDB();


    }

    public void izbranaRegija() {
        ConnectToDB();
        DropdownIzbrani.ClearOptions();
        Dropdown.OptionData ttt = new Dropdown.OptionData();
        ttt.text = "Izberi klub";
        DropdownIzbrani.GetComponent<Dropdown>().options.Add(ttt);
        string regija = DropdownRegija.GetComponent<Dropdown>().value.ToString();

        string sqlQuery = "select * from klub where Regija = '"+ regija +"'";
        cmd = new MySqlCommand(sqlQuery, conn);
        rdr = cmd.ExecuteReader();

        while (rdr.Read()) {
            Dropdown.OptionData tmp = new Dropdown.OptionData();
            tmp.text = rdr["imeKluba"].ToString();
            DropdownIzbrani.GetComponent<Dropdown>().options.Add(tmp);
        }



        DisconnectFromDB();
    }


    public void izbranKlub() {
        ConnectToDB();
        string klub = DropdownIzbrani.options[DropdownIzbrani.value].text;

        string sqlQuery = "select posts.*, klub.imeKluba from posts join klub on posts.klub_id = klub.klubId where klub.imeKluba = '" + klub + "';";
        cmd = new MySqlCommand(sqlQuery, conn);
        rdr = cmd.ExecuteReader();

        foreach (Transform obj in IzbraniPostContainer.transform)
        {
            Destroy(obj.gameObject);
        }

        while (rdr.Read())
        {
            GameObject tmp = Instantiate(postPrefab, IzbraniPostContainer.transform);
            tmp.GetComponent<Post>().AssignValues(rdr[0].ToString(), rdr[1].ToString(), rdr[2].ToString(), rdr[3].ToString(), rdr[4].ToString(), rdr[5].ToString(), rdr[6].ToString(), true, rdr[7].ToString(), rdr["imeKluba"].ToString(), adminKlubId.ToString());

        }
        DisconnectFromDB();
    }


    public void AdminSlider() {
        ConnectToDB();
        string sqlQuery = "select klub.imeKluba, klub.followers, count(posts.id), user_data.ime, user_data.priimek from user_data join admin_has_klub on user_data.id = admin_has_klub.user_data_id join klub on admin_has_klub.klub_id = klub.klubId join posts on klub.klubId = posts.klub_id where user_data.id = '"+ userId +"';";
        cmd = new MySqlCommand(sqlQuery, conn);
        rdr = cmd.ExecuteReader();

        imeKluba.text = "";
        steviloSledilcev.text = "";
        steviloObjav.text = "";
        imePriimek.text = "";

        while (rdr.Read()) {
            imeKluba.text = "Vi ste admin kluba: \n"+rdr[0].ToString();
            steviloSledilcev.text = "Vašemu klubu sledi: \n" + rdr[1].ToString()+" uporabnikov";
            steviloObjav.text = "Objavljenih imate: \n" + rdr[2].ToString() + " objav";
            imePriimek.text = rdr[3].ToString() + " " + rdr[4].ToString();
        }
        DisconnectFromDB();

    }

    public void UserSlider() {
        if (userId != -1) {
            ConnectToDB();
            string sqlQuery = "select user_data.ime, user_data.priimek from user_data where user_data.id = '"+ userId +"'; ";
            cmd = new MySqlCommand(sqlQuery, conn);
            rdr = cmd.ExecuteReader();

            imeKluba.text = "";
            steviloSledilcev.text = "";
            steviloObjav.text = "";
            imePriimek.text = "";

            while (rdr.Read())
            {
                imePriimek.text = rdr[0].ToString() + " " + rdr[1].ToString();
            }
            DisconnectFromDB();
        }
    }


    public void alertPost(int id)
    {
        GetComponent<ScreenChanger>().alert();

        alertId = id;

    }

    public void deleteAlertPost()
    {
        AdminDeletePost(alertId);
    }

    public void AdminDeletePost(int id)
    {
        ConnectToDB();
        string sqlQuery1 = "DELETE FROM likes WHERE posts_id = '" + id + "';";
        cmd = new MySqlCommand();
        cmd.CommandText = sqlQuery1;
        cmd.Connection = conn;
        cmd.ExecuteNonQuery();

        string sqlQuery = "DELETE FROM posts WHERE id = '" + id + "';";
        cmd = new MySqlCommand();
        cmd.CommandText = sqlQuery;
        cmd.Connection = conn;
        cmd.ExecuteNonQuery();


        
        SSTools.ShowMessage("Objava je izbrisana", SSTools.Position.bottom, SSTools.Time.twoSecond);
        getAllAdminPosts();

        DisconnectFromDB();
    }

    public void clearAfterLogout() {
        imeKluba.text = "";
        steviloSledilcev.text = "";
        steviloObjav.text = "";
        imePriimek.text = "";
        adminKlubId = 0;

        GetComponent<ScreenChanger>().ScreenChangeLogout();


    }
}




