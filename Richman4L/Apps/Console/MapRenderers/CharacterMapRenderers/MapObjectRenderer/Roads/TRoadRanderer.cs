﻿using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

namespace WenceyWang . Richman4L . Apps . Console . MapRenderers . MapObjectRenderer . Roads
{

	// ReSharper disable once InconsistentNaming
	//public class TRoadRanderer : CharacterMapObjectRenderer <TRoad>
	//{

	//	public override void Update ( )
	//	{
	//		List <Road> exitList = new List <Road> { Target . FirstExit , Target . SecondExit , Target . ThirdExit } ;
	//		int aviliableExitCount = exitList . Count ( road => Target . GetAzimuth ( road ) != BlockAzimuth . None ) ;
	//		if ( Unit == ConsoleSize . Small )
	//		{
	//			switch ( aviliableExitCount )
	//			{
	//				case 1 :
	//				{
	//					Road exit = exitList . Single ( road => Target . GetAzimuth ( road ) != BlockAzimuth . None ) ;
	//					switch ( Target . GetAzimuth ( exit ) )
	//					{
	//						case BlockAzimuth . Up :
	//						{
	//							CurrentView [ 0 , 0 ] = new ConsoleChar ( '╨' , ConsoleColor . White , ConsoleColor . DarkGray ) ;
	//							break ;
	//						}
	//						case BlockAzimuth . Down :
	//						{
	//							CurrentView [ 0 , 0 ] = new ConsoleChar ( '╥' , ConsoleColor . White , ConsoleColor . DarkGray ) ;
	//							break ;
	//						}
	//						case BlockAzimuth . Left :
	//						{
	//							CurrentView [ 0 , 0 ] = new ConsoleChar ( '╡' , ConsoleColor . White , ConsoleColor . DarkGray ) ;
	//							break ;
	//						}
	//						case BlockAzimuth . Right :
	//						{
	//							CurrentView [ 0 , 0 ] = new ConsoleChar ( '╞' , ConsoleColor . White , ConsoleColor . DarkGray ) ;
	//							break ;
	//						}
	//						default :
	//						{
	//							CurrentView [ 0 , 0 ] = new ConsoleChar ( '▫' , ConsoleColor . White , ConsoleColor . DarkGray ) ;
	//							break ;
	//						}
	//					}

