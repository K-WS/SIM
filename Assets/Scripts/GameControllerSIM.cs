using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


    private string[][] loseCombos;



    //-----------------Initialization--------------------//
    //---------------------------------------------------//
    void Start()
    {
        allLinePrefabs = GameObject.FindGameObjectsWithTag("Unselected Line");
        freeLinePrefabs = allLinePrefabs;

        player = 0;
        computer = 1;
        currentPlayer = 0;


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

        foreach (var line in allLinePrefabs)
        {
            Debug.Log(line.name == "LineBlank (6)");

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


        //---Change player---//
        //-------------------//
        if (currentPlayer == 0)
            currentPlayer = 1;
        else
            currentPlayer = 0;
    }

    public int WhoseTurn() {
        return currentPlayer;
    }

    public string[][] loseCombinations() {
        return loseCombos;
    }
}
