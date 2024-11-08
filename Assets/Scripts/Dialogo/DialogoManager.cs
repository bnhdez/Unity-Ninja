using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogoManager : Singleton<DialogoManager>
{
	[SerializeField] private GameObject panelDialogo;
	[SerializeField] private Image npcIcono;
	[SerializeField] private TextMeshProUGUI npcNombreTMP;
	[SerializeField] private TextMeshProUGUI npcConversacionTMP;
	[SerializeField] private Transform jugador; // Referencia al jugador
	[SerializeField] private float distanciaMaxima = 5f; // Distancia máxima permitida

	public NPCInteraccion NPCDisponible { get; set; }

	private Queue<string> dialogosSecuencia;
	private bool dialogoAnimado;
	private bool despedidaMostrada;

	private void Start()
	{
		dialogosSecuencia = new Queue<string>();

		if (jugador == null)
		{
			Debug.LogError("La referencia del jugador no está asignada en DialogoManager.");
		}
	}

	private void Update()
	{
		if (NPCDisponible == null) return;

		// Detectar distancia y alejar
		if (panelDialogo.activeSelf)
		{
			float distanciaActual = Vector3.Distance(jugador.position, NPCDisponible.transform.position);
			if (distanciaActual > distanciaMaxima)
			{
				CerrarDialogoPorDistancia();
				return;
			}
		}

		if (Input.GetKeyDown(KeyCode.E))
		{
			if (!panelDialogo.activeSelf) // Abrir el panel si no está ya activo
			{
				ConfigurarPanel(NPCDisponible.Dialogo);
			}
		}

		if (Input.GetKeyDown(KeyCode.Space) && panelDialogo.activeSelf)
		{
			// Reincorpora la verificación de dialogoAnimado
			if (dialogoAnimado)
			{
				ContinuarDialogo();
			}
		}
	}

	public void AbrirCerrarPanelDialogo(bool estado)
	{
		panelDialogo.SetActive(estado);
	}

	private void ConfigurarPanel(NPCDialogo npcDialogo)
	{
		AbrirCerrarPanelDialogo(true);
		CargarDialogosSencuencia(npcDialogo);

		npcIcono.sprite = npcDialogo.Icono;
		npcNombreTMP.text = $"{npcDialogo.Nombre}:";
		MostrarTextoConAnimacion(npcDialogo.Saludo);
	}

	private void CargarDialogosSencuencia(NPCDialogo npcDialogo)
	{
		if (npcDialogo.Conversacion == null || npcDialogo.Conversacion.Length <= 0)
		{
			return;
		}

		dialogosSecuencia.Clear(); // Limpiar cualquier diálogo previo

		for (int i = 0; i < npcDialogo.Conversacion.Length; i++)
		{
			dialogosSecuencia.Enqueue(npcDialogo.Conversacion[i].Oracion);
		}
	}

	private void ContinuarDialogo()
	{
		if (!dialogoAnimado) // Evita avanzar si la animación de texto no ha terminado
		{
			return;
		}

		if (NPCDisponible == null || dialogosSecuencia.Count == 0)
		{
			string despedida = NPCDisponible.Dialogo.Despedida;
			MostrarTextoConAnimacion(despedida);
			despedidaMostrada = true;
			return;
		}

		string siguienteDialogo = dialogosSecuencia.Dequeue();
		MostrarTextoConAnimacion(siguienteDialogo);
	}

	private IEnumerator AnimarTexto(string oracion)
	{
		dialogoAnimado = false;
		npcConversacionTMP.text = "";
		char[] letras = oracion.ToCharArray();
		for (int i = 0; i < letras.Length; i++)
		{
			npcConversacionTMP.text += letras[i];
			yield return new WaitForSeconds(0.03f);
		}

		dialogoAnimado = true;
		Debug.Log("Texto completado. Puedes avanzar al siguiente diálogo.");
	}

	private void MostrarTextoConAnimacion(string oracion)
	{
		StartCoroutine(AnimarTexto(oracion));
	}

	private void CerrarDialogoPorDistancia()
	{
		panelDialogo.SetActive(false);
		despedidaMostrada = false;
		dialogosSecuencia.Clear();
		NPCDisponible = null;
	}

	public bool IsDialogoAbierto()
	{
		return panelDialogo.activeSelf;
	}
}