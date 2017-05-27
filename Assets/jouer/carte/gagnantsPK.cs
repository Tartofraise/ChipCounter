using Assets.jouer.carte;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gagnantsPK : MonoBehaviour
{
    public static List<joueur> players = new List<joueur>() { };
    public static int pot = 1;
    public static int dealer;

    public GameObject prefabChecker;
    public GameObject panelJoueurs;
    public GameObject info;
    public GameObject infotxt;

    private void Start()
    {
        foreach (joueur player in players)
        {
            GameObject a = (GameObject)Instantiate(prefabChecker);
            Text txt = a.GetComponentInChildren<Transform>().Find("Label").GetComponent<Text>();
            if (player.passer == true)
            {
                txt.fontStyle = FontStyle.Italic;
                a.GetComponent<Toggle>().interactable = false;
            }
            txt.text = player.name + " | Banque: " + player.banque;

            a.transform.SetParent(panelJoueurs.transform, false);
        }
    }

    public void nouvellePartie()
    {
        int i = 0;
        List<int> gagnants = new List<int>();
        foreach (Transform go in panelJoueurs.transform)
        {
            players[i].mise_j = 0;
            players[i].role = enumes.role.JOUEURS;
            players[i].passer = false;

            if (go.gameObject.GetComponent<Toggle>().isOn)
            {
                gagnants.Add(i);
            }
            i++;
        }

        foreach (int pl in gagnants)
        {
            players[pl].banque += pot / gagnants.Count;
            Debug.Log(players[pl].name);
        }
        int joueursavecdelargent = 0;
        string seulgagnant = "";
        foreach (joueur pl in players)
        {
            if (pl.banque == 0)
            {
                pl.passer = true;
            }
            else
            {
                seulgagnant = pl.name;
                joueursavecdelargent += 1;
            }
        }

        if (joueursavecdelargent == 1)
        {
            show_info(seulgagnant + " remporte la partie !");
        }
        else
        {
            poker.remiseazero();
            poker.joueurs = players;
            poker.dealer = chip.joueurAprès(dealer);

            //UnityEngine.SceneManagement.SceneManager.UnloadScene("gagnants");
            UnityEngine.SceneManagement.SceneManager.LoadScene("chip");
        }
    }

    public void ok()
    {
        info.SetActive(false);
        //UnityEngine.SceneManagement.SceneManager.UnloadScene("chip");
        //UnityEngine.SceneManagement.SceneManager.UnloadScene("gagnants");
        UnityEngine.SceneManagement.SceneManager.LoadScene("menu_V");
    }

    public void show_info(string s)
    {
        info.SetActive(true);
        infotxt.GetComponent<Text>().text = s;
    }

    private void Update()
    {
    }

    public void quiter()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("menu_v");
    }
}