using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderTest : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other == null) return;
        if (GameObject.FindGameObjectWithTag("GameControl").GetComponent<GameController>().GetFightIsOn())
        {
            //print(gameObject.transform.root.transform.name);
            //print(other.gameObject.name);
            //if (!gameObject.transform.root.GetComponent<MoveObject>().InRangeTarget()) return;
            if ((other.gameObject.tag == "Enemy" && other.gameObject.name == gameObject.transform.root.GetComponent<MoveObject>().target.name) || (other.gameObject.tag == "Ally" && other.gameObject.name == gameObject.transform.root.GetComponent<MoveObject>().target.name))
            {
                gameObject.transform.root.GetComponent<MoveObject>().InflictDamage();
            }
        }
    }
}
