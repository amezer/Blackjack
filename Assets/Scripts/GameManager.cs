using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardLoc{
    public int deckIdx;
    public int valueIdx;
    public CardLoc(int dIdx, int vIdx){
        deckIdx = dIdx;
        valueIdx = vIdx;
    }
    public bool Equals(CardLoc other){
        return this.deckIdx == other.deckIdx && this.valueIdx == other.valueIdx;
    }
}

public class GameManager : MonoBehaviour
{
    //generic stuff
    public GameObject titleBackground;
    public GameObject btnBackground;
    public GameObject pokerTable;
    public GameObject playerBackground;
    public GameObject startTitles;
    public GameObject playerBtns;
    public GameObject betUI;
    public Slider betSlider;
    public Button betBtn;
    public GameObject mainBet;
    public Text betTxt;
    private int bets;
    public GameObject sideBet;
    public Text sideBetTxt;
    public Text totalBets;
    private int betsLeft;
    private int startingBets = 500;
    public GameObject restart;
    private Button restartBtn;

    public GameObject surrender;
    private Button surrenderBtn;
    
    public GameObject insurance;
    public GameObject ddown;
    private Button insuranceBtn;
    private Button ddownBtn;

    public GameObject bust;
    public GameObject win;
    public GameObject lose;
    public GameObject push;
    public GameObject hit;
    public GameObject stay;
    private Button hitBtn;
    private Button stayBtn;
    public List<GameObject> cards;
   
    public GameObject cardB;
    public GameObject dealer;
    public GameObject[] players;

    //locations for instantiating the backgrounds
    private Vector3 pos1 = new Vector3 (-16.98f, 11.35f, 0f);
    private Vector3 pos2 = new Vector3 (1f, 11.35f, 0f);
    
    private int numOfPlayers;
    private float range = 12.5f;
    private int delta = 0;

    private PlayerControl pc0;
    private PlayerControl pc1;
    private PlayerControl pc2;
    private PlayerControl pc3;
    private PlayerControl pc4;
    private PlayerControl pc5;
    private PlayerControl dealer6;
    private PlayerControl[] npc;
    private int dealerCardDownIdx;

    private int card1;
    private int card2;

    private List <int> cardValues = new List<int> {2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 10, 10, 10, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 10, 10, 10, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 10, 10, 10, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 10, 10, 10};
    private List <CardLoc> usedCards = new List<CardLoc>{};

    void Start()
    {
        Instantiate(btnBackground, new Vector3(-3.3f, 11.351f, 0), Quaternion.identity);
        Instantiate(titleBackground, pos1, Quaternion.identity);
        //the player
        pc0 = players[0].GetComponent<PlayerControl>();
        //NPCs
        pc1 = players[1].GetComponent<PlayerControl>();
        pc2 = players[2].GetComponent<PlayerControl>();
        pc3 = players[3].GetComponent<PlayerControl>();
        pc4 = players[4].GetComponent<PlayerControl>();
        pc5 = players[5].GetComponent<PlayerControl>();
        //dealer
        dealer6 = players[6].GetComponent<PlayerControl>();
        npc = new PlayerControl[5] {pc1, pc2, pc3, pc4, pc5};

        hitBtn = hit.GetComponent<Button>();
        stayBtn = stay.GetComponent<Button>();
        hitBtn.onClick.AddListener(hitFunc);
        stayBtn.onClick.AddListener(stayFunc);

        insuranceBtn = insurance.GetComponent<Button>();
        ddownBtn = ddown.GetComponent<Button>();
        insuranceBtn.onClick.AddListener(insuranceFunc);
        //splitBtn.onClick.AddListener(splitFunc);
        ddownBtn.onClick.AddListener(ddownFunc);

        restartBtn = restart.GetComponent<Button>();
        restartBtn.onClick.AddListener(RestartGame);

        betBtn.onClick.AddListener(dealCards);

        surrenderBtn = surrender.GetComponent<Button>();
        surrenderBtn.onClick.AddListener(surrenderGame);
    }
    // Update is called once per frame
    void Update()
    {
        betBtn.GetComponentInChildren<Text>().text = betSlider.value.ToString();
    }

