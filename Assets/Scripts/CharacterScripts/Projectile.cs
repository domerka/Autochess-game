using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private GameObject target;

    private MoveObject moveObject;


    float flyingSpeed = 10.0f;

    public void SetValues(GameObject _target, MoveObject _moveObject, float speed)
    {
        target = _target;
        moveObject = _moveObject;
        flyingSpeed = speed;
    }



    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 3.0f, gameObject.transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        if(target.tag == "Dead") Destroy(this.transform.root.gameObject);

        
        MoveProjectile();
    }

    private void MoveProjectile()
    {
        float rotationSpeed = 150.0f;

        float singleStep = rotationSpeed * Time.deltaTime;
        Vector3 newDir = Vector3.RotateTowards(gameObject.transform.forward, target.transform.position - gameObject.transform.position, singleStep, 0.0f);
        gameObject.transform.rotation = Quaternion.LookRotation(newDir);

        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, new Vector3(target.transform.position.x, target.transform.position.y, target.transform.position.z), Time.deltaTime * flyingSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (GameObject.FindGameObjectWithTag("GameControl").GetComponent<GameController>().GetFightIsOn())
        {
            if (other.gameObject.tag == "Enemy" && other.gameObject.name == target.name)
            {
                moveObject.InflictDamage();
                Destroy(this.transform.root.gameObject);
            }
        }
    }

}
