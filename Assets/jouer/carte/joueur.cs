using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.jouer.carte
{
    public class joueur
    {
        public string name;
        public int banque;
        public List<cartes> main_cartes = new List<cartes>();
        public enumes.role role = enumes.role.JOUEURS;
        public int mise_j = 0;
        public bool passer = false;

        public joueur(string name, int banque)
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
}