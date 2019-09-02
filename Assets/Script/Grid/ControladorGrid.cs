using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ControladorGrid : MonoBehaviour
{   
    public Camera CameraPrincipal;
    public LayerMask Geral;
    public GameObject prefabSlotGrid;
    public Vector3 Inicio = Vector3.zero;
    public int LarguraGrid;
    public int ProfundidadeGrid;
    public GameObject[,] ArrayGrid;
    public List<GameObject> ListGrid;
    public float Escala;
    public GameObject Jogador;
    public float MenorDist;
    public RaycastHit hit;
    float Dist8;
    float Dist7;
    float Dist6;
    float Dist5;
    float Dist1;
    float Dist2;
    float Dist3;
    float Dist4;
    
    public List<float> ListaSquare;
    public List<GameObject> ListaCaminho;
    public int indexAtual;
    public bool Chegou;
    public bool PrimeiraProcura = true;

    


    void Awake(){
        Jogador = GameObject.Find("Jogador");
        ArrayGrid = new GameObject[LarguraGrid,ProfundidadeGrid];
        Inicio = this.transform.position;
        if(prefabSlotGrid!=null){
            GerarGrid();
        }
    }
        
    void Update()
    {
    
    SelecionandoChegada();
      
    }

    void GerarGrid(){

        for(int p_largura = 0 ; p_largura< LarguraGrid ; p_largura++){
             for(int p_profundidade = 0 ; p_profundidade< ProfundidadeGrid ; p_profundidade++){
                 GameObject SlotGrid = Instantiate(prefabSlotGrid,new Vector3((Inicio.x + p_largura)*Escala,Inicio.y,(Inicio.z+p_profundidade)*Escala),Quaternion.identity);
                 SlotGrid.transform.SetParent(this.transform);
                 SlotGrid.transform.eulerAngles = new Vector3(90,0,0);
                 SlotGrid.transform.localScale = new Vector3(Escala*5,Escala*5,0);
                 SlotGrid.GetComponent<SlotGrid>().pos_x = p_largura;
                 SlotGrid.GetComponent<SlotGrid>().pos_y = p_profundidade;
                 ArrayGrid[p_largura,p_profundidade] = SlotGrid.gameObject;
                 ListGrid.Add(SlotGrid.gameObject);
            }
        }
    }

    void SelecionandoChegada(){
        
        
        if(Physics.Raycast(CameraPrincipal.ScreenPointToRay(Input.mousePosition).origin,CameraPrincipal.ScreenPointToRay(Input.mousePosition).direction,out hit, 1000,Geral,QueryTriggerInteraction.Collide)){
            Debug.DrawLine(CameraPrincipal.ScreenPointToRay(Input.mousePosition).origin,hit.point,Color.red);
            if(hit.collider.gameObject.tag == "SlotGrid"){
                if(Input.GetMouseButtonDown(0)){
                    //Colocar os custos de caminho e adicionar o caminho final 
                    Valores_ASTAR(hit.collider.gameObject.GetComponent<SlotGrid>().pos_x,hit.collider.gameObject.GetComponent<SlotGrid>().pos_y,1,50);
                    
                }
            }
        }
    }

    void Valores_ASTAR(int Slotx, int Sloty , int IncioIndex, int limiteindex){
        
        Chegou=false;
        MenorDist = -1;
        ListaSquare.Clear();
        
        foreach (GameObject SlotGrid in ArrayGrid){
            if(PrimeiraProcura){
                SlotGrid.gameObject.GetComponent<SlotGrid>().Custo = -1;
                SlotGrid.gameObject.GetComponent<SlotGrid>().Alvo = false;
            }
            SlotGrid.gameObject.GetComponent<SlotGrid>().Marcado =0;
            
        }
        
        
        //Custo do local final =0
        hit.collider.gameObject.GetComponent<SlotGrid>().Alvo =true;
        
        for(int index = IncioIndex ; index<limiteindex; index++){     
            
            //Diagonais 
            
            /*
            if(!ArrayGrid[Slotx+1,Sloty+1].gameObject.GetComponent<SlotGrid>().Alvo && ArrayGrid[Slotx+1,Sloty+1].gameObject.GetComponent<SlotGrid>().Custo!=100){
                ArrayGrid[Slotx+1,Sloty+1].gameObject.GetComponent<SlotGrid>().Custo = index; 
                ArrayGrid[Slotx+1,Sloty+1].gameObject.GetComponent<SpriteRenderer>().color = Color.red;
                Dist1 = ArrayGrid[Slotx+1,Sloty+1].gameObject.GetComponent<SlotGrid>().DistanciaJogador; 
            }
            
            if(!ArrayGrid[Slotx-1,Sloty+1].gameObject.GetComponent<SlotGrid>().Alvo && ArrayGrid[Slotx-1,Sloty+1].gameObject.GetComponent<SlotGrid>().Custo!=100){
                ArrayGrid[Slotx-1,Sloty+1].gameObject.GetComponent<SlotGrid>().Custo = index;
                ArrayGrid[Slotx-1,Sloty+1].gameObject.GetComponent<SpriteRenderer>().color = Color.red;
                Dist2 = ArrayGrid[Slotx-1,Sloty+1].gameObject.GetComponent<SlotGrid>().DistanciaJogador; 
            }

            if(!ArrayGrid[Slotx-1,Sloty-1].gameObject.GetComponent<SlotGrid>().Alvo && ArrayGrid[Slotx-1,Sloty-1].gameObject.GetComponent<SlotGrid>().Custo !=100){
                ArrayGrid[Slotx-1,Sloty-1].gameObject.GetComponent<SlotGrid>().Custo = index;
                ArrayGrid[Slotx-1,Sloty-1].gameObject.GetComponent<SpriteRenderer>().color = Color.red;
                Dist3 = ArrayGrid[Slotx-1,Sloty-1].gameObject.GetComponent<SlotGrid>().DistanciaJogador;
            }

            if(!ArrayGrid[Slotx+1,Sloty-1].gameObject.GetComponent<SlotGrid>().Alvo && ArrayGrid[Slotx+1,Sloty-1].gameObject.GetComponent<SlotGrid>().Custo!=100){
                ArrayGrid[Slotx+1,Sloty-1].gameObject.GetComponent<SlotGrid>().Custo = index;
                ArrayGrid[Slotx+1,Sloty-1].gameObject.GetComponent<SpriteRenderer>().color = Color.red;
                Dist4 = ArrayGrid[Slotx+1,Sloty-1].gameObject.GetComponent<SlotGrid>().DistanciaJogador;
            }
            */
            
            //HORTOGONAIS
            if(Sloty-1!=ProfundidadeGrid){
            if(!ArrayGrid[Slotx,Sloty+1].gameObject.GetComponent<SlotGrid>().Alvo && ArrayGrid[Slotx,Sloty+1].gameObject.GetComponent<SlotGrid>().Custo != 100 ){
                ArrayGrid[Slotx,Sloty+1].gameObject.GetComponent<SlotGrid>().Custo = index;
                ArrayGrid[Slotx,Sloty+1].gameObject.GetComponent<SpriteRenderer>().color = Color.red;
                ArrayGrid[Slotx,Sloty+1].gameObject.GetComponent<SlotGrid>().Marcado += 1;
                Dist5 = ArrayGrid[Slotx,Sloty+1].gameObject.GetComponent<SlotGrid>().DistanciaJogador; 
            }
            }
            
            
            if(Sloty!=0){
            if(!ArrayGrid[Slotx,Sloty-1].gameObject.GetComponent<SlotGrid>().Alvo && ArrayGrid[Slotx,Sloty-1].gameObject.GetComponent<SlotGrid>().Custo != 100){
                ArrayGrid[Slotx,Sloty-1].gameObject.GetComponent<SlotGrid>().Custo = index;
                ArrayGrid[Slotx,Sloty-1].gameObject.GetComponent<SpriteRenderer>().color = Color.red;
                ArrayGrid[Slotx,Sloty-1].gameObject.GetComponent<SlotGrid>().Marcado += 1;
                Dist6 = ArrayGrid[Slotx,Sloty-1].gameObject.GetComponent<SlotGrid>().DistanciaJogador; 
            }
            }
            
            if(Slotx!=0){
            if(!ArrayGrid[Slotx-1,Sloty].gameObject.GetComponent<SlotGrid>().Alvo && ArrayGrid[Slotx-1,Sloty].gameObject.GetComponent<SlotGrid>().Custo != 100){
                ArrayGrid[Slotx-1,Sloty].gameObject.GetComponent<SlotGrid>().Custo = index;
                ArrayGrid[Slotx-1,Sloty].gameObject.GetComponent<SpriteRenderer>().color = Color.red;
                ArrayGrid[Slotx-1,Sloty].gameObject.GetComponent<SlotGrid>().Marcado += 1;
                Dist7 = ArrayGrid[Slotx-1,Sloty].gameObject.GetComponent<SlotGrid>().DistanciaJogador;
            }
            }
            
            if(Slotx-1!=LarguraGrid){
            if(!ArrayGrid[Slotx+1,Sloty].gameObject.GetComponent<SlotGrid>().Alvo && ArrayGrid[Slotx+1,Sloty].gameObject.GetComponent<SlotGrid>().Custo != 100){
                ArrayGrid[Slotx+1,Sloty].gameObject.GetComponent<SlotGrid>().Custo = index;
                ArrayGrid[Slotx+1,Sloty].gameObject.GetComponent<SpriteRenderer>().color = Color.red;
                ArrayGrid[Slotx+1,Sloty].gameObject.GetComponent<SlotGrid>().Marcado += 1;
                Dist8 = ArrayGrid[Slotx+1,Sloty].gameObject.GetComponent<SlotGrid>().DistanciaJogador;
            }
            }
            
            
            foreach (GameObject SlotGrid in ArrayGrid){
                if(
                !SlotGrid.GetComponent<SlotGrid>().Cheio && 
                SlotGrid.GetComponent<SlotGrid>().Custo == index && 
                SlotGrid.GetComponent<SlotGrid>().Custo !=100 &&
                SlotGrid.GetComponent<SlotGrid>().Marcado==1 &&
                !SlotGrid.GetComponent<SlotGrid>().Alvo
                ){
                ListaSquare.Add(SlotGrid.GetComponent<SlotGrid>().DistanciaJogador);  
                }
            }
            
            if(ListaSquare.Count == 4){
                MenorDist = Mathf.Min(ListaSquare[0],ListaSquare[1],ListaSquare[2],ListaSquare[3]);
            }else if(ListaSquare.Count == 3){
                MenorDist = Mathf.Min(ListaSquare[0],ListaSquare[1],ListaSquare[2]);
            }else if (ListaSquare.Count == 2){
                MenorDist = Mathf.Min(ListaSquare[0],ListaSquare[1]);
            }else if (ListaSquare.Count == 1){
                MenorDist = ListaSquare[0];
            }

            foreach (GameObject SlotGrid in ArrayGrid){
                if(SlotGrid.GetComponent<SlotGrid>().DistanciaJogador == MenorDist && 
                SlotGrid.GetComponent<SlotGrid>().Custo == index &&
                SlotGrid.GetComponent<SlotGrid>().Custo !=100 && 
                !SlotGrid.GetComponent<SlotGrid>().Cheio &&
                SlotGrid.GetComponent<SlotGrid>().Marcado ==1 &&
                !SlotGrid.GetComponent<SlotGrid>().Alvo){
                    Slotx = SlotGrid.GetComponent<SlotGrid>().pos_x;
                    Sloty = SlotGrid.GetComponent<SlotGrid>().pos_y;
                    //ListaCaminho.Add(SlotGrid.gameObject);
                    //break;
                }
            }
            ListaSquare.Clear();
            indexAtual = index;
        }      

        foreach (GameObject SlotGrid in ArrayGrid){
            if(SlotGrid.gameObject.GetComponent<SlotGrid>().OcupadoJogador && SlotGrid.gameObject.GetComponent<SlotGrid>().Marcado>0){
                Chegou = true;
                PrimeiraProcura = true;
                
            }  
        }

        if(!Chegou){
            ReiniciarProcura();
        }
         
    }

    void ReiniciarProcura(){
            PrimeiraProcura =false;
            List<GameObject> ListPonto =  new List<GameObject>();
            // Nao chegou no destino 
            foreach (GameObject SlotGrid in ArrayGrid){
                if( SlotGrid.GetComponent<SlotGrid>().Custo == 49){
                    ListPonto.Add(SlotGrid.gameObject);   
                }
            }
            int indice = 0;
            Valores_ASTAR(ListPonto[indice].gameObject.GetComponent<SlotGrid>().pos_x,ListPonto[indice].gameObject.GetComponent<SlotGrid>().pos_y,ListPonto[indice].gameObject.GetComponent<SlotGrid>().Custo,ListPonto[indice].gameObject.GetComponent<SlotGrid>().Custo+50);
        
    }

    


}
