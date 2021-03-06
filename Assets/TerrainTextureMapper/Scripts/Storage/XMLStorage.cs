using UnityEngine;
using System.Xml;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using System;

public class XMLStorage : IStorage {
	
	private string dir = "/BasicMapper/Saved/Resources/";
	
	private void CheckDir(string directory)
	{
		if (!Directory.Exists(directory))
			Directory.CreateDirectory(directory);
	}
	
	private string CompleteFileName(IItem item)
	{
		return Application.dataPath + dir + "/" + item.Preffix + ".xml";
	}
	
	public void SaveAll(IItem itemInformation, string content)
	{
		string directory = Application.dataPath + dir;
		CheckDir(directory);	
		
		string fileName = CompleteFileName(itemInformation);
		
		TextWriter tw = new StreamWriter(fileName);
		tw.Write(content);
		tw.Close();
	}
	
	public List<T> Load<T>(IItem itemInformation)
	{
		XmlSerializer ser = new XmlSerializer(typeof(List<T>));
		List<T>  items = new List<T> ();
		try
		{
			TextReader reader = new StreamReader(CompleteFileName(itemInformation));
			if (reader != null)
			{
				items = (List<T>)ser.Deserialize(reader);
				reader.Close();
			}
		}
		catch
		{
			
		}
		
		if (items == null || items.Count == 0)
		{
			try
			{
				ser = new XmlSerializer(typeof(List<T>));
				TextAsset o = (TextAsset)Resources.Load(itemInformation.Preffix, typeof(TextAsset));
				if (o == null)
					return items;
				TextReader anotherReader = new StringReader(o.text);
				if (anotherReader != null)
				{
					items = (List<T>)ser.Deserialize(anotherReader);
					anotherReader.Close();
				}
			}
			catch
			{
				
			}
		}
		return items;
	}
}
