﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatHUD_Master : MonoBehaviour {
	private GameObject announce;
	[SerializeField]
	private GameObject combat_log;

	private GameObject button_end_turn;

	private GameObject hero_stats_canvas;
	private GameObject health_bar;
	private Image health_image;
	private Text health_text;
	private GameObject action_point_text;
	private GameObject movement_point_text;
	private GameObject hero_skill_bar;

	private GameObject ennemy_stats_canvas;
	private GameObject ennemy_health_bar;
	private Image ennemy_health_image;
	private Text ennemy_health_text;

	private GameObject end_screen;
	public Sprite victory_sprite;
	public Sprite defeat_sprite;


	// Use this for initialization
	void OnEnable () {
		SetInitialReferences ();
	}

	//Fonction qui récupère les références en fonction de leur position hiérarchique (père/fils)
	void SetInitialReferences()
	{
		hero_stats_canvas = transform.GetChild (0).gameObject;
		health_bar = hero_stats_canvas.transform.GetChild (0).GetChild (2).GetChild (1).gameObject;
		health_image = health_bar.transform.GetChild(0).GetComponent<Image> ();
		health_text = health_bar.GetComponentInChildren<Text> ();
		movement_point_text = hero_stats_canvas.transform.GetChild (0).GetChild (1).gameObject;
		action_point_text = hero_stats_canvas.transform.GetChild (0).GetChild (0).gameObject;
		hero_skill_bar = hero_stats_canvas.transform.GetChild (1).gameObject;

		ennemy_stats_canvas = transform.GetChild (1).gameObject;
		ennemy_health_bar = ennemy_stats_canvas.transform.GetChild(0).GetChild(0).GetChild (1).gameObject;
		ennemy_health_image = ennemy_health_bar.transform.GetChild(0).GetComponent<Image> ();
		ennemy_health_text = ennemy_health_bar.GetComponentInChildren<Text> ();

		button_end_turn = transform.GetChild (2).gameObject;
		announce = transform.GetChild (3).gameObject;
		end_screen = transform.GetChild (4).gameObject;
	}

	//Fonction qui affiche/cache le combat log
	public void enable_disable_combat_log(){
		combat_log.GetComponent<Canvas> ().enabled = !combat_log.GetComponent<Canvas> ().enabled;
	}

	//Fonction qui met à jour les infos du personnage
	public void Set_Hero_Points(GameObject hero){		
		Set_Hero_Action_Point (hero);
		Set_Hero_Movement_Point (hero);
		Set_Hero_Health (hero);
	}

	//Fonction qui active/desactive le bouton fin de tour et les informations du héros
	public void enable_disable_button_and_stats(){
		button_end_turn.SetActive (!button_end_turn.activeInHierarchy);
		enable_disable_stats ();
	}

	//Fonction qui active désactive les informations du héros
	public void enable_disable_stats(){
		hero_stats_canvas.SetActive (!hero_stats_canvas.activeInHierarchy);
	}

	//Fonction qui affiche la vie de l'allié quand un ennemi l'attaque
	public void enable_disable_stats_for_ennemy(){
		enable_disable_stats ();
		hero_skill_bar.SetActive (!hero_stats_canvas.activeInHierarchy);
	}

	//Fonction qui active/desactive les informations de l'ennemi
	public void enable_disable_ennemy_stats(){
		ennemy_stats_canvas.SetActive (!ennemy_stats_canvas.activeInHierarchy);
	}

	//Fonction qui change les points d'actions
	public void Set_Hero_Action_Point(GameObject hero){
		Hero_Master hero_master = hero.GetComponent<Hero_Master> ();
		action_point_text.GetComponentInChildren<Text>().text = "Action Point = "+ hero_master.Get_Action_Point();
	}

	//Fonction qui change les points de mouvement
	public void Set_Hero_Movement_Point(GameObject hero){
		Hero_Master hero_master = hero.GetComponent<Hero_Master> ();
		movement_point_text.GetComponentInChildren<Text> ().text = "Movement Point = " + hero_master.Get_Movement_Point ();
	}

	//Fonction qui affiche les PV du héros
	public void Set_Hero_Health(GameObject hero){
		Hero_Master hero_master = hero.GetComponent<Hero_Master> ();
		health_text.text = hero_master.Get_Current_Health ().ToString();
		float current_health = hero_master.Get_Current_Health();
		float health_stats = hero_master.Get_Health_Stats();
		health_image.fillAmount = current_health/health_stats;
	}

	//Fonction qui affiche les PV de l'ennemi
	public void Set_Ennemy_Health(GameObject ennemy){
		Ennemy_Master ennemy_master = ennemy.GetComponent<Ennemy_Master> ();
		ennemy_health_text.text = ennemy_master.Get_Current_Health ().ToString();
		float current_health = ennemy_master.Get_Current_Health();
		float health_stats = ennemy_master.Get_Health_Stats();
		ennemy_health_image.fillAmount = current_health/health_stats;
	}

	//Fonction qui change la barre de vie du héros
	public void Change_Hero_Health(int old_hero_health, int new_hero_health, int max_health){
		if (new_hero_health > old_hero_health) {
			StartCoroutine(Increase_Hero_Bar (new_hero_health,max_health));
		} else {
			StartCoroutine(Decrease_Hero_Bar (new_hero_health,max_health));
		}
		health_text.text = new_hero_health.ToString ();
	}

	//Fonction qui change la barre de vie de l'ennemi
	public void Change_Ennemy_Health(int old_ennemy_health, int new_ennemy_health, int ennemy_max_health){
		if (new_ennemy_health > old_ennemy_health) {
			StartCoroutine(Increase_Ennemy_Bar (new_ennemy_health, ennemy_max_health));
		} else {
			StartCoroutine(Decrease_Ennemy_Bar (new_ennemy_health, ennemy_max_health));
		}
		ennemy_health_text.text = new_ennemy_health.ToString ();
	}

	//Fonction qui augmente la barre de vie du héros
	IEnumerator Increase_Hero_Bar(float new_hero_health, float max_health){
		while (health_image.fillAmount < new_hero_health / max_health) {
			yield return new WaitForSeconds (Time.deltaTime*2);
			health_image.fillAmount += 0.01f;
		}
		health_image.fillAmount = new_hero_health / max_health;
	}

	//Fonction qui diminue la barre de vie du héros
	IEnumerator Decrease_Hero_Bar(float new_hero_health, float max_health){
		while (health_image.fillAmount > new_hero_health / max_health) {
			yield return new WaitForSeconds (Time.deltaTime*2);
			health_image.fillAmount -= 0.01f;
		}
		health_image.fillAmount = new_hero_health / max_health;  	
	}

	//Fonction qui augmente la barre de vie de l'ennemi
	IEnumerator Increase_Ennemy_Bar(float new_ennemy_health, float max_health){
		while (ennemy_health_image.fillAmount < new_ennemy_health / max_health) {
			yield return new WaitForSeconds (Time.deltaTime*2);
			ennemy_health_image.fillAmount += 0.01f;
		}
		ennemy_health_image.fillAmount = new_ennemy_health / max_health;
	}

	//Fonction qui diminue la barre de vie de l'ennemi
	IEnumerator Decrease_Ennemy_Bar(float new_ennemy_health, float max_health){
		while (ennemy_health_image.fillAmount > new_ennemy_health / max_health) {
			yield return new WaitForSeconds (Time.deltaTime*2);
			ennemy_health_image.fillAmount -= 0.01f;
		}
		ennemy_health_image.fillAmount = new_ennemy_health / max_health;  
	}

	//Fonction qui change le message de l'annonce
	public void Announce(string message){
		announce.gameObject.SetActive (true);
		announce.GetComponentInChildren<Text> ().text = message;
		StartCoroutine (Disable_Announce());
		combat_log.GetComponent<CombatLog_Manager> ().Add_Text (message,0);
	}

	//Fait disparaitre l'annonce
	IEnumerator Disable_Announce(){
		yield return new WaitForSeconds (1);
		announce.gameObject.SetActive (false);
	}

	//Affiche l'écran de fin
	public void Show_End_Screen(bool victory){
		Image image = end_screen.GetComponentInChildren<Image> ();
		if (victory) {
			image.sprite = victory_sprite;
		} else {
			image.sprite = defeat_sprite;
		}
		StartCoroutine (Slow_Show_End_Screen (image));
	}

	//Réalise un fondu pour l'apparition de l'écran de fin
	IEnumerator Slow_Show_End_Screen(Image i){
		end_screen.SetActive (true);
		byte transparence = 0;
		while (transparence < 255) {																//Change frame par frame la transparence de l'image
			yield return new WaitForSeconds (Time.deltaTime);
			i.color = new Color32(255,255,255,transparence);
			transparence += 3;
		}
		button_end_turn = GameObject.Find (this.name+"/End_screen/Button");							//A la fin de l'animation affiche le bouton pour rejouer
		button_end_turn.SetActive(true);
	}
}