	//					break ;
	//				}
	//				case 2 :
	//				{
	//					Road forwardRoad = exitList . First ( road => Target . GetAzimuth ( road ) != BlockAzimuth . None ) ;
	//					Road backwardRoad = exitList . Last ( road => Target . GetAzimuth ( road ) != BlockAzimuth . None ) ;
	//					if ( Target . GetAzimuth ( forwardRoad ) == BlockAzimuth . Up &&
	//						Target . GetAzimuth ( backwardRoad ) == BlockAzimuth . Down ||
	//						Target . GetAzimuth ( forwardRoad ) == BlockAzimuth . Down &&
	//						Target . GetAzimuth ( backwardRoad ) == BlockAzimuth . Up )
	//					{
	//						CurrentView [ 0 , 0 ] = new ConsoleChar ( '║' , ConsoleColor . White , ConsoleColor . DarkGray ) ;
	//					}
	//					else if ( Target . GetAzimuth ( forwardRoad ) == BlockAzimuth . Left &&
	//							Target . GetAzimuth ( backwardRoad ) == BlockAzimuth . Right ||
	//							Target . GetAzimuth ( forwardRoad ) == BlockAzimuth . Right &&
	//							Target . GetAzimuth ( backwardRoad ) == BlockAzimuth . Left )
	//					{
	//						CurrentView [ 0 , 0 ] = new ConsoleChar ( '═' , ConsoleColor . White , ConsoleColor . DarkGray ) ;
	//					}
	//					else if ( Target . GetAzimuth ( forwardRoad ) == BlockAzimuth . Left &&
	//							Target . GetAzimuth ( backwardRoad ) == BlockAzimuth . Up ||
	//							Target . GetAzimuth ( forwardRoad ) == BlockAzimuth . Up &&
	//							Target . GetAzimuth ( backwardRoad ) == BlockAzimuth . Left )
	//					{
	//						CurrentView [ 0 , 0 ] = new ConsoleChar ( '╝' , ConsoleColor . White , ConsoleColor . DarkGray ) ;
	//					}
	//					else if ( Target . GetAzimuth ( forwardRoad ) == BlockAzimuth . Right &&
	//							Target . GetAzimuth ( backwardRoad ) == BlockAzimuth . Up ||
	//							Target . GetAzimuth ( forwardRoad ) == BlockAzimuth . Up &&
	//							Target . GetAzimuth ( backwardRoad ) == BlockAzimuth . Right )
	//					{
	//						CurrentView [ 0 , 0 ] = new ConsoleChar ( '╚' , ConsoleColor . White , ConsoleColor . DarkGray ) ;
	//					}
	//					else if ( Target . GetAzimuth ( forwardRoad ) == BlockAzimuth . Left &&
	//							Target . GetAzimuth ( backwardRoad ) == BlockAzimuth . Down ||
	//							Target . GetAzimuth ( forwardRoad ) == BlockAzimuth . Down &&
	//							Target . GetAzimuth ( backwardRoad ) == BlockAzimuth . Left )
	//					{
	//						CurrentView [ 0 , 0 ] = new ConsoleChar ( '╗' , ConsoleColor . White , ConsoleColor . DarkGray ) ;
	//					}
	//					else if ( Target . GetAzimuth ( forwardRoad ) == BlockAzimuth . Right &&
	//							Target . GetAzimuth ( backwardRoad ) == BlockAzimuth . Down ||
	//							Target . GetAzimuth ( forwardRoad ) == BlockAzimuth . Down &&
	//							Target . GetAzimuth ( backwardRoad ) == BlockAzimuth . Right )
	//					{
	//						CurrentView [ 0 , 0 ] = new ConsoleChar ( '╔' , ConsoleColor . White , ConsoleColor . DarkGray ) ;
	//					}

	//					break ;
	//				}
	//				case 3 :
	//				{
	//					if ( exitList . All ( road => Target . GetAzimuth ( road ) != BlockAzimuth . Down ) )
	//					{
	//						CurrentView [ 0 , 0 ] = new ConsoleChar ( '┻' , ConsoleColor . White , ConsoleColor . DarkGray ) ;
	//					}
	//					else if ( exitList . All ( road => Target . GetAzimuth ( road ) != BlockAzimuth . Up ) )
	//					{
	//						CurrentView [ 0 , 0 ] = new ConsoleChar ( '┳' , ConsoleColor . White , ConsoleColor . DarkGray ) ;
	//					}
	//					else if ( exitList . All ( road => Target . GetAzimuth ( road ) != BlockAzimuth . Right ) )
	//					{
	//						CurrentView [ 0 , 0 ] = new ConsoleChar ( '┫' , ConsoleColor . White , ConsoleColor . DarkGray ) ;
	//					}
	//					else if ( exitList . All ( road => Target . GetAzimuth ( road ) != BlockAzimuth . Left ) )
	//					{
	//						CurrentView [ 0 , 0 ] = new ConsoleChar ( '┳' , ConsoleColor . White , ConsoleColor . DarkGray ) ;
	//					}
	//					break ;
	//				}
	//			}
	//		}
	//		else if ( Unit == ConsoleSize . Large )
	//		{
	//			switch ( aviliableExitCount )
	//			{
	//				case 0 :
	//				{
	//					for ( int x = 0 ; x < 5 ; x++ )
	//					{
	//						CurrentView [ x , 0 ] = new ConsoleChar ( "┏━━━┓" [ x ] , ConsoleColor . White , ConsoleColor . DarkGray ) ;
	//						CurrentView [ x , 1 ] = new ConsoleChar ( "┃   ┃" [ x ] , ConsoleColor . White , ConsoleColor . DarkGray ) ;
	//						CurrentView [ x , 2 ] = new ConsoleChar ( "┗━━━┛" [ x ] , ConsoleColor . White , ConsoleColor . DarkGray ) ;
	//					}

