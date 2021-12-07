using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DamagePopUp : MonoBehaviour
{

    private TextMeshProUGUI text;
    [SerializeField] private float lifetime = 0.5f;
    private float minDist = 2f;
    private float maxDist = 3f;

    private Vector3 startPos;
    private Vector3 targetPos;

    private float timer;


    private void Awake()
    {
        text = transform.FindDeepChild("DamageText").GetComponent<TextMeshProUGUI>();
        transform.localScale = Vector3.zero;
    }
    // Start is called before the first frame update
    void Start()
    {
        transform.LookAt(2 * transform.position - Camera.main.transform.position);

        float direction = Random.rotation.eulerAngles.z;
        startPos = transform.position;
        float dist = Random.Range(minDist, maxDist);
        targetPos = startPos + (Quaternion.Euler(0, 0, direction) * new Vector3(dist, dist, 0f));
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        Destroy(gameObject, lifetime);

        transform.localPosition = Vector3.Lerp(startPos, targetPos, Mathf.Sin(timer / lifetime));
        transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, Mathf.Sin(timer / lifetime));
    }

    public void SetStyle(float amount)
    {
        text.text = amount.ToString();
    }

    public void SetStyleCrit(float amount)
    {
        //Set CritImage active
        text.text = amount.ToString();
    }
}
