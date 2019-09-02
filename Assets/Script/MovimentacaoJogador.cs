using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using TMPro;
public class MovimentacaoJogador : MonoBehaviour 
{   

    public Camera CameraPrincipal;
    public GameObject PivorCameraPrincipal;
    RaycastHit PontoFinalRaioMouse;
    public RaycastHit RaioPegar;
    public float DistanciaMaximaRaio;
    public LayerMask CamadaRaio;
    public LayerMask CamadaPegar;
    public Text LogRaio;
    public bool Alcance;
    public float VelocidadeJogador;
    CharacterController ControleJogador;
    public float EscalaGravidade;
    Vector3 PontofinalJogadorOlhar;
    [HideInInspector] public float DistanciaJogador_PontoFinal;
    public float DistanciaMinima;
    public Vector3 MovimentacaoFinal;
    public GameObject PivorJogador;
    public GameObject LerpRotacao ;
    public float VelocidadeRotacao;
    public bool CameraManual;
    public GameObject PivorCamera;
    ControleCamera tempControleCamera;
    Vector3 Gravidade;
    public bool PegandoItem;
    public float PegarDistanciaMaxima;
    public bool PegouItem;
    public GameObject ItemSelecionado;
    public Material MaterialItemSelecionado;
    public GameObject UltimoItemSelecionado ;
    public bool EnviandoInfoItem;
    public bool EnviandoInfoMoeda;
    public Image Fundo_InfoNomeItem;
    public TextMeshProUGUI Info_NomeItem;
    public string publicNomeItem;
    public int publicPesoItem;
    public int publicQuantidadeItem;
    public Sprite publicThumbItem;
    public bool publicItemAgrupavel;
    public int publicQuantidadeMoedas;
    public string publicTipoItem;
    public int publicIndiceRef3d;
    public GameObject Inventario;
    public float AlturaInfoNomeItem;
    Material ObjSelecionado_Material;
    public bool PodePegarItem;
    Renderer JogadorShader;
    public bool JogadorNaCamera;
    public LayerMask MascaraTodos;
    Renderer ApagarUltimo;
    Renderer EmpataVisao;
    CombateJogador tempCombateJogador;
    public GameObject JogadorAnimavel;
    public Animator AnimJogador;
    public bool Correndo;
    public bool Parado;
    public float VelocidadeAndando;
    public float VelocidadeCorrendo;
    public bool Vigiando;
    public Material OutlineObjeto;
    public int Cliques;
    public bool CliqueDuplo;


    
    
   

    
    void Start()
    { 
    //Shader
    JogadorShader = this.GetComponent<Renderer>();
    ControleJogador = this.GetComponent<CharacterController>();
    tempControleCamera = PivorCamera.GetComponent<ControleCamera>();
    tempCombateJogador = this.GetComponent<CombateJogador>();
    AnimJogador = JogadorAnimavel.GetComponent<Animator>();
    }

    
    void Update()
    {
    FuncaoPegandoItem();
    SempreVisivel();
    F_Correndo();
    DuploClique();
    
    if(!tempControleCamera.InteragindoInterface && !PegandoItem){
            Movimentacao();
        }
    }

    
    
