using Assets.jouer.carte;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class chip : MonoBehaviour
{
    public static int blind = 5;
    public static int mise = 1000;
    public static List<string> etat = new List<string>() { "Pre-Flop", "Flop", "Turn", "River" };
    public static List<Pl> list_pl = new List<Pl>();
    public static int dealer = 1;

    private static int joueur_en_cours = 0;
    private static int etatcartes = 0;
    private static int premierjoueur = 0;
    private static int potamount = 0;

    public GameObject encours_joueur_text;
    public GameObject pot;
    public GameObject etatcartes_text;
    public GameObject miser_panel;
    public GameObject check_suivre;
    public GameObject panel_joueurs;
    public GameObject alert;

    public GameObject txtmiser;
    public GameObject scrollbar;

    public Font font_s;

    public Button b_passer;
    public Button b_check;
    public Button b_Miser;
    public Button b_Miser_Panel;

    //public InputField mise_amount;
    public static void remiseazero()
    {
        blind = 5;
        mise = 1000;
        etat = new List<string>() { "Pre-Flop", "Flop", "Turn", "River" };
        list_pl = new List<Pl>();
        dealer = 0;

        joueur_en_cours = 0;
        etatcartes = 0;
        premierjoueur = 0;
        potamount = 0;
    }

    private void Start()
    {
        /*list_pl.Add(new Pl("aa",0));
        list_pl.Add(new Pl("aab", 0));
        list_pl.Add(new Pl("aac", 0));
        list_pl.Add(new Pl("aad", 0));
        list_pl.Add(new Pl("aae", 1000));
        list_pl.Add(new Pl("aaf", 1000));*/
        etablirRole();
        aff();
    }

    public void faireMiser(int joueurid, int amount)
    {
        if (list_pl[joueurid].banque - amount >= 0)
        {
            list_pl[joueurid].banque -= amount;
            list_pl[joueurid].mise_j += amount;
        }
        else
        {
            list_pl[joueurid].mise_j += list_pl[joueur_en_cours].banque;
            list_pl[joueurid].banque = 0;
        }
    }

    public void etablirRole()
    {
        if (list_pl.Count - 1 >= 2)
        {
            list_pl[dealer].role = enumes.role.DEALER;

            int petiteblind = joueurAprès(dealer);

            list_pl[petiteblind].role = enumes.role.PETITE_BLIND;

            faireMiser(petiteblind, blind);

            int grosseblind = joueurAprès(petiteblind);

            list_pl[grosseblind].role = enumes.role.GROSSE_BLIND;

            faireMiser(grosseblind, 2 * blind);

            joueur_en_cours = joueurAprès(grosseblind);
            premierjoueur = joueur_en_cours;
        }
        else
        {
            list_pl[dealer].role = enumes.role.PETITE_BLIND;
            faireMiser(dealer, blind); //La petite blinde
            Debug.Log(dealer);
            int grosseblind = joueurAprès(dealer); //La grosse blind est à coté de la petite blind qui est à la fois le dealer car il y'a seulement deux joueurs
            list_pl[grosseblind].role = enumes.role.GROSSE_BLIND; //On lui attribut le rôle
            Debug.Log(grosseblind);
            faireMiser(grosseblind, 2 * blind); //La grosse blind doit obligatoirement miser deux fois la blind

            joueur_en_cours = joueurAprès(grosseblind); //Le joueur a coté de la grosse blinde est celui qui commence
            premierjoueur = joueur_en_cours;
        }
    }

    private void Update()
    {
    }

    public static int joueurAprès(int joueurencours)
    {
        int joueursuivant = 0;
        if (joueurencours == list_pl.Count - 1)
        {
            joueursuivant = 0;
        }
        else
        {
            joueursuivant = joueurencours + 1;
        }

        if (list_pl[joueursuivant].passer == true)
        {
            return joueurAprès(joueursuivant);
        }
        else
        {
            return joueursuivant;
        }
    }

    public int joueurAvant(int joueursencours)
    {
        int joueuravant = 0;
        if (joueursencours == 0)
        {
            joueuravant = list_pl.Count - 1;
        }
        else
        {
            joueuravant = joueursencours - 1;
        }

        if (list_pl[joueuravant].passer == true)
        {
            return joueurAvant(joueuravant);
        }
        else
        {
            return joueuravant;
        }
    }

    public bool checkAllMise()
    {
        List<int> allmise = new List<int>();

        foreach (Pl player in list_pl)
        {
            if (player.banque != 0 && player.passer == false)
            {
                allmise.Add(player.mise_j);
            }
        }

        return !allmise.Distinct().Skip(1).Any();
    }

    public void passer()
    {
        list_pl[joueur_en_cours].passer = true;
        // list_pl.Remove(list_pl[joueur_en_cours]);
        int i = 0;
        foreach (Pl pl in list_pl)
        {
            if (pl.passer == false)
            {
                i++;
            }
        }
        if (i > 1)
        {
            suivant();
        }
        else
        {
            gagnants.pot = potamount;
            gagnants.players = list_pl;
            gagnants.dealer = dealer;
            //UnityEngine.SceneManagement.SceneManager.UnloadScene("chip");
            UnityEngine.SceneManagement.SceneManager.LoadScene("gagnants");
        }
        Handheld.Vibrate();
    }

    public void annuler()
    {
    }

    public void suivant()
    {
        joueur_en_cours = joueurAprès(joueur_en_cours);

        if (joueur_en_cours == joueurAvant(joueurAprès(premierjoueur)))
        {
            if (checkAllMise())
            {
                if (etatcartes == 3)
                {
                    gagnants.pot = potamount;
                    gagnants.players = list_pl;
                    gagnants.dealer = dealer;
                    //UnityEngine.SceneManagement.SceneManager.UnloadScene("chip");
                    UnityEngine.SceneManagement.SceneManager.LoadScene("gagnants");
                }
                else
                {
                    etatcartes += 1;
                    etatcartes_text.GetComponent<Text>().text = etat[etatcartes];
                    joueur_en_cours = joueurAprès(dealer);
                    premierjoueur = joueur_en_cours;
                }
            }
        }

        int joueuravant = joueurAvant(joueur_en_cours);

        if (list_pl[joueuravant].mise_j > list_pl[joueur_en_cours].mise_j && list_pl[joueur_en_cours].banque != 0)
        {
            check_suivre.GetComponent<Text>().text = "Suivre";
        }
        else
        {
            check_suivre.GetComponent<Text>().text = "Check";
        }
        if (list_pl[joueur_en_cours].banque == 0)
        {
            b_Miser.interactable = false;
        }
        else
        {
            b_Miser.interactable = true;
        }

        aff();
    }

    public void check()
    {
        if (check_suivre.GetComponent<Text>().text.Contains("Suivre"))
        {
            int poursuivre = list_pl[joueurAvant(joueur_en_cours)].mise_j - list_pl[joueur_en_cours].mise_j;

            faireMiser(joueur_en_cours, poursuivre);
        }
        Handheld.Vibrate();
        suivant();
    }

    public void miser()
    {
        int poursuivre = list_pl[joueurAvant(joueur_en_cours)].mise_j - list_pl[joueur_en_cours].mise_j;
        Slider sl = scrollbar.GetComponent<Slider>();
        sl.minValue = poursuivre * 2;
        sl.value = sl.minValue;
        sl.maxValue = list_pl[joueur_en_cours].banque;
        miser_panel.SetActive(true);
    }

    public void afficher_miserPlusQue()
    {
        int poursuivre = list_pl[joueurAvant(joueur_en_cours)].mise_j - list_pl[joueur_en_cours].mise_j;
        alert.GetComponent<Text>().text = "Vous devez miser plus que " + poursuivre;
        alert.SetActive(true);
    }

    /*public void afficher_Rien()
    {
        alert.GetComponent<Text>().text = "Vous n'avez pas assez d'argent pour miser autant";
        alert.SetActive(false);
    }

    public void afficher_pasAssezDargent()
    {
        alert.GetComponent<Text>().text = "Vous n'avez pas assez d'argent pour miser autant";
        alert.SetActive(true);
    }*/

    public void miser_panel_button()
    {
        faireMiser(joueur_en_cours, Convert.ToInt32(scrollbar.GetComponent<Slider>().value));

        miser_panel.SetActive(false);
        suivant();
        Handheld.Vibrate();
    }

    public void valueChangedScroll()
    {
        txtmiser.GetComponent<Text>().text = scrollbar.GetComponent<Slider>().value.ToString();
        scrollbar.GetComponent<Slider>().maxValue = list_pl[joueur_en_cours].banque;
    }

    public void aff()
    {
        potamount = 0;

        Pl objjoueurs = list_pl[joueur_en_cours];

        foreach (Transform child in panel_joueurs.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        foreach (Pl player in list_pl)
        {
            potamount += player.mise_j;

            GameObject go = new GameObject(player.name);
            go.transform.SetParent(panel_joueurs.transform);
            go.AddComponent<Text>().text = player.name + " | Banque: " + player.banque + " | Mise: " + player.mise_j;
            go.GetComponent<Text>().fontSize = Mathf.Min(Mathf.FloorToInt(Screen.width * 55 / 1000), Mathf.FloorToInt(Screen.height * 55 / 1000));

            go.GetComponent<Text>().font = font_s;
            if (player.passer == true)
            {
                go.GetComponent<Text>().fontStyle = FontStyle.Italic;
            }
            else
            {
                go.GetComponent<Text>().fontStyle = FontStyle.Bold;
            }

            if (list_pl[joueur_en_cours] == player)
            {
                go.GetComponent<Text>().color = new Color32(241, 196, 15, 255);
            }
            else if (player.role == enumes.role.DEALER)
            {
                go.GetComponent<Text>().color = new Color32(41, 128, 185, 255);
            }
            else
            {
                go.GetComponent<Text>().color = Color.white;
            }

            go.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
            go.GetComponent<Text>().lineSpacing = 0;

            go.AddComponent<ContentSizeFitter>().horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            go.GetComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.Unconstrained;
        }
        encours_joueur_text.GetComponent<Text>().text = objjoueurs.name;
        pot.GetComponent<Text>().text = potamount.ToString();
    }

    private void savegame()
    {
        string names = "00";

        foreach (Pl pl in list_pl)
        {
            PlayerPrefs.DeleteAll();
            if (names == "00")
            {
                names = pl.name;
            }
            else
            {
                names = names + "_" + pl.name;
            }
            PlayerPrefs.SetInt(pl.name + "_role", pl.roletoint(pl.role));
            PlayerPrefs.SetInt(pl.name + "_mise", pl.mise_j);
            PlayerPrefs.SetInt(pl.name + "_banque", pl.banque);
        }
        PlayerPrefs.SetString("names", names);
        PlayerPrefs.SetString("type", "c");
        PlayerPrefs.Save();
    }

    public static void loadsave()
    {
        string names = PlayerPrefs.GetString("names");
        List<string> players = new List<string>(names.Split('_'));
        foreach (string name in players)
        {
            Pl pl = new Pl(name, PlayerPrefs.GetInt(name + "_banque"));
            pl.role = pl.inttorole(PlayerPrefs.GetInt(name + "_role"));
            list_pl.Add(pl);
        }
        list_pl.Remove(list_pl[list_pl.Count - 1]);
    }
}

public class Pl
{
    public enumes.role role = enumes.role.JOUEURS;

    public int banque;
    public int mise_j = 0;
    public string name;
    public bool passer = false;

    public Pl(string name, int banque)
    {
        this.name = name;
        this.banque = banque;
    }

    public int roletoint(enumes.role roles)
    {
        int o = 0;
        switch (roles)
        {
            case enumes.role.DEALER:
                o = 1;
                break;

            case enumes.role.GROSSE_BLIND:
                o = 2;
                break;

            case enumes.role.JOUEURS:
                o = 3;
                break;

            case enumes.role.PETITE_BLIND:
                o = 4;
                break;

            default:
                break;
        }

        return o;
    }

    public enumes.role inttorole(int roles)
    {
        enumes.role o = enumes.role.JOUEURS;
        switch (roles)
        {
            case 1:
                o = enumes.role.DEALER;
                break;

            case 2:
                o = enumes.role.GROSSE_BLIND;
                break;

            case 3:
                o = enumes.role.JOUEURS;
                break;

            case 4:
                o = enumes.role.PETITE_BLIND;
                break;

            default:
                break;
        }

        return o;
    }
}