using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ControleThumbItem : MonoBehaviour
{
    public string NomeItem;
    public string TipoItem;
    public int AtaqueItem;
    public int DefesaItem;
    public int PesoItem;
    public int QuantidadeItem; 
    public bool DentroInventario;
    public bool ItemAgrupavel;
    public int QuantidadeMoeda;
    public Sprite ThumItem;
    public TextMeshProUGUI TextoQuantidade;
    public TextMeshProUGUI Opcao;
    public Image ItemOpcoes;
    GameObject Inventario;
    ControleInventario tempControleInventario;
    public bool _MouseDentro;
    public bool OpcaoAberta;
    
    //Opcao
    TextMeshProUGUI OpcaoUsar;
    TextMeshProUGUI OpcaoJogarFora;
    


    void Start()
    {
        TextoQuantidade = this.GetComponentInChildren<TextMeshProUGUI>();
        Inventario = GameObject.Find("Inventario").gameObject;
        tempControleInventario = Inventario.GetComponent<ControleInventario>();
        Opcao = tempControleInventario.Opcao;
        ItemOpcoes = tempControleInventario.OpcoesItem;
        
    }

    
    
    
    void Update()
    {   
        
        //Mostrando Quantidade item
        TextoQuantidade.text = QuantidadeItem.ToString();
        
        //Abrindo Opcoes
        if(_MouseDentro && Input.GetMouseButtonDown(1) && !OpcaoAberta){
            ClicouItem();
            OpcaoAberta = true;
        }

        //Fechando Opcoes
        if(OpcaoAberta && !_MouseDentro && !tempControleInventario.MouseDentroLista){
            FecharOpcao();
        }

    }

    public void ClicouItem(){
        tempControleInventario.ItemSelecionadoDentroInventario = this.gameObject;

        if(TipoItem =="Pocao"){
            tempControleInventario.OpcaoAtiva = true;
            ItemOpcoes.transform.position = Input.mousePosition;
            
            OpcaoUsar = Instantiate(Opcao,Vector3.zero,Quaternion.identity);
            OpcaoUsar.GetComponentInChildren<TextMeshProUGUI>().text = "Usar";
            OpcaoUsar.transform.SetParent(ItemOpcoes.transform);
            OpcaoUsar.GetComponent<Opcao>().ItemReferencia = this.gameObject;

            OpcaoJogarFora = Instantiate(Opcao,Vector3.zero,Quaternion.identity);
            OpcaoJogarFora.GetComponentInChildren<TextMeshProUGUI>().text = "Abandonar";
            OpcaoJogarFora.transform.SetParent(ItemOpcoes.transform);
            OpcaoJogarFora.GetComponent<Opcao>().ItemReferencia = this.gameObject;
        }   
    }

    public void MouseDentro(){
        _MouseDentro=true;
    }

    public void MouseFora(){   
        //Retorna que o mouse esta fora do item
        _MouseDentro = false;
        
    }

    public void FecharOpcao(){
        
        //Destroy Opcaoes
        tempControleInventario.OpcaoAtiva = false;

        //Content de opcoes vai para fora da hud 
        ItemOpcoes.rectTransform.position = new Vector2(-30,-30);

        //Evita criar Duplicatas
        OpcaoAberta = false; 
    }

    
}   
