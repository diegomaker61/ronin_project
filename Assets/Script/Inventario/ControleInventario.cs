using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ControleInventario : MonoBehaviour
{

    public List<Item> Itens = new List<Item>();
    public List<Image> Slots;
    public bool InventarioLigado;
    public GameObject Jogador;
    public MovimentacaoJogador tempMovimentacaoJogador;
    public Image IconeItem;
    public int QuantidadeSlots;
    public int Moedas;
    public int PesoAtual;
    public int PesoMaximo;
    public TextMeshProUGUI TextoMoedas;
    public GameObject FundoInventario;
    public bool SlotNoLimite;
    public int SlotCheio;
    public bool InventarioCheio_FaltaSlot;
    public List<Image> SlotsCheios;
    public Image OpcoesItem;
    public bool OpcaoAtiva;
    public TextMeshProUGUI Opcao;
    public bool MouseDentroLista;



    [Header("Janela Quantidade Itens")]
    public GameObject JanelaQuantidadeItem;
    public bool JanelaQuantidadeItemAtiva;
    public int QuantidadeItensAbandonados;
    public GameObject ItemSelecionadoDentroInventario;
    public TMP_InputField Texto_QuantidadeItens;
    public GameObject Prefab_Item;

    [Header("Equipamentos")]

    public Image SlotArma1;
    public Image SlotArma2;
    public Image SlotArmadura;
    public Image SlotCalcado;
    public Image SlotAmuleto;  

    public GameObject[] ItensReferencia3D;
    public GameObject MaoDireitaJogador;

    void Start()
    {
    tempMovimentacaoJogador = Jogador.GetComponent<MovimentacaoJogador>();
    

    }
    void Update()
    {
    
   
    //Atualizando moedas do inventario 
    TextoMoedas.text = Moedas.ToString();
    
    LigandoDesligandoInventario();
    LigandoDesligandoJanelaQuantidadeItem();

    
    

    //Adicionando Item ao inventario
    if(tempMovimentacaoJogador.EnviandoInfoItem){

        for(int index = 0; tempMovimentacaoJogador.EnviandoInfoItem && index<Slots.Count; index += 1){
            
            
            ControleSlot tempControleSlot = Slots[index].gameObject.GetComponent<ControleSlot>();

            if(!tempControleSlot.SlotOcupado){
                
                //Criacao do icone
                Image NovoIcone = Instantiate(IconeItem,Vector3.zero,Quaternion.identity);
                
                //Filho do slot
                NovoIcone.rectTransform.SetParent(Slots[index].rectTransform);
                
                //Posicao relativa a hierarquia 
                NovoIcone.rectTransform.localPosition = Vector2.zero;
                
                
                ControleThumbItem tempControleThumbItem = NovoIcone.GetComponent<ControleThumbItem>();
                
                //Adicionar atributos restantes
                tempControleThumbItem.NomeItem = tempMovimentacaoJogador.publicNomeItem;
                tempControleThumbItem.TipoItem = tempMovimentacaoJogador.publicTipoItem;
                tempControleThumbItem.PesoItem = tempMovimentacaoJogador.publicPesoItem;
                tempControleThumbItem.QuantidadeItem = tempMovimentacaoJogador.publicQuantidadeItem;
                tempControleThumbItem.ItemAgrupavel = tempMovimentacaoJogador.publicItemAgrupavel;
                tempControleThumbItem.QuantidadeMoeda = tempMovimentacaoJogador.publicQuantidadeMoedas;
                tempControleThumbItem.Referencia3D = tempMovimentacaoJogador.publicIndiceRef3d;
                tempControleThumbItem.ThumItem = tempMovimentacaoJogador.publicThumbItem;
                
                //Imagem do icone
                NovoIcone.sprite = tempControleThumbItem.ThumItem;
                

                //Adiciona de fato o item
                Itens.Add(new Item(
                    tempMovimentacaoJogador.publicNomeItem,
                    tempMovimentacaoJogador.publicTipoItem,
                    0,
                    0,
                    tempMovimentacaoJogador.publicPesoItem,
                    tempMovimentacaoJogador.publicQuantidadeItem,
                    true,tempMovimentacaoJogador.publicItemAgrupavel,
                    0,
                    tempMovimentacaoJogador.publicThumbItem));
                

                //Zerando e Parando FOR
                tempMovimentacaoJogador.EnviandoInfoItem = false;
                OcuparSlot(index);
                break;

                

            }else{
                //Caso tenha algo no slot 
                //Verifique se esse item é o mesmo que esta tentando adicionar
                //Se o ultimo adicionado é igual a algum junte eles
               if(tempMovimentacaoJogador.publicNomeItem == Slots[index].GetComponentInChildren<ControleThumbItem>().NomeItem && tempMovimentacaoJogador.publicItemAgrupavel){
                   
                   //Bloqueio de Slot Individual por Quantidade de Item
                   if(Slots[index].GetComponentInChildren<ControleThumbItem>().QuantidadeItem <= Slots[index].GetComponent<ControleSlot>().LimiteSlot){
                   
                        //Adicionar o restante dos atributos
                        Slots[index].GetComponentInChildren<ControleThumbItem>().PesoItem += tempMovimentacaoJogador.publicPesoItem;
                        Slots[index].GetComponentInChildren<ControleThumbItem>().QuantidadeItem += tempMovimentacaoJogador.publicQuantidadeItem;
                        
                        //Adicionar o restante dos atriburos
                        Itens[index].PesoItem += tempMovimentacaoJogador.publicPesoItem;
                        Itens[index].QuantidadeItem += tempMovimentacaoJogador.publicQuantidadeItem;

                        //Zerando e Parando FOR
                        tempMovimentacaoJogador.EnviandoInfoItem = false;
                        OcuparSlot(index);
                        break;

                   }                   
               }
            }
        }
    }


    foreach(Image Slot in Slots){
        if(Slot.GetComponentInChildren<ControleThumbItem>() != null){
        if(Slot.GetComponent<ControleSlot>().SlotOcupado && 
            !SlotsCheios.Contains(Slot) && 
            Slot.GetComponentInChildren<ControleThumbItem>().QuantidadeItem == Slot.GetComponent<ControleSlot>().LimiteSlot+1)
        {
            SlotsCheios.Add(Slot);
            
        }   
        } 
    }

    SlotCheio = SlotsCheios.Count;

    if(SlotCheio == Slots.Count){
        InventarioCheio_FaltaSlot =true;
    }else{
        InventarioCheio_FaltaSlot =false;
    }

    }

    public void LigandoDesligandoInventario(){
        
        if(Input.GetKeyDown(KeyCode.I)){
            InventarioLigado = !InventarioLigado; 
        }

        if(InventarioLigado){
            FundoInventario.SetActive(true);
        }else{
            FundoInventario.SetActive(false);
        }
    }

    public void OcuparSlot( int indexAtual){
        Slots[indexAtual].gameObject.GetComponent<ControleSlot>().SlotOcupado = true;
    }

    public void MouseDentroListaOpcoes(){
        MouseDentroLista = true;
    }

    public void MouseForaListaOpcoes(){
        MouseDentroLista = false;
    }


    /********************************************* JANELA QUANTIDADE *********************************************************************************/

    public void LigandoDesligandoJanelaQuantidadeItem(){
        if(JanelaQuantidadeItemAtiva){
            JanelaQuantidadeItem.SetActive(true);  
        }else{
            JanelaQuantidadeItem.SetActive(false);
            QuantidadeItensAbandonados=1;
        }
    }

    public void QuantidadeItensAdicionar(){

        if( QuantidadeItensAbandonados < ItemSelecionadoDentroInventario.GetComponent<ControleThumbItem>().QuantidadeItem){
            QuantidadeItensAbandonados+=1;
            Texto_QuantidadeItens.text = QuantidadeItensAbandonados.ToString();  
        }
    }

    public void QuantidadeItensRetirar(){
        
        if(QuantidadeItensAbandonados>1){
            QuantidadeItensAbandonados-=1;
            Texto_QuantidadeItens.text = QuantidadeItensAbandonados.ToString();  
        }
    }

    public void QuantidadeItensInput(){
        int.TryParse(Texto_QuantidadeItens.text,out QuantidadeItensAbandonados);
    }


    public void OK_JanelaQuantidade(){
        InputExtrapolado();
        
        if(ItemSelecionadoDentroInventario.GetComponent<ControleThumbItem>().QuantidadeItem > QuantidadeItensAbandonados){ 
            //Subtrai a quantidade do item atual 
            ItemSelecionadoDentroInventario.GetComponent<ControleThumbItem>().QuantidadeItem -= QuantidadeItensAbandonados;
            //Jogar ITem no chao
            JogarItemChao();
            //Desliga Janela
            JanelaQuantidadeItemAtiva=false;
 

        }else{
            //Se voce tirar o valor total de um Slot
            if(ItemSelecionadoDentroInventario.GetComponent<ControleThumbItem>().QuantidadeItem == 100){
                Slots.Remove(ItemSelecionadoDentroInventario.GetComponentInParent<Image>());
            }
            //Deixa o slot desocupado
            ItemSelecionadoDentroInventario.GetComponentInParent<ControleSlot>().SlotOcupado = false;
            //Jogar ITem no chao
            JogarItemChao();
            //Destroi Item no inventario 
            GameObject.Destroy(ItemSelecionadoDentroInventario);
            //Desliga Janela
            JanelaQuantidadeItemAtiva=false;
        }
        
    }

    public void Cancelar_JanelaQuantidade(){
            JanelaQuantidadeItemAtiva=false;
    }

    public void JogarItemChao(){
        
        //Local Aleatorio 
        Vector3 LocalRandomico = new Vector3(Random.Range(Jogador.transform.position.x+2,Jogador.transform.position.x-2),0.2f,Random.Range(Jogador.transform.position.z+2,Jogador.transform.position.z-2));
        
        //Criacao do item 
        GameObject ItemNoChao = Instantiate(Prefab_Item,LocalRandomico,Quaternion.identity);
        
        //Componentes
        ControleThumbItem tempControleItemInventario = ItemSelecionadoDentroInventario.GetComponent<ControleThumbItem>();
        ControleItem tempControleNovoItem = ItemNoChao.GetComponent<ControleItem>();
        
        //Info Novo item 
        tempControleNovoItem.NomeItem = tempControleItemInventario.NomeItem;
        tempControleNovoItem.TipoItem= tempControleItemInventario.TipoItem;
        tempControleNovoItem.QuantidadeItem = QuantidadeItensAbandonados;
        tempControleNovoItem.ThumItem = ItemSelecionadoDentroInventario.GetComponent<Image>().sprite;
        tempControleNovoItem.ItemAgrupavel = tempControleItemInventario.ItemAgrupavel;
        

    }

    public void InputExtrapolado(){
        if(QuantidadeItensAbandonados>ItemSelecionadoDentroInventario.GetComponent<ControleThumbItem>().QuantidadeItem){
           QuantidadeItensAbandonados =  ItemSelecionadoDentroInventario.GetComponent<ControleThumbItem>().QuantidadeItem;
           
        } 
        if(QuantidadeItensAbandonados<1){
            QuantidadeItensAbandonados=1;   
        }
        Texto_QuantidadeItens.text = QuantidadeItensAbandonados.ToString(); 
    }


    


    
}
