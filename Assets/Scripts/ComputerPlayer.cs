using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerPlayer : MonoBehaviour {


    public GameControllerSIM GC;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    //See vajab...
    //kõiki alasid?
    //alasid, mille peale saab käia

    //Main function 
    public void getTurn()
    {


        GameObject bestMove = getTurnRandom();
        bestMove.GetComponent<LineColorer>().doTurn();
    }


    private GameObject[] getAllLines()
    {
        return GC.allLinePrefabs;
    }

    private GameObject[] getFreeMoves()
    {
        return GC.freeLinePrefabs;
    }

    private GameObject getTurnRandom()
    {
        GameObject[] freeMoves = getFreeMoves();
        return freeMoves[Random.Range(0, freeMoves.Length)];
    }



}
