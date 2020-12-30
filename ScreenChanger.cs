using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenChanger : MonoBehaviour
{

    public GameObject loginScreen;
    public GameObject registerScreen;
    public GameObject userHomeScreen;
    public GameObject userPost;
    public GameObject adminHomeScreen;
    public GameObject addPostScreen;
    public GameObject userPrivateHomeScreen;
    public GameObject userListClubs;
    public GameObject TopLikedPage;
    public GameObject guestScreen;
    public GameObject commentScreen;
    public GameObject createCommentScreen;
    public GameObject dogodkiVkraju;
    public GameObject postAlert;
    public GameObject commentAlert;
    public GameObject postScreenAdmin;
    public GameObject commentScreenAdmin;


    public void switchUserPostToComments() {
        userPost.transform.localScale = new Vector3(0, 0, 0);
        commentScreen.SetActive(true);
    }

    public void switchAdminPostToComments() {
        postScreenAdmin.transform.localScale = new Vector3(0, 0, 0);
        commentScreenAdmin.SetActive(true);
    }

    public void switchCommentsToPost()
    {
        commentScreen.SetActive(false);
        userPost.transform.localScale = new Vector3(1, 1, 1);
    }

    public void switchCommentsAdminToPost()
    {
        commentScreenAdmin.SetActive(false);
        postScreenAdmin.transform.localScale = new Vector3(1, 1, 1);
    }

    public void switchFromLoginToRegister() {

        loginScreen.SetActive(false);
        registerScreen.SetActive(true);
    }
    public void switchFromRegisterToLogin()
    {
        loginScreen.SetActive(true);
        registerScreen.SetActive(false);
    }

    public void SwitchLoginToUserHome()
    {
        loginScreen.SetActive(false);
        userHomeScreen.SetActive(true);
    }

    public void switchPostsToPost() {
        dogodkiVkraju.SetActive(false);
        userHomeScreen.SetActive(false);
        adminHomeScreen.SetActive(false);
        addPostScreen.SetActive(false);
        userPrivateHomeScreen.SetActive(false);
        userListClubs.SetActive(false);
        TopLikedPage.SetActive(false);
        guestScreen.SetActive(false);
        commentScreen.SetActive(false);
        userPost.SetActive(true);
    }
    public void switchPostToPosts()
    {
        commentScreen.SetActive(false);
        userPost.transform.localScale = new Vector3(1, 1, 1);
        userPost.SetActive(false);
        userHomeScreen.SetActive(true);
        GetComponent<MysqlManager>().getAllPosts();
    }

    public void switchLoginToAdminHome()
    {
        loginScreen.SetActive(false);
        adminHomeScreen.SetActive(true);
    }

    public void switchAdminHomeToAddPosts()
    {
        userHomeScreen.SetActive(false);
        userPost.SetActive(false);
        adminHomeScreen.SetActive(false);
        addPostScreen.SetActive(false);
        userPrivateHomeScreen.SetActive(false);
        userListClubs.SetActive(false);
        TopLikedPage.SetActive(false);
        guestScreen.SetActive(false);
        commentScreen.SetActive(false);
        dogodkiVkraju.SetActive(false);
        postScreenAdmin.SetActive(false);
        commentScreenAdmin.SetActive(false);     
        adminHomeScreen.SetActive(false);
        addPostScreen.SetActive(true);
    }

    public void switchAddPostsToAdminHome()
    {
        addPostScreen.SetActive(false);
        adminHomeScreen.SetActive(true);

    }

    public void switchPublicNewsToPrivateNews() {
        userHomeScreen.SetActive(false);
        userPost.SetActive(false);
        userListClubs.SetActive(false);
        TopLikedPage.SetActive(false);
        commentScreen.SetActive(false);
        dogodkiVkraju.SetActive(false);
        userPrivateHomeScreen.SetActive(true);
    }

    public void switchPrivateNewsToPublicNews()
    {
        userPrivateHomeScreen.SetActive(false);
        userHomeScreen.SetActive(true);
    }

    public void switchUserHomeScreenToUserListClubs() {
        userHomeScreen.SetActive(false);
        userPost.SetActive(false);
        TopLikedPage.SetActive(false);
        commentScreen.SetActive(false);
        dogodkiVkraju.SetActive(false);
        userPrivateHomeScreen.SetActive(false);
        userListClubs.SetActive(true);
    }

    public void switchUserListClubsToUserHomeScreen()
    {
        userListClubs.SetActive(false);
        userHomeScreen.SetActive(true);
    }

    public void switchHomeScreenToTopLikedPage(){
        userHomeScreen.SetActive(false);
        userPost.SetActive(false);
        commentScreen.SetActive(false);
        dogodkiVkraju.SetActive(false);
        userPrivateHomeScreen.SetActive(false);
        userListClubs.SetActive(false);
        TopLikedPage.SetActive(true);
    }

    public void switchTopLikedPageToHomeScreen()
    {
        TopLikedPage.SetActive(false);
        userHomeScreen.SetActive(true);
    }

    public void switchLoginToGuestScreen()
    {
        loginScreen.SetActive(false);
        guestScreen.SetActive(true);
    }

    public void ScreenChangeLogout() {
        userHomeScreen.SetActive(false);
        userPost.SetActive(false);
        adminHomeScreen.SetActive(false);
        addPostScreen.SetActive(false);
        userPrivateHomeScreen.SetActive(false);
        userListClubs.SetActive(false);
        TopLikedPage.SetActive(false);
        guestScreen.SetActive(false);
        commentScreen.SetActive(false);
        dogodkiVkraju.SetActive(false);
        postScreenAdmin.SetActive(false);
        commentScreenAdmin.SetActive(false);
        loginScreen.SetActive(true);
    }

    public void switchToAdminHome()
    {
        userHomeScreen.SetActive(false);
        userPost.SetActive(false);
        addPostScreen.SetActive(false);
        userPrivateHomeScreen.SetActive(false);
        userListClubs.SetActive(false);
        TopLikedPage.SetActive(false);
        guestScreen.SetActive(false);
        commentScreen.SetActive(false);
        adminHomeScreen.SetActive(true);
    }


    public void switchToDogodkiVKraju() {
        userPost.SetActive(false);
        TopLikedPage.SetActive(false);
        commentScreen.SetActive(false);
        userPrivateHomeScreen.SetActive(false);
        userListClubs.SetActive(false);
        userHomeScreen.SetActive(false);
        dogodkiVkraju.SetActive(true);
    }

    public void switchUserHomeScreenToDogodkiVKraju()
    {
        userPost.SetActive(false);
        TopLikedPage.SetActive(false);
        commentScreen.SetActive(false);
        userPrivateHomeScreen.SetActive(false);
        userListClubs.SetActive(false);
        userHomeScreen.SetActive(false);
        dogodkiVkraju.SetActive(true);
    }

    public void switchToHomeScreen()
    {
        GameObject.Find("ConnectionManager").GetComponent<MysqlManager>().getAllPosts();
        userPost.SetActive(false);
        TopLikedPage.SetActive(false);
        commentScreen.SetActive(false);
        userPrivateHomeScreen.SetActive(false);
        userListClubs.SetActive(false);
        dogodkiVkraju.SetActive(false);
        userHomeScreen.SetActive(true);
    }

    public void switchDogodkiVKrajuToUserHomeScreen()
    {
        dogodkiVkraju.SetActive(false);
        userHomeScreen.SetActive(true);
    }

    public void alert()
    {
        postAlert.SetActive(true);
    }

    public void closeAlert()
    {
        postAlert.SetActive(false);
    }

    public void alertComment()
    {
        commentAlert.SetActive(true);
    }

    public void closeAlertComment()
    {
        commentAlert.SetActive(false);
    }

    public void switchPostToPostsAdmin()
    {
        commentScreen.SetActive(false);
        postScreenAdmin.transform.localScale = new Vector3(1, 1, 1);
        postScreenAdmin.SetActive(false);
        adminHomeScreen.SetActive(true);
        GetComponent<MysqlManager>().getAllAdminPosts();
    }

    public void switchPostsToPostAdmin()
    {
        adminHomeScreen.SetActive(false);
        addPostScreen.SetActive(false);
        commentScreen.SetActive(false);
        postScreenAdmin.SetActive(true);
    }

}