	//					break ;
	//				}
	//				case 1 :
	//				{
	//					Road exit = exitList . Single ( road => Target . GetAzimuth ( road ) != BlockAzimuth . None ) ;
	//					switch ( Target . GetAzimuth ( exit ) )
	//					{
	//						case BlockAzimuth . Up :
	//						{
	//							for ( int x = 0 ; x < 5 ; x++ )
	//							{
	//								CurrentView [ x , 0 ] = new ConsoleChar ( "┃ ┋ ┃" [ x ] , ConsoleColor . White , ConsoleColor . DarkGray ) ;
	//								CurrentView [ x , 1 ] = new ConsoleChar ( "┗━━━┛" [ x ] , ConsoleColor . White , ConsoleColor . DarkGray ) ;
	//								CurrentView [ x , 2 ] = new ConsoleChar ( ' ' , ConsoleColor . White , ConsoleColor . DarkGray ) ;
	//							}

	//							break ;
	//						}
	//						case BlockAzimuth . Down :
	//						{
	//							for ( int x = 0 ; x < 5 ; x++ )
	//							{
	//								CurrentView [ x , 0 ] = new ConsoleChar ( ' ' , ConsoleColor . White , ConsoleColor . DarkGray ) ;
	//								CurrentView [ x , 1 ] = new ConsoleChar ( "┏━━━┓" [ x ] , ConsoleColor . White , ConsoleColor . DarkGray ) ;
	//								CurrentView [ x , 2 ] = new ConsoleChar ( "┃ ┋ ┃" [ x ] , ConsoleColor . White , ConsoleColor . DarkGray ) ;
	//							}

	//							break ;
	//						}
	//						case BlockAzimuth . Left :
	//						{
	//							for ( int x = 0 ; x < 5 ; x++ )
	//							{
	//								CurrentView [ x , 0 ] = new ConsoleChar ( "━━┓  " [ x ] , ConsoleColor . White , ConsoleColor . DarkGray ) ;
	//								CurrentView [ x , 1 ] = new ConsoleChar ( "┅ ┃  " [ x ] , ConsoleColor . White , ConsoleColor . DarkGray ) ;
	//								CurrentView [ x , 2 ] = new ConsoleChar ( "━━┛  " [ x ] , ConsoleColor . White , ConsoleColor . DarkGray ) ;
	//							}

	//							break ;
	//						}
	//						case BlockAzimuth . Right :
	//						{
	//							for ( int x = 0 ; x < 5 ; x++ )
	//							{
	//								CurrentView [ x , 0 ] = new ConsoleChar ( "  ┏━━" [ x ] , ConsoleColor . White , ConsoleColor . DarkGray ) ;
	//								CurrentView [ x , 1 ] = new ConsoleChar ( "  ┃ ┅" [ x ] , ConsoleColor . White , ConsoleColor . DarkGray ) ;
	//								CurrentView [ x , 2 ] = new ConsoleChar ( "  ┗━━" [ x ] , ConsoleColor . White , ConsoleColor . DarkGray ) ;
	//							}

	//							break ;
	//						}
	//					}

	//					break ;
	//				}
	//				case 2 :
	//				{
	//					Road forwardRoad = exitList . First ( road => Target . GetAzimuth ( road ) != BlockAzimuth . None ) ;
	//					Road backwardRoad = exitList . Last ( road => Target . GetAzimuth ( road ) != BlockAzimuth . None ) ;
	//					if ( Target . GetAzimuth ( forwardRoad ) == BlockAzimuth . Up &&
	//						Target . GetAzimuth ( backwardRoad ) == BlockAzimuth . Down ||
	//						Target . GetAzimuth ( forwardRoad ) == BlockAzimuth . Down &&
	//						Target . GetAzimuth ( backwardRoad ) == BlockAzimuth . Up )
	//					{
	//						for ( int y = 0 ; y < 3 ; y++ )
	//						{
	//							for ( int x = 0 ; x < 5 ; x++ )
	//							{
	//								CurrentView [ x , y ] = new ConsoleChar ( "┃ ┋ ┃" [ x ] , ConsoleColor . White , ConsoleColor . DarkGray ) ;
	//							}
	//						}
	//					}
	//					else if ( Target . GetAzimuth ( forwardRoad ) == BlockAzimuth . Left &&
	//							Target . GetAzimuth ( backwardRoad ) == BlockAzimuth . Right ||
	//							Target . GetAzimuth ( forwardRoad ) == BlockAzimuth . Right &&
	//							Target . GetAzimuth ( backwardRoad ) == BlockAzimuth . Left )
	//					{
	//						for ( int x = 0 ; x < 5 ; x++ )
	//						{
	//							for ( int y = 0 ; y < 3 ; y++ )
	//							{
	//								CurrentView [ x , y ] = new ConsoleChar ( "━┅━" [ y ] , ConsoleColor . White , ConsoleColor . DarkGray ) ;
	//							}
	//						}
	//					}

