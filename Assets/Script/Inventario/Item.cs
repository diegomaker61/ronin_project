using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Item : IComparable<Item>
{
    public string NomeItem;
    public string TipoItem;
    public int AtaqueItem;
    public int DefesaItem;
    public int PesoItem;
    public int QuantidadeItem; 
    public bool DentroInventario;
    public bool ItemAgrupavel;
    public int QuantidadeMoeda;
    public Sprite ThumbItem;

    public Item(string NovoNomeItem,
                string NovoTipoItem,
                int NovoAtaqueItem, 
                int NovoDefesaItem,
                int NovoPesoItem,
                int NovoQuantidadeItem,
                bool NovoDentroInventario,
                bool NovoItemAgrupavel,
                int NovoQuantidadeMoeda,
                Sprite NovoThumbItem)
    {
    NomeItem = NovoNomeItem ;
    TipoItem = NovoTipoItem;
    AtaqueItem = NovoAtaqueItem;
    DefesaItem = NovoDefesaItem;
    PesoItem = NovoPesoItem;
    QuantidadeItem = NovoQuantidadeItem ; 
    DentroInventario = NovoDentroInventario;
    ItemAgrupavel = NovoItemAgrupavel;
    QuantidadeMoeda = NovoQuantidadeMoeda;
    ThumbItem = NovoThumbItem;
    }

    public int CompareTo(Item other)
    {
        return 0;
    }


   
}
