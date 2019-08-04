using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Opcao : MonoBehaviour
{
    GameObject Inventario;
    public bool OpcaoAtiva;
    public bool DestruaOpcao;
    public bool _mouseDentroOpcao;
    TextMeshProUGUI EssaOpcao;
    public GameObject ItemReferencia;
    ControleInventario tempControleInventario ;
    Image ListaOpcoes;

    void Start()
    {
    Inventario = GameObject.Find("Inventario");
    EssaOpcao = this.GetComponent<TextMeshProUGUI>();
    tempControleInventario = Inventario.GetComponent<ControleInventario>();
    

    }

    // Update is called once per frame
    void Update()
    {
        OpcaoAtiva = Inventario.GetComponent<ControleInventario>().OpcaoAtiva;
        //Apagando Opcao
        if(!OpcaoAtiva){
            GameObject.Destroy(this.gameObject);
        }

        if(_mouseDentroOpcao){
            EssaOpcao.color = new Color32(0,160,0,180);
        }else{
            EssaOpcao.color = new Color32(255,255,255,180);
        }

        if(_mouseDentroOpcao && Input.GetMouseButtonDown(0)){

            if(EssaOpcao.text =="Abandonar"){
                
                if(ItemReferencia.GetComponent<ControleThumbItem>().QuantidadeItem==1){

                    ItemReferencia.GetComponentInParent<ControleSlot>().SlotOcupado = false;
                    GameObject.Destroy(ItemReferencia);
                    tempControleInventario.OpcaoAtiva = false;
                    tempControleInventario.OpcoesItem.rectTransform.position = new Vector2(-30,-30);
                    tempControleInventario.JogarItemChao();

                    
                }

                if(ItemReferencia.GetComponent<ControleThumbItem>().QuantidadeItem>1){
                    tempControleInventario.JanelaQuantidadeItemAtiva = true;
                    tempControleInventario.QuantidadeItensAbandonados =1;
                    tempControleInventario.Texto_QuantidadeItens.text = "1";
                }
            }  
        }
    }

    public void MouseDentroOpcao(){
        _mouseDentroOpcao = true;
    }

    public void MouseForaOpcao(){
        _mouseDentroOpcao= false;
    }

    
}
