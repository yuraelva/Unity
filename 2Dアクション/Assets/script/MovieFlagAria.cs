using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovieFlagAria : MonoBehaviour
{
    public bool flag;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "player")
        {
            flag = true;
        }
    }
    public bool getFlag()
    {
        return flag;
    }
}
