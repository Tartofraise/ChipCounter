using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class setup : MonoBehaviour
{
    public string lastgameobject;

    public List<string> players;
    public Font font_s;
    public bool virtuel = false;

    public Canvas canava;

    public InputField mise;
    public InputField blind;
    public InputField input;

    public GameObject content;
    public GameObject panel;
    public GameObject bouton;
    public GameObject info;
    public GameObject infotxt;
    public Button b_poker;

    // Use this for initialization
    private void Start()
    {
        panel.SetActive(false);
        b_poker.GetComponent<Image>().color = new Color32(231, 76, 60, 255);
        virtuel = false;
        mise.text = "1000";
        blind.text = "5";
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void ok()
    {
        info.SetActive(false);
    }

    public void show_info(string s)
    {
        info.SetActive(true);
        infotxt.GetComponent<Text>().text = s;
    }

    public void add()
    {
        string txt = input.text;
        if (txt.Trim() == null) { return; }
        players.Add(txt);
        GameObject go = new GameObject(txt);
        go.transform.SetParent(content.transform);
        go.AddComponent<Text>().text = txt;
        go.GetComponent<Text>().fontSize = Mathf.Min(Mathf.FloorToInt(Screen.width * 60 / 1000), Mathf.FloorToInt(Screen.height * 60 / 1000));

        go.GetComponent<Text>().font = font_s;
        go.GetComponent<Text>().color = Color.black;
        go.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
        go.GetComponent<Text>().lineSpacing = 0;

        go.AddComponent<ContentSizeFitter>().horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
        go.GetComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.Unconstrained;

        go.AddComponent<Button>().onClick.AddListener(scroll_go);
        input.text = null;
        if (players.Count == 8) { input.DeactivateInputField(); bouton.SetActive(false); }
    }

    private void scroll_go()
    {
        panel.SetActive(true);
        lastgameobject = EventSystem.current.currentSelectedGameObject.name;
    }

    public void up()
    {
        int count = GameObject.Find(lastgameobject).transform.GetSiblingIndex();

        if (count != 0)
        {
            GameObject.Find(lastgameobject).transform.SetSiblingIndex(count - 1);
            switch_list(count - 1, count);
        }

        panel.SetActive(false);
    }

    public void down()
    {
        int count = GameObject.Find(lastgameobject).transform.GetSiblingIndex();
        if (count != players.Count - 1)
        {
            GameObject.Find(lastgameobject).transform.SetSiblingIndex(count + 1);
            switch_list(count, count + 1);
        }

        panel.SetActive(false);
    }

    public void supp()
    {
        if (players.Count == 8) { input.gameObject.SetActive(true); bouton.SetActive(true); }
        players.Remove(GameObject.Find(lastgameobject).name);
        DestroyImmediate(GameObject.Find(lastgameobject));
        panel.SetActive(false);
    }

    private void switch_list(int index_obj1, int index_obj2)
    {
        string mem = players[index_obj1];
        players[index_obj1] = players[index_obj2];
        players[index_obj2] = mem;
    }

    public void demarrer()
    {
        if (players.Count <= 1)
        {
            show_info("Vous devez être 2 ou plus pour jouer !");
            return;
        }
        if ((0.5 / 100) * Convert.ToInt32(mise.text) < Convert.ToInt32(blind.text))
        {
            show_info("La blind doit être supérieur ou égale à 0.5% de l'argent en banque.");
            return;
        }
        if (virtuel == false)
        {
            chip.remiseazero();

            foreach (string pl in players)
            {
                chip.list_pl.Add(new Pl(pl, Convert.ToInt32(mise.text)));
            }

            chip.blind = Convert.ToInt32(blind.text);

            UnityEngine.SceneManagement.SceneManager.LoadScene("chip");
        }
        if (virtuel == true)
        {
            poker.remiseazero();

            foreach (string pl in players)
            {
                poker.players.Add(pl);
            }

            poker.blind = Convert.ToInt32(blind.text);

            UnityEngine.SceneManagement.SceneManager.LoadScene("poker");
        }

        //UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync("setup");
    }

    public void quit()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("menu_V");
    }

    public void poker_virtuel()
    {
        Color32 red = new Color32(231, 76, 60, 255);
        Color32 green = new Color32(39, 174, 96, 255);

        switch (virtuel)
        {
            case true:
                b_poker.GetComponent<Image>().color = red;
                virtuel = false;
                break;

            case false:
                b_poker.GetComponent<Image>().color = green;
                virtuel = true;
                break;

            default:
                break;
        }
    }

    /*
    public void def_font_sit()
    {
        int h = Screen.currentResolution.height;
        int v = Screen.currentResolution.width;
        int div = h / v;
        int div2 = v / h;
        if (h / v > 1)
        {
            div = v / h;
            div2 = h / v;
        }
        Debug.Log(div);
        Debug.Log(div2);
    }*/
}