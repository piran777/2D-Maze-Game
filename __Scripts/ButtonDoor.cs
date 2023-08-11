using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonDoor : MonoBehaviour
{
    [SerializeField] private Door door;

    private void Update(){
        if (Input.GetKeyDown(KeyCode.F)){
            door.openDoor();
        }
    
    }   
}
