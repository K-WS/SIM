using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControllerSIM : MonoBehaviour
{

    public GameObject[] allLinePrefabs;
    public GameObject[] freeLinePrefabs;
    public ComputerPlayer CP;
    private int player;
    private int computer;
    private int currentPlayer;

    

    // Use this for initialization
    void Start()
    {
        allLinePrefabs = GameObject.FindGameObjectsWithTag("Unselected Line");
        freeLinePrefabs = allLinePrefabs;
        player = 0;
        computer = 1;
        currentPlayer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentPlayer == computer)
        {
            CP.getTurn();
        }


    }


    public void UpdateInfo()
    {
        if (currentPlayer == 0)
            currentPlayer = 1;
        else
            currentPlayer = 0;

        allLinePrefabs = GameObject.FindGameObjectsWithTag("Unselected Line");

        List<GameObject> freeLines = new List<GameObject>();

        foreach (var line in allLinePrefabs) {
            if (line.GetComponent<LineColorer>().selectable == true)
                freeLines.Add(line);
        }
    
        freeLinePrefabs = freeLines.ToArray();
    }

    public int WhoseTurn() {
        return currentPlayer;
    }
}
