using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour 
{
	public GameObject Lights;
    public GameObject Door;
	void Start () // Use this for initialization
    {
		
	}
	void Update () // Update is called once per frame
    {

	}
    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.E) && other.CompareTag("Player"))
        {
            Debug.Log("Generator has been activated");
            //Lights.SetActive(true);
            Door.SetActive(false);
        }
    }
}
