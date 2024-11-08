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

			// Mostrar botón de interacción solo si el diálogo no está abierto
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
				DialogoManager.Instance.AbrirCerrarPanelDialogo(false); // Cerrar el diálogo si está abierto
			}

			DialogoManager.Instance.NPCDisponible = null;
			npcButtonInteractuar.SetActive(false);
		}
	}
}
