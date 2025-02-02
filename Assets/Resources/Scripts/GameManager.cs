using System;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [SerializeField] AudioClip bubblePowClip,jumpClip;
    private AudioSource bubbleSource;
    [SerializeField] AudioSource playerSource;

    [SerializeField] bool diaBool = false;
    [SerializeField]  List<SpriteRenderer> bolhasRender = new List<SpriteRenderer>();

    [SerializeField] List<SpriteRenderer> allSpriteDia,allSpriteNoite;
    [SerializeField] List<GameObject> allPlataforDia,allPlataforNoite;

    float randoPitch;

    [SerializeField] TMP_Text contador;
    private float tContador;
    bool canRunTime;

    Color corTransp = Color.white;
    

    void Awake()
    {
        bubbleSource = GetComponent<AudioSource>();
        tContador = 2;
        corTransp = Color.white;
        corTransp.a = 0.5f;

        // Busca todos os GameObjects com a tag "BuRender"
        GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag("BuRender");

        // Percorre cada objeto e adiciona o SpriteRenderer à lista, se existir
        foreach (GameObject obj in objectsWithTag)
        {
            SpriteRenderer spriteRenderer = obj.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                bolhasRender.Add(spriteRenderer);
            }
        }

        // Busca todos os GameObjects com a tag "BuRender"
        GameObject[] objsTagObjsDia = GameObject.FindGameObjectsWithTag("ObjsDia");

        // Percorre cada objeto e adiciona o SpriteRenderer à lista, se existir
        foreach (GameObject obj in objsTagObjsDia)
        {
            SpriteRenderer spriteRenderer = obj.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                allSpriteDia.Add(spriteRenderer);
            }
        }
        GameObject[] objsTagObjsNoite = GameObject.FindGameObjectsWithTag("ObjsNoite");
        foreach (GameObject obj in objsTagObjsNoite)
        {
            SpriteRenderer spriteRenderer = obj.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                allSpriteNoite.Add(spriteRenderer);
            }
        }
        foreach (SpriteRenderer sprsNoite in allSpriteNoite)
        {
            if (sprsNoite != null)
            {
                sprsNoite.enabled = false;
            }
        }

        // Exibe a quantidade de elementos encontrados
        Debug.Log("Total de SpriteRenderers encontrados: " + bolhasRender.Count);

        //Adicionar todas as plataformas Dia (Objs) para allPlataforDia
        GameObject[] plataformasDia = GameObject.FindGameObjectsWithTag("PlataforDia");

        foreach (GameObject plataforma in plataformasDia)
        {
            allPlataforDia.Add(plataforma);
        }
        //Adicionar todas as plataformas Noite (Objs) para allPlataforNoite
        GameObject[] plataformasNoite = GameObject.FindGameObjectsWithTag("PlataforNoite");

        foreach (GameObject plataforma in plataformasNoite)
        {
            allPlataforNoite.Add(plataforma);
        }
        //Transparencia Plataform
        PlataformasDiaNoite(allPlataforNoite, corTransp, false);
        //PlataformasDiaNoite(Color.white, corTransp, true, false);
    }

    private void Update()
    {
        randoPitch = Random.Range(1.0f, 1.9f);
        if (Input.GetKeyDown(KeyCode.Return))
        {
            GameOver();
            print("Restarting...");
        }

        if(canRunTime)
        {
            tContador -= Time.deltaTime;
            contador.text = Mathf.FloorToInt(tContador).ToString();
            if (tContador < 0)
            {
                tContador = 2;
                contador.text = Mathf.FloorToInt(0).ToString();
                canRunTime = false;
                if (diaBool) PlataformasDiaNoite(allPlataforNoite, corTransp, false);
                else if(!diaBool) PlataformasDiaNoite(allPlataforDia, corTransp, false);
                //if (diaBool) PlataformasDiaNoite(Color.white, corTransp, false, true);
                //else if (!diaBool) PlataformasDiaNoite(corTransp, Color.white, false, true);
            }
        }
    }
    public void BubblePow()
    {
        canRunTime = true;
        //Som de Estouro
        playerSource.pitch = randoPitch;
        playerSource.PlayOneShot(bubblePowClip,0.8f);
        //Se esta de Dia todas as plataformas da Noite são Ativadas
        if (diaBool) PlataformasDiaNoite(allPlataforNoite, Color.white, true);
        else if (!diaBool) PlataformasDiaNoite(allPlataforDia, Color.white, true);

        //Trocar para Dia/Noite
        if (diaBool) //Se é dia = Ser Noite
        {
            diaBool = false;
            //PlataformasDiaNoite(corTransp, Color.white);
            foreach (SpriteRenderer sprsDia in allSpriteDia)
            {
                if (sprsDia != null)
                {
                    sprsDia.enabled = false;
                }
            }
            foreach (SpriteRenderer sprsNoite in allSpriteNoite)
            {
                if (sprsNoite != null)
                {
                    sprsNoite.enabled = true;
                }
            }
        }
        else if(!diaBool) //Se é Noite = Ser Dia
        {
            diaBool = true;
            //PlataformasDiaNoite(Color.white, corTransp);
            foreach (SpriteRenderer sprsDia in allSpriteDia)
            {
                if (sprsDia != null)
                {
                    sprsDia.enabled = true;
                }
            }
            foreach (SpriteRenderer sprsNoite in allSpriteNoite)
            {
                if (sprsNoite != null)
                {
                    sprsNoite.enabled = false;
                }
            }
        }
    }

    public void JumpSound()
    {
        
        bubbleSource.PlayOneShot(jumpClip,0.5f);
    }

    public void GameOver()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void DiaNoiteOnOff(ref SpriteRenderer[] spriteChange, bool onOff)
    {
        for (int i = 0; i < 3; i++)
        {
            spriteChange[i].enabled = onOff;
        }
    }

    //Transparencia e Colisão das Plataformas
    void PlataformasDiaNoite(List<GameObject> allPlataform,Color dDiaNoite,bool boDiaNoite)
    {
        //PlataformasDia
        foreach (GameObject plataformAtual in allPlataform)
        {
            plataformAtual.GetComponent<SpriteRenderer>().color = dDiaNoite;
            plataformAtual.GetComponent<BoxCollider2D>().enabled = boDiaNoite;
        }
    }

    IEnumerator PequenoAtraso()
    {
        yield return new WaitForSeconds(1);
    }
}
