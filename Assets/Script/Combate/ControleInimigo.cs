using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;


public class ControleInimigo : MonoBehaviour 
{   
    GameObject AgroExterno;
    public float TamanhoAgroExterno;
    public float TamanhoAgroInterno;
    GameObject Jogador;
    NavMeshAgent NavAgente;
    public GameObject Lerp;
    public float VelocidadeRotacao;
    public Vector3 LocalRandomico;
    CombateInimigo tempCombateInimigo;
    CapsuleCollider thisCollider;
    public bool Vivo = true ;
    public bool Morto;
    public float TempoDestruicaoInimigo;
    public GameObject InfoInimigo;
    public bool JogadorProximo;
    InfoJogador tempInfoJogador;
    CombateJogador tempCombateJogador;
    public float DistanciaPatrulha;


    [Header("Info HUD")]
    public TextMeshProUGUI TextoHit;
    public Camera CameraPrincipal;
    public Canvas HUD;
    Animator AnimInimigo;


    [Header("Loot")] 
    public GameObject [] QuedaLoot;
    public int DropMoedasMax = 0; 
    public GameObject Moedas;
    AgroExterno tempAgroExterno;
    GameObject Waypoint;
    public GameObject Prefab_Waypoint;
    float crono;
    public float TempoDescanso;
    
   



    public void OnTriggerStay(Collider other){
        //Se o target for gerado muito proximo 
        if(other.gameObject == Waypoint){
            crono += Time.deltaTime;    
            if(crono>=TempoDescanso){//Depois de 2 segundos que esta dentro do waypoint atualize 
                 Waypoint.transform.position = Patrulha();
                 crono = 0;
            }
        }
    }
    
    void Start()
    {
        NavAgente = this.GetComponent<NavMeshAgent>();
        Jogador = GameObject.Find("Jogador").gameObject; 
        tempCombateInimigo = this.GetComponent<CombateInimigo>();
        thisCollider = this.GetComponent<CapsuleCollider>();
        tempInfoJogador = Jogador.GetComponent<InfoJogador>();
        tempCombateJogador = Jogador.GetComponent<CombateJogador>();
        InvokeRepeating("InimigoAtacando",0,1);
        AnimInimigo = this.gameObject.transform.GetChild(0).gameObject.GetComponent<Animator>();
        AgroExterno = this.gameObject.transform.parent.gameObject;
        tempAgroExterno = this.gameObject.GetComponentInParent<AgroExterno>();
        Waypoint = Instantiate(Prefab_Waypoint,new Vector3(this.transform.position.x,0,this.transform.position.z+2),Quaternion.identity);
        
        
        
    }

    
    void  Update(){
        Combate();
        IA_controle();

        //Atualizando agro externo 
        TamanhoAgroExterno = tempAgroExterno.TamanhoAgroExterno;
    }


    public void IA_controle(){
        
        LocalRandomico.y = this.transform.position.y;
        float Distancia = Vector3.Distance(Jogador.transform.position,this.transform.position);
        
        if(Vivo){
        if(Distancia<=TamanhoAgroInterno){
            NavAgente.SetDestination(Jogador.transform.position);
            OlhandoPara();
        }else{
            NavAgente.SetDestination(Waypoint.transform.position);
            OlhandoPara(); 
        }
        }  
        

    }

    public void OnDrawGizmos(){
        //Cor do gizmo
        Gizmos.color = Color.red;
        //Agro Interno
        Gizmos.DrawWireSphere(this.transform.position,TamanhoAgroInterno);
        //Target
        //Gizmos.DrawWireSphere(navHit.position,0.3f);
    }

    public void OlhandoPara(){
        Lerp.transform.LookAt(new Vector3(NavAgente.steeringTarget.x,this.transform.position.y,NavAgente.steeringTarget.z));
        float LerpAngle = Mathf.LerpAngle(this.transform.eulerAngles.y,Lerp.transform.eulerAngles.y,Time.deltaTime * VelocidadeRotacao);
        this.transform.eulerAngles = new Vector3(this.transform.eulerAngles.x, LerpAngle,this.transform.eulerAngles.z);
    }

    
    public Vector3 Patrulha(){
        LocalRandomico = Random.insideUnitSphere * TamanhoAgroExterno;
        LocalRandomico += AgroExterno.transform.position;
        NavMeshHit navHit;
        NavMesh.SamplePosition (LocalRandomico,out navHit, TamanhoAgroExterno,1);
        return navHit.position;
    }

    
    public void Combate(){
        
        //Morrendo 
        if(tempCombateInimigo.VidaAtual<=0 && !Morto){
            AnimInimigo.SetTrigger("Morrendo");
            Vivo = false;
            thisCollider.isTrigger = true;
            InvokeRepeating("DestroindoInimigo",TempoDestruicaoInimigo,0);
            InfoInimigo.SetActive(false);
            Loot();
            Morto= true;   
        }

        //Proximo do Jogador 
        float DistanciaJogadorInimigo = Vector3.Distance(Jogador.transform.position,this.transform.position);
        if(DistanciaJogadorInimigo< tempCombateInimigo.Alcance && !Morto){
            JogadorProximo = true;
        }else{
            JogadorProximo = false;
        }


    }

    public void DestroindoInimigo(){
        GameObject.Destroy(this.gameObject);
    }

    public void InimigoAtacando(){
        if(JogadorProximo){
            AnimInimigo.SetTrigger("Atacando");
            int Dano = Random.Range(tempCombateInimigo.Ataque-3,tempCombateInimigo.Ataque+3) - tempInfoJogador.Defesa;
            tempInfoJogador.VidaAtual -= Dano;
            tempCombateJogador.MensagemLog("Você sofreu dano de " + Dano.ToString() + " de hp.", new Color32(220,32,32,255));
            FeedHitJogador(Dano, Color.red);
        }
    }

    public void FeedHitJogador(int Hit , Color Cor){  
        Vector3 InimigoTopo = new Vector3(Jogador.transform.position.x,Jogador.transform.position.y+1,Jogador.transform.position.z);
        TextMeshProUGUI NovoHit =Instantiate(TextoHit,CameraPrincipal.WorldToScreenPoint(InimigoTopo),Quaternion.identity);
        NovoHit.transform.SetParent(HUD.transform);
        NovoHit.text = Hit.ToString();
        NovoHit.color = Cor; 
    }

    public void Loot(){
        
        //Random Quantidade Moedas
        //80% de chance de queda de moedas
        if(Random.value< 0.8f){ 
            int NumeroAleatorio = Random.Range(1,DropMoedasMax);    
        if(NumeroAleatorio!=0){
            //Criando Moedas 
            Vector3 LocalAletorioMoedas = new Vector3(Random.Range(this.transform.position.x-1,this.transform.position.x+1),0.05f,Random.Range(this.transform.position.z-1,this.transform.position.z+1));
            GameObject moedas = Instantiate(Moedas,LocalAletorioMoedas,Quaternion.identity);
            moedas.GetComponent<ControleItem>().QuantidadeMoeda = NumeroAleatorio;

        }
        }
        
        //Itens Comuns
        if(Random.value<0.5f){ 
        //Colocar Items 
        }

        //Itens Raros
        if(Random.value <0.1f){  
        //Colocar Items 
        }

    }

}
