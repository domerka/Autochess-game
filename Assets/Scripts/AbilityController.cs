using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class AbilityController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //This class is needed because of various animations and stuff has to be done separetely for everyone
    public static void CastAbility(string name)
    {
        switch (name)
        {
            case "boxer":
                CastBoxer();
                break;
            case "archer":
                CastArcher();
                break;

        }
    }

    private static void CastBoxer()
    {

    }

    private static void CastArcher()
    {

    }

}
