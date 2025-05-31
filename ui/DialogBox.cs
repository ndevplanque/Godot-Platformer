using Godot;
using System;
using System.Threading.Tasks;

public partial class DialogBox : Control
{
	[Export] private RichTextLabel _label;
	[Export] private AnimationPlayer _animator;

	private string _fullText = "";
	private float _charSpeed = 0.03f; // secondes par caractère
	private bool _isPlaying = false;

	public override void _Ready()
	{
		_label = GetNode<RichTextLabel>("NinePatchRect/RichTextLabel");
		_animator = GetNode<AnimationPlayer>("AnimationPlayer");
		Hide();
	}

	public async void ShowDialog(string text)
	{
		if (_isPlaying) return; // Évite les appels multiples

		_fullText = text;
		_label.Text = "";
		_isPlaying = true;

		Show();
		_animator.Play("open");

		await ToSignal(_animator, "animation_finished");

		await PlayTextAnimation();
	}

	private async Task PlayTextAnimation()
	{
		for (int i = 0; i < _fullText.Length; i++)
		{
			_label.Text += _fullText[i];
			await ToSignal(GetTree().CreateTimer(_charSpeed), "timeout");
		}

		_isPlaying = false;
	}

	public async void CloseDialog()
	{
		if (_isPlaying) return;

		_animator.Play("close");
		await ToSignal(_animator, "animation_finished");

		Hide();
	}
}