    public void Movimentacao(){
        
        //Raio da tela para o mundo
        if(Physics.Raycast(CameraPrincipal.ScreenPointToRay(Input.mousePosition).origin,CameraPrincipal.ScreenPointToRay(Input.mousePosition).direction,
            out PontoFinalRaioMouse,DistanciaMaximaRaio,CamadaRaio,QueryTriggerInteraction.Collide)){
            Debug.DrawLine(CameraPrincipal.ScreenPointToRay(Input.mousePosition).origin,PontoFinalRaioMouse.point,Color.blue);
            Alcance = true;
                       
        }else{
            
            Alcance = false;
        }
        
       
        Vector3 PontoFinalRaio = PontoFinalRaioMouse.point;
        PontofinalJogadorOlhar = new Vector3(PontoFinalRaio.x,this.transform.position.y,PontoFinalRaio.z);
        
        //Distancia do Ponto final ate o jogador
        float DistanciaJogador_PontoFinal = Vector3.Distance(this.transform.position,PontofinalJogadorOlhar);
        
        //Jogador Andando
        if(!tempCombateJogador.MouseEntrouSemEstarPressionado && !tempCombateJogador.JogadorAtacando && Input.GetMouseButton(0) && Alcance || CliqueDuplo ){
            
            //Rotacao 
            LerpRotacao.transform.LookAt(PontofinalJogadorOlhar);
            float AnguloFinal = Mathf.LerpAngle(this.transform.GetChild(1).gameObject.transform.eulerAngles.y,LerpRotacao.transform.eulerAngles.y,Time.deltaTime*VelocidadeRotacao);
           
            //Rotacao so é aplicada se o ponto final estiver a uma distancia minima  
            //if(DistanciaJogador_PontoFinal>0.2){
                this.transform.GetChild(1).gameObject.transform.eulerAngles = new Vector3(0,AnguloFinal,0);
            //} 
            
            MovimentacaoFinal = this.transform.GetChild(1).gameObject.transform.forward;
            
            //Estado 1/2 = Andando/Correndo 
            if(Correndo){
                AnimJogador.SetInteger("Estado",2);     
            }else{
                AnimJogador.SetInteger("Estado",1);     
            }

            Parado= false;
            Vigiando=false;
            
             
        }else {
            MovimentacaoFinal.x = 0;
            MovimentacaoFinal.z = 0;

            
            if(!Vigiando){
                AnimJogador.SetInteger("Estado",0);
            }
            
            Parado = true;
        }

            MovimentacaoFinal.y = EscalaGravidade * Physics.gravity.y;
            ControleJogador.Move(MovimentacaoFinal*VelocidadeJogador*Time.deltaTime);
                  

    }

    public void FuncaoPegandoItem(){
        
        //Desligando Item Selecionado 
        if(UltimoItemSelecionado!=null){
        
        //Retirando Outline 
        Outline LarguraLinha = UltimoItemSelecionado.transform.GetChild(0).gameObject.GetComponent<Outline>();
        LarguraLinha.OutlineWidth = 0;
          
        //Deixando Nulo
        UltimoItemSelecionado = null;

        }

        if(Physics.Raycast(CameraPrincipal.ScreenPointToRay(Input.mousePosition).origin,CameraPrincipal.ScreenPointToRay(Input.mousePosition).direction,
            out RaioPegar,PegarDistanciaMaxima,CamadaPegar,QueryTriggerInteraction.Collide)){
                Debug.DrawLine(CameraPrincipal.ScreenPointToRay(Input.mousePosition).origin,PontoFinalRaioMouse.point,Color.red);    
                
                
                if(RaioPegar.collider.gameObject.tag == "Item" || RaioPegar.collider.gameObject.tag == "Moeda"){
                    
                    ItemSelecionado = RaioPegar.collider.gameObject;
                    
                    //Colocando OutLine
                    Outline LarguraLinha = ItemSelecionado.transform.GetChild(0).gameObject.GetComponent<Outline>();
                    LarguraLinha.OutlineWidth = 4;
                    
                    
                    //So pode pegar item se o mouse estiver solto quando passar
                    if(!Input.GetMouseButton(0)){
                        PodePegarItem = true;
                    }
                    
                    //Nome Item
                    Fundo_InfoNomeItem.gameObject.SetActive(true);
                    MostrarNomeItemSelecionado();
                    
                    if(PodePegarItem){
                        //Trava Jogador 
                        PegandoItem = true;
                        //tempControleCamera.ControleAba=false;
                    }

                    if(PodePegarItem && Input.GetMouseButtonDown(0)){
                        //Pegando Item Padrao
                        if(RaioPegar.collider.gameObject.tag == "Item" && !Inventario.GetComponent<ControleInventario>().InventarioCheio_FaltaSlot){
                            ControleItem tempControleItem = RaioPegar.collider.gameObject.GetComponent<ControleItem>();
                            PegarInformacaoItem(tempControleItem.NomeItem,tempControleItem.TipoItem,tempControleItem.PesoItem,tempControleItem.QuantidadeItem,tempControleItem.ThumItem,tempControleItem.ItemAgrupavel,tempControleItem.IndiceRef3D);
                            Destroy(RaioPegar.collider.gameObject);
                            PegouItem = true;
                        }
                        if(RaioPegar.collider.gameObject.tag == "Moeda"){
                            ControleItem tempControleItem = RaioPegar.collider.gameObject.GetComponent<ControleItem>();
                            ControleInventario tempControleInventario = Inventario.GetComponent<ControleInventario>();
                            tempControleInventario.Moedas += tempControleItem.QuantidadeMoeda;
                            Destroy(RaioPegar.collider.gameObject);
                            PegouItem = true;
                        }  
                    }
                }

                UltimoItemSelecionado = ItemSelecionado;
        }else{
            
            if(!PegouItem){
                PegandoItem = false;
                
                //tempControleCamera.ControleAba=true; 
                PodePegarItem = false;
            }   
                Fundo_InfoNomeItem.gameObject.SetActive(false);
                        
        }
        
        if(PegouItem && Input.GetMouseButtonUp(0)){
            PegandoItem = false; 
            
            //tempControleCamera.ControleAba=true;
            PegouItem = false;
        }

    }

