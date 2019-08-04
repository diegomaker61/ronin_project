using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class ControleInimigo : MonoBehaviour
{   
    public GameObject AgroExterno;
    public float TamanhoAgroExterno;
    public float TamanhoAgroInterno;
    public GameObject Jogador;
    public NavMeshAgent NavAgente;
    public GameObject Lerp;
    public float VelocidadeRotacao;
    public Vector3 LocalRandomico;
    public bool CaminhoLivre;
    NavMeshHit NavHit;

    
    void Start()
    {
        NavAgente = this.GetComponent<NavMeshAgent>();
        Jogador = GameObject.Find("Jogador").gameObject; 
        NavHit.position = new Vector3(this.transform.position.x,0,this.transform.position.z);
    }

    
    void Update(){
        LocalRandomico.y = this.transform.position.y;
        float Distancia = Vector3.Distance(Jogador.transform.position,this.transform.position);
        if(Distancia<=TamanhoAgroInterno){
            NavAgente.SetDestination(Jogador.transform.position);
            OlhandoPara();
        }else{
            
            Patrulha();
            OlhandoPara();
            
        }

        
    }

    public void OnDrawGizmos(){
        Gizmos.color = Color.red;
        //Agro Externo
        Gizmos.DrawWireCube(AgroExterno.transform.position,new Vector3(TamanhoAgroExterno*2,1,TamanhoAgroExterno*2));
        //Agro Interno
        Gizmos.DrawWireSphere(this.transform.position,TamanhoAgroInterno);
        //Ponto randomico Dentro do nav
        Gizmos.DrawWireSphere(NavHit.position,1);
    }

    public void OlhandoPara(){
        Lerp.transform.LookAt(new Vector3(NavAgente.steeringTarget.x,this.transform.position.y,NavAgente.steeringTarget.z));
        float LerpAngle = Mathf.LerpAngle(this.transform.eulerAngles.y,Lerp.transform.eulerAngles.y,Time.deltaTime * VelocidadeRotacao);
        this.transform.eulerAngles = new Vector3(this.transform.eulerAngles.x, LerpAngle,this.transform.eulerAngles.z);
    }

    


    public void Patrulha(){
        
        //Geracao randomica dentro de um agro 
        float Distancia = Vector3.Distance(new Vector3(this.transform.position.x,0,this.transform.position.z),NavHit.position);
        if(Distancia<=1.5f){
            Vector3 PontoAleatorio = new Vector3(Random.Range(AgroExterno.transform.position.x-TamanhoAgroExterno,AgroExterno.transform.position.x+TamanhoAgroExterno),0,
            Random.Range(AgroExterno.transform.position.z-TamanhoAgroExterno,AgroExterno.transform.position.z+TamanhoAgroExterno));
            NavMesh.SamplePosition(PontoAleatorio,out NavHit,TamanhoAgroExterno,1);   
        }
        NavAgente.SetDestination(NavHit.position);
    }
}
