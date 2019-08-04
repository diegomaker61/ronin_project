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
    public int Reflexo; //Reflexo Atual Jogador 

    [Header("Experiencia / Nivel ")]
    
    public int NivelAtual;  // Nivel Atual do jogador
    public int ExpAtual;   // Experiencia atual do jogador 
    public int[] Niveis; //Array de Niveis

    void Start(){
        VidaMaxima = 100;

    }

    
    void Update(){
        
    }
}
