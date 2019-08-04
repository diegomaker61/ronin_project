using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombateJogador : MonoBehaviour
{
    Camera CameraPrincipal;
    public LayerMask CamadaInimigos;
    void Start()
    {
        
    }

    
    void Update()
    {
    SelecaoInimigo();
        
    }

    public void SelecaoInimigo(){



        
        RaycastHit FinalRaio;
        if(Physics.Raycast(CameraPrincipal.ScreenPointToRay(Input.mousePosition).origin,CameraPrincipal.ScreenPointToRay(Input.mousePosition).direction,out FinalRaio,100,CamadaInimigos,QueryTriggerInteraction.Collide)){
            Debug.DrawLine(CameraPrincipal.ScreenPointToRay(Input.mousePosition).origin,FinalRaio.point,Color.blue);
            
        }
    }
}
