using System;
using System.Runtime.Serialization;

namespace p3z
{
	[Serializable]
	public class Samochod : IDeserializationCallback
	{
		public string rejestracja;

		[NonSerialized] public string miasto;

		public DateTime dataOstatniegoPrzegladu;

		public DateTime dataPierwszejRejestracji;

		public void OnDeserialization(object sender)
        {
			string line;
			miasto = rejestracja.Substring(0,rejestracja.IndexOf(" "));
			var reader = new System.IO.StreamReader("rejestracje.txt");
			while ((line = reader.ReadLine()) != null)
			{
				if(line.Contains(miasto)) 
				{
					miasto = line.Substring(line.IndexOf(" ") + 1);
				}
            }
			if (miasto.Contains(" ")) { miasto = miasto.Substring(0, miasto.IndexOf(" ")); }
        }
	}
}
