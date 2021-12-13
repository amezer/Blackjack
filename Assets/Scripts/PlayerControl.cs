using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public int playerID = 0;
    public int cardValue = 0;
    public bool haveRst = false;
    public int cards = 2;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool getCard(){
        if (cardValue >= 16){
            return false;
        }
        cards ++;
        return true;
    }
}
