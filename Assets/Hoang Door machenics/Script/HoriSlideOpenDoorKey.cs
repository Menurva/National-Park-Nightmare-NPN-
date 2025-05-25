using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoriSlideOpenDoorKey : MonoBehaviour
{
    public GameObject Instruction;
    public GameObject AnimeObject;
    public AudioSource DoorOpenSound;
    public bool Action = false;

    void Start()
    {
        Instruction.SetActive(false);

    }

    void OnTriggerEnter(Collider collision) //this is when player interact with the door trigger area
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
                AnimeObject.GetComponent<Animator>().Play("HoriSlideDoorOpen"); //this plays the door openning animation
                DoorOpenSound.Play(); //this plays the sound of the door
                Action = false; //this disables the above functions until players re-enter the door trigger area
            }
        }

    }
}