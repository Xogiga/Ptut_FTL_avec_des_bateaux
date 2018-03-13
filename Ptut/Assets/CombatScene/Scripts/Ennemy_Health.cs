﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Ennemy_Health : MonoBehaviour {

	private float health_stat = 100;
	private float current_health;
	private GameObject hb;
	public Text health_text;
	public Image health_bar;

	void OnMouseEnter()
	{
		hb.SetActive(true);
	}

	void OnMouseExit()
	{
		hb.SetActive(false);
	}
	void OnEnable()
	{
		SetInitialReferences();
	}

	void SetInitialReferences()
	{
		current_health = health_stat;
		hb = GameObject.Find("Enemy_stats_canvas");
		hb.SetActive(false);
		SetUI();
	}


	void IncreaseHealth(int health_change)
	{
		current_health += health_change;
		if (current_health > health_stat)
		{
			current_health = health_stat;
		}
		SetUI();
		StartCoroutine(IncreaseBar());
	}

	public void DeductHealth(int health_change)
	{
		current_health -= health_change;
		if (current_health < 0)
		{
			current_health = 0;
			//Destroy(this.gameObject,1);									//Détruit l'ennemi quand il n'a plus de PV, crée plein d'erreur de référencement dans d'autres scripts
		}
		SetUI();
		StartCoroutine(DecreaseBar());
	}

	void SetUI()
	{
		health_text.text = current_health.ToString();
	}

	IEnumerator IncreaseBar()
	{
		hb.SetActive(true);
		while (health_bar.fillAmount <= current_health / health_stat)
		{
			yield return new WaitForSeconds(Time.deltaTime);
			health_bar.fillAmount += 0.01f;
		}
		health_bar.fillAmount = current_health / health_stat;
	}

	IEnumerator DecreaseBar()
	{
		hb.SetActive(true);
		while (health_bar.fillAmount >= current_health / health_stat)
		{
			yield return new WaitForSeconds(Time.deltaTime);
			health_bar.fillAmount -= 0.01f;
		}
		health_bar.fillAmount = current_health / health_stat;           	// Les calculs sur les floats merdent toujours, donc je lui redonne la valeur exacte à la fin
	}
}