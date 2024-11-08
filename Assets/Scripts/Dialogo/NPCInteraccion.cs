using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

public class NPCInteraccion : MonoBehaviour
{
    [SerializeField] private GameObject npcButtonInteractuar;
    [SerializeField] private NPCDialogo npcDialogo;

    public NPCDialogo Dialogo => npcDialogo;

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			DialogoManager.Instance.NPCDisponible = this;

			// Mostrar bot�n de interacci�n solo si el di�logo no est� abierto
			if (!DialogoManager.Instance.IsDialogoAbierto())
			{
				npcButtonInteractuar.SetActive(true);
			}
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			if (DialogoManager.Instance != null && DialogoManager.Instance.NPCDisponible == this)
			{
				DialogoManager.Instance.AbrirCerrarPanelDialogo(false); // Cerrar el di�logo si est� abierto
			}

			DialogoManager.Instance.NPCDisponible = null;
			npcButtonInteractuar.SetActive(false);
		}
	}
}
