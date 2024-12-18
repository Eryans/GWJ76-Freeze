using Godot;
using System;

public partial class DialogManager : Control
{
	public static event Action<bool> SetOpenDialog;
	public static event Action<string[], int> SetDialogTexts;
	private RichTextLabel dialogText;
	private Button DialogButton;
	private string[] textArray;
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

	public void SetDialogOpenOrClose(bool isOpen)
	{
		if (isOpen)
		{
			Visible = true;
		}
		else
		{
			CloseDialog();
		}
	}

	public void SetDialogText(string[] texts, int index = 0)
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
		DialogButton.Text = textArray.Length > 1 ? "next" : "close";
		if (currentTextIndex < textArray.Length - 1)
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
