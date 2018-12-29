using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameControllerSIM : MonoBehaviour
{
    //----------------Initial Variables------------------//
    //---------------------------------------------------//
    public GameObject[] allLinePrefabs;
    public GameObject[] freeLinePrefabs;
    public GameObject[] p1Prefabs;
    public GameObject[] p2Prefabs;
    public ComputerPlayer CP;
    private int player;
    private int computer;
    private int currentPlayer;


    public Text startText;
    public Button p1Button;
    public Button p2Button;
    public Image P1Win;
    public Image P2Win;


    private string[][] loseCombos;

    private bool resetStarted;
    private float resetTime;
    private float resetStartTime;
    private float currentTime;




    //-----------------Initialization--------------------//
    //---------------------------------------------------//
    void Start()
    {
        allLinePrefabs = GameObject.FindGameObjectsWithTag("Unselected Line");
        freeLinePrefabs = allLinePrefabs;

        player = -1;
        computer = -1;
        currentPlayer = 0;
        resetStarted = false;
        resetTime = 5;

        p1Button.onClick.AddListener(() => buttonClicked(0, 1));
        p2Button.onClick.AddListener(() => buttonClicked(1, 0));


        loseCombos = new string[][] { 
            new string[] {"LineBlank","LineBlank (1)","LineBlank (13)"},
            new string[] {"LineBlank","LineBlank (6)","LineBlank (10)"},
            new string[] {"LineBlank","LineBlank (7)","LineBlank (12)"},
            new string[] {"LineBlank","LineBlank (5)","LineBlank (9)"},

            new string[] {"LineBlank (1)","LineBlank (2)","LineBlank (10)"},
            new string[] {"LineBlank (1)","LineBlank (7)","LineBlank (14)"},
            new string[] {"LineBlank (1)","LineBlank (8)","LineBlank (9)"},

            new string[] {"LineBlank (2)","LineBlank (3)","LineBlank (14)"},
            new string[] {"LineBlank (2)","LineBlank (8)","LineBlank (11)"},
            new string[] {"LineBlank (2)","LineBlank (6)","LineBlank (13)"},

            new string[] {"LineBlank (3)","LineBlank (4)","LineBlank (11)"},
            new string[] {"LineBlank (3)","LineBlank (6)","LineBlank (12)"},
            new string[] {"LineBlank (3)","LineBlank (7)","LineBlank (10)"},

            new string[] {"LineBlank (4)","LineBlank (5)","LineBlank (12)"},
            new string[] {"LineBlank (4)","LineBlank (7)","LineBlank (9)"},
            new string[] {"LineBlank (4)","LineBlank (8)","LineBlank (14)"},

            new string[] {"LineBlank (5)","LineBlank (8)","LineBlank (13)"},
            new string[] {"LineBlank (5)","LineBlank (6)","LineBlank (11)"},

            new string[] {"LineBlank (9)","LineBlank (10)","LineBlank (11)"},

            new string[] {"LineBlank (12)","LineBlank (13)","LineBlank (14)"}
        };

        foreach (GameObject line in allLinePrefabs)
        {
			line.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }

    }

    //----------------Per Frame Update-------------------//
    //---------------------------------------------------//
    void Update()
    {
        if (currentPlayer == computer)
        {
            CP.getTurn();
        }
        if(resetStarted == true)
        {
            currentTime = Time.time;

            if(currentTime - resetStartTime >= resetTime)
            {
                SceneManager.LoadScene("MainGame");
            }
        }
    }

    //-------------Controller Info Update----------------//
    //---------------------------------------------------//
    public void UpdateInfo()
    {

        //---Update line array---//
        //-----------------------//
        allLinePrefabs = GameObject.FindGameObjectsWithTag("Unselected Line");


        //---Lists to separate lines---//
        //-----------------------------//
        List<GameObject> freeLines = new List<GameObject>();
        List<GameObject> p1Lines = new List<GameObject>();
        List<GameObject> p2Lines = new List<GameObject>();

        //---For loop that preps all lists to transfer over to arrays
        foreach (var line in allLinePrefabs) {
            if (line.GetComponent<LineColorer>().selectable == true)
                freeLines.Add(line);
            else if (line.GetComponent<LineColorer>().whichPlayer == 0)
                p1Lines.Add(line);
            else if (line.GetComponent<LineColorer>().whichPlayer == 1)
                p2Lines.Add(line);

        }

        //---Set lines to arrays 
        freeLinePrefabs = freeLines.ToArray();
        p1Prefabs = p1Lines.ToArray();
        p2Prefabs = p2Lines.ToArray();


        //---Loss Declaration---//
        //----------------------//

        foreach (var combo in loseCombos) {
            bool check = checkLoss(combo);

            if(check == true)
            {
                if (currentPlayer == 0)
                    P2Win.gameObject.SetActive(true);
                else
                    P1Win.gameObject.SetActive(true);

                foreach (GameObject line in freeLinePrefabs)
                {
					line.gameObject.GetComponent<SpriteRenderer>().enabled = false;
                }

                //indicate that game can now restart

                resetStartTime = Time.time;
                resetStarted = true;

                return;
            }
        }


        //---Change player---//
        //-------------------//
        if (currentPlayer == 0)
            currentPlayer = 1;
        else
            currentPlayer = 0;
    }

    public void setPlayers(int pl, int ai) {
        player = pl;
        computer = ai;
    }

    public int WhoseTurn() {
        return currentPlayer;
    }

    public string[][] loseCombinations() {
        return loseCombos;
    }

    private void buttonClicked(int plr, int com)
    {
        player = plr;
        computer = com;
        p1Button.gameObject.SetActive(false);
        p2Button.gameObject.SetActive(false);
        startText.gameObject.SetActive(false);

        foreach (GameObject line in allLinePrefabs)
        {
			line.gameObject.GetComponent<SpriteRenderer>().enabled = true;
        }
    }

    private bool checkLoss(string[] combo)
    {
        GameObject[] checkable;
        if (currentPlayer == 0)
            checkable = p1Prefabs;
        else
            checkable = p2Prefabs;

        int inARow = 0;

        foreach (string name in combo)
        {
            foreach(GameObject prefab in checkable)
            {
                if(prefab.name == name)
                {
                    inARow++;
                    break;
                }
            }
        }
        if (inARow == 3)
            return true;
        return false;
    }
}
