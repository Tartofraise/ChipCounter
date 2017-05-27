using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.jouer.carte;
using UnityEngine;

namespace Assets.jouer.carte
{
    public class Func
    {
        public void point_cartes(joueur joueur, joueur table)
        {
            List<cartes> j = joueur.main_cartes;
            List<cartes> t = table.main_cartes;
            List<cartes> v = new List<cartes>();

            //trie
            foreach (cartes item in j)
            {
                if (v.Count == 0)
                {
                    v.Add(item);
                }
                else
                {
                    for (int i = 0; i < v.Count; i++)
                    {
                        if (v[i].Numero <= item.Numero)
                        {
                            if (i == v.Count - 1)
                            {
                                v.Add(item);
                            }
                            else
                            {
                                v.Add(v[v.Count - 1]);
                                for (int o = i; o < v.Count - 2; o--)
                                {
                                }
                            }
                        }
                        else
                        {
                        }
                    }
                }
            }
        }
    }
}