    public void StartGame(int p){
        numOfPlayers = p;
        //Instantiate(pokerTable, pos1, Quaternion.identity);
        //Instantiate(playerBackground, pos2, Quaternion.identity);
        pokerTable.SetActive(true);
        playerBackground.SetActive(true);
        startTitles.SetActive(false);
        betUI.SetActive(true);
        playerBtns.SetActive(false);
        restart.SetActive(false);
        insurance.SetActive(false);
        ddown.SetActive(false);
        mainBet.SetActive(false);
        sideBet.SetActive(false);
        pc0.cardValue = 0;
        pc1.cardValue = 0;
        pc2.cardValue = 0;
        pc3.cardValue = 0;
        pc4.cardValue = 0;
        pc5.cardValue = 0;
        dealer6.cardValue = 0;
        delta = 0;
        pc0.cards = 0;
    }

    public void RestartGame(){
        var cards = GameObject.FindGameObjectsWithTag ("Card");
        foreach (var card in cards){
            Destroy(card);
        }

        var results = GameObject.FindGameObjectsWithTag("Result");
        foreach (var rst in results){
            Destroy(rst);
        }

        startingBets = betsLeft;
        betSlider.maxValue = startingBets;
        StartGame(numOfPlayers);
    }

    public void surrenderGame(){
        betsLeft  = betsLeft + (bets/2);
        totalBets.text = "Bets Left: " + betsLeft.ToString();
        restart.SetActive(true);
    }
   public void dealCards(){
       betUI.SetActive(false);
       bets = (int)betSlider.value;
       betTxt.text = bets.ToString();
       betsLeft = startingBets - bets;
       totalBets.text = "Bets Left: " + betsLeft.ToString();

       //instantiate cards for player
        instCard(new Vector3 (-1, 7.3f, 0), 0, false);
        card1 = pc0.cardValue;

        instCard(new Vector3(-1.5f, 7.3f, 0), 0, false);
        card2 = pc0.cardValue - card1;
        
        if(pc0.cardValue == 22){
            pc0.cardValue = 12;
        }

        //inst cards for NPCs
        float tempx = range/numOfPlayers;
        for (int i = 0; i < numOfPlayers; i++){
            instCard(new Vector3(-18.75f+tempx*i, 7.3f, 0), i+1, false);
            instCard(new Vector3(-18.25f+tempx*i, 7.3f, 0), i+1, false);
        }

        //inst cards for dealer
        instCard(new Vector3 (-13.7f, 14, 0), 6, false);
        int dealerCardUpValue = dealer6.cardValue;

        instCard(new Vector3(-13.2f, 14, 0), 6, true);
        
        mainBet.SetActive(true);
        playerBtns.SetActive(true);
        surrender.SetActive(true);
        Debug.Log("player: " + pc0.cardValue);
        Debug.Log("card up: " + dealerCardUpValue);

        if (dealerCardUpValue == 11){
            insurance.SetActive(true);
        }
        if(pc0.cardValue >= 9 && pc0.cardValue <= 11){
            ddown.SetActive(true);
        }else{
            rounds();
        }
    }

    public void insuranceFunc(){
        sideBet.SetActive(true);
        sideBetTxt.text = (bets/2).ToString();
        betsLeft = betsLeft - (int) (bets/2);
        totalBets.text = "Bets Left: " + betsLeft.ToString();
        playerBtns.SetActive(false);
        //dealer flip card up
        Instantiate(cards[dealerCardDownIdx], new Vector3(-13.2f, 14, 0), Quaternion.identity);
        if(dealer6.cards == 21 && pc0.cardValue == 21){
            betsLeft = betsLeft + bets*2 + (bets/2);
        }else if(dealer6.cardValue > pc0.cardValue){
            betsLeft = betsLeft + (bets/2);
        }else if(dealer6.cardValue < pc0.cardValue){
            betsLeft = betsLeft + bets + bets;
        }else if (dealer6.cardValue == 21 || (dealer6.cardValue != 21 && pc0.cardValue == dealer6.cardValue)){
            betsLeft = betsLeft + bets;
        }
        totalBets.text = "Bets Left: " + betsLeft.ToString();
        restart.SetActive(true);
    }

