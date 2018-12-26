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



    //-----------------Initialization--------------------//
    //---------------------------------------------------//
    void Start()
    {
        allLinePrefabs = GameObject.FindGameObjectsWithTag("Unselected Line");
        freeLinePrefabs = allLinePrefabs;
        player = 0;
        computer = 1;
        currentPlayer = 0;
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
}
