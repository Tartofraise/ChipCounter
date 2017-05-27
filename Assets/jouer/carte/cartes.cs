using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.jouer.carte
{
    public class cartes
    {
        private int iD;
        private enumes.couleur couleur;
        private enumes.symbole symbole;
        private int numero;

        public cartes()
        {
        }

        public int ID
        {
            get
            {
                return iD;
            }

            set
            {
                iD = value;
            }
        }

        public int Numero
        {
            get
            {
                return numero;
            }

            set
            {
                numero = value;
            }
        }

        internal enumes.couleur Couleur
        {
            get
            {
                return couleur;
            }

            set
            {
                couleur = value;
            }
        }

        internal enumes.symbole Symbole
        {
            get
            {
                return symbole;
            }

            set
            {
                symbole = value;
            }
        }
    }
}