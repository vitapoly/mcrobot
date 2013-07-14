using System;

public class MindWaveSensedEventArgs : EventArgs
{
	public readonly string Phrase;
	
	public MindWaveSensedEventArgs(string phrase)
	{
		Phrase = phrase;
	}
}

