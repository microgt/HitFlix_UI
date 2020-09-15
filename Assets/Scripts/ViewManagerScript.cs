using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ViewManagerScript : MonoBehaviour
{
    private string popurl = "http://melarian2020.pythonanywhere.com/popularMovies?movies_count=10";
    //private string popurl = "https://recommendation-movies.herokuapp.com/popularMovies?movies_count=10";
    private string itemurl;
    private string recurl;
    private string[] popularMovies;
    private string[] itemRecommendations;
    private string[] recommendedMovies;
    public Transform contentPane;


    private GameObject popular_Pane;
    private GameObject item_Pane;
    private GameObject user_Pane;
    GridLayoutGroup glg3;

    public GameObject movie;
    public GameObject content_pane;

    void OnEnable() {
        Initiate();
    }

    public void Initiate() {

        ClearContentPane();

        if (PlayerPrefs.GetString("movieName") != "")
        {
            itemurl = "http://melarian2020.pythonanywhere.com/bestMoviesRecommendations?imdb_id=" + PlayerPrefs.GetString("movieName") + "&count=10";
            recurl = "http://melarian2020.pythonanywhere.com/getUserRecommendations?movieUserId=500&imdb_id=" + PlayerPrefs.GetString("movieName") + "&movies_count=10";
            //itemurl = "https://recommendation-movies.herokuapp.com/bestMoviesRecommendations?imdb_id=" + PlayerPrefs.GetString("movieName") + "&count=10";
            //recurl = "https://recommendation-movies.herokuapp.com/getUserRecommendations?movieUserId=500&imdb_id=" + PlayerPrefs.GetString("movieName") + "&movies_count=10";
        }

        StartCoroutine(GetPopular());
        StartCoroutine(GetItem());
        StartCoroutine(GetRecommended());
    }

    void ClearContentPane() {
        if (contentPane.childCount > 0)
        {
            foreach (RectTransform g in contentPane.GetComponentInChildren<RectTransform>()) {
                Destroy(g.gameObject);
            }
        }

        //Create all panes
        glg3 = contentPane.GetComponentInChildren<GridLayoutGroup>();

        popular_Pane = GameObject.Instantiate(content_pane, contentPane);
        popular_Pane.GetComponent<Text>().text = "Popular Now:";
        popular_Pane.GetComponent<RectTransform>().localScale = Vector3.zero;

        item_Pane = GameObject.Instantiate(content_pane, contentPane);
        item_Pane.GetComponent<Text>().text = "You may also like:";
        item_Pane.GetComponent<RectTransform>().localScale = Vector3.zero;

        user_Pane = GameObject.Instantiate(content_pane, contentPane);
        user_Pane.GetComponent<Text>().text = "Recommended for you:";
        user_Pane.GetComponent<RectTransform>().localScale = Vector3.zero;
    }

    void FillPopular() {
        foreach (string s in popularMovies)
        {
            GameObject mov = GameObject.Instantiate(movie, popular_Pane.transform.GetChild(0).transform);
            mov.GetComponent<Movie>().setIMDBID(s);
        }
    }

    void FillItem()
    {
        foreach (string s in itemRecommendations)
        {
            GameObject mov = GameObject.Instantiate(movie, item_Pane.transform.GetChild(0).transform);
            mov.GetComponent<Movie>().setIMDBID(s);
        }
    }

    void FillUser()
    {
        foreach (string s in recommendedMovies)
        {
            GameObject mov = GameObject.Instantiate(movie, user_Pane.transform.GetChild(0).transform);
            mov.GetComponent<Movie>().setIMDBID(s);
        }
    }

    IEnumerator GetPopular()
    {
        // Start a download of the given URL
        using (WWW w = new WWW(popurl))
        {
            // Wait for download to complete
            yield return w;

            // get movie list
            if (w.text != "" && w.text.ToCharArray()[0] == 't') {
                popularMovies = w.text.Split(',');
                PopulateOnDemand();
            }   
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
            if (w.text != "" && w.text.ToCharArray()[0] == 't')
            {
                itemRecommendations = w.text.Split(',');
                PopulateOnDemand();
            }
        }
    }
    IEnumerator GetRecommended()
    {
        // Start a download of the given URL
        using (WWW w = new WWW(recurl))
        {
            // Wait for download to complete
            yield return w;

            // get movie list
            if (w.text != "" && w.text.ToCharArray()[0] == 't')
            {
                recommendedMovies = w.text.Split(',');
                PopulateOnDemand();
            }
        }
    }

    /*IEnumerator GetText()
    {
        UnityWebRequest www = UnityWebRequest.Get(popurl);
        Debug.Log("starting");
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            // Show results as text
            Debug.Log(www.downloadHandler.text);
        }
    }*/

    float getSpacing() {
        float result = 0;

        float firstPass = popularMovies.Length / 5.0f;
        float secondPass = Mathf.Ceil(firstPass);

        result = secondPass * 262.5f;

        return result;
    }


    void OrganizePanes() {
        //only popular is available
        if (itemRecommendations == null || itemRecommendations.Length <= 0)
        {
            popular_Pane.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);

            GridLayoutGroup glg = popular_Pane.GetComponentInChildren<GridLayoutGroup>();
            glg.constraintCount = 5;
            glg.spacing = new Vector2(40, 100);

            glg3.spacing = new Vector2(0, 262.5f);
            glg3.constraintCount = 4;
        }
        //popular and item recommended is visible
        else if (itemRecommendations.Length > 0 && (recommendedMovies == null || recommendedMovies.Length <= 0))
        {
            popular_Pane.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            item_Pane.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);

            GridLayoutGroup glg = popular_Pane.GetComponentInChildren<GridLayoutGroup>();
            glg.constraintCount = 5;
            glg.spacing = new Vector2(40, 100);

            GridLayoutGroup glg2 = item_Pane.GetComponentInChildren<GridLayoutGroup>();
            glg2.constraintCount = 5;
            glg2.spacing = new Vector2(40, 100);

            glg3.spacing = new Vector2(0, getSpacing());
            glg3.constraintCount = 3;
        }
        //everything is visible
        else if (itemRecommendations.Length > 0 && recommendedMovies.Length > 0)
        {
            popular_Pane.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            item_Pane.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            user_Pane.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);

            GridLayoutGroup glg = popular_Pane.GetComponentInChildren<GridLayoutGroup>();
            glg.constraintCount = 5;
            glg.spacing = new Vector2(40, 100);

            GridLayoutGroup glg2 = item_Pane.GetComponentInChildren<GridLayoutGroup>();
            glg2.constraintCount = 5;
            glg2.spacing = new Vector2(40, 100);

            GridLayoutGroup glg4 = user_Pane.GetComponentInChildren<GridLayoutGroup>();
            glg4.constraintCount = 5;
            glg4.spacing = new Vector2(40, 100);

            glg3.spacing = new Vector2(0, getSpacing());
            glg3.constraintCount = 4;
        }

        glg3.GetComponent<RectTransform>().localPosition = Vector3.zero;
    }

    void PopulateOnDemand() {

        if (popular_Pane == null || item_Pane == null || user_Pane == null) {
            StartCoroutine(WaitandResume());
            return;
        }

        if (popular_Pane.transform.GetChild(0).childCount < popularMovies.Length) {
            FillPopular();
            OrganizePanes();
        }

        if (itemRecommendations != null && item_Pane.transform.GetChild(0).childCount < itemRecommendations.Length) {
            FillItem();
            OrganizePanes();
        }

        if (recommendedMovies != null && user_Pane.transform.GetChild(0).childCount < recommendedMovies.Length) {
            FillUser();
            OrganizePanes();
            return;
        }

        StartCoroutine(WaitandResume());
    }

    IEnumerator WaitandResume() {
        yield return new WaitForSeconds(2);
        PopulateOnDemand();
    }
}
