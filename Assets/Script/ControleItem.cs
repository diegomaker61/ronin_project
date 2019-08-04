using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControleItem : MonoBehaviour
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
    public bool SelecionadoForaInventario;
    public Material ItemMaterial;
    public Sprite ThumItem;

    
    

    void Start()
    {
        ItemMaterial = this.GetComponent<Renderer>().material;
    }


    void Update()
    {       
      
    }

    

    public void AlterarBrilho(){
        if(SelecionadoForaInventario){
            ItemMaterial.SetColor("_EmissionColor",new Color32(30,30,30,30));
        }else{
            ItemMaterial.SetColor("_EmissionColor",new Color32(0,0,0,0));
        }
    }
}
