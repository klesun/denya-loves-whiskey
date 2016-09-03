﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp;

public class HeroStats : MonoBehaviour 
{
	public QuoteBox quoteBox;
	public NpcControl npc;

	public Dictionary<ETrophy, int> trophyCounts = new Dictionary<ETrophy, int> ();
	public Dictionary<ETrophy, int> trophyTotalCounts = new Dictionary<ETrophy, int> ();

	void Start () 
	{
		// so it was runt after the Start() of all scripts that generate items
		Tls.inst().mainThreadBridge.RunInMainThread(() => {
			foreach (var _ in Object.FindObjectsOfType<ITrophy>()) {
				var trophy = _;
				if (!trophyCounts.ContainsKey(trophy.GetName())) {
					trophyTotalCounts [trophy.GetName()] = 0;
				}
				++trophyTotalCounts [trophy.GetName()];
				trophyCounts [trophy.GetName()] = 0;
				trophy.SetOnCollected (() => Inc(trophy.GetName(), 1));
			}
		});
	}

	void Inc(ETrophy trophyName, int count)
	{
		if (!trophyCounts.ContainsKey(trophyName)) {
			trophyCounts [trophyName] = 0;
		}
		trophyCounts [trophyName] += count;

		var text = "";
		foreach (var entry in trophyCounts) {
			var total = trophyTotalCounts [entry.Key];
			text += entry.Key + ": " + entry.Value + "/" + total + "\n";
		}
		quoteBox.ShowStats(text, npc);
	}
}
