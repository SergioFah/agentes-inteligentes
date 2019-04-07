using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

/*
 * 
 * Nosso agente deve seguir conforme a tabela especificada no relatório
 * 
 * Ele segue sempre na ordem A, B, C, D, A, B.... indefinidamente
 * Verificando sempre se o area está ou não limpo.
 * 
 * A frequência da sujeira aparece em uma frequência predeterminada diferente para cada 
 * area, fator que não é levado em conta pelo nosso agente, que anda de maneira cíclica e linear
 * entre eles.
 * 
 * 
 * A cada setor limpo o agente GANHA 2 pontos.
 * 
 * A cada segundo que passa o agente PERDE 1 ponto
 * 
 * R reseta os scores.
 * 
 * 
 */



public class AgenteReativoSimples : MonoBehaviour
{

    public GameObject areaA, areaB, areaC, areaD;        //Recebe a posição das áreas das salas
    public string areaAtual = "areaA";                   //Guarda informação da área atual
    public float pontuacao = 0.0f;                       //Amazena pontuação
    public float contador = 0.0f;                        //Contador para facilitar a availiação de desempenho
    public bool andando = false;                         //variavel que bloqueia a chamada da tabela caso o aspirador esteja se movendo
    public Text txt;                                     //recebe o texto que aparecerá na tela
    public Text contTxt;                                 //recebe o texto que aparecerá na tela

    //tabela que será seguida pelo agente
    void tabela()
    {
        // [Area atual == A] e [sala Limpa]  -> ir para areaB  (-1 ponto)
        if ((areaAtual == "areaA") && (!verificarSujeira()))        
        {
            StartCoroutine(irPara(areaB));
            perdeuPonto();
            return;
        }
        //[Area atual == A] e [sala Suja]  -> Limpe a sala     (+2 ponto)
        else if ((areaAtual == "areaA") && (verificarSujeira()))
        {
            limpar();
            ganharPonto();
            return;
        }
        //[Area atual == B] e [sala Limpa]  -> ir para areaC   (-1 ponto)
        else if ((areaAtual == "areaB") && (!verificarSujeira()))
        {
            StartCoroutine(irPara(areaC));
            perdeuPonto();
            return;
        }
        //[Area atual == B] e [sala Suja]  -> Limpe a sala     (+2 ponto)
        else if ((areaAtual == "areaB") && (verificarSujeira()))
        {
            limpar();
            ganharPonto();
            return;
        }
        //[Area atual == C] e [sala Limpa]  -> ir para areaD   (-1 ponto)
        else if ((areaAtual == "areaC") && (!verificarSujeira()))
        {
            StartCoroutine(irPara(areaD));
            perdeuPonto();
            return;
        }
        //[Area atual == C] e [sala Suja]  -> Limpe a sala      (+2 ponto)
        else if ((areaAtual == "areaC") && (verificarSujeira()))
        {
            limpar();
            ganharPonto();
            return;
        }
        //[Area atual == D] e [sala Limpa]  -> ir para areaA   (-1 ponto)
        else if ((areaAtual == "areaD") && (!verificarSujeira()))
        {
            StartCoroutine(irPara(areaA));
            perdeuPonto();
            return;
        }
        //[Area atual == D] e [sala Suja]  -> Limpe a sala     (+2 ponto)
        else if ((areaAtual == "areaD") && (verificarSujeira()))
        {
            limpar();
            ganharPonto();
            return;
        }
    }
    //função callback a cada frame.
    void Update()
    {
        atualizarScores();
        //consultar a tabela apenas se o aspirador não estiver andando
        if (!andando)
        {
            tabela();
        }
    }


    //simula sensores de sujeira, retornando a informação do mundo sobre a area em que o aspirador se encotnra
    bool verificarSujeira()
    {
        if (areaAtual == areaA.name)
        {
            return GameObject.Find("Mundo").GetComponent<Mundo>().sujeiraA;
        }
        else if (areaAtual == areaB.name)
        {
            return GameObject.Find("Mundo").GetComponent<Mundo>().sujeiraB;
        }
        else if (areaAtual == areaC.name)
        {
            return GameObject.Find("Mundo").GetComponent<Mundo>().sujeiraC;
        }
        else
        {
            return GameObject.Find("Mundo").GetComponent<Mundo>().sujeiraD;
        }
    }
    // trava as atualizações da tabela, olha pra área destino, vai até ela, seta a nova área e destrava as atualizações da tabela
    IEnumerator irPara(GameObject area) 
    {
        // travar atualização da tabela
        andando = true;
        //olhar para a área em questao
        Vector3 posicaoArea = new Vector3(area.transform.position.x, this.transform.position.y, area.transform.position.z);
        this.transform.LookAt(posicaoArea);
        //ir até área em questão
        while (Vector3.Distance(transform.position, area.transform.position) > 0.005f)
        {
            transform.position = Vector3.MoveTowards(transform.position, area.transform.position, 2.0f * Time.deltaTime);
            yield return null;
        }
        //seta a nova área
        areaAtual = area.name;
        //destrava atualizações da tabela
        andando = false;
    }
    //deleta o objeto e muda a variável de sujeira da áera especificada no script MUNDO.
    void limpar()
    {
        if (areaAtual == areaA.name)
        {
            Destroy(GameObject.Find("pA"));
            GameObject.Find("Mundo").GetComponent<Mundo>().sujeiraA = false;
        }
        else if (areaAtual == areaB.name)
        {
            Destroy(GameObject.Find("pB"));
            GameObject.Find("Mundo").GetComponent<Mundo>().sujeiraB = false;
        }
        else if (areaAtual == areaC.name)
        {
            Destroy(GameObject.Find("pC"));
            GameObject.Find("Mundo").GetComponent<Mundo>().sujeiraC = false;
        }
        else
        {
            Destroy(GameObject.Find("pD"));
            GameObject.Find("Mundo").GetComponent<Mundo>().sujeiraD = false;
        }
    }


    //Atualiza os scores na tela e o contador.
    void atualizarScores()
    {
        txt.text = (int)pontuacao + " pontos";

        contador += Time.deltaTime;
        contTxt.text = (int)contador + " segundos";

        if (Input.GetKeyDown(KeyCode.R))
        {
            contador = 0;
            pontuacao = 0;
        }
    }
    //incremento de 2 pontos.
    void ganharPonto()
    {
        pontuacao += 2;
    }
    //decremento de 1 ponto.
    void perdeuPonto()
    {
        pontuacao -= 1;
    }
}
