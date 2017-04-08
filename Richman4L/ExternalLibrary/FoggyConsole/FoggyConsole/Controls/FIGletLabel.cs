﻿using System ;
using System . Collections . Generic ;
using System . Linq ;

using WenceyWang . FIGlet ;
using WenceyWang . FoggyConsole . Controls . Renderers ;

namespace WenceyWang . FoggyConsole . Controls
{

	// ReSharper disable once InconsistentNaming
	public class FIGletLabel : TextualBase
	{

		private ContentAlign _align ;

		private AsciiArt _asciiArt ;

		private CharacterWidth _characterWidth ;

		private FIGletFont _font ;

		public AsciiArt AsciiArt
		{
			get
			{
				if ( _asciiArt == null )
				{
					UpdateText ( ) ;
				}
				return _asciiArt ;
			}
			private set => _asciiArt = value ;
		}

		/// <summary>
		///     The align of the text
		/// </summary>
		public ContentAlign Align
		{
			get { return _align ; }
			set
			{
				if ( _align != value )
				{
					_align = value ;
					RequestRedraw ( ) ;
				}
			}
		}

		public CharacterWidth CharacterWidth
		{
			get { return _characterWidth ; }
			set
			{
				if ( _characterWidth != value )
				{
					_characterWidth = value ;
					RequestMeasure ( ) ;
				}
			}
		}

		public FIGletFont Font
		{
			get { return _font ; }
			set
			{
				if ( _font != value )
				{
					_font = value ;
					RequestMeasure ( ) ;
				}
			}
		}

		public string [ ] ActualText
		{
			get
			{
				if ( AsciiArt == null )
				{
					UpdateText ( ) ;
				}
				return AsciiArt . Result ;
			}
		}

		public override Size Size
		{
			get
			{
				if ( base . Size == new Size ( 0 , 0 ) )
				{
					return new Size ( AsciiArt . Width , AsciiArt . Height ) ;
				}

				return base . Size ;
			}
			set => base . Size = value ;
		}

		public override bool CanFocus => false ;

		/// <summary>
		///     Creates a new <code>Label</code>
		/// </summary>
		/// <param name="text">The text on the Label</param>
		/// <param name="renderer">
		///     The <code>ControlRenderer</code> to use. If null a new instance of <code>LabelRenderer</code>
		///     will be used.
		/// </param>
		/// <exception cref="ArgumentException">
		///     Thrown if the <code>ControlRenderer</code> which should be set already has an other
		///     Control assigned
		/// </exception>
		public FIGletLabel ( IControlRenderer renderer = null )
			: base ( renderer ?? new FIGletLabelRenderer ( ) )
		{
			TextChanged += FIGletLabel_TextChanged ;
		}

		public override void Measure ( Size availableSize )
		{
			UpdateText ( ) ;
			base . Measure ( availableSize ) ;
		}

		private void UpdateText ( ) { AsciiArt = new AsciiArt ( Text , Font , _characterWidth ) ; }

		private void FIGletLabel_TextChanged ( object sender , EventArgs e ) { UpdateText ( ) ; }

	}

}
