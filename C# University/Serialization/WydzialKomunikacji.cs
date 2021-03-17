using System;
using System.Runtime.Serialization;
using System.IO;

namespace p3z
{

	public class WydzialKomunikacji
	{
		public readonly string name = "Wydzial Komunikacji";
		public WydzialKomunikacji()
		{
			if(Directory.Exists(name)) { Directory.Delete(name, true); }
			Directory.CreateDirectory(name);
        }

		public void SerializujSamochod(Samochod s, string path)
        {
			FileStream fs = new FileStream(path, FileMode.Create);
			DataContractSerializer dcs = new DataContractSerializer(typeof(Samochod));
			dcs.WriteObject(fs, s);
			fs.Close();
		}

		public Samochod DeserializujSamochod(string path)
		{
			var fs = new FileStream(path, FileMode.Open);
			var dcs = new DataContractSerializer(typeof(Samochod));
			Samochod s = (Samochod)dcs.ReadObject(fs);
			fs.Close();
			return s;
		}

		public void DodajSamochod(Samochod s)
        {
			string path = name;
			string letters = s.rejestracja.Substring(0, s.rejestracja.IndexOf(" "));
			string numbers = s.rejestracja.Substring(letters.Length);
			for (int i = 0; i < letters.Length; i++)
				path += "/" + letters[i];
			path += "/" + letters;
			Directory.CreateDirectory(path);

			string filePath = path + "\\" + numbers + ".soap";
			if (File.Exists(filePath))
            {
				Console.WriteLine("Samochód o podanej rejestracji istnieje już w bazie");
				return;
            }
			FileStream fs = new FileStream(filePath, FileMode.Create);
			DataContractSerializer dcs = new DataContractSerializer(typeof(Samochod));
			dcs.WriteObject(fs, s);
			fs.Close();
		}

		public void UsunSamochod(string rejestracja)
        {
			string path = name;
			string firstPart = rejestracja.Substring(0, rejestracja.IndexOf(" "));
			string secondPart = rejestracja.Substring(firstPart.Length);
			for (int i = 0; i < firstPart.Length; i++)
				path += "/" + firstPart[i];
			path += "/" + firstPart;
			string filePath = path + "\\" + secondPart + ".soap";

			if (!File.Exists(filePath)) 
			{ 
				Console.WriteLine("Samochód o rejestracji {0} nie istnieje w bazie", rejestracja); 
				return; 
			}
			File.Delete(filePath);
			Console.WriteLine("Usunięto samochód o rejestracji {0}", rejestracja);
			if(Directory.GetFiles(path).Length + Directory.GetDirectories(path).Length == 0) 
			{ 
				Directory.Delete(path);
				path = path.Substring(0, path.Length - firstPart.Length);
				for(int i = 0; i < firstPart.Length - 1; i++)
                {
					if (Directory.GetFiles(path).Length + Directory.GetDirectories(path).Length != 0) { break; }
					Directory.Delete(path);
					path = path.Substring(0, path.Length - 2);
				}
			}
		}

		public int IleDoPrzegladu(string path = "")
        {
			if(path == "") { path = name; }
			int howMany = 0;
			Samochod temp;
			foreach (var d in Directory.GetDirectories(path))
			{
				foreach (var f in Directory.GetFiles(d))
				{
					temp = DeserializujSamochod(f);
					
					if ((DateTime.Today - temp.dataPierwszejRejestracji).TotalDays > 5 * 365 && 
						(DateTime.Today - temp.dataOstatniegoPrzegladu).TotalDays > 365) { howMany++; }
					else if ((DateTime.Today - temp.dataPierwszejRejestracji).TotalDays > 3 * 365 && 
						(DateTime.Today - temp.dataOstatniegoPrzegladu).TotalDays > 2 * 365) { howMany++; }	
				}
				howMany += IleDoPrzegladu(d);
			}
			return howMany;
        }
	}
}
