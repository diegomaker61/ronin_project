using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CombateJogador : MonoBehaviour
{
    public Camera CameraPrincipal;
    public LayerMask CamadaTodos;
    public GameObject UltimoInimigoSelecionado;
    public GameObject InimigoSelecionado;
    public TextMeshProUGUI Texto_InimigoSelecionado;
     public TextMeshProUGUI THUMBTexto_InimigoSelecionado;
    public Image Fundo_InimigoSelecionado;
    public RaycastHit FinalRaio;
    public float AlturaNomeInimigo;
    public GameObject InfoInimigo;
    public GameObject CameraRetrato;
    public float distanciaMaximaInimigo_Jogador;
    public Image VidaAtualInimigo;
    public bool InfoInimigo_Ativo;
    public TextMeshProUGUI VidaInimigoPorcentagem;
    InfoJogador _InfoJogador ;
    public bool JogadorAtacando;
    public bool MouseEntrouSemEstarPressionado;
    public TextMeshProUGUI Prefab_Log;
    public Image Scroll_log;
    public TextMeshProUGUI TextoHit;
    public Canvas HUD;
    MovimentacaoJogador tempMovJog;
    public bool Alcance;
    public Image VidaJogador;
    public Image StaminaJogador;
    public bool GerandoRaioSeletor;




    //Info Jogador




    void Start()
    {   
        _InfoJogador = this.GetComponent<InfoJogador>();
        InvokeRepeating("AtaqueJogador",0,_InfoJogador.Reflexo);
        tempMovJog = this.GetComponent<MovimentacaoJogador>();
    }

    
    void Update()
    {
    StatusJogador();
    SelecaoInimigo();
    VidaInimigo();
    CliqueAtaque();
    

    }

    public void SelecaoInimigo(){
        
        //Deselecionando  
        if(UltimoInimigoSelecionado!=null){
            Outline LarguraLinha = UltimoInimigoSelecionado.transform.GetChild(0).gameObject.GetComponent<Outline>();
            LarguraLinha.OutlineWidth = 0;
            UltimoInimigoSelecionado = null;
        }


        //Selecionando 
        if(Physics.Raycast(CameraPrincipal.ScreenPointToRay(Input.mousePosition).origin,CameraPrincipal.ScreenPointToRay(Input.mousePosition).direction,out FinalRaio,100,CamadaTodos,QueryTriggerInteraction.Collide)){
            Debug.DrawLine(CameraPrincipal.ScreenPointToRay(Input.mousePosition).origin,FinalRaio.point,Color.blue);
            if(FinalRaio.collider.gameObject.tag == "Inimigo" && FinalRaio.collider.gameObject.GetComponent<ControleInimigo>().Vivo){
                GerandoRaioSeletor = true;
                InimigoSelecionado = FinalRaio.collider.gameObject;
                
                //Selecionando Inimigo 
                Outline LarguraLinha = InimigoSelecionado.transform.GetChild(0).gameObject.GetComponent<Outline>();
                LarguraLinha.OutlineWidth = 4;
                UltimoInimigoSelecionado = InimigoSelecionado;
                
                //Nomeando Inimigo 
                Fundo_InimigoSelecionado.gameObject.SetActive(true);
                GameObject PivorNomeInimigo = InimigoSelecionado.transform.GetChild(2).gameObject;
                Vector3 FinalPos = CameraPrincipal.WorldToScreenPoint(PivorNomeInimigo.transform.position);
                FinalPos.y += AlturaNomeInimigo;
                Fundo_InimigoSelecionado.rectTransform.position = FinalPos;
                Texto_InimigoSelecionado.text = InimigoSelecionado.GetComponent<CombateInimigo>().Nome;
                THUMBTexto_InimigoSelecionado.text = InimigoSelecionado.GetComponent<CombateInimigo>().Nome;

                //Thumb Inimigo 
                InfoInimigo.SetActive(true);
                CameraRetrato.transform.SetParent(InimigoSelecionado.transform.GetChild(3).gameObject.transform);
                CameraRetrato.transform.position = InimigoSelecionado.transform.GetChild(3).gameObject.transform.position;
                CameraRetrato.transform.eulerAngles = InimigoSelecionado.transform.GetChild(3).gameObject.transform.eulerAngles;
                InimigoSelecionado.layer = 12;
                InimigoSelecionado.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.layer =12;
                InfoInimigo_Ativo=true;

            }else{
                Fundo_InimigoSelecionado.gameObject.SetActive(false);
                GerandoRaioSeletor = false;
            }

            
        }

        if(InimigoSelecionado!=null){
        float DistanciaInimigoSelecionado = Vector3.Distance(this.transform.position,InimigoSelecionado.transform.position);
            if(DistanciaInimigoSelecionado>distanciaMaximaInimigo_Jogador){
                InfoInimigo.SetActive(false);
                InimigoSelecionado.layer = 0;
                InimigoSelecionado.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.layer = 0;
                InfoInimigo_Ativo=false;
            }
            
        }

        //Distancia Inimigo -- Jogador
        if(InimigoSelecionado!=null){
            float  DistanciaJogadorInimigo = Vector3.Distance(this.transform.position,InimigoSelecionado.transform.position);
            if(DistanciaJogadorInimigo<_InfoJogador.Alcance){
                Alcance = true;
            }else{
                Alcance = false;
            }
        }
    }

    public void VidaInimigo(){
        
        if(InimigoSelecionado!=null){
            CombateInimigo tempCombateInimigo = InimigoSelecionado.GetComponent<CombateInimigo>();
            if(InfoInimigo_Ativo){
                VidaAtualInimigo.rectTransform.sizeDelta = new Vector2(tempCombateInimigo.VidaAtual*150/tempCombateInimigo.VidaMax,20);
                VidaInimigoPorcentagem.text = (tempCombateInimigo.VidaAtual*100/tempCombateInimigo.VidaMax).ToString() + "%";
            }
        }
    } 

    public void AtaqueJogador(){
        
        if(InimigoSelecionado!=null){
            if(JogadorAtacando && Alcance){
                if(InimigoSelecionado.GetComponent<CombateInimigo>().VidaAtual>0){
                    int Dano = Random.Range(_InfoJogador.Ataque-3,_InfoJogador.Ataque+3);
                        if(Dano - InimigoSelecionado.GetComponent<CombateInimigo>().Defesa > 0 ){
                            tempMovJog.AnimJogador.SetTrigger("AtaqueEspada");
                            InimigoSelecionado.GetComponent<CombateInimigo>().VidaAtual-= Dano - InimigoSelecionado.GetComponent<CombateInimigo>().Defesa;
                            int DanoFinal = Dano - InimigoSelecionado.GetComponent<CombateInimigo>().Defesa;
                            MensagemLog("Causou dano de " + DanoFinal.ToString() + " de hp.",new Color32(60,170,60,255));
                            FeedHitInimigo(DanoFinal);
                        }
                                        
                }
            }
        }
    }

    public void CliqueAtaque(){

        if(InimigoSelecionado!= null){
            
            
            if(FinalRaio.collider.gameObject.tag == "Inimigo" && !Input.GetMouseButton(0)){
                MouseEntrouSemEstarPressionado = true;
            }

            if(FinalRaio.collider.gameObject.tag != "Inimigo"){
                MouseEntrouSemEstarPressionado = false;
            }

            if(MouseEntrouSemEstarPressionado &&  FinalRaio.collider.gameObject.GetComponent<ControleInimigo>().Vivo){
                if(Input.GetMouseButton(0)){
                    JogadorAtacando = true;
                }
            }

            if(JogadorAtacando && Input.GetMouseButtonUp(0)){
                JogadorAtacando = false;
                MouseEntrouSemEstarPressionado=false;
            }
            
        }

        //Rotacionando Jogador para o Inimigo 
        if(JogadorAtacando && InimigoSelecionado!=null){
            tempMovJog.LerpRotacao.transform.LookAt(InimigoSelecionado.transform);
            float AnguloFinal = Mathf.LerpAngle(this.transform.GetChild(1).gameObject.transform.eulerAngles.y,tempMovJog.LerpRotacao.transform.eulerAngles.y,Time.deltaTime*tempMovJog.VelocidadeRotacao);
            this.transform.GetChild(1).gameObject.transform.eulerAngles = new Vector3(0,AnguloFinal,0);
        }
    }

    public void MensagemLog(string Mensagem ,Color Cor){
        
        TextMeshProUGUI NovaMensagem = Instantiate(Prefab_Log,Vector3.zero,Quaternion.identity);
        NovaMensagem.text = Mensagem;
        NovaMensagem.color = Cor;
        NovaMensagem.transform.SetParent(Scroll_log.transform);
    }

    public void FeedHitInimigo(int Hit){  

        Vector3 InimigoTopo = new Vector3(InimigoSelecionado.transform.position.x,InimigoSelecionado.transform.position.y+1,InimigoSelecionado.transform.position.z);
        TextMeshProUGUI NovoHit =Instantiate(TextoHit,CameraPrincipal.WorldToScreenPoint(InimigoTopo),Quaternion.identity);
        NovoHit.transform.SetParent(HUD.transform);
        NovoHit.text = Hit.ToString();
    }

    public void StatusJogador(){
        VidaJogador.rectTransform.sizeDelta = new Vector2(_InfoJogador.VidaAtual*200/_InfoJogador.VidaMaxima,20);
        StaminaJogador.rectTransform.sizeDelta = new Vector2(_InfoJogador.StaminaAtual*200/_InfoJogador.StaminaMaxima,20);
    }
    
}