    public void ddownFunc(){
        betsLeft -= bets;
        bets *= 2;
        totalBets.text = "Bets Left: " + betsLeft.ToString();
        betTxt.text = bets.ToString();
        int oldValue = pc0.cardValue;
        instCard(new Vector3(-6+delta*1.25f, 12f, 0), 0, false);
        int newCard = pc0.cardValue - oldValue;
        if(newCard == 11 && pc0.cardValue > 21){
            pc0.cardValue -= 10;
        }
        surrender.SetActive(false);
        endGame();
    }

    public void rounds(){
        float tempx = range/numOfPlayers;
        for (int i = 0; i < numOfPlayers; i++){
            float y = 0;
            for(int j = 0; j < 5; j++){
                if(npc[i].getCard()){
                    instCard(new Vector3(-18.75f+tempx*i, 10+y, 0), i+1, false);
                    y+=0.5f;
                }else{
                    break;
                }
            }
            
            if(npc[i].cardValue > 21){
                Instantiate(bust, new Vector3(-18.75f+tempx*i, 10, 0), Quaternion.identity);
                npc[i].haveRst = true;
            }else if(npc[i].cardValue == 21){
                Instantiate(win, new Vector3(-18.75f+tempx*i, 10, 0), Quaternion.identity);
                npc[i].haveRst = true;
            }
        }
    }

    private bool checkIsIn(CardLoc temp){
        for (int i = 0; i < usedCards.Count; i++){
            if (usedCards[i].Equals(temp)){
                return true;
            }
        }
        return false;
    }

    private void instCard(Vector3 location, int playerID, bool isBack){
        //instantiate random cards
        int deckIdx = Random.Range(0, 6);
        int valueIdx = Random.Range(0, cards.Count);
        CardLoc temp = new CardLoc(deckIdx, valueIdx);
        while(checkIsIn(temp)){
            deckIdx = Random.Range(0, 6);
            valueIdx = Random.Range(0, cards.Count);
            temp = new CardLoc(deckIdx, valueIdx);
        }
        
        if (isBack){
            Instantiate(cardB, location, Quaternion.identity);
            dealerCardDownIdx = valueIdx;
        }else{
            Instantiate(cards[valueIdx], location, Quaternion.identity);
        }
        
        //update card values
        switch(playerID){
            case 0:
                pc0.cardValue += cardValues[valueIdx];
                break;
            case 1:
                pc1.cardValue += cardValues[valueIdx];
                break;
            case 2:
                pc2.cardValue += cardValues[valueIdx];
                break;
            case 3:
                pc3.cardValue += cardValues[valueIdx];
                break;
            case 4:
                pc4.cardValue += cardValues[valueIdx];
                break;
            case 5:
                pc5.cardValue += cardValues[valueIdx];
                break;  
            case 6:
                dealer6.cardValue += cardValues[valueIdx];
                break;  
        }
        //Debug.Log("cardidx: " + idx + " card value: " + cardValues[idx]);
        //remove cards from deck
        usedCards.Add(temp);
    }

    private void hitFunc(){
        int oldValue = pc0.cardValue;
        sideBet.SetActive(false);
        insurance.SetActive(false);
        ddown.SetActive(false);
        surrender.SetActive(false);
        instCard(new Vector3(-6+delta*1.25f, 12f, 0), 0, false);
        int newCard = pc0.cardValue - oldValue;
        if(newCard == 11 && pc0.cardValue > 21){
            pc0.cardValue -= 10;
        }
        pc0.cards ++;
        delta++;
        if (pc0.cardValue > 21 || pc0.cardValue == 21 || pc0.cards == 5){
            endGame();
        }
    }

    private void stayFunc(){
        //end player's turn
        surrender.SetActive(false);
        endGame();
    }

