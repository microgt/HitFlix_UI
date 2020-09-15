using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TubeUrlScript : MonoBehaviour
{
    YoutubeAPIManager youtubeapi;
    public YoutubePlayer player;
    public string title;
    // Start is called before the first frame update
    void Start()
    {
        youtubeapi = GameObject.FindObjectOfType<YoutubeAPIManager>();
        player = GetComponent<YoutubePlayer>();
        GetTrailerURL();
    }


    void GetTrailerURL()
    {
        YoutubeAPIManager.YoutubeSearchOrderFilter mainFilter = YoutubeAPIManager.YoutubeSearchOrderFilter.none;
        youtubeapi.Search(title + " trailer", 10, mainFilter, YoutubeAPIManager.YoutubeSafeSearchFilter.none, "", OnSearchDone);
    }
    void OnSearchDone(YoutubeData[] results)
    {
      
        string trailerURL = "https://www.youtube.com/watch?v=" + results[0].id;
        //player.youtubeUrl = trailerURL;
        player.LoadYoutubeVideo(trailerURL);
        Debug.Log(trailerURL);
        //player.Start();
        //player.PlayButton();
    }
}
