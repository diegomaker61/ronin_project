using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Hit : MonoBehaviour
{
    public float Velocidade;
    public float TempoDeDestruicao;
    TextMeshProUGUI EsseTexto;

    void Start(){
        InvokeRepeating("DestruirHit",TempoDeDestruicao,0);
        EsseTexto = this.GetComponent<TextMeshProUGUI>();
    }
    
    void Update()
    {
        this.transform.position = new Vector2(this.transform.position.x,this.transform.position.y + Velocidade* Time.deltaTime);
        EsseTexto.rectTransform.localScale += new Vector3(0.03f,0.03f);
    }

    void DestruirHit(){
        GameObject.Destroy(this.gameObject);
    }


}
