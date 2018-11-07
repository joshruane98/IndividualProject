using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour {
    [SerializeField]
    private int id;

    public int getID()
    {
        return id;
    }

    public virtual void interactAction()
    {
        Debug.Log("Interacted with");
    }
}
