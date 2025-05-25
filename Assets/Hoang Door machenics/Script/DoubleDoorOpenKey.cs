using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleDoorOpenKey : MonoBehaviour
{
    public GameObject Instruction;
    public GameObject AnimeObject1;
    public GameObject AnimeObject2;
    public AudioSource DoorOpenSound;
    public bool Action = false;

    void Start()
    {
        Instruction.SetActive(false);

    }

    void OnTriggerEnter(Collider collision)  //this is when player interact with the door trigger area
    {
        if (collision.transform.tag == "Player")
        {
            Instruction.SetActive(true);
            Action = true;
        }
    }

    void OnTriggerExit(Collider collision) //this is when player leaves the door trigger area
        {
        Instruction.SetActive(false);
        Action = false;
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) 
        {
            if (Action == true) //this occurs when player collides with the door's triggers
                {
                Instruction.SetActive(false); //this makes the "Press [E] to open" instruction appear
                    AnimeObject1.GetComponent<Animator>().Play("SwingDoorOpen"); //this plays the door openning animation
                    AnimeObject2.GetComponent<Animator>().Play("SwingDoorOpen"); //this plays the sound of the door
                    DoorOpenSound.Play(); //this disables the above functions until players re-enter the door trigger area
                    Action = false; //this disables the above functions until players re-enter the door trigger area
                }
        }

    }
}