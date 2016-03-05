using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using UnityEngine;

namespace com.huldagames.TapDrive2D
{
	[XmlRoot ("WayPointCollection")]
	public class WayPointContainer
	{
		[XmlArray ("WayPoints"),XmlArrayItem ("WayPoints")]
		public WayPoint[] WayPoints;

		public void Save (string path)
		{
			var serializer = new XmlSerializer (typeof(WayPointContainer));
			using (var stream = new FileStream (path, FileMode.Create)) {
				serializer.Serialize (stream, this);
			}
		}

		public static WayPointContainer Load (string path)
		{
			var serializer = new XmlSerializer (typeof(WayPointContainer));
			using (var stream = new FileStream (path, FileMode.Open)) {
				return serializer.Deserialize (stream) as WayPointContainer;
			}
		}

		//Loads the xml directly from the given string. Useful in combination with www.text.
		public static WayPointContainer LoadFromText (string text)
		{
			var serializer = new XmlSerializer (typeof(WayPointContainer));
			return serializer.Deserialize (new StringReader (text)) as WayPointContainer;
		}
	}
}