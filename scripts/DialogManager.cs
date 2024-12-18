using Godot;
using System;
using System.Collections.Generic;

public partial class DialogManager : Control
{
	public static event Action SetOpenDialog;
	public static event Action<List<string>, int> SetDialogTexts;
	private RichTextLabel dialogText;
	private Button DialogButton;
	private List<string> textArray;
	private int currentTextIndex = 0;

	public override void _Ready()
	{
		Visible = false;
		dialogText = GetNode<RichTextLabel>("%DialogText");
		DialogButton = GetNode<Button>("%DialogButton");
		DialogButton.Pressed += NextDialogOrClose;
		SetOpenDialog += SetDialogOpenOrClose;
		SetDialogTexts += SetDialogText;
	}
	public static void InvokeOpenDialog()
	{
		SetOpenDialog?.Invoke();
	}
	public static void InvokeSetDialogText(List<string> texts, int index = 0)
	{
		SetDialogTexts?.Invoke(texts, index);
	}
	public void SetDialogOpenOrClose()
	{
		Visible = true;
	}

	public void SetDialogText(List<string> texts, int index = 0)
	{
		textArray = texts;
		currentTextIndex = index;
		dialogText.Text = texts[currentTextIndex];
	}

	public void CloseDialog()
	{
		Visible = false;
		dialogText.Text = "";
	}

	public void NextDialogOrClose()
	{
		DialogButton.Text = currentTextIndex == textArray.Count - 1 ? "next" : "close";
		if (currentTextIndex < textArray.Count - 1)
		{
			currentTextIndex++;
			dialogText.Text = textArray[currentTextIndex];
		}
		else
		{
			CloseDialog();
		}
	}
}
