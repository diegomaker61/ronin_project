using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoJogador : MonoBehaviour
{
    [Header("Atributos")]
    
    public int Forca;   // Influencia No ataque e peso limite
    public int Constituicao;    // Influencia na defesa e na vida maxima 
    public int Agilidade;   // Influencia no Reflexo e Stamina 
    public int PontoDeAtributosAtual; // Pontos Para distribuir em Atributos 

    [Header("Caracteristicas")]
    
    public int Ataque; // Influencia no Dano do personagem.
    public int PesoLimite; //Maximo de Peso que o Jogador Carrega
    public int VidaMaxima; // Vida Maxima Jogador 
    public int VidaAtual; //Vida Atual Jogador
    public int Defesa; //Defesa atual do Jogador 
    public int StaminaMaxima; //Stamina Maxima do Jogador 
    public int StaminaAtual; //Stamina Atual do jogador
    public float Reflexo; //Reflexo Atual Jogador 
    public float Alcance; //Alcance do Ataque

    [Header("Experiencia / Nivel ")]
    
    public int NivelAtual;  // Nivel Atual do jogador
    public int ExpAtual;   // Experiencia atual do jogador 
    public List<int> TabelaEXP;
    

    void Start(){
        
    }

    
    void Update(){
    SistemaEXP();
        
    }

    public void SistemaEXP(){
    //Tabela exp
    while(TabelaEXP.Count<101){
        TabelaEXP.Add((TabelaEXP.Count*TabelaEXP.Count)*100);  
    }  

    NivelAtual = Mathf.RoundToInt(Mathf.Sqrt((ExpAtual/100)));
        
    }
}
