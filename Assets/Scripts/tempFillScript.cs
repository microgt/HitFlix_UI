using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tempFillScript : MonoBehaviour
{
    public GameObject thumbnail;
    public GameObject content;
    // Start is called before the first frame update
    void Awake()
    {
        for (int i = 0; i < 30; i++) {
            Instantiate(thumbnail, content.transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
