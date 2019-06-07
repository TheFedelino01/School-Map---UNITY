//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//   >>>>>>>>>>>>>>>>>>>>>>   NON UTIULIZZATO PERCHE' DA PROBLEMI IN RETE <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<


//public struct Team
//{
//    public static readonly string[] NOMI_POSSIBILI = { "RED", "BLUE" };
//    private string nome;
//    private bool attacco;

//    public string Nome { get => nome; }
//    public bool Attacco { get => attacco; }
//    public Team(string nome)
//    {
//        Debug.Log("Imposto la squadra: " + nome);
//        if (System.Array.IndexOf(NOMI_POSSIBILI, nome) >= 0)
//        {
//            this.nome = nome;
//            this.attacco = matchManager.Instance.AttackTeam == nome;
//            Debug.Log("Squadra impostata: " + this.nome + " - " + nome + " - " + Nome);
//        }
//        else
//            throw new System.ArgumentException("Squadra non valida");
//    }

//    public override string ToString()
//    {
//        if (nome != null)
//            return nome.ToString();
//        else
//            return "Squadra non impostata";
//    }
//}

//NON VIENE SINCRONIZZATO IN RETE
//public enum TeamName
//{
//    RED, BLUE
//};
