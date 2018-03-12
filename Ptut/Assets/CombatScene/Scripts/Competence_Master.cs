﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Competence_Master : MonoBehaviour {
	private GameObject infobulle;
	private GameManager_Master game_master;

	public void OnEnable(){
		game_master = GameObject.FindGameObjectWithTag ("GameManager").GetComponent<GameManager_Master> ();
	}

	private void Find_Infobulle(int Numero_competence){
		infobulle = GameObject.Find("Hero_"+game_master.get_indice_playing_perso()+"/Hero Stats Canvas/Barre des competences/Competence_"+Numero_competence+"/Infobulle_"+Numero_competence);
	}

	public void Show_infobulle(int Numero_competence){
		Find_Infobulle(Numero_competence);
		infobulle.SetActive (true);
	}

	public void Hide_infobulle(int Numero_competence){
		infobulle.SetActive (false);
	}

}