	//					else if ( Target . GetAzimuth ( forwardRoad ) == BlockAzimuth . Left &&
	//							Target . GetAzimuth ( backwardRoad ) == BlockAzimuth . Up ||
	//							Target . GetAzimuth ( forwardRoad ) == BlockAzimuth . Up &&
	//							Target . GetAzimuth ( backwardRoad ) == BlockAzimuth . Left )
	//					{
	//						for ( int x = 0 ; x < 5 ; x++ )
	//						{
	//							CurrentView [ x , 0 ] = new ConsoleChar ( "┛ ┋ ┃" [ x ] , ConsoleColor . White , ConsoleColor . DarkGray ) ;
	//							CurrentView [ x , 1 ] = new ConsoleChar ( "┅┅┛ ┃" [ x ] , ConsoleColor . White , ConsoleColor . DarkGray ) ;
	//							CurrentView [ x , 2 ] = new ConsoleChar ( "━━━━┛" [ x ] , ConsoleColor . White , ConsoleColor . DarkGray ) ;
	//						}
	//					}

	//					else if ( Target . GetAzimuth ( forwardRoad ) == BlockAzimuth . Right &&
	//							Target . GetAzimuth ( backwardRoad ) == BlockAzimuth . Up ||
	//							Target . GetAzimuth ( forwardRoad ) == BlockAzimuth . Up &&
	//							Target . GetAzimuth ( backwardRoad ) == BlockAzimuth . Right )
	//					{
	//						for ( int x = 0 ; x < 5 ; x++ )
	//						{
	//							CurrentView [ x , 0 ] = new ConsoleChar ( "┃ ┋ ┗" [ x ] , ConsoleColor . White , ConsoleColor . DarkGray ) ;
	//							CurrentView [ x , 1 ] = new ConsoleChar ( "┃ ┗┅┅" [ x ] , ConsoleColor . White , ConsoleColor . DarkGray ) ;
	//							CurrentView [ x , 2 ] = new ConsoleChar ( "┗━━━━" [ x ] , ConsoleColor . White , ConsoleColor . DarkGray ) ;
	//						}
	//					}

	//					else if ( Target . GetAzimuth ( forwardRoad ) == BlockAzimuth . Left &&
	//							Target . GetAzimuth ( backwardRoad ) == BlockAzimuth . Down ||
	//							Target . GetAzimuth ( forwardRoad ) == BlockAzimuth . Down &&
	//							Target . GetAzimuth ( backwardRoad ) == BlockAzimuth . Left )
	//					{
	//						for ( int x = 0 ; x < 5 ; x++ )
	//						{
	//							CurrentView [ x , 0 ] = new ConsoleChar ( "━━━━┓" [ x ] , ConsoleColor . White , ConsoleColor . DarkGray ) ;
	//							CurrentView [ x , 1 ] = new ConsoleChar ( "┅┅┓ ┃" [ x ] , ConsoleColor . White , ConsoleColor . DarkGray ) ;
	//							CurrentView [ x , 2 ] = new ConsoleChar ( "┓ ┋ ┃" [ x ] , ConsoleColor . White , ConsoleColor . DarkGray ) ;
	//						}
	//					}

