using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Class: Interactable
//The class parent class for all interactable objects.
public class Interactable : MonoBehaviour {
    [SerializeField] //To show in Inspector.
    private int id;

    public int getID()
    {
        return id;
    }

    //Function: interactAction
    //Virtual method to be overridden by subclasses. Defines the behaviour of the object when it is interacted with.
    public virtual void interactAction()
    {
        Debug.Log("Interacted with");
        this.gameObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
    }
}
