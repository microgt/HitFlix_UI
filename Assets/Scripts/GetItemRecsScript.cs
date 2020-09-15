using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YoutubeLight;

public class GetItemRecsScript : MonoBehaviour
{
    private string itemurl;
    private string[] itemRecommendations;
    public GameObject mainMenu;
    public Transform contentPane;
    public GameObject movie;
    public YoutubePlayer player;

    public Text title_text;
    public Text tagline_text;
    public Text genre_text;
    public Image poster_img;
    public Text discribtion_text;
    public Text rating_text;

    string title;
    string tagline;
    Sprite poster;
    string discribtion;
    string genre;
    float rating;
    string trailerURL;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponentInChildren<YoutubePlayer>();

        if (PlayerPrefs.GetString("movieName") != "")
        {
            itemurl = "http://melarian2020.pythonanywhere.com/getUserRecommendations?imdb_id=" + PlayerPrefs.GetString("movieName") + "&count=10";
            //itemurl = "https://recommendation-movies.herokuapp.com/bestMoviesRecommendations?imdb_id=" + PlayerPrefs.GetString("movieName") + "&count=10";
        }

        StartCoroutine(GetItem());
    }

    // Update is called once per frame
    string[] GetList ()
    {
        string[] result = new string[] {};
        return result;
    }

    public void CloseWindow() {
        mainMenu.SetActive(true);
        Destroy(gameObject);
    }
    public void setMainMenu(GameObject menu) {
        mainMenu = menu;
    }
    public void setMovieInfo(string title, string tagline, string genre, Sprite poster, string discribtion, float rating) {
        this.title = title;
        this.tagline = tagline;
        this.genre = genre;
        this.poster = poster;
        this.discribtion = discribtion;
        this.rating = rating;

        title_text.text = this.title;
        tagline_text.text = this.tagline;
        genre_text.text = genre;
        poster_img.sprite = this.poster;
        discribtion_text.text = this.discribtion;
        rating_text.text = this.rating.ToString();
        GetTrailerURL();
    }

    void GetTrailerURL() {
        YoutubeAPIManager ytm = GameObject.FindObjectOfType<YoutubeAPIManager>();
        YoutubeAPIManager.YoutubeSearchOrderFilter mainFilter = YoutubeAPIManager.YoutubeSearchOrderFilter.none;
        ytm.Search(title + " trailer", 10, mainFilter, YoutubeAPIManager.YoutubeSafeSearchFilter.none, "", OnSearchDone);
    }
    void OnSearchDone(YoutubeData[] results)
    {
       trailerURL = "https://www.youtube.com/watch?v=" + results[0].id;
       player.LoadYoutubeVideo(trailerURL);
       //player.PlayButton();
    }

    void FillItem()
    {
        foreach (string s in itemRecommendations)
        {     
            GameObject mov = GameObject.Instantiate(movie, contentPane);
            mov.GetComponent<Movie>().setIMDBID(s);
            mov.GetComponent<Movie>().SetMainMenu(mainMenu);
        }
    }

    IEnumerator GetItem()
    {
        // Start a download of the given URL
        using (WWW w = new WWW(itemurl))
        {
            // Wait for download to complete
            yield return w;

            // get movie list

            itemRecommendations = w.text.Split(',');

            FillItem();
        }
    }
}