	//					else if ( Target . GetAzimuth ( forwardRoad ) == BlockAzimuth . Right &&
	//							Target . GetAzimuth ( backwardRoad ) == BlockAzimuth . Down ||
	//							Target . GetAzimuth ( forwardRoad ) == BlockAzimuth . Down &&
	//							Target . GetAzimuth ( backwardRoad ) == BlockAzimuth . Right )
	//					{
	//						for ( int x = 0 ; x < 5 ; x++ )
	//						{
	//							CurrentView [ x , 0 ] = new ConsoleChar ( "┏━━━━" [ x ] , ConsoleColor . White , ConsoleColor . DarkGray ) ;
	//							CurrentView [ x , 1 ] = new ConsoleChar ( "┃ ┏┅┅" [ x ] , ConsoleColor . White , ConsoleColor . DarkGray ) ;
	//							CurrentView [ x , 2 ] = new ConsoleChar ( "┃ ┋ ┏" [ x ] , ConsoleColor . White , ConsoleColor . DarkGray ) ;
	//						}
	//					}

	//					break ;
	//				}
	//				case 3 :
	//				{
	//					if ( exitList . All ( road => Target . GetAzimuth ( road ) != BlockAzimuth . Down ) )
	//					{
	//						for ( int x = 0 ; x < 5 ; x++ )
	//						{
	//							CurrentView [ x , 0 ] = new ConsoleChar ( "┛ ┋ ┗" [ x ] , ConsoleColor . White , ConsoleColor . DarkGray ) ;
	//							CurrentView [ x , 1 ] = new ConsoleChar ( "┅┅┻┅┅" [ x ] , ConsoleColor . White , ConsoleColor . DarkGray ) ;
	//							CurrentView [ x , 2 ] = new ConsoleChar ( "━━━━━" [ x ] , ConsoleColor . White , ConsoleColor . DarkGray ) ;
	//						}
	//					}
	//					else if ( exitList . All ( road => Target . GetAzimuth ( road ) != BlockAzimuth . Up ) )
	//					{
	//						for ( int x = 0 ; x < 5 ; x++ )
	//						{
	//							CurrentView [ x , 0 ] = new ConsoleChar ( "━━━━━" [ x ] , ConsoleColor . White , ConsoleColor . DarkGray ) ;
	//							CurrentView [ x , 1 ] = new ConsoleChar ( "┅┅┳┅┅" [ x ] , ConsoleColor . White , ConsoleColor . DarkGray ) ;
	//							CurrentView [ x , 2 ] = new ConsoleChar ( "┓ ┋ ┏" [ x ] , ConsoleColor . White , ConsoleColor . DarkGray ) ;
	//						}
	//					}
	//					else if ( exitList . All ( road => Target . GetAzimuth ( road ) != BlockAzimuth . Right ) )
	//					{
	//						for ( int x = 0 ; x < 5 ; x++ )
	//						{
	//							CurrentView [ x , 0 ] = new ConsoleChar ( "┛ ┋ ┃" [ x ] , ConsoleColor . White , ConsoleColor . DarkGray ) ;
	//							CurrentView [ x , 1 ] = new ConsoleChar ( "┅┅┫ ┃" [ x ] , ConsoleColor . White , ConsoleColor . DarkGray ) ;
	//							CurrentView [ x , 2 ] = new ConsoleChar ( "┓ ┋ ┃" [ x ] , ConsoleColor . White , ConsoleColor . DarkGray ) ;
	//						}
	//					}
	//					else if ( exitList . All ( road => Target . GetAzimuth ( road ) != BlockAzimuth . Left ) )
	//					{
	//						for ( int x = 0 ; x < 5 ; x++ )
	//						{
	//							CurrentView [ x , 0 ] = new ConsoleChar ( "┃ ┋ ┗" [ x ] , ConsoleColor . White , ConsoleColor . DarkGray ) ;
	//							CurrentView [ x , 1 ] = new ConsoleChar ( "┃ ┣┅┅" [ x ] , ConsoleColor . White , ConsoleColor . DarkGray ) ;
	//							CurrentView [ x , 2 ] = new ConsoleChar ( "┃ ┋ ┏" [ x ] , ConsoleColor . White , ConsoleColor . DarkGray ) ;
	//						}
	//					}

	//					break ;
	//				}
	//			}
	//		}
	//	}

	//}

}
