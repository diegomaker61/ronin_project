using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgroExterno : MonoBehaviour
{
    public float TamanhoAgroExterno;
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    
    public void OnDrawGizmos()
    {
    Gizmos.color = Color.yellow;
    Gizmos.DrawWireCube(this.transform.position,new Vector3(TamanhoAgroExterno*2,0.5f,TamanhoAgroExterno*2));   
    }
}
