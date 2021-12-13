using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ButtonControll : MonoBehaviour
{
    public int numberOfPlayers;
    private Button button;
    private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        button = GetComponent<Button>();
        button.onClick.AddListener(setPlayers);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void setPlayers(){
        gameManager.StartGame(numberOfPlayers);
    }
}
