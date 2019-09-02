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
                    ApagandoCaixaOpcao();
                    tempControleInventario.JogarItemChao();

                    
                }

                if(ItemReferencia.GetComponent<ControleThumbItem>().QuantidadeItem>1){
                    tempControleInventario.JanelaQuantidadeItemAtiva = true;
                    tempControleInventario.QuantidadeItensAbandonados =1;
                    tempControleInventario.Texto_QuantidadeItens.text = "1";
                }
            } 
            
            if(EssaOpcao.text == "Equipar"){
            //Se o item for uma arma
                if(ItemReferencia.GetComponent<ControleThumbItem>().TipoItem == "Arma"){
                // Verificar se tem alguma arma equipada 
                    
                    //Limpou Slot Atual
                    ItemReferencia.GetComponent<ControleThumbItem>().Equipado = true;
                    tempControleInventario.tempMovimentacaoJogador.AnimJogador.SetBool("Armada",true); 
                    ItemReferencia.GetComponentInParent<ControleSlot>().SlotOcupado = false;
                    ItemReferencia.transform.SetParent(tempControleInventario.SlotArma1.transform);
                    tempControleInventario.SlotArma1.gameObject.GetComponent<ControleSlot>().SlotOcupado = true;
                    GameObject NovaArma = Instantiate(tempControleInventario.ItensReferencia3D[ItemReferencia.GetComponent<ControleThumbItem>().Referencia3D],Vector3.zero,Quaternion.identity);
                    NovaArma.transform.SetParent(tempControleInventario.MaoDireitaJogador.transform);
                    NovaArma.transform.localPosition = Vector3.zero;
                    NovaArma.transform.localEulerAngles = new Vector3(0,0,-180);
                    ApagandoCaixaOpcao();
                }
            } 
             
            if( EssaOpcao.text == "Desequipar"){
                
                //Se desequipar a arma 
                ControleThumbItem tempItemReferencia = ItemReferencia.GetComponent<ControleThumbItem>();
                tempControleInventario.tempMovimentacaoJogador.PegarInformacaoItem(tempItemReferencia.NomeItem,tempItemReferencia.TipoItem,tempItemReferencia.PesoItem,tempItemReferencia.QuantidadeItem,tempItemReferencia.ThumItem,tempItemReferencia.ItemAgrupavel,tempItemReferencia.Referencia3D);   
                ItemReferencia.GetComponentInParent<ControleSlot>().SlotOcupado = false; 
                    
                    // Se O item a ser desequipado for uma arma 
                    if(ItemReferencia.GetComponent<ControleThumbItem>().TipoItem == "Arma"){
                        tempControleInventario.tempMovimentacaoJogador.AnimJogador.SetBool("Armada",false);
                        GameObject.Destroy(tempControleInventario.MaoDireitaJogador.transform.GetChild(1).gameObject);
                        GameObject.Destroy(ItemReferencia);
                        ApagandoCaixaOpcao();
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


    public void ApagandoCaixaOpcao(){
        tempControleInventario.OpcaoAtiva = false;
        tempControleInventario.OpcoesItem.rectTransform.position = new Vector2(-30,-30);
    }
    
}
