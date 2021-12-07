using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderTest : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (GameObject.FindGameObjectWithTag("GameControl").GetComponent<GameController>().GetFightIsOn())
        {
            if (other.gameObject.tag == "Enemy" && other.gameObject.name == this.gameObject.transform.root.GetComponent<MoveObject>().target.name)
            {
                this.gameObject.transform.root.GetComponent<MoveObject>().InflictDamage();
            }
        }
    }
}