    public void PegarInformacaoItem(string NomeItem,string TipoItem, int PesoItem, int QuantidadeItem,Sprite ThumbItem,bool ItemAgrupavel,int IndiceRef3D){
        EnviandoInfoItem = true;
        publicNomeItem = NomeItem;
        publicPesoItem = PesoItem;
        publicQuantidadeItem = QuantidadeItem;
        publicThumbItem = ThumbItem;
        publicItemAgrupavel = ItemAgrupavel;
        publicTipoItem = TipoItem;
        publicIndiceRef3d = IndiceRef3D;
       

    }

    public void MostrarNomeItemSelecionado(){
        
        Vector3 ItemSelecionado = RaioPegar.collider.gameObject.transform.position;
        Vector2 Pos = CameraPrincipal.WorldToScreenPoint(ItemSelecionado);
        Fundo_InfoNomeItem.rectTransform.position = new Vector2(Pos.x,Pos.y+AlturaInfoNomeItem);
        
        
        //Colocando Nome
        if( RaioPegar.collider.gameObject.GetComponent<ControleItem>().TipoItem != "Moeda"){
            Info_NomeItem.text = RaioPegar.collider.gameObject.GetComponent<ControleItem>().NomeItem;
        }else{
            if(RaioPegar.collider.gameObject.GetComponent<ControleItem>().QuantidadeMoeda==1){
                Info_NomeItem.text = RaioPegar.collider.gameObject.GetComponent<ControleItem>().QuantidadeMoeda.ToString() + " Moeda";
            }else{
                Info_NomeItem.text = RaioPegar.collider.gameObject.GetComponent<ControleItem>().QuantidadeMoeda.ToString() + " Moedas";
            }
            
        }
        
    }

    public void SempreVisivel(){
        
        
        if(ApagarUltimo != null){
           Color32 Cor = EmpataVisao.material.color; 
           ApagarUltimo.material.SetColor("_Color",new Color32(Cor.r,Cor.g,Cor.b,255));
           ApagarUltimo = null; 
        }

        RaycastHit hit;
        if(Physics.Raycast(PivorCameraPrincipal.transform.position,CameraPrincipal.transform.forward,out hit,200,MascaraTodos,QueryTriggerInteraction.Collide)){
            Debug.DrawLine(PivorCameraPrincipal.transform.position,hit.point);            
            if(hit.collider.gameObject.tag == "Obstaculo"){
                EmpataVisao =  hit.collider.gameObject.GetComponent<Renderer>();
                Color32 Cor = EmpataVisao.material.color;
                EmpataVisao.material.SetColor("_Color",new Color32(Cor.r,Cor.g,Cor.b,50));
            }
            ApagarUltimo = EmpataVisao;
        }

    }

    public void F_Correndo(){
        
        //Mudando Velocidades
        if(Correndo){
            VelocidadeJogador = VelocidadeCorrendo;
        }else{
            VelocidadeJogador = VelocidadeAndando;
        }

        //Setando corrida
        if(Input.GetKeyDown(KeyCode.C)){
            Correndo =!Correndo;
        }
    }

    public void DuploClique(){
        
        //PrimeiroClique 
        if(Input.GetMouseButtonDown(0)){
            Cliques +=1;
            InvokeRepeating("DesativarPrimeiroClique",0.2f,0);   
        }

        // Ativando Duplo Clique 
        if(Cliques == 2){
            CliqueDuplo = true;
            if(!Input.GetMouseButton(0)){
                Cliques = 0;
            }
        }

        //Desativando Duplo clique 
        if(CliqueDuplo && Input.GetMouseButtonDown(0)){
            CliqueDuplo = false;
        }
    }

    public void DesativarPrimeiroClique(){
        if(Cliques ==1){
            Cliques = 0;
        }
        
    }

}
