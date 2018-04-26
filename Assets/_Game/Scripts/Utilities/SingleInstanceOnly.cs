using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleInstanceOnly : MonoBehaviour {

    #region variables
    [Tooltip("If a new object is created with the same instance number as the one already existing in the world, destroy the new instance object")]
    [SerializeField]
    private int m_instanceId = 0;

    static private Dictionary<int, SingleInstanceOnly> s_idToInstanceMapping = new Dictionary<int, SingleInstanceOnly>();
    #endregion  

    private void Awake()
    {
        //If the object with the same instance id already exist, delete this object
        if (s_idToInstanceMapping.ContainsKey(m_instanceId))
            Destroy(gameObject);
        //If the object hasn't existed in the game, add this object to the dictionary
        else
            s_idToInstanceMapping[m_instanceId] = this;
    }
}
