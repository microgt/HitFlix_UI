using IMDbApiLib;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Movie : MonoBehaviour
{
    //private string url = "https://www.imdb.com/title/tt7286456/?ref_=hm_fanfav_tt_3_pd_fp1";
    private string url = "https://m.media-amazon.com/images/M/MV5BNGVjNWI4ZGUtNzE0MS00YTJmLWE0ZDctN2ZiYTk2YmI3NTYyXkEyXkFqcGdeQXVyMTkxNjUyNQ@@._V1_UX182_CR0,0,182,268_AL_.jpg";

    public GameObject movieScreen;
    GameObject mainMenu;

    private Image thumbnail;
    public Text title_text;
    public Text rating_text;
    private Animator anim;
    private float pause;

    //Movie information
    private string imdbID = "tt1375666";
    private Sprite poster;
    private string discribtion;
    private float rating;
    private string title;
    private string tagline;
    private string trailer;
    private string genre;

    void Awake() {
        thumbnail = GetComponent<Image>();
        anim = GetComponent<Animator>();
        pause = Random.Range(0.05f, 0.1f);
        
    }
    void OnEnable() {
        //StartAsync();
    }

    // Start is called before the first frame update
    async void StartAsync ()
    {
        var apiLib = new ApiLib("k_GjnD0m0X");
        var data = await apiLib.TitleAsync(imdbID);

        Debug.Log(data.ErrorMessage);
        title = data.FullTitle;
        discribtion = data.Plot;
        tagline = data.Tagline;
        genre = data.Genres;
        rating = float.Parse(data.IMDbRating);
        url = data.Image;
        //trailer = data.Trailer.Link;
        StartCoroutine("SetThumbnail");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        pause -= Time.deltaTime;
        if (pause <= 0)
        {
            anim.Play("TitleFadeIn");
        }
    }

    IEnumerator SetThumbnail()
    {
        // Start a download of the given URL
        using (WWW www = new WWW(url))
        {
            // Wait for download to complete
            yield return www;

            // assign texture
            poster = Sprite.Create(www.texture, new Rect(0.0f, 0.0f, www.texture.width, www.texture.height), new Vector2(0.5f, 0.5f), 100.0f);
            thumbnail.sprite = poster;
            title_text.text = title;
            rating_text.text = rating.ToString();

        }
    }


    //send info to API here **************************************************************************************************
    public void OpenMovieDetails() {

        PlayerPrefs.SetString("movieName", imdbID);

        GameObject mov = new GameObject();

        if (GameObject.FindWithTag("MainMenu") != null)
        {
            GameObject mainMenu = GameObject.FindWithTag("MainMenu");
            GameObject ui = GameObject.FindWithTag("UI");
            mainMenu.GetComponent<Animator>().enabled = false;
            mainMenu.SetActive(false);

            mov = GameObject.Instantiate(movieScreen, ui.transform);
            mov.GetComponentInChildren<GetItemRecsScript>().setMainMenu(mainMenu);
        }
        else
        {
             GameObject ui = GameObject.FindWithTag("UI");
            mov = GameObject.Instantiate(movieScreen, ui.transform);
            mov.GetComponentInChildren<GetItemRecsScript>().setMainMenu(mainMenu);
            Destroy(GetComponentInParent<GetItemRecsScript>().gameObject);
        }
        mov.GetComponent<GetItemRecsScript>().setMovieInfo(title, tagline, genre, poster, discribtion, rating);
    }

    public void SetMainMenu(GameObject menu) {
        mainMenu = menu;
    }
    public void setIMDBID(string id) {
        imdbID = id;
        StartAsync();
    }
}
