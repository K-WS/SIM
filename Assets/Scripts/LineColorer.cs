using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class LineColorer : MonoBehaviour {

    public Color defaultColor;
    public Color selectedColor;
    public Color player1;
    public Color player2;

    public GameControllerSIM GC;

    public bool selectable;

	// Use this for initialization
	void Start () {
        selectable = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ColorMe(int player)
    {
        if (player == 0) 
            GetComponent<SpriteRenderer>().color = player1;
        else //player == 1, loodetavasti
            GetComponent<SpriteRenderer>().color = player2;
        selectable = false;
    }

    //---------Hovering over an unselected line----------//
    //---------------------------------------------------//
    private void OnMouseOver()
    {
        if(selectable == true)
        {
            GetComponent<SpriteRenderer>().color = selectedColor;
        }
            
    }

    private void OnMouseExit()
    {
        if(selectable == true)
            GetComponent<SpriteRenderer>().color = defaultColor;
    }


    //-------------A line has been selected--------------//
    //---------------------------------------------------//
    private void OnMouseDown()
    {
        if (selectable == true) {
            doTurn();
        }
    }


    public void doTurn() {
        int currentPlayer = GC.WhoseTurn();
        ColorMe(currentPlayer);
        GC.UpdateInfo();
    }

}
