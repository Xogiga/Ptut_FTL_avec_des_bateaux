﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile_Script : MonoBehaviour {
	public Sprite classic;
	public Sprite mouseover;
	private GameManager_Master game_master;
	private GameObject player;									
	private Hero_Master hero_master;
	private GameManager_Pathfinding game_pathfinding;
	private List<Tile> path;

	void OnEnable(){
		Set_initial_references();
	}

	void Set_initial_references(){
		game_master = GameObject.FindWithTag ("GameManager").GetComponent<GameManager_Master>();
		game_pathfinding = GameObject.FindWithTag ("GameManager").GetComponent<GameManager_Pathfinding> ();
	}

	public void Set_new_references(){																											//On reprend les références des cases et des personnages à chaque tour allié
		path = null;
		player = game_master.get_playing_perso ();
		hero_master = player.GetComponent<Hero_Master> ();
	}		


	//Fonction qui change le sprite de la case survolée si elle est accessible
	public void OnMouseEnter(){
		Set_new_references ();
		if (game_master.is_it_your_turn == true 																								//Si c'est au tour du joueur de jouer
			&& hero_master.is_moving == false 																									//Si le héros n'est pas déjà entrain de bouger
			&& game_master.get_matrice_case(Mathf.RoundToInt(this.transform.position.x), Mathf.RoundToInt(this.transform.position.y)) == 0){	//Si la case de la matrice est égale à 0
				game_pathfinding.Find_Path (player.transform.position, this.transform.position);												//Détermine le chemin entre le héros et la case
				path = game_pathfinding.Get_Path ();																							//Récupère le chemin
				if (path != null && path.Count <=  hero_master.Get_Movement_Point()) {															//Si le chemin existe et est accessible avec les points de mouvements disponibles
					foreach (Tile t in path) {																									//Change les sprite
						t.obj.GetComponent<SpriteRenderer> ().sprite = mouseover;
						}
				}
		}
	}

	//Fonction qui permet de réinitialiser les sprites si l'utilisateur enlève sa souris de la case
	public void OnMouseExit(){
		if (game_master.is_it_your_turn == true && path != null) {
			foreach (Tile t in path) {																											//Change tous les sprites du chemin
				t.obj.GetComponent<SpriteRenderer> ().sprite = classic;
			}
			path = null;
		}

		print ("delete");
	}

	//Fonction qui permet de réinitialiser les sprites si l'utilisateur fait "Fin de tour" sans bouger sa souris
	void Update(){																														
		if (game_master.is_it_your_turn == false && path != null) {
			foreach (Tile t in path) {																											//Change tous les sprites du chemin
				t.obj.GetComponent<SpriteRenderer> ().sprite = classic;
			}
			path = null;
		}

	}
}
