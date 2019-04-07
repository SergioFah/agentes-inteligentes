using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 *
 * O ambiente consiste em 4 salas (numeradas de A à D) onde em cada sala
 * existe uma frequência de aparecimento de poeira.
 * Cada agente só tem acesso à percepção de se a sala está suja ou não.
 * 
 */


public class Mundo : MonoBehaviour
{

    public Transform areaA, areaB, areaC, areaD;
    public bool sujeiraA, sujeiraB, sujeiraC, sujeiraD;
    public float frequenciaA, frequenciaB, frequenciaC, frequenciaD;
    public GameObject poeira;
    private float contadorA, contadorB,contadorC,contadorD;
    void Start()
    {
        // carrega os valores iniciais
        contadorA = frequenciaA;
        contadorB = frequenciaB;
        contadorC = frequenciaC;
        contadorD = frequenciaD;

    }

    // Update is called once per frame
    void Update()
    {
        // decrementa contadores
        contadorA -= Time.deltaTime;
        contadorB -= Time.deltaTime;
        contadorC -= Time.deltaTime;
        contadorD -= Time.deltaTime;

        //só instancia uma sujeira caso não esteja sujo.
        if (sujeiraA == false)
        {
            if (contadorA <= 0)
            {
                instanciarPoeira(areaA,"pA");
                contadorA = frequenciaA;
                sujeiraA = true;
            }
        }

        if (sujeiraB == false)
        {
            if (contadorB <= 0)
            {
                instanciarPoeira(areaB,"pB");
                contadorB = frequenciaB;
                sujeiraB = true;
            }
        }


        if (sujeiraC == false)
        {
            if (contadorC <= 0)
            {
                instanciarPoeira(areaC,"pC");
                contadorC = frequenciaC;
                sujeiraC = true;
            }
        }

        if (sujeiraD == false)
        {
            if (contadorD <= 0)
            {
                instanciarPoeira(areaD,"pD");
                contadorD = frequenciaD;
                sujeiraD = true;
            }
        }





    }

    //Gera a poeira nos espaços desejados
    void instanciarPoeira(Transform x, string nome)
    {
        GameObject poeiraAtual = Instantiate(poeira, x.position, x.rotation);
        poeiraAtual.name = nome;
    }

    public void mudouTextoA(string newText)
    {
        frequenciaA = float.Parse(newText);
    }
    public void mudouTextoB(string newText)
    {
        frequenciaB = float.Parse(newText);
    }
    public void mudouTextoC(string newText)
    {
        frequenciaC = float.Parse(newText);
    }
    public void mudouTextoD(string newText)
    {
        frequenciaD = float.Parse(newText);
    }
}
