using Assets.jouer.carte;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;

public class poker : MonoBehaviour
{
    // Use this for initialization
    public static List<string> players = new List<string>();

    public static List<joueur> joueurs = new List<joueur>();
    public joueur cartestable = new joueur("cartestable", 0);
    public static int blind;
    public static int mise;
    public List<cartes> cartesjeu = new List<cartes>();
    private System.Random rd = new System.Random();
    public GameObject panneljoueur;
    public GameObject panneltable;
    private Func Func;

    public static List<string> etat = new List<string>() { "Pre-Flop", "Flop", "Turn", "River" };
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

    private void Start()
    {
        players.Add("aa");
        players.Add("aab");
        blind = 10;
        mise = 1000;

        int j = 0;
        foreach (enumes.symbole symb in Enum.GetValues(typeof(enumes.symbole)))
        {
            for (int i = 1; i <= 13; i++)
            {
                cartes crt = new cartes();
                crt.Numero = i;
                crt.ID = j;
                crt.Symbole = symb;
                if (symb == enumes.symbole.COEUR || symb == enumes.symbole.CARREAU)
                {
                    crt.Couleur = enumes.couleur.ROUGE;
                }
                else
                {
                    crt.Couleur = enumes.couleur.NOIR;
                }
                cartesjeu.Add(crt);
                j++;
            }
        }
        foreach (cartes crt in cartesjeu)
        {
            Debug.LogError(crt.ID + " " + crt.Symbole + " " + crt.Couleur + " " + crt.Numero);
        }
        foreach (string name in players)
        {
            joueur pl = new joueur(name, mise);
            piocher(pl);
            piocher(pl);
            joueurs.Add(pl);
        }
        etablirRole();

        disp_carte(joueurs[joueur_en_cours]);
        aff();
    }

    public void undisp_carte(joueur j)
    {
        foreach (GameObject item in GameObject.FindGameObjectsWithTag("carte"))
        {
            GameObject.Destroy(item);
        }
    }

    public void disp_carte(joueur j)
    {
        foreach (cartes crt in j.main_cartes)
        {
            GameObject go = new GameObject(crt.ID.ToString());
            go.transform.SetParent(panneljoueur.transform);
            //go.AddComponent<Transform>();
            go.AddComponent<CanvasRenderer>();
            Byte[] FileData;
            Texture2D texture2d;
            Sprite sprite = new Sprite();
            FileData = File.ReadAllBytes(CartesPath(crt));
            texture2d = new Texture2D(2, 2);
            texture2d.LoadImage(FileData);
            sprite = Sprite.Create(texture2d, new Rect(0, 0, texture2d.width, texture2d.height), new Vector2());
            sprite.name = Cartesname(crt);

            go.AddComponent<AspectRatioFitter>().aspectMode = AspectRatioFitter.AspectMode.HeightControlsWidth;
            go.GetComponent<AspectRatioFitter>().aspectRatio = texture2d.height / texture2d.width;
            go.AddComponent<Image>().sprite = sprite;
            go.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            go.tag = "carte";
        }
    }

    public void undisp_table()
    {
        for (int i = 0; i < panneltable.transform.childCount - 1; i++)
        {
            GameObject.Destroy(panneltable.transform.GetChild(i));
        }
    }

    public void disp_table()
    {
        undisp_table();
        foreach (cartes crt in cartestable.main_cartes)
        {
            GameObject go = new GameObject(crt.ID.ToString());
            go.transform.SetParent(panneltable.transform);
            go.AddComponent<Transform>();
            go.AddComponent<CanvasRenderer>();
            Byte[] FileData;
            Texture2D texture2d;
            Sprite sprite = new Sprite();
            FileData = File.ReadAllBytes(CartesPath(crt));
            texture2d = new Texture2D(2, 2);
            texture2d.LoadImage(FileData);
            sprite = Sprite.Create(texture2d, new Rect(0, 0, texture2d.width, texture2d.height), new Vector2());
            sprite.name = CartesPath(crt).Split('/')[CartesPath(crt).Split('/').Length - 1].Split('.')[0];
            go.AddComponent<AspectRatioFitter>().aspectMode = AspectRatioFitter.AspectMode.HeightControlsWidth;
            go.GetComponent<AspectRatioFitter>().aspectRatio = texture2d.height / texture2d.width;
            go.AddComponent<Image>().sprite = sprite;
        }
    }

    public static void remiseazero()
    {
        blind = 5;
        mise = 1000;
        etat = new List<string>() { "Pre-Flop", "Flop", "Turn", "River" };
        joueurs = new List<joueur>();
        dealer = 0;

        joueur_en_cours = 0;
        etatcartes = 0;
        premierjoueur = 0;
        potamount = 0;
    }

    public cartes getCarteFromID(int ID)
    {
        foreach (cartes crt in cartesjeu)
        {
            if (crt.ID == ID)
            {
                return crt;
            }
        }
        return null;
    }

