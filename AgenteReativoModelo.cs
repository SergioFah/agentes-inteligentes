using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;


/*
 * 
 *  Nosso agente deve seguir com base no modelo especificado no relatório
 *  
 *  possuirá um contador interno para cada sala e uma observação de frequência parcial 
 *  de aparecimento de sujeira para cada sala, que será aprimorado a cada vez que ele
 *  capturar o lixo em uma sala, assim ele poderá ter mais noção de qual sala está suja
 *  para realizar o trabalho de maneira otimizada, dando ênfase à salas com maior
 *  frequência e dando possíveis pausas.
 *  
 *  A cada setor limpo o agente GANHA 2 pontos.
 * 
 *  A cada segundo que passa o agente PERDE 1 ponto
 * 
 *  R reseta os scores.
 * 
 * 
 */



public class AgenteReativoModelo : MonoBehaviour
{
    public GameObject areaA, areaB, areaC, areaD;        //Recebe a posição das áreas das salas
    public string areaAtual = "areaA";                   //Guarda informação da área atual
    float pontuacao = 0.0f;                              //Amazena pontuação
    float contador = 0.0f;                               //Contador para facilitar a availiação de desempenho
    int limpezas = 0;
    public bool bloqueado = false;                         //variavel que bloqueia a chamada do caso o aspirador esteja se movendo ou em espera
    public Text txt;                                     //recebe o texto que aparecerá na tela
    public Text contTxt;                                 //recebe o texto que aparecerá na tela
    public Text limpTxt;                                 //recebe o texto que aparecerá na tela
    public string atual = "areaA";
    public float contadorA, contadorB, contadorC, contadorD = 0.0f;             //contadores usados para atualizar as frequencias parciais
    public float freqParcialA, freqParcialB, freqParcialC, freqParcialD = 0.0f; //percepção de tempo decorrido até cair uma sujeira no ambiente

    // contador >= freqParcial = SALA SUJA

    //modelo que o agente deve seguir
    void modelo()
    {
        
        if (!verificarSujeira())    // caso a sala esteja limpa
        {
            if (escolherSala() != null)    // Caso a função retorne alguma sala que pode ter tido sua frequencia parcial extrapolada
            {
                StartCoroutine(irPara(escolherSala()));  //se locomove até a sala escolhida
            }
            else
            {
                StartCoroutine(esperar());  //espera 1 segundo
            }
        }
        else
        {
            limpar();  // limpa a sala.
        }

    }

    //função atualizada a cada segundo
    void Update()
    {
        atualizarScores();
        atualizarContadores();
        mudouDeSala();
        if(bloqueado == false)
        {
            modelo();
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
    // trava as atualizações do modelo, olha pra área destino, vai até ela, seta a nova área e destrava as atualizações da tabela
    IEnumerator irPara(GameObject area)
    {
        // travar atualização da tabela
        bloqueado = true;
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
        //destrava atualizações do modelo
        bloqueado = false;
    }
    //espera 1 segundo
    IEnumerator esperar()
    {
        bloqueado = true;
        yield return new WaitForSeconds(1);
        bloqueado = false;
    }
    //deleta o objeto e muda a variável de sujeira da áera especificada no script MUNDO.
    void limpar()
    {
        if (areaAtual == areaA.name)
        {
            freqParcialA = contadorA;
            contadorA = 0.0f;
            GameObject.Find("pA").GetComponent<Poeira>().serDestruido = true;
            GameObject.Find("Mundo").GetComponent<Mundo>().sujeiraA = false;
            return;
        }
        else if (areaAtual == areaB.name)
        {
            freqParcialB = contadorB;
            contadorB = 0.0f;
            GameObject.Find("pB").GetComponent<Poeira>().serDestruido = true;
            GameObject.Find("Mundo").GetComponent<Mundo>().sujeiraB = false;
            return;
        }
        else if (areaAtual == areaC.name)
        {
            freqParcialC = contadorC;
            contadorC = 0.0f;
            GameObject.Find("pC").GetComponent<Poeira>().serDestruido = true;
            GameObject.Find("Mundo").GetComponent<Mundo>().sujeiraC = false;
            return;
        }
        else if(areaAtual == areaD.name)
        {
            freqParcialD = contadorD;
            contadorD = 0.0f;
            GameObject.Find("pD").GetComponent<Poeira>().serDestruido = true;
            GameObject.Find("Mundo").GetComponent<Mundo>().sujeiraD = false;
            return;
        }
    }
    // função para auxiliar a pontuação
    void mudouDeSala()
    {
        if(atual != areaAtual)
        {
            perdeuPonto();
        }
        atual = areaAtual;
    }


    //escolhe sala com base nas observações de contadores e frequencia parcial, pra tentar prever qual sala estará vazia.
    GameObject escolherSala()
    {
        if (freqParcialA <= contadorA)
        {

            return areaA;
        }else if (freqParcialB <= contadorB)
        {
           
            return areaB;
        }else if (freqParcialC <= contadorC)
        {
           
            return areaC;
        }else if (freqParcialD <= contadorD)
        {
           
            return areaD;
        }
        else
        {
            return null;
        }
    }



    void atualizarContadores()
    {
        contadorA += Time.deltaTime;
        contadorB += Time.deltaTime;
        contadorC += Time.deltaTime;
        contadorD += Time.deltaTime;


        if (freqParcialA > 20)
        {
            freqParcialA = 20;
        }
        if (freqParcialB > 20)
        {
            freqParcialB = 20;
        }
        if (freqParcialC > 20)
        {
            freqParcialC = 20;
        }
        if (freqParcialD > 20)
        {
            freqParcialD = 20;
        }
    }
    //Atualiza os scores na tela e o contador.
    void atualizarScores()
    {
        txt.text = (int)pontuacao + " pontos";
        limpTxt.text = (int)limpezas + " limpezas";
        contador += Time.deltaTime;
        contTxt.text = (int)contador + " segundos";

        if (Input.GetKeyDown(KeyCode.R))
        {
            contador = 0;
            pontuacao = 0;
            limpezas = 0;
        }
    }
    //incremento de 2 pontos.
    public void ganharPonto()
    {
        pontuacao += 2;
        limpezas = limpezas + 1;
    }
    //decremento de 1 ponto.
    void perdeuPonto()
    {
        pontuacao -= 1;
    }
}
