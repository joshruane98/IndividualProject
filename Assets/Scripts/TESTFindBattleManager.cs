using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TESTFindBattleManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        BattleManager battleManager = (BattleManager)GameObject.Find("BattleManagerObject").GetComponent(typeof(BattleManager));
        if (battleManager != null)
        {
            Debug.Log("Theres a battle manager");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
