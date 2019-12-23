using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace bmfont2cs.Xml {

	// NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
	/// <remarks/>
	[System.Serializable()]
	[System.ComponentModel.DesignerCategory( "code" )]
	[XmlType( AnonymousType = true )]
	[XmlRoot( ElementName = "font", Namespace = "", IsNullable = false )]
	public partial class BmFont {

		/// <remarks/>
		[XmlElement( ElementName = "info" )]
		public BmFontInfo Info {
			get; set;
		}

		/// <remarks/>
		[XmlElement( ElementName = "common" )]
		public BmFontCommon Common {
			get; set;
		}

		/// <remarks/>
		[XmlArray( ElementName = "pages" )]
		[XmlArrayItem( ElementName = "page" )]
		public BmFontPagesPage[] Pages {
			get; set;
		}

		/// <remarks/>
		[XmlElement( ElementName = "chars" )]
		public BmFontChars Chars {
			get; set;
		}

		/// <remarks/>
		[XmlElement( ElementName = "kernings" )]
		public BmFontKernings Kernings {
			get; set;
		}

		public static BmFont Deserialize( TextReader textReader ) {
			var serializer = new XmlSerializer( typeof( BmFont ) );
			return serializer.Deserialize( textReader ) as BmFont;
		}
	}
}
