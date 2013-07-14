using System;

public class OpenEarsHeardEventArgs : EventArgs
{
	public readonly string Phrase;
	
	public OpenEarsHeardEventArgs(string phrase)
	{
		Phrase = phrase;
	}
}

