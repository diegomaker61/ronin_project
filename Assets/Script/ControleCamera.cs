using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControleCamera : MonoBehaviour
{
    public GameObject PivorJogador;
    public GameObject Jogador;
    public Camera CameraPrincipal;
    public bool InteragindoInterface;
    public float cameraZoom;
    public float VelocidadeZoom;
    public float ZoomMaximo;
    public float ZoomMinimo;
    public float VelocidadeCamera;
    public float SensibilidadeMouse;
    public float rotacaoCameray;



    void Start()
    {
        
    }

    
    void Update()
    {
    //Camera seguindo jogador
    FuncaoCameraZoom();
    CameraRotacao();
    }

    
    
    public void FuncaoCameraZoom(){
        //Zoom Camera 
        cameraZoom -= Input.GetAxis("Mouse ScrollWheel")*5;
        cameraZoom = Mathf.Clamp(cameraZoom,ZoomMinimo,ZoomMaximo);
        Vector3 VectorZoom = Jogador.transform.position + CameraPrincipal.transform.forward * cameraZoom*(-1);
        this.transform.position = Vector3.Lerp(this.transform.position,VectorZoom,Time.deltaTime*VelocidadeZoom);

    }
    
    public void CameraRotacao(){
        if(Input.GetMouseButton(1) && !InteragindoInterface){
            rotacaoCameray += Input.GetAxis("Mouse X")*SensibilidadeMouse;
            PivorJogador.transform.eulerAngles = new Vector3(0,rotacaoCameray,0);
        }
    }

    public void InterfaceAtiva(){
        InteragindoInterface = true;
    }

    public void InterfaceDesativa(){
        InteragindoInterface = false;
    }

}  


    


