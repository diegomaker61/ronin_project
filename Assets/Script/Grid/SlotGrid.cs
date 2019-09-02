using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotGrid : MonoBehaviour
{
    public int Custo = -1;
    public int pos_x = 0;
    public int pos_y = 0;
    public TextMesh TextoCusto;
    public TextMesh TextoDistancia;
    ControladorGrid tempCG;
    public GameObject Grid;
    SpriteRenderer Spritethis;
    public float DistanciaJogador;
    public bool Alvo;
    public bool Cheio = false;
    public int  Marcado;
    public bool OcupadoJogador;
   
    
    
    void OnTriggerEnter(Collider other){
        if(other.gameObject.name !="Jogador"){
            Cheio = true;
        }
        
        if(other.gameObject.name =="Jogador"){
            OcupadoJogador=true;
        }
        
    }

    void OnTriggerExit(Collider other) {
        Cheio = false;
        if(other.gameObject.name =="Jogador"){
            OcupadoJogador=false;
        }
          
    }


    void Start()
    {
        TextoCusto = this.transform.GetChild(0).gameObject.GetComponent<TextMesh>();
        TextoDistancia =  this.transform.GetChild(1).gameObject.GetComponent<TextMesh>();
        Grid = this.transform.parent.gameObject;
        Spritethis = this.GetComponent<SpriteRenderer>();
        tempCG = Grid.GetComponent<ControladorGrid>();
    }

    // Update is called once per frame
    void Update()
    {
        TextoCusto.text = Custo.ToString();
        TextoDistancia.text = DistanciaJogador.ToString();
        
        //Incluir comente os visiveis
        DistanciaJogador = Vector3.Distance(new Vector3(tempCG.Jogador.transform.position.x,this.transform.position.y,tempCG.Jogador.transform.position.z),this.transform.position);
        DistanciaJogador = Mathf.RoundToInt(DistanciaJogador);

        //if(Custo!=0 && Custo !=-1){
            //int cor = Mathf.RoundToInt((255*(Custo*10))/100);
            //Spritethis.color = new Color32(255,(byte)cor,(byte)cor,255);
        //}
        if(Custo ==-1){
            Spritethis.color = Color.white;
        }
        if(Alvo){
            Spritethis.color = Color.green;
            Custo=0;
        }

        if(Cheio){
            Spritethis.color = Color.black;
            Custo = 100;
        }

        
    }

    
}
