using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControleCamera : MonoBehaviour
{
    public GameObject PivorJogador;
    public GameObject Jogador;
    public Image AncoraCima;
    public Image PivorDireitaJogador_Screen;
    public Image AbaDireitaRotacao;
    public Image AncoraDireita;
    public Image AncoraEsquerda;
    public GameObject PivorDireitaJogador_World;
    public GameObject PivorEsquerdaJogador_World;
    public Image PivorEsquerdaJogador_Screen;
    public Image AbaEsquerdaRotacao;
    public Image AbaDebug;
    public Camera CameraPrincipal;
    public bool CameraRotacionarEsquerda;
    public bool CameraRotacionarDireita;
    public bool CameraManual;
    public bool LookCameraVertical;
    public bool InteragindoInterface;
    public bool AbasRotacaoVisivel;
    public bool TransicaoCamera;
    public bool Debug;
    public bool ControleAba;
    public float rotacaoCamerax;
    public float rotacaoCameray;
    public float VelocidadeRotacaoCameraManual;
    public float VelocidadeRotacaoCameraAutomatica;
    public float cameraZoom;
    public float LarguraAbaDireita;
    public float LarguraAbaEsquerda;
    public float VelocidadeTransicao;



    void Start()
    {
        
    }

    
    void Update()
    {
    PivorJogador.transform.position = Jogador.transform.position;
    FuncaoCameraZoom();
    AbasRotacaoAutomatica();
    
    //Visibilidade das Abas de Rotacao
    if(AbasRotacaoVisivel){
        AbaDireitaRotacao.color = new Color32(255,255,255,80);
        AbaEsquerdaRotacao.color = new Color32(255,255,255,80);
    }else{
        AbaDireitaRotacao.color = new Color32(255,255,255,0);
        AbaEsquerdaRotacao.color = new Color32(255,255,255,0);
    }

    if(Input.GetKeyDown("d")){
        Debug = !Debug;
    }

    if(Debug){
        AbaDebug.gameObject.SetActive(true);
    }else{
        AbaDebug.gameObject.SetActive(false);
    }

    }

    void FixedUpdate() {
    FuncaoCameraManual();
    
    if(ControleAba){
        if(CameraRotacionarDireita && Input.GetMouseButton(0) ){
            PivorJogador.transform.Rotate(Jogador.transform.up*VelocidadeRotacaoCameraAutomatica*Time.deltaTime);
        }
    
        if(CameraRotacionarEsquerda && Input.GetMouseButton(0)){
            PivorJogador.transform.Rotate(Jogador.transform.up*-VelocidadeRotacaoCameraAutomatica*Time.deltaTime);
        }
    }

        if(TransicaoCamera && Input.GetMouseButton(0)){
            float LerpAngle = Mathf.LerpAngle(PivorJogador.transform.eulerAngles.x,0,Time.deltaTime*VelocidadeTransicao);
            PivorJogador.transform.eulerAngles = new Vector3(LerpAngle,PivorJogador.transform.eulerAngles.y,0);
            if(PivorJogador.transform.eulerAngles.x == 0){
                TransicaoCamera = false;
            }
        }

        if(CameraManual){
            TransicaoCamera = false;
        }

    }

     public void AbasRotacaoAutomatica(){

        LarguraAbaDireita = Vector2.Distance(AncoraDireita.transform.position,PivorDireitaJogador_Screen.transform.position);
        AbaDireitaRotacao.transform.position = CameraPrincipal.WorldToScreenPoint(PivorDireitaJogador_World.transform.position);
        
        LarguraAbaEsquerda = Vector2.Distance(AncoraEsquerda.transform.position,PivorEsquerdaJogador_Screen.transform.position);
        AbaEsquerdaRotacao.transform.position = CameraPrincipal.WorldToScreenPoint(PivorEsquerdaJogador_World.transform.position);
        
    }



    public void FuncaoCameraManual(){
        rotacaoCamerax = PivorJogador.transform.eulerAngles.y;
        rotacaoCameray = PivorJogador.transform.eulerAngles.x;
        //Rotacao Camera 
        if(Input.GetMouseButton(1) && CameraManual){
            rotacaoCamerax += Input.GetAxis("Mouse X")*VelocidadeRotacaoCameraManual;
            if(!LookCameraVertical){
                rotacaoCameray += Input.GetAxis("Mouse Y")*-VelocidadeRotacaoCameraManual;
            } 
            PivorJogador.transform.eulerAngles = new Vector3(rotacaoCameray,rotacaoCamerax,0);
        }
    }

    public void FuncaoCameraZoom(){
        
        //Zoom Camera 
        cameraZoom -= Input.GetAxis("Mouse ScrollWheel")*5;
        cameraZoom = Mathf.Clamp(cameraZoom,3,12);
        Vector3 VectorZoom = Jogador.transform.position + CameraPrincipal.transform.forward * cameraZoom*(-1);
        this.transform.position = Vector3.Lerp(this.transform.position,VectorZoom,Time.deltaTime*VelocidadeTransicao);

    }


    public void LookCamera(){
        LookCameraVertical=!LookCameraVertical;
    }

    public void CameraManualToglle(){
        CameraManual=!CameraManual;

        if(!CameraManual){
            TransicaoCamera = true;
        }
    }

    public void CameraRotacionarDireitaAtivando(){
        if(!CameraManual){
            CameraRotacionarDireita = true;
        }
    }

    public void AbasRotacaoVisibilidade(){
        AbasRotacaoVisivel =! AbasRotacaoVisivel;
    }
    
    public void CameraRotacionarDireitaDesativando(){
        CameraRotacionarDireita = false;
    }

    public void CameraRotacionarEsquerdaAtivando(){
        if(!CameraManual ){
            CameraRotacionarEsquerda = true;
        }
    }

    public void CameraRotacionarEsquerdaDesativando(){
        CameraRotacionarEsquerda = false;
    }

    public void FuncaoInteragirInterfaceAtivando(){
        InteragindoInterface = true;
    }

    public void FuncaoInteragirInterfaceDesativando(){
        InteragindoInterface=false;
    }

}
