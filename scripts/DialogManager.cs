using Godot;
using System;

public partial class DialogManager : Control
{
	public static event Action DialogIsOpen;
	public static event Action DialogIsClosed;
	private RichTextLabel dialogText;
	public override void _Ready()
	{
		Visible = false;
		dialogText = GetNode<RichTextLabel>("%DialogText");
	}

	public void OpenDialog()
	{
		DialogIsOpen?.Invoke();
		Visible = true;
	}

	public void SetDialogText(string text)
	{
		dialogText.Text = text;
	}

	public void CloseDialog()
	{
		DialogIsClosed?.Invoke();
		Visible = false;
		dialogText.Text = "";
	}
}
