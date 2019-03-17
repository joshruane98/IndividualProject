using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour {
    [SerializeField] //To show in Inspector.
    private int id;

    public int getID()
    {
        return id;
    }

    public virtual void interactAction()
    {
        Debug.Log("Interacted with");
        this.gameObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
    }
}