    private void endGame(){
        playerBtns.SetActive(false);
        //reveal dealer's card down
        Instantiate(cards[dealerCardDownIdx], new Vector3(-13.2f, 14, 0), Quaternion.identity);
        int i = 0;
        while(dealer6.cardValue <= 16){ //must draw if < 17
            instCard(new Vector3(-10.85f+i*0.5f, 14f, 0), 6, false);
            i++;
        }
        if (dealer6.cardValue > 21){
            float tempx = range/numOfPlayers;
            for(int j = 0; j < numOfPlayers; j++){
                if(npc[j].cardValue < 21){
                    Instantiate(win, new Vector3(-18.75f+tempx*j, 10, 0), Quaternion.identity);
                }
            }
            if(pc0.cardValue < 21){
                GameObject temp = Instantiate(win, new Vector3(-3, 10.85f, 0), Quaternion.identity);
                temp.transform.localScale = new Vector3 (0.7f, 0.7f, 0.7f);
            }
        }else{
            Debug.Log("dealer tot: " + dealer6.cardValue);
            float tempx = range/numOfPlayers;
            for(int j = 0; j < numOfPlayers; j++){
                if(!npc[j].haveRst){
                    if(npc[j].cardValue > dealer6.cardValue){
                        Instantiate(win, new Vector3(-18.75f+tempx*j, 10, 0), Quaternion.identity);
                    }else if(npc[j].cardValue < dealer6.cardValue){
                        Instantiate(lose, new Vector3(-18.75f+tempx*j, 10, 0), Quaternion.identity);
                    }else if(npc[j].cardValue == dealer6.cardValue){
                        Instantiate(push, new Vector3(-18.75f+tempx*j, 10, 0), Quaternion.identity);
                    }
                }
            }
            // if(!pc0.haveRst){
            //     //Debug.Log("is called");
            //     if(pc0.cardValue > dealer6.cardValue){
            //         GameObject temp = Instantiate(win, new Vector3(-3, 10.85f, 0), Quaternion.identity);
            //         temp.transform.localScale = new Vector3 (0.7f, 0.7f, 0.7f);
            //     }else if(pc0.cardValue < dealer6.cardValue){
            //         GameObject temp = Instantiate(lose, new Vector3(-3, 10.85f, 0), Quaternion.identity);
            //         temp.transform.localScale = new Vector3 (0.7f, 0.7f, 0.7f);
            //     }else if(pc0.cardValue == dealer6.cardValue){
            //         GameObject temp = Instantiate(push, new Vector3(-3, 10.85f, 0), Quaternion.identity);
            //         temp.transform.localScale = new Vector3 (0.7f, 0.7f, 0.7f);
            //     }
            // }
        }
        if(pc0.cardValue > 21){
            GameObject temp = Instantiate(bust, new Vector3(-3, 10.85f, 0), Quaternion.identity);
            temp.transform.localScale = new Vector3 (0.7f, 0.7f, 0.7f);
        }else if(pc0.cardValue == 21 && pc0.cards == 0){
            betsLeft = betsLeft + 2*bets + (bets/2);
            GameObject temp = Instantiate(win, new Vector3(-3, 10.85f, 0), Quaternion.identity);
            temp.transform.localScale = new Vector3 (0.7f, 0.7f, 0.7f);
        }else if(pc0.cards == 5){
            betsLeft = betsLeft + 2*bets;
            GameObject temp = Instantiate(win, new Vector3(-3, 10.85f, 0), Quaternion.identity);
            temp.transform.localScale = new Vector3 (0.7f, 0.7f, 0.7f);
        }else if(pc0.cardValue == dealer6.cardValue){
            betsLeft += bets;
            GameObject temp = Instantiate(push, new Vector3(-3, 10.85f, 0), Quaternion.identity);
            temp.transform.localScale = new Vector3 (0.7f, 0.7f, 0.7f);
        }else if(pc0.cardValue > dealer6.cardValue || dealer6.cardValue > 21){
            betsLeft = betsLeft + 2*bets;
            GameObject temp = Instantiate(win, new Vector3(-3, 10.85f, 0), Quaternion.identity);
            temp.transform.localScale = new Vector3 (0.7f, 0.7f, 0.7f);
        }else{
            GameObject temp = Instantiate(lose, new Vector3(-3, 10.85f, 0), Quaternion.identity);
            temp.transform.localScale = new Vector3 (0.7f, 0.7f, 0.7f);
        }
        Debug.Log("num of cards: " + pc0.cards);
        totalBets.text = "Bets Left: " + betsLeft.ToString();
        restart.SetActive(true);
    }
}
