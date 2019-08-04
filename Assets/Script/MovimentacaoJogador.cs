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
    public GameObject Inventario;
    public float AlturaInfoNomeItem;
    Material ObjSelecionado_Material;
    public bool PodePegarItem;
    

    void Start()
    {
    ControleJogador = this.GetComponent<CharacterController>();
    tempControleCamera = PivorCamera.GetComponent<ControleCamera>();
    }

    
    void Update()
    {
    FuncaoPegandoItem();
    }

    void FixedUpdate(){
        if(!tempControleCamera.InteragindoInterface && !PegandoItem){
            Movimentacao();
        }
    }
    
    public void Movimentacao(){
        
        //Raio da tela para o mundo
        if(Physics.Raycast(CameraPrincipal.ScreenPointToRay(Input.mousePosition).origin,CameraPrincipal.ScreenPointToRay(Input.mousePosition).direction,
            out PontoFinalRaioMouse,DistanciaMaximaRaio,CamadaRaio,QueryTriggerInteraction.Collide)){
            //Debug.DrawLine(CameraPrincipal.ScreenPointToRay(Input.mousePosition).origin,PontoFinalRaioMouse.point,Color.blue);
            LogRaio.text = "Dentro do Alcance";
            Alcance = true;
        }else{
            LogRaio.text = "Fora do Alcance";
            Alcance = false;
        }
        
       
        Vector3 PontoFinalRaio = PontoFinalRaioMouse.point;
        PontofinalJogadorOlhar = new Vector3(PontoFinalRaio.x,this.transform.position.y,PontoFinalRaio.z);
        
        //Distancia do Ponto final ate o jogador
        float DistanciaJogador_PontoFinal = Vector3.Distance(this.transform.position,PontofinalJogadorOlhar);
        if(Input.GetMouseButton(0) && Alcance){
            
            LerpRotacao.transform.LookAt(PontofinalJogadorOlhar);
            float AnguloFinal = Mathf.LerpAngle(this.transform.eulerAngles.y,LerpRotacao.transform.eulerAngles.y,Time.deltaTime*VelocidadeRotacao);
           
            //Rotacao so é aplicada se o ponto final estiver a uma distancia minima  
            if(DistanciaJogador_PontoFinal>0.2){
                this.transform.eulerAngles = new Vector3(0,AnguloFinal,0);
            } 
            
            MovimentacaoFinal = this.transform.forward;      
        }else {
            MovimentacaoFinal.x = 0;
            MovimentacaoFinal.z = 0;
        }

            MovimentacaoFinal.y = EscalaGravidade * Physics.gravity.y;
            ControleJogador.Move(MovimentacaoFinal*VelocidadeJogador*Time.deltaTime);
                  

    }

    public void FuncaoPegandoItem(){
        //Desligando Item Selecionado 
        if(UltimoItemSelecionado!=null){
          Material  MaterialUltimoItemSelecionado = UltimoItemSelecionado.GetComponent<Renderer>().material;
          MaterialUltimoItemSelecionado.SetColor("_EmissionColor",new Color32(0,0,0,0));
          UltimoItemSelecionado = null;
        }

        if(Physics.Raycast(CameraPrincipal.ScreenPointToRay(Input.mousePosition).origin,CameraPrincipal.ScreenPointToRay(Input.mousePosition).direction,
            out RaioPegar,PegarDistanciaMaxima,CamadaPegar,QueryTriggerInteraction.Collide)){
                Debug.DrawLine(CameraPrincipal.ScreenPointToRay(Input.mousePosition).origin,PontoFinalRaioMouse.point,Color.red);    
                
                
                if(RaioPegar.collider.gameObject.tag == "Item" || RaioPegar.collider.gameObject.tag == "Moeda"){
                    
                    ItemSelecionado = RaioPegar.collider.gameObject;
                    MaterialItemSelecionado = ItemSelecionado.GetComponent<Renderer>().material;
                    MaterialItemSelecionado.SetColor("_EmissionColor",new Color32(30,30,30,30));

                    //So pode pegar item se o mouse estiver solto quando passar
                    if(!Input.GetMouseButton(0)){
                        PodePegarItem = true;
                    }
                    
                    //InfoItem
                    Fundo_InfoNomeItem.gameObject.SetActive(true);
                    MostrarNomeItemSelecionado();
                    
                    if(PodePegarItem){
                        //Trava Jogador 
                        PegandoItem = true;
                        tempControleCamera.ControleAba=false;
                    }

                    if(PodePegarItem && Input.GetMouseButtonDown(0)){
                        //Pegando Item Padrao
                        if(RaioPegar.collider.gameObject.tag == "Item" && !Inventario.GetComponent<ControleInventario>().InventarioCheio_FaltaSlot){
                            ControleItem tempControleItem = RaioPegar.collider.gameObject.GetComponent<ControleItem>();
                            PegarInformacaoItem(tempControleItem.NomeItem,tempControleItem.TipoItem,tempControleItem.PesoItem,tempControleItem.QuantidadeItem,tempControleItem.ThumItem,tempControleItem.ItemAgrupavel);
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
                tempControleCamera.ControleAba=true; 
                PodePegarItem = false;
            }   
                Fundo_InfoNomeItem.gameObject.SetActive(false);
                        
        }
        
        if(PegouItem && Input.GetMouseButtonUp(0)){
            PegandoItem = false; 
            tempControleCamera.ControleAba=true;
            PegouItem = false;
        }

    }

    public void PegarInformacaoItem(string NomeItem,string TipoItem, int PesoItem, int QuantidadeItem,Sprite ThumbItem,bool ItemAgrupavel ){
        EnviandoInfoItem = true;
        publicNomeItem = NomeItem;
        publicPesoItem = PesoItem;
        publicQuantidadeItem = QuantidadeItem;
        publicThumbItem = ThumbItem;
        publicItemAgrupavel = ItemAgrupavel;
        publicTipoItem = TipoItem;

    }

    public void MostrarNomeItemSelecionado(){
        
        Vector3 ItemSelecionado = RaioPegar.collider.gameObject.transform.position;
        ItemSelecionado.y = ItemSelecionado.y+ AlturaInfoNomeItem;
        Fundo_InfoNomeItem.rectTransform.position = CameraPrincipal.WorldToScreenPoint(ItemSelecionado); 
        Info_NomeItem.text = RaioPegar.collider.gameObject.GetComponent<ControleItem>().NomeItem;
    }


}
