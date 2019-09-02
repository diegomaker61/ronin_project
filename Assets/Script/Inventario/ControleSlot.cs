using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class ControleSlot : MonoBehaviour
{   

    public bool SlotOcupado;
    public int LimiteSlot;
    public int NumeroDeFilhos;
    public bool mouseDentro;
    public string TipoSlot;
    Image EsseSlot;



    void Start() {
    EsseSlot = this.GetComponent<Image>();
    

    }
    void Update(){
        if(mouseDentro){
            if(SlotOcupado){
                EsseSlot.color = new Color32(154,154,60,225);
            }
        }else{
            EsseSlot.color = new Color32(22,22,22,255);
        }
        
    }

    public void MouseDentroSlot(){
        mouseDentro = true;
    }

    public void MouseForaSlot(){
        mouseDentro = false;
    }

    
}