    public void infocarte(cartes crt)
    {
        Debug.Log("ID : " + crt.ID);
        Debug.Log("sybole : " + crt.Symbole.ToString());
        Debug.Log("numero : " + crt.Numero.ToString());
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void piocher(joueur name)
    {
        int nb = rd.Next(cartesjeu.Count);
        name.main_cartes.Add(cartesjeu[nb]);
        cartesjeu.Remove(cartesjeu[nb]);
    }

    public string CartesPath(cartes name)
    {
        string nb = "00";

        switch (name.Numero)
        {
            case 1:
                nb = "ace";
                break;

            case 11:
                nb = "jack";
                break;

            case 12:
                nb = "queen";
                break;

            case 13:
                nb = "king";
                break;

            default:
                nb = name.Numero.ToString();
                break;
        }

        Debug.Log(nb.ToString());
        enumes.symbole_eng sym = enumes.symbole_eng.clubs;

        switch (name.Symbole)
        {
            case enumes.symbole.COEUR:
                sym = enumes.symbole_eng.hearts;
                break;

            case enumes.symbole.CARREAU:
                sym = enumes.symbole_eng.diamonds;
                break;

            case enumes.symbole.PIQUE:
                sym = enumes.symbole_eng.spades;
                break;

            case enumes.symbole.TREFLE:
                sym = enumes.symbole_eng.clubs;
                break;

            default:
                break;
        }
        Debug.Log(sym.ToString());
        string nom;
        nom = Application.dataPath + "/sprite/cartes/" + nb.ToString() + "_of_" + sym.ToString() + ".png";
        return nom.Trim();
    }

    public string Cartesname(cartes name)
    {
        string nb = "00";

        switch (name.Numero)
        {
            case 1:
                nb = "ace";
                break;

            case 11:
                nb = "jack";
                break;

            case 12:
                nb = "quenn";
                break;

            case 13:
                nb = "king";
                break;

            default:
                nb = name.Numero.ToString();
                break;
        }

        Debug.Log(nb.ToString());
        enumes.symbole_eng sym = enumes.symbole_eng.clubs;

        switch (name.Symbole)
        {
            case enumes.symbole.COEUR:
                sym = enumes.symbole_eng.hearts;
                break;

            case enumes.symbole.CARREAU:
                sym = enumes.symbole_eng.diamonds;
                break;

            case enumes.symbole.PIQUE:
                sym = enumes.symbole_eng.spades;
                break;

            case enumes.symbole.TREFLE:
                sym = enumes.symbole_eng.clubs;
                break;

            default:
                break;
        }
        Debug.Log(sym.ToString());
        string nom;
        nom = nb.ToString() + "_of_" + sym.ToString();
        return nom.Trim();
    }

    public void faireMiser(int joueurid, int amount)
    {
        if (joueurs[joueurid].banque - amount >= 0)
        {
            joueurs[joueurid].banque -= amount;
            joueurs[joueurid].mise_j += amount;
        }
        else
        {
            joueurs[joueurid].mise_j += joueurs[joueur_en_cours].banque;
            joueurs[joueurid].banque = 0;
        }
    }

    public void etablirRole()
    {
        if (joueurs.Count - 1 >= 2)
        {
            joueurs[dealer].role = enumes.role.DEALER;

            int petiteblind = joueurAprès(dealer);

            joueurs[petiteblind].role = enumes.role.PETITE_BLIND;

            faireMiser(petiteblind, blind);

            int grosseblind = joueurAprès(petiteblind);

            joueurs[grosseblind].role = enumes.role.GROSSE_BLIND;

            faireMiser(grosseblind, 2 * blind);

            joueur_en_cours = joueurAprès(grosseblind);
            premierjoueur = joueur_en_cours;
        }
        else
        {
            joueurs[dealer].role = enumes.role.PETITE_BLIND;
            faireMiser(dealer, blind); //La petite blinde
            Debug.Log(dealer);
            int grosseblind = joueurAprès(dealer); //La grosse blind est à coté de la petite blind qui est à la fois le dealer car il y'a seulement deux joueurs
            joueurs[grosseblind].role = enumes.role.GROSSE_BLIND; //On lui attribut le rôle
            Debug.Log(grosseblind);
            faireMiser(grosseblind, 2 * blind); //La grosse blind doit obligatoirement miser deux fois la blind

            joueur_en_cours = joueurAprès(grosseblind); //Le joueur a coté de la grosse blinde est celui qui commence
            premierjoueur = joueur_en_cours;
        }
    }

    public static int joueurAprès(int joueurencours)
    {
        int joueursuivant = 0;
        if (joueurencours == joueurs.Count - 1)
        {
            joueursuivant = 0;
        }
        else
        {
            joueursuivant = joueurencours + 1;
        }

        if (joueurs[joueursuivant].passer == true)
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
            joueuravant = joueurs.Count - 1;
        }
        else
        {
            joueuravant = joueursencours - 1;
        }

        if (joueurs[joueuravant].passer == true)
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

        foreach (joueur player in joueurs)
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
        joueurs[joueur_en_cours].passer = true;
        // joueurs.Remove(joueurs[joueur_en_cours]);
        int i = 0;
        foreach (joueur pl in joueurs)
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
            gagnantsPK.pot = potamount;
            gagnantsPK.players = joueurs;
            gagnantsPK.dealer = dealer;
            //UnityEngine.SceneManagement.SceneManager.UnloadScene("chip");
            UnityEngine.SceneManagement.SceneManager.LoadScene("gagnants");

            Handheld.Vibrate();
        }
    }

    public void annuler()
    {
    }

    public void suivant()
    {
        undisp_carte(joueurs[joueur_en_cours]);
        joueur_en_cours = joueurAprès(joueur_en_cours);

        if (joueur_en_cours == joueurAvant(joueurAprès(premierjoueur)))
        {
            if (checkAllMise())
            {
                if (etatcartes == 3)
                {
                    gagnantsPK.pot = potamount;
                    gagnantsPK.players = joueurs;
                    gagnantsPK.dealer = dealer;
                    //UnityEngine.SceneManagement.SceneManager.UnloadScene("chip");
                    UnityEngine.SceneManagement.SceneManager.LoadScene("gagnants");
                }
                else
                {
                    etatcartes += 1;
                    if (etatcartes == 1)
                    {
                        piocher(cartestable);
                        piocher(cartestable);
                        piocher(cartestable);
                        disp_table();
                    }
                    if (etatcartes == 2 || etatcartes == 3)
                    {
                        piocher(cartestable);
                        disp_table();
                    }

                    etatcartes_text.GetComponent<Text>().text = etat[etatcartes];
                    joueur_en_cours = joueurAprès(dealer);
                    premierjoueur = joueur_en_cours;
                }
            }
        }

        disp_carte(joueurs[joueur_en_cours]);
        int joueuravant = joueurAvant(joueur_en_cours);

        if (joueurs[joueuravant].mise_j > joueurs[joueur_en_cours].mise_j && joueurs[joueur_en_cours].banque != 0)
        {
            check_suivre.GetComponentInChildren<Text>().text = "Suivre";
        }
        else
        {
            check_suivre.GetComponentInChildren<Text>().text = "Check";
        }
        if (joueurs[joueur_en_cours].banque == 0)
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
        if (check_suivre.GetComponentInChildren<Text>().text.Contains("Suivre"))
        {
            int poursuivre = joueurs[joueurAvant(joueur_en_cours)].mise_j - joueurs[joueur_en_cours].mise_j;

            faireMiser(joueur_en_cours, poursuivre);
        }
        Handheld.Vibrate();
        suivant();
    }

    public void miser()
    {
        int poursuivre = joueurs[joueurAvant(joueur_en_cours)].mise_j - joueurs[joueur_en_cours].mise_j;
        Slider sl = scrollbar.GetComponent<Slider>();
        sl.minValue = poursuivre * 2;
        sl.value = sl.minValue;
        sl.maxValue = joueurs[joueur_en_cours].banque;
        miser_panel.SetActive(true);
    }

    public void afficher_miserPlusQue()
    {
        int poursuivre = joueurs[joueurAvant(joueur_en_cours)].mise_j - joueurs[joueur_en_cours].mise_j;
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

        Handheld.Vibrate();

        suivant();
    }

    public void valueChangedScroll()
    {
        txtmiser.GetComponent<Text>().text = scrollbar.GetComponent<Slider>().value.ToString();
        scrollbar.GetComponent<Slider>().maxValue = joueurs[joueur_en_cours].banque;
    }

    public void aff()
    {
        potamount = 0;

        joueur objjoueurs = joueurs[joueur_en_cours];

        foreach (Transform child in panel_joueurs.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        foreach (joueur player in joueurs)
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

            if (joueurs[joueur_en_cours] == player)
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

        foreach (joueur pl in joueurs)
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
        PlayerPrefs.SetString("type", "p");
        PlayerPrefs.Save();
    }

    public static void loadsave()
    {
        string names = PlayerPrefs.GetString("names");
        List<string> players = new List<string>(names.Split('_'));
        foreach (string name in players)
        {
            joueur pl = new joueur(name, PlayerPrefs.GetInt(name + "_banque"));
            pl.role = pl.inttorole(PlayerPrefs.GetInt(name + "_role"));
            joueurs.Add(pl);
        }
        joueurs.Remove(joueurs[players.Count - 1]);
    }

    public void boutoncarte()
    {
        GameObject go = panneljoueur.transform.parent.gameObject;
        if (go.activeInHierarchy == true)
        {
            go.SetActive(false);
        }
        else { go.SetActive(true); }
    }
}