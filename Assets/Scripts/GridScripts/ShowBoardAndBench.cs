using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowBoardAndBench
{
    public static void Show(bool show, bool fightIsOn)
    {
        GameObject grid = GameObject.FindGameObjectWithTag("Grid");
        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                GameObject tile = grid.transform.Find(i.ToString() + j.ToString()).gameObject;
                //Bottom half of the board
                if (tile.name[1] - '0' > 3 && !fightIsOn) tile.GetComponent<Renderer>().enabled = show;
                //Bench
                if (i + j < 9) grid.transform.Find((i + j).ToString()).gameObject.GetComponent<Renderer>().enabled = show;
            }
        }
    }
}
