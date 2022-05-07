/* Written by Kaz Crowe */
/* UltimateButtonWindow.cs */
using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;
using System.Collections.Generic;

public class UltimateButtonWindow : EditorWindow
{
	static string version = "2.5";// ALWAYS UDPATE
	static int importantChanges = 2;// UPDATE ON IMPORTANT CHANGES
	// Important Change - Version Number
	// 2 - 2.5
	// 1 - ?
	static string menuTitle = "Main Menu";

	// LAYOUT STYLES //
	int sectionSpace = 20;
	int itemHeaderSpace = 10;
	int paragraphSpace = 5;
	GUIStyle sectionHeaderStyle = new GUIStyle();
	GUIStyle itemHeaderStyle = new GUIStyle();
	GUIStyle paragraphStyle = new GUIStyle();

	GUILayoutOption[] buttonSize = new GUILayoutOption[] { GUILayout.Width( 200 ), GUILayout.Height( 35 ) }; 
	GUILayoutOption[] docSize = new GUILayoutOption[] { GUILayout.Width( 300 ), GUILayout.Height( 330 ) };
	GUISkin style;
	Texture2D scriptReference;
	Texture2D utPromo, usbPromo;
	
	class PageInformation
	{
		public string pageName = "";
		public Vector2 scrollPosition = Vector2.zero;
		public delegate void TargetMethod();
		public TargetMethod targetMethod;
	}
	static PageInformation mainMenu = new PageInformation() { pageName = "Main Menu" };
	static PageInformation howTo = new PageInformation() { pageName = "How To" };
	static PageInformation overview = new PageInformation() { pageName = "Overview" };
	static PageInformation documentation = new PageInformation() { pageName = "Documentation" };
	static PageInformation otherProducts = new PageInformation() { pageName = "Other Products" };
	static PageInformation feedback = new PageInformation() { pageName = "Feedback" };
	static PageInformation changeLog = new PageInformation() { pageName = "Change Log" };
	static PageInformation versionChanges = new PageInformation() { pageName = "Version Changes" };
	static PageInformation thankYou = new PageInformation() { pageName = "Thank You" };
	static PageInformation settings = new PageInformation() { pageName = "Window Settings" };
	static List<PageInformation> pageHistory = new List<PageInformation>();
	static PageInformation currentPage = new PageInformation();

	enum FontSize
	{
		Small,
		Medium,
		Large
	}
	FontSize fontSize = FontSize.Small;
	bool configuredFontSize = false;

	#region Documentation Info
	class DocumentationInfo
	{
		public string functionName = "";
		public AnimBool showMore = new AnimBool( false );
		public string[] parameter;
		public string returnType = "";
		public string description = "";
		public string codeExample = "";
	}
	DocumentationInfo p_UpdatePositioning = new DocumentationInfo()
	{
		functionName = "UpdatePositioning()",
		description = "Updates the size and positioning of the Ultimate Button. This function can be used to update any options that may have been changed prior to Start().",
		codeExample = "button.buttonSize = 4.0f;\nbutton.UpdatePositioning();"
	};
	DocumentationInfo p_UpdateBaseColor = new DocumentationInfo()
	{
		functionName = "UpdateBaseColor()",
		showMore = new AnimBool( false ),
		parameter = new string[ 1 ]
		{ 
			"Color targetColor - The color to apply to the base image."
		},
		description = "Updates the color of the assigned button base images with the targeted color. The targetColor option will overwrite the current setting for base color.",
		codeExample = "button.UpdateBaseColor( Color.white );"
	};
	DocumentationInfo p_UpdateHighlightColor = new DocumentationInfo()
	{
		functionName = "UpdateHighlightColor()",
		showMore = new AnimBool( false ),
		parameter = new string[ 1 ]
		{ 
			"Color targetColor - The color to apply to the highlight images."
		},
		description = "Updates the colors of the assigned highlight images with the targeted color if the showHighlight variable is set to true. The targetColor variable will overwrite the current color setting for highlightColor and apply immediately to the highlight images.",
		codeExample = "button.UpdateHighlightColor( Color.white );"
	};
	DocumentationInfo p_UpdateTensionColors = new DocumentationInfo()
	{
		functionName = "UpdateTensionColors()",
		showMore = new AnimBool( false ),
		parameter = new string[ 2 ]
		{
			"Color targetTensionNone - The color to apply to the default state of the tension accent image.",
			"Color targetTensionFull - The colors to apply to the touched state of the tension accent images."
		},
		description = "Updates the tension accent image colors with the targeted colors if the showTension variable is true. The tension colors will be set to the targeted colors, and will be applied when the button is next used.",
		codeExample = "button.UpdateTensionColors( Color.white, Color.green );"
	};
	DocumentationInfo p_DisableButton = new DocumentationInfo()
	{
		functionName = "DisableButton()",
		showMore = new AnimBool( false ),
		description = "This function will reset the Ultimate Button and disable the gameObject. Use this function when wanting to disable the Ultimate Button from being used.",
		codeExample = "button.DisableButton();"
	};
	DocumentationInfo p_EnableButton = new DocumentationInfo()
	{
		functionName = "EnableButton()",
		showMore = new AnimBool( false ),
		description = "This function will ensure that the Ultimate Button is completely reset before enabling itself to be used again.",
		codeExample = "button.EnableButton();"
	};
	
	// STATIC //
	DocumentationInfo s_GetUltimateButton = new DocumentationInfo()
	{
		functionName = "GetUltimateButton()",
		showMore = new AnimBool( false ),
		parameter = new string[ 1 ]
		{
			"string buttonName - The name that the targeted Ultimate Button has been registered with."
		},
		returnType = "UltimateButton",
		description = "Returns the Ultimate Button registered with the buttonName string. This function can be used to call local functions on the Ultimate Button to apply color changes or position updates at runtime.",
		codeExample = "UltimateButton jumpButton = UltimateButton.GetUltimateButton( \"Jump\" );"
	};
	DocumentationInfo s_GetButtonDown = new DocumentationInfo()
	{
		functionName = "GetButtonDown()",
		showMore = new AnimBool( false ),
		parameter = new string[ 1 ]
		{
			"string buttonName - The name that the targeted Ultimate Button has been registered with."
		},
		description = "Returns true on the frame that the targeted Ultimate Button is pressed down.",
		codeExample = "if( UltimateButton.GetButtonDown( \"Jump\" ) )\n{\n    Debug.Log( \"The user has touched down on the jump button!\" );\n}"
	};
	DocumentationInfo s_GetButtonUp = new DocumentationInfo()
	{
		functionName = "GetButtonUp()",
		showMore = new AnimBool( false ),
		parameter = new string[ 1 ]
		{
			"string buttonName - The name that the targeted Ultimate Button has been registered with."
		},
		description = "Returns true on the frame that the targeted Ultimate Button is released.",
		codeExample = "if( UltimateButton.GetButtonUp( \"Jump\" ) )\n{\n    Debug.Log( \"The user has released the touch on the jump button!\" );\n}"
	};
	DocumentationInfo s_GetButton = new DocumentationInfo()
	{
		functionName = "GetButton()",
		showMore = new AnimBool( false ),
		parameter = new string[ 1 ]
		{
			"string buttonName - The name that the targeted Ultimate Button has been registered with."
		},
		description = "Returns true on the frames that the targeted Ultimate Button is being interacted with.",
		codeExample = "if( UltimateButton.GetButton( \"Jump\" ) )\n{\n    Debug.Log( \"The user is touching the jump button!\" );\n}"
	};
	DocumentationInfo s_DisableButton = new DocumentationInfo()
	{
		functionName = "DisableButton()",
		showMore = new AnimBool( false ),
		parameter = new string[ 1 ]
		{
			"string buttonName - The name that the targeted Ultimate Button has been registered with."
		},
		description = "This function will reset the Ultimate Button and disable the gameObject. Use this function when wanting to disable the Ultimate Button from being used.",
		codeExample = "UltimateButton.DisableButton( \"Jump\" );"
	};
	DocumentationInfo s_EnableButton = new DocumentationInfo()
	{
		functionName = "EnableButton()",
		showMore = new AnimBool( false ),
		parameter = new string[ 1 ]
		{
			"string buttonName - The name that the targeted Ultimate Button has been registered with."
		},
		description = "This function will ensure that the Ultimate Button is completely reset before enabling itself to be used again.",
		codeExample = "UltimateButton.EnableButton( \"Jump\" );"
	};
	#endregion

	[MenuItem( "Window/Tank and Healer Studio/Ultimate Button", false, 20 )]
	static void InitializeWindow ()
	{
		EditorWindow window = GetWindow<UltimateButtonWindow>( true, "Tank and Healer Studio Asset Window", true );
		window.maxSize = new Vector2( 500, 500 );
		window.minSize = new Vector2( 500, 500 );
		window.Show();
	}

	public static void OpenDocumentation ()
	{
		InitializeWindow();

		if( !pageHistory.Contains( documentation ) )
			NavigateForward( documentation );
	}

	void OnEnable ()
	{
		style = ( GUISkin )Resources.Load( "UltimateButtonEditorSkin" );

		scriptReference = ( Texture2D )Resources.Load( "UB_ScriptRef" );
		utPromo = ( Texture2D )Resources.Load( "UT_Promo" );
		usbPromo = ( Texture2D )Resources.Load( "USB_Promo" );

		if( !pageHistory.Contains( mainMenu ) )
			pageHistory.Insert( 0, mainMenu );

		mainMenu.targetMethod = MainMenu;
		howTo.targetMethod = HowTo;
		overview.targetMethod = Overview;
		documentation.targetMethod = Documentation;
		otherProducts.targetMethod = OtherProducts;
		feedback.targetMethod = Feedback;
		changeLog.targetMethod = ChangeLog;
		versionChanges.targetMethod = VersionChanges;
		thankYou.targetMethod = ThankYou;
		settings.targetMethod = WindowSettings;

		if( pageHistory.Count == 1 )
			currentPage = mainMenu;
	}
	
	void OnGUI ()
	{
		if( style == null )
		{
			GUILayout.BeginVertical( "Box" );
			GUILayout.FlexibleSpace();
			ErrorScreen();
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndVertical();
			return;
		}

		GUI.skin = style;

		paragraphStyle = GUI.skin.GetStyle( "ParagraphStyle" );
		itemHeaderStyle = GUI.skin.GetStyle( "ItemHeader" );
		sectionHeaderStyle = GUI.skin.GetStyle( "SectionHeader" );

		if( !configuredFontSize )
		{
			configuredFontSize = true;
			if( paragraphStyle.fontSize == 14 )
				fontSize = FontSize.Large;
			else if( paragraphStyle.fontSize == 12 )
				fontSize = FontSize.Medium;
			else
				fontSize = FontSize.Small;
		}
		
		GUILayout.BeginVertical( "Box" );
		
		EditorGUILayout.BeginHorizontal();

		EditorGUILayout.LabelField( "Ultimate Button", GUI.skin.GetStyle( "WindowTitle" ) );

		if( GUILayout.Button( "", GUI.skin.GetStyle( "SettingsButton" ) ) && currentPage != settings && !pageHistory.Contains( settings ) )
			NavigateForward( settings );

		var rect = GUILayoutUtility.GetLastRect();
		EditorGUIUtility.AddCursorRect( rect, MouseCursor.Link );

		EditorGUILayout.EndHorizontal();

		GUILayout.Space( 3 );
		
		if( GUILayout.Button( "Version " + version, GUI.skin.GetStyle( "VersionNumber" ) ) && currentPage != changeLog && !pageHistory.Contains( changeLog ) )
			NavigateForward( changeLog );

		rect = GUILayoutUtility.GetLastRect();
		EditorGUIUtility.AddCursorRect( rect, MouseCursor.Link );

		GUILayout.Space( 12 );

		EditorGUILayout.BeginHorizontal();
		GUILayout.Space( 5 );
		if( pageHistory.Count > 1 )
		{
			if( GUILayout.Button( "", GUI.skin.GetStyle( "BackButton" ), GUILayout.Width( 80 ), GUILayout.Height( 40 ) ) )
				NavigateBack();

			rect = GUILayoutUtility.GetLastRect();
			EditorGUIUtility.AddCursorRect( rect, MouseCursor.Link );
		}
		else
			GUILayout.Space( 80 );

		GUILayout.Space( 15 );
		EditorGUILayout.LabelField( menuTitle, GUI.skin.GetStyle( "MenuTitle" ) );
		GUILayout.FlexibleSpace();
		GUILayout.Space( 80 );
		EditorGUILayout.EndHorizontal();

		GUILayout.Space( 10 );

		EditorGUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();

		if( currentPage.targetMethod != null )
			currentPage.targetMethod();

		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();

		GUILayout.FlexibleSpace();

		GUILayout.Space( 25 );

		EditorGUILayout.EndVertical();

		Repaint();
	}

	void ErrorScreen ()
	{
		EditorGUILayout.BeginHorizontal();
		GUILayout.Space( 50 );
		EditorGUILayout.LabelField( "ERROR", EditorStyles.boldLabel );
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		GUILayout.Space( 50 );
		EditorGUILayout.LabelField( "Could not find the needed GUISkin located in the Editor / Resources folder. Please ensure that the correct GUISkin, UltimateButtonEditorSkin, is in the right folder( Ultimate Button / Editor / Resources ) before trying to access the Ultimate Button Window.", paragraphStyle );
		GUILayout.Space( 50 );
		EditorGUILayout.EndHorizontal();
	}
	
	static void NavigateBack ()
	{
		pageHistory.RemoveAt( pageHistory.Count - 1 );
		menuTitle = pageHistory[ pageHistory.Count - 1 ].pageName;
		currentPage = pageHistory[ pageHistory.Count - 1 ];
	}

	static void NavigateForward ( PageInformation menu )
	{
		pageHistory.Add( menu );
		menuTitle = menu.pageName;
		currentPage = menu;
	}
	
	void MainMenu ()
	{
		mainMenu.scrollPosition = EditorGUILayout.BeginScrollView( mainMenu.scrollPosition, false, false, docSize );

		GUILayout.Space( 25 );
		EditorGUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if( GUILayout.Button( "How To", buttonSize ) )
			NavigateForward( howTo );

		var rect = GUILayoutUtility.GetLastRect();
		EditorGUIUtility.AddCursorRect( rect, MouseCursor.Link );

		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();

		GUILayout.FlexibleSpace();

		EditorGUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if( GUILayout.Button( "Overview", buttonSize ) )
			NavigateForward( overview );

		rect = GUILayoutUtility.GetLastRect();
		EditorGUIUtility.AddCursorRect( rect, MouseCursor.Link );

		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();

		GUILayout.FlexibleSpace();

		EditorGUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if( GUILayout.Button( "Documentation", buttonSize ) )
			NavigateForward( documentation );

		rect = GUILayoutUtility.GetLastRect();
		EditorGUIUtility.AddCursorRect( rect, MouseCursor.Link );

		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();

		GUILayout.FlexibleSpace();

		EditorGUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if( GUILayout.Button( "Other Products", buttonSize ) )
			NavigateForward( otherProducts );

		rect = GUILayoutUtility.GetLastRect();
		EditorGUIUtility.AddCursorRect( rect, MouseCursor.Link );

		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();

		GUILayout.FlexibleSpace();

		EditorGUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if( GUILayout.Button( "Feedback", buttonSize ) )
			NavigateForward( feedback );

		rect = GUILayoutUtility.GetLastRect();
		EditorGUIUtility.AddCursorRect( rect, MouseCursor.Link );

		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();

		GUILayout.FlexibleSpace();

		EditorGUILayout.EndScrollView();
	}

	void HowTo ()
	{
		StartPage( howTo );

		EditorGUILayout.LabelField( "How To Create", sectionHeaderStyle );

		EditorGUILayout.LabelField( Indent + "To create an Ultimate Button in your scene, simply find the Ultimate Button prefab that you would like to add and click on the \"Add to Scene\" button on the Ultimate Button inspector. What this does is creates that Ultimate Button within the scene and ensures that there is a Canvas and an EventSystem so that it can work correctly. If these are not present in the scene, they will be created for you.", paragraphStyle );

		GUILayout.Space( sectionSpace );

		EditorGUILayout.LabelField( "How To Reference", sectionHeaderStyle );
		EditorGUILayout.LabelField( Indent + "One of the great things about the Ultimate Button is how easy it is to reference to other scripts. The first thing that you will want to make sure to do is determine how you want to use the Ultimate Button within your scripts. If you are used to using the Events that are used in Unity's default UI buttons, then you may want to use the Unity Events options located within the Button Events section of the Ultimate Button inspector. However, if you are used to using Unity's Input system for getting input, then the Script Reference section would probably suit you better.", paragraphStyle );

		GUILayout.Space( paragraphSpace );

		EditorGUILayout.LabelField( "For this example, we'll go over how to use the Script Reference section. First thing to do is assign the Button Name within the Script Reference section. After this is complete, you will be able to reference that particular button by it's name from a static function within the Ultimate Button script.", paragraphStyle );

		GUILayout.Space( sectionSpace );

		EditorGUILayout.LabelField( "Example", sectionHeaderStyle );

		EditorGUILayout.LabelField( Indent + "If you are going to use the Ultimate Button for making a player jump, then you will need to check the button's state to determine when the user has touched the button and is wanting the player to jump. So for this example, let's assign the name \"Jump\" in the Script Reference section of the Ultimate Button.", paragraphStyle );

		GUILayout.Space( itemHeaderSpace );

		EditorGUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.Label( scriptReference );
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();

		GUILayout.Space( itemHeaderSpace );

		EditorGUILayout.LabelField( "There are several functions that allow you to check the different states that the Ultimate Button is in. For more information on all the functions that you have available to you, please see the documentation section of this Help Window.", paragraphStyle );

		GUILayout.Space( paragraphSpace );

		EditorGUILayout.LabelField( "For this example we will be using the GetButtonDown function to see if the user has pressed down on the button. It is worth noting that this function is useful when wanting to make the player start the jump action on the exact frame that the user has pressed down on the button, and not after at all.", paragraphStyle );

		GUILayout.Space( itemHeaderSpace );

		EditorGUILayout.LabelField( "Example Code:", itemHeaderStyle );
		EditorGUILayout.TextArea( "if( UltimateButton.GetButtonDown( \"Jump\" ) )\n{\n	// Call player jump function.\n}", GUI.skin.GetStyle( "TextArea" ) );

		GUILayout.Space( itemHeaderSpace );

		EditorGUILayout.LabelField( "Feel free to experiment with the different functions of the Ultimate Button to get it working exactly the way you want to. Additionally, if you are curious about how the Ultimate Button has been implemented into an Official Tank and Healer Studio example, then please see the README.txt that is included with the example files for the project.", paragraphStyle );

		GUILayout.Space( itemHeaderSpace );

		EndPage();
	}
	
	void Overview ()
	{
		StartPage( overview );
		
		/* //// --------------------------- < SIZE AND PLACEMENT > --------------------------- \\\\ */
		EditorGUILayout.LabelField( "Size And Placement", sectionHeaderStyle );
		EditorGUILayout.LabelField( Indent + "The Size and Placement section allows you to customize the button's size and placement on the screen, as well as determine where the user's touch can be processed for the selected button.", paragraphStyle );

		GUILayout.Space( paragraphSpace );

		// Scaling Axis
		EditorGUILayout.LabelField( "Scaling Axis", itemHeaderStyle );
		EditorGUILayout.LabelField( "Determines which axis the button will be scaled from. If Height is chosen, then the button will scale itself proportionately to the Height of the screen.", paragraphStyle );

		GUILayout.Space( paragraphSpace );

		// Anchor
		EditorGUILayout.LabelField( "Anchor", itemHeaderStyle );
		EditorGUILayout.LabelField( "Determines which side of the screen that the button will be anchored to.", paragraphStyle );
		
		GUILayout.Space( paragraphSpace );

		// Touch Size
		EditorGUILayout.LabelField( "Touch Size", itemHeaderStyle );
		EditorGUILayout.LabelField( "Touch Size configures the size of the area where the user can touch. You have the options of either 'Default','Medium', or 'Large'.", paragraphStyle );

		GUILayout.Space( paragraphSpace );
		
		// Button Size
		EditorGUILayout.LabelField( "Button Size", itemHeaderStyle );
		EditorGUILayout.LabelField( "Button Size will change the scale of the button. Since everything is calculated out according to screen size, your Touch Size option and other properties will scale proportionately with the button's size along your specified Scaling Axis.", paragraphStyle );
		
		GUILayout.Space( paragraphSpace );

		// Button Position
		EditorGUILayout.LabelField( "Button Position", itemHeaderStyle );
		EditorGUILayout.LabelField( "Button Position will present you with two sliders. The X value will determine how far the button is away from the Left and Right sides of the screen, and the Y value from the Top and Bottom. This will encompass 50% of your screen, relevant to your Anchor selection.", paragraphStyle );
		/* \\\\ -------------------------- < END SIZE AND PLACEMENT > --------------------------- //// */

		GUILayout.Space( sectionSpace );

		/* //// ----------------------------- < STYLE AND OPTIONS > ----------------------------- \\\\ */
		EditorGUILayout.LabelField( "Style And Options", sectionHeaderStyle );
		EditorGUILayout.LabelField( Indent + "The Style and Options section contains options that affect how the button is visually presented to the user.", paragraphStyle );
		
		GUILayout.Space( paragraphSpace );

		// Image Style
		EditorGUILayout.LabelField( "Image Style", itemHeaderStyle );
		EditorGUILayout.LabelField( "Determines whether the input range should be circular or square. This option affects how the Input Range and Track Input options function.", paragraphStyle );

		GUILayout.Space( paragraphSpace );

		// Input Range
		EditorGUILayout.LabelField( "Input Range", itemHeaderStyle );
		EditorGUILayout.LabelField( "The range that the Ultimate Button will react to when initiating and dragging the input.", paragraphStyle );

		GUILayout.Space( paragraphSpace );

		// Track Input
		EditorGUILayout.LabelField( "Track Input", itemHeaderStyle );
		EditorGUILayout.LabelField( "If the Track Input option is enabled, then the Ultimate Button will reflect it's state according to where the user's input currently is. This means that if the input moves off of the button, then the button state will turn to false. When the input returns to the button the state will return to true. If the Track Input option is disabled, then the button will reflect the state of only pressing and releasing the button.", paragraphStyle );

		GUILayout.Space( paragraphSpace );

		// Transmit Input
		EditorGUILayout.LabelField( "Transmit Input", itemHeaderStyle );
		EditorGUILayout.LabelField( "The Transmit Input option will allow you to send the input data to another script that uses Unity's EventSystem. For example, if you are using the Ultimate Joystick package, you could place the Ultimate Button on top of the Ultimate Joystick, and still have the Ultimate Button and Ultimate Joystick function correctly when interacted with.", paragraphStyle );

		GUILayout.Space( paragraphSpace );

		// Tap Count Option
		EditorGUILayout.LabelField( "Tap Count Option", itemHeaderStyle );
		EditorGUILayout.LabelField( "The Tap Count option allows you to decide if you want to store the amount of taps that the button receives. The options provided with the Tap Count will allow you to customize the target amount of taps, the tap time window, and the event to be called when the tap count has been achieved.", paragraphStyle );
		
		GUILayout.Space( paragraphSpace );

		// Base Color
		EditorGUILayout.LabelField( "Base Color", itemHeaderStyle );
		EditorGUILayout.LabelField( "The Base Color option determines the color of the button base images.", paragraphStyle );

		GUILayout.Space( paragraphSpace );

		// Show Highlight
		EditorGUILayout.LabelField( "Show Highlight", itemHeaderStyle );
		EditorGUILayout.LabelField( "Show Highlight will allow you to customize the set highlight images with a custom color. With this option, you will also be able to customize and set these images at runtime using the UpdateHighlightColor function. See the Documentation section for more details.", paragraphStyle );

		GUILayout.Space( paragraphSpace );
		
		// Show Tension
		EditorGUILayout.LabelField( "Show Tension", itemHeaderStyle );
		EditorGUILayout.LabelField( "With Show Tension enabled, the button will display interactions visually using custom colors and images to display the intensity of the press. With this option enabled, you will be able to update the tension colors at runtime using the UpdateTensionColors function. See the Documentation section for more information.", paragraphStyle );
		
		GUILayout.Space( paragraphSpace );

		// Use Animation
		EditorGUILayout.LabelField( "Use Animation", itemHeaderStyle );
		EditorGUILayout.LabelField( "If you would like the button to play an animation when being interacted with, then you will want to enable the Use Animation option.", paragraphStyle );
		
		GUILayout.Space( paragraphSpace );

		// Use Fade
		EditorGUILayout.LabelField( "Use Fade", itemHeaderStyle );
		EditorGUILayout.LabelField( "The Use Fade option will present you with settings for the targeted alpha for the touched and untouched states, as well as the duration for the fade between the targeted alpha settings.", paragraphStyle );
		/* //// --------------------------- < END STYLE AND OPTIONS > --------------------------- \\\\ */

		GUILayout.Space( sectionSpace );

		/* //// ----------------------------- < SCRIPT REFERENCE > ------------------------------ \\\\ */
		EditorGUILayout.LabelField( "Script Reference", sectionHeaderStyle );
		EditorGUILayout.LabelField( Indent + "The Script Reference section contains fields for naming and helpful code snippets that you can copy and paste into your scripts. Be sure to refer to the Documentation Window for information about the functions that you have available to you.", paragraphStyle );
		
		GUILayout.Space( paragraphSpace );
		
		// Button Name
		EditorGUILayout.LabelField( "Button Name", itemHeaderStyle );
		EditorGUILayout.LabelField( "The unique name of your Ultimate Button. This name is what will be used to reference this particular button from the public static functions.", paragraphStyle );
		
		GUILayout.Space( paragraphSpace );

		// Example Code
		EditorGUILayout.LabelField( "Example Code", itemHeaderStyle );
		EditorGUILayout.LabelField( "This section will present you with code snippets that are determined by your selection. This code can be copy and pasted into your custom scripts. Please note that this section is only designed to help you get the Ultimate Button working in your scripts quickly. Any options within this section do have affect the actual functionality of the button.", paragraphStyle );
		/* //// --------------------------- < END SCRIPT REFERENCE > ---------------------------- \\\\ */

		GUILayout.Space( sectionSpace );

		/* //// ------------------------------- < BUTTON EVENTS > ------------------------------- \\\\ */
		EditorGUILayout.LabelField( "Button Events", sectionHeaderStyle );
		EditorGUILayout.LabelField( Indent + "The Button Events section contains Unity Events that can be created for when the Ultimate Button is pressed and released. Also, if you have the Tap Count Option set, then you can assign a Unity Event for the Tap Count Event option.", paragraphStyle );
		
		GUILayout.Space( itemHeaderSpace );
		/* //// ----------------------------- < END BUTTON EVENTS > ----------------------------- \\\\ */
		EndPage();
	}
	
	void Documentation ()
	{
		StartPage( documentation );

		/* //// --------------------------- < PUBLIC FUNCTIONS > --------------------------- \\\\ */
		EditorGUILayout.LabelField( "Public Functions", sectionHeaderStyle );

		GUILayout.Space( paragraphSpace );

		EditorGUILayout.LabelField( Indent + "All of the following public functions are only available from a reference to the Ultimate Button. Each example provided relies on having a Ultimate Button variable named 'button' stored inside your script. When using any of the example code provided, make sure that you have a public Ultimate Button variable like the one below:", paragraphStyle );

		EditorGUILayout.TextArea( "public UltimateButton button;", GUI.skin.textArea );

		EditorGUILayout.LabelField( "Please click on the function name to learn more.", paragraphStyle );

		GUILayout.Space( paragraphSpace );

		ShowDocumentation( p_UpdatePositioning );
		ShowDocumentation( p_UpdateBaseColor );
		ShowDocumentation( p_UpdateHighlightColor );
		ShowDocumentation( p_UpdateTensionColors );
		ShowDocumentation( p_DisableButton );
		ShowDocumentation( p_EnableButton );

		GUILayout.Space( sectionSpace );
		
		/* //// --------------------------- < STATIC FUNCTIONS > --------------------------- \\\\ */
		EditorGUILayout.LabelField( "Static Functions", sectionHeaderStyle );

		GUILayout.Space( paragraphSpace );

		EditorGUILayout.LabelField( Indent + "The following functions can be referenced from your scripts without the need for an assigned local Ultimate Button variable. However, each function must have the targeted Ultimate Button name in order to find the correct Ultimate Button in the scene. Each example code provided uses the name 'Jump' as the button name.", paragraphStyle );

		GUILayout.Space( paragraphSpace );

		ShowDocumentation( s_GetUltimateButton );
		ShowDocumentation( s_GetButtonDown );
		ShowDocumentation( s_GetButtonUp );
		ShowDocumentation( s_GetButton );
		ShowDocumentation( s_DisableButton );
		ShowDocumentation( s_EnableButton );
		
		GUILayout.Space( itemHeaderSpace );
		
		/* //// --------------------------- < PUBLIC FUNCTIONS > --------------------------- \\\\ *
		EditorGUILayout.LabelField( "Public Functions", sectionHeaderStyle );

		GUILayout.Space( paragraphSpace );

		// UpdatePositioning()
		EditorGUILayout.LabelField( "UpdatePositioning()", itemHeaderStyle );
		EditorGUILayout.LabelField( "Updates the size and positioning of the Ultimate Button. This function can be used to update any options that may have been changed prior to Start().", paragraphStyle );

		GUILayout.Space( paragraphSpace );
		/*
		// UpdateBaseColor()
		EditorGUILayout.LabelField( "UpdateBaseColor( Color targetColor )", itemHeaderStyle );
		EditorGUILayout.LabelField( "Updates the color of the assigned button base images with the targeted color. The targetColor option will overwrite the current setting for base color.", paragraphStyle );

		GUILayout.Space( paragraphSpace );

		// UpdateHighlightColor()
		EditorGUILayout.LabelField( "UpdateHighlightColor( Color targetColor )", itemHeaderStyle );
		EditorGUILayout.LabelField( "Updates the colors of the assigned highlight images with the targeted color if the showHighlight variable is set to true. The targetColor variable will overwrite the current color setting for highlightColor and apply immediately to the highlight images.", paragraphStyle );
				
		GUILayout.Space( paragraphSpace );

		// UpdateTensionColors()
		EditorGUILayout.LabelField( "UpdateTensionColors( Color targetTensionNone, Color targetTensionFull )", itemHeaderStyle );
		EditorGUILayout.LabelField( "Updates the tension accent image colors with the targeted colors if the showTension variable is true. The tension colors will be set to the targeted colors, and will be applied when the button is next used.", paragraphStyle );

		GUILayout.Space( paragraphSpace );

		// DisableButton()
		EditorGUILayout.LabelField( "DisableButton()", itemHeaderStyle );
		EditorGUILayout.LabelField( "Disables the Ultimate Button in the scene.", paragraphStyle );
		
		GUILayout.Space( paragraphSpace );

		// EnableButton()
		EditorGUILayout.LabelField( "EnableButton()", itemHeaderStyle );
		EditorGUILayout.LabelField( "Enables the Ultimate Button within the scene.", paragraphStyle );

		GUILayout.Space( sectionSpace );
		
		/* //// --------------------------- < STATIC FUNCTIONS > --------------------------- \\\\ *
		EditorGUILayout.LabelField( "Static Functions", sectionHeaderStyle );

		GUILayout.Space( paragraphSpace );

		// UltimateButton.GetUltimateButton
		EditorGUILayout.LabelField( "UltimateButton UltimateButton.GetUltimateButton( string buttonName )", itemHeaderStyle );
		EditorGUILayout.LabelField( "Returns the Ultimate Button registered with the buttonName string. This function can be used to call local functions on the Ultimate Button to apply color changes or position updates at runtime.", paragraphStyle );
						
		GUILayout.Space( paragraphSpace );

		// UltimateButton.GetButtonDown
		EditorGUILayout.LabelField( "bool UltimateButton.GetButtonDown( string buttonName )", itemHeaderStyle );
		EditorGUILayout.LabelField( "Returns true on the frame that the targeted Ultimate Button is pressed down.", paragraphStyle );
						
		GUILayout.Space( paragraphSpace );
		
		// UltimateButton.GetButtonUp
		EditorGUILayout.LabelField( "bool UltimateButton.GetButtonUp( string buttonName )", itemHeaderStyle );
		EditorGUILayout.LabelField( "Returns true on the frame that the targeted Ultimate Button is released.", paragraphStyle );
						
		GUILayout.Space( paragraphSpace );
		
		// UltimateButton.GetButton
		EditorGUILayout.LabelField( "bool UltimateButton.GetButton( string buttonName )", itemHeaderStyle );
		EditorGUILayout.LabelField( "Returns true on the frames that the targeted Ultimate Button is being interacted with.", paragraphStyle );
						
		GUILayout.Space( paragraphSpace );
				
		// UltimateButton.GetTapCount
		EditorGUILayout.LabelField( "bool UltimateButton.GetTapCount( string buttonName )", itemHeaderStyle );
		EditorGUILayout.LabelField( "Returns true on the frame that the Tap Count option has been achieved.", paragraphStyle );

		GUILayout.Space( paragraphSpace );

		// DisableButton
		EditorGUILayout.LabelField( "DisableButton( string buttonName )", itemHeaderStyle );
		EditorGUILayout.LabelField( "Disables the Ultimate Button in the scene that has been registered with the targeted button name.", paragraphStyle );
		
		GUILayout.Space( paragraphSpace );

		// EnableButton
		EditorGUILayout.LabelField( "EnableButton( string buttonName )", itemHeaderStyle );
		EditorGUILayout.LabelField( "Enables the Ultimate Button in the scene that has been registered with the targeted button name.", paragraphStyle );

		GUILayout.Space( itemHeaderSpace );
		*/
		EndPage();
	}
	
	void OtherProducts ()
	{
		StartPage( otherProducts );

		/* -------------- < ULTIMATE TOUCHPAD > -------------- */
		EditorGUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.Space( 15 );
		GUILayout.Label( utPromo, GUILayout.Width( 250 ), GUILayout.Height( 125 ) );
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();

		GUILayout.Space( paragraphSpace );

		EditorGUILayout.LabelField( "Ultimate Touchpad", sectionHeaderStyle );

		EditorGUILayout.LabelField( Indent + "The Ultimate Touchpad is a simple input tool for the development of your mobile games. The Ultimate Touchpad catches the users input and translates it into consistent values that can be used with any screen ratio. With this information, it can be easily implemented via the copy-and-paste Script Reference for use with a camera. Additionally, it features a Tap Count option that allows for the tracking of taps on the screen within the customizable touchpad area. If you're looking for an easy to use mobile input tool, the Ultimate Touchpad is a dependable solution.", paragraphStyle );

		GUILayout.Space( itemHeaderSpace );

		EditorGUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if( GUILayout.Button( "More Info", buttonSize ) )
			Application.OpenURL( "https://www.tankandhealerstudio.com/ultimate-touchpad.html" );

		var rect = GUILayoutUtility.GetLastRect();
		EditorGUIUtility.AddCursorRect( rect, MouseCursor.Link );


		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();
		/* ------------ < END ULTIMATE TOUCHPAD > ------------ */

		GUILayout.Space( sectionSpace );

		/* ------------ < ULTIMATE STATUS BAR > ------------ */
		EditorGUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.Space( 15 );
		GUILayout.Label( usbPromo, GUILayout.Width( 250 ), GUILayout.Height( 125 ) );
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();

		GUILayout.Space( paragraphSpace );

		EditorGUILayout.LabelField( "Ultimate Status Bar", sectionHeaderStyle );

		EditorGUILayout.LabelField( Indent + "The Ultimate Status Bar is a complete solution to display virtually any status for your game. With an abundance of options and customization available to you, as well as the simplest integration, the Ultimate Status Bar makes displaying any condition a cinch. Whether it’s health and energy for your player, the health of a target, or the progress of loading your scene, the Ultimate Status Bar can handle it with ease. ", paragraphStyle );

		GUILayout.Space( itemHeaderSpace );

		EditorGUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if( GUILayout.Button( "More Info", buttonSize ) )
			Application.OpenURL( "http://www.tankandhealerstudio.com/ultimate-status-bar.html" );

		rect = GUILayoutUtility.GetLastRect();
		EditorGUIUtility.AddCursorRect( rect, MouseCursor.Link );

		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();
		/* -------------- < END ULTIMATE STATUS BAR > --------------- */

		GUILayout.Space( itemHeaderSpace );

		EndPage();
	}
	
	void Feedback ()
	{
		StartPage( feedback );

		EditorGUILayout.LabelField( "Having Problems?", sectionHeaderStyle );

		EditorGUILayout.LabelField( Indent + "If you experience any issues with the Ultimate Button, please send us an email right away! We will lend any assistance that we can to resolve any issues that you have.\n\n<b>Support Email:</b>", paragraphStyle );

		GUILayout.Space( paragraphSpace );

		EditorGUILayout.SelectableLabel( "tankandhealerstudio@outlook.com", itemHeaderStyle, GUILayout.Height( 15 ) );

		GUILayout.Space( 25 );

		EditorGUILayout.LabelField( "Good Experiences?", sectionHeaderStyle );

		EditorGUILayout.LabelField( Indent + "If you have appreciated how easy the Ultimate Button is to get into your project, leave us a comment and rating on the Unity Asset Store. We are very grateful for all positive feedback that we get.", paragraphStyle );

		GUILayout.Space( itemHeaderSpace );

		EditorGUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if( GUILayout.Button( "Rate Us", buttonSize ) )
			Application.OpenURL( "https://www.assetstore.unity3d.com/en/#!/content/28824" );

		var rect = GUILayoutUtility.GetLastRect();
		EditorGUIUtility.AddCursorRect( rect, MouseCursor.Link );

		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();

		GUILayout.Space( 25 );

		EditorGUILayout.LabelField( "Show Us What You've Done!", sectionHeaderStyle );

		EditorGUILayout.LabelField( Indent + "If you have used any of the assets created by Tank & Healer Studio in your project, we would love to see what you have done. Contact us with any information on your game and we will be happy to support you in any way that we can!\n\n<b>Contact Us:</b>", paragraphStyle );

		GUILayout.Space( paragraphSpace );

		EditorGUILayout.SelectableLabel( "tankandhealerstudio@outlook.com" , itemHeaderStyle, GUILayout.Height( 15 ) );

		GUILayout.Space( itemHeaderSpace );

		EditorGUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		EditorGUILayout.LabelField( "Happy Game Making,\n	-Tank & Healer Studio", paragraphStyle, GUILayout.Height( 30 ) );
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();

		GUILayout.Space( 25 );

		EndPage();
	}
	
	void ChangeLog ()
	{
		StartPage( changeLog );

		EditorGUILayout.LabelField( "Version 2.5( Major Update )", itemHeaderStyle );
		EditorGUILayout.LabelField( "  • Reordered folders ( again ) to better conform to Unity's new standard for folder structure. This may cause some errors if you already had the Ultimate Button inside of your project. Please COMPLETELY REMOVE the Ultimate Button from your project and re-import the Ultimate Button after.", paragraphStyle );
		EditorGUILayout.LabelField( "  • Removed the ability to create an Ultimate Button from the Create menu because of the new folder structure. In order to create an Ultimate Button in your scene, use the method explained in the How To section of this window.", paragraphStyle );
		EditorGUILayout.LabelField( "  • Major improvements to the Ultimate Button Editor.", paragraphStyle );
		EditorGUILayout.LabelField( "  • Added a new script to handle updating with screen size. The script is named UltimateButtonScreenSizeUpdater.", paragraphStyle );
		EditorGUILayout.LabelField( "  • Added an official way to disable and enable the Ultimate Button through code.\n     • DisableButton\n     • EnableButton", paragraphStyle );

		GUILayout.Space( itemHeaderSpace );

		EditorGUILayout.LabelField( "Version 2.1.1", itemHeaderStyle );
		EditorGUILayout.LabelField( "  • Drastically improved the functionality of the Ultimate Button Documentation Window.", paragraphStyle );
		EditorGUILayout.LabelField( "  • Minor editor fixes.", paragraphStyle );

		GUILayout.Space( itemHeaderSpace );

		EditorGUILayout.LabelField( "Version 2.1", itemHeaderStyle );
		EditorGUILayout.LabelField( "  • Some folder structure and existing functionality has been updated and improved. ( None of these changes should affect any existing use of the Ultimate Button ).", paragraphStyle );
		EditorGUILayout.LabelField( "  • Removed the Touch Actions section. All options previously located in the Touch Actions section are now located in the Style and Options section.", paragraphStyle );
		EditorGUILayout.LabelField( "  • Expanded the functionality of using the Ultimate Button in your scripts. Added a new section titled Button Events. Now you can use either the Script Reference or the Button Events section to implement into your scripts.", paragraphStyle );
		EditorGUILayout.LabelField( "  • Removed example files from the Plugins folder. All example files will now be in the folder named: Ultimate Button Examples.", paragraphStyle );
		EditorGUILayout.LabelField( "  • Added four new Ultimate Button textures that can be used in your projects.", paragraphStyle );
		EditorGUILayout.LabelField( "  • Removed the Ultimate Button PSD from the project files.", paragraphStyle );
		EditorGUILayout.LabelField( "  • Improved Tension Accent functionality.", paragraphStyle );

		GUILayout.Space( itemHeaderSpace );

		EditorGUILayout.LabelField( "Version 2.0.2", itemHeaderStyle );
		EditorGUILayout.LabelField( "  • Minor changes to the editor script.", paragraphStyle );

		GUILayout.Space( itemHeaderSpace );

		EditorGUILayout.LabelField( "Version 2.0.1", itemHeaderStyle );
		EditorGUILayout.LabelField( "  • Minor changes to editor window.", paragraphStyle );
		EditorGUILayout.LabelField( "  • Small modifications to example scene.", paragraphStyle );

		GUILayout.Space( itemHeaderSpace );

		EditorGUILayout.LabelField( "Version 2.0", itemHeaderStyle );
		EditorGUILayout.LabelField( "  • Added a new in-engine documentation window.", paragraphStyle );
		EditorGUILayout.LabelField( "  • Removed old files from the previous example scene.", paragraphStyle );
		EditorGUILayout.LabelField( "  • Added new 2D example assets for the new example scene.", paragraphStyle );
		EditorGUILayout.LabelField( "  • Created new scripts to help show how to use the Ultimate Button more effectively.", paragraphStyle );
		EditorGUILayout.LabelField( "  • Overall improvement to performance.", paragraphStyle );
		EditorGUILayout.LabelField( "  • Improved overall performance.", paragraphStyle );

		EndPage();
	}

	void ThankYou ()
	{
		StartPage( thankYou );

		GUILayout.Space( 10 );

		EditorGUILayout.LabelField( "We here at Tank & Healer Studio would like to thank you for purchasing the Ultimate Button asset package from the Unity Asset Store. If you have any questions about this product please don't hesitate to contact us at: ", paragraphStyle );

		GUILayout.Space( paragraphSpace );
		EditorGUILayout.SelectableLabel( "tankandhealerstudio@outlook.com" , itemHeaderStyle, GUILayout.Height( 15 ) );
		GUILayout.Space( sectionSpace );

		EditorGUILayout.LabelField( Indent + " We hope that the Ultimate Button will be a great help to you in the development of your game. After pressing the continue button below, you will be presented with helpful information on this asset to assist you in implementing Ultimate Button into your project.\n", paragraphStyle );

		EditorGUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		EditorGUILayout.LabelField( "Happy Game Making,\n	-Tank & Healer Studio", GUILayout.Height( 30 ) );
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();

		GUILayout.Space( 15 );

		EditorGUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if( GUILayout.Button( "Continue", buttonSize ) )
			NavigateBack();

		var rect = GUILayoutUtility.GetLastRect();
		EditorGUIUtility.AddCursorRect( rect, MouseCursor.Link );

		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();

		EndPage();
	}
	
	void VersionChanges ()
	{
		StartPage( versionChanges );
		
		EditorGUILayout.LabelField( "  Thank you for downloading the most recent version of the Ultimate Button. There has been some major changes that could affect any existing reference of the Ultimate Button. If you are experiencing any errors, please completely remove the Ultimate Button from your project and re-import it. As always, if you run into any issues with the Ultimate Button, please contact us at:", paragraphStyle );

		GUILayout.Space( paragraphSpace );
		EditorGUILayout.SelectableLabel( "tankandhealerstudio@outlook.com", itemHeaderStyle, GUILayout.Height( 15 ) );
		GUILayout.Space( sectionSpace );

		EditorGUILayout.LabelField( "GENERAL CHANGES", itemHeaderStyle );
		EditorGUILayout.LabelField( "  • We have updated the Folder Structure yet again to help conform to the new way of doing things for the Unity Asset Store All files have been moved into a base folder named 'Ultimate Button'.", paragraphStyle );
		EditorGUILayout.LabelField( "  • The way to create an Ultimate Button has been changed because of the new folder structure. Please see the How To section to learn more about how to add an Ultimate Button to your scene now.", paragraphStyle );
		EditorGUILayout.LabelField( "  • The Ultimate Button Editor has been simplified to help make it easier to use and understand.", paragraphStyle );
		EditorGUILayout.LabelField( "  • The Ultimate Button Documentation Window has been slightly changed to be easier to use.", paragraphStyle );
		EditorGUILayout.LabelField( "  • The Ultimate Button Documentation Window now has a Settings page where you can change the font size to your preference.", paragraphStyle );
		
		GUILayout.Space( 10 );

		EditorGUILayout.LabelField( "NEW FUNCTIONS", itemHeaderStyle );
		EditorGUILayout.LabelField( "  • DisableButton()", paragraphStyle );
		EditorGUILayout.LabelField( "  • EnableButton()", paragraphStyle );

		GUILayout.Space( 15 );

		EditorGUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if( GUILayout.Button( "Got it!", buttonSize ) )
			NavigateBack();

		var rect = GUILayoutUtility.GetLastRect();
		EditorGUIUtility.AddCursorRect( rect, MouseCursor.Link );

		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();

		EndPage();
	}

	void WindowSettings ()
	{
		StartPage( settings );

		EditorGUI.BeginChangeCheck();
		fontSize = ( FontSize )EditorGUILayout.EnumPopup( "Font Size", fontSize );
		if( EditorGUI.EndChangeCheck() )
		{
			switch( fontSize )
			{
				case FontSize.Small:
				default:
				{
					GUI.skin.textArea.fontSize = 11;
					paragraphStyle.fontSize = 11;
					itemHeaderStyle.fontSize = 11;
					sectionHeaderStyle.fontSize = 14;
				}
				break;
				case FontSize.Medium:
				{
					GUI.skin.textArea.fontSize = 12;
					paragraphStyle.fontSize = 12;
					itemHeaderStyle.fontSize = 12;
					sectionHeaderStyle.fontSize = 16;
				}
				break;
				case FontSize.Large:
				{
					GUI.skin.textArea.fontSize = 14;
					paragraphStyle.fontSize = 14;
					itemHeaderStyle.fontSize = 14;
					sectionHeaderStyle.fontSize = 18;
				}
				break;
			}
		}

		GUILayout.Space( 20 );
		
		EditorGUILayout.LabelField( "Example Font Size", sectionHeaderStyle );
		EditorGUILayout.LabelField( "Updated Font Size", itemHeaderStyle );
		EditorGUILayout.LabelField( "This is an example paragraph to see the size of the text after modification.", paragraphStyle );

		EndPage();
	}

	void StartPage ( PageInformation pageInfo )
	{
		pageInfo.scrollPosition = EditorGUILayout.BeginScrollView( pageInfo.scrollPosition, false, false, docSize );
		GUILayout.Space( 15 );
	}

	void EndPage ()
	{
		EditorGUILayout.EndScrollView();
	}

	void ShowDocumentation ( DocumentationInfo info )
	{
		GUILayout.Space( paragraphSpace );

		EditorGUILayout.LabelField( info.functionName, itemHeaderStyle );
		var rect = GUILayoutUtility.GetLastRect();
		EditorGUIUtility.AddCursorRect( rect, MouseCursor.Link );
		if( Event.current.type == EventType.MouseDown && rect.Contains( Event.current.mousePosition ) && ( info.showMore.faded == 0.0f || info.showMore.faded == 1.0f ) )
		{
			info.showMore.target = !info.showMore.target;
		}

		if( EditorGUILayout.BeginFadeGroup( info.showMore.faded ) )
		{
			if( info.parameter != null )
			{
				for( int i = 0; i < info.parameter.Length; i++ )
					EditorGUILayout.LabelField( Indent + "<i>Parameter:</i> " + info.parameter[ i ], paragraphStyle );
			}
			if( info.returnType != string.Empty )
				EditorGUILayout.LabelField( Indent + "<i>Return type:</i> " + info.returnType, paragraphStyle );

			EditorGUILayout.LabelField( Indent + "<i>Description:</i> " + info.description, paragraphStyle );

			if( info.codeExample != string.Empty )
				EditorGUILayout.TextArea( info.codeExample, GUI.skin.textArea );

			GUILayout.Space( paragraphSpace );
		}
		EditorGUILayout.EndFadeGroup();
	}

	string Indent
	{
		get
		{
			return "    ";
		}
	}

	[InitializeOnLoad]
	class UltimateButtonInitialLoad
	{
		static UltimateButtonInitialLoad ()
		{
			// If the user has a older version of Ultimate Button that used the bool for startup...
			if( EditorPrefs.HasKey( "UltimateButtonStartup" ) && !EditorPrefs.HasKey( "UltimateButtonVersion" ) )
			{
				// Set the new pref to 0 so that the pref will exist and the version changes will be shown.
				EditorPrefs.SetInt( "UltimateButtonVersion", 0 );
			}

			// If this is the first time that the user has downloaded the Ultimate Button...
			if( !EditorPrefs.HasKey( "UltimateButtonVersion" ) )
			{
				// Set the current menu to the thank you page.
				NavigateForward( thankYou );

				// Set the version to current so they won't see these version changes.
				EditorPrefs.SetInt( "UltimateButtonVersion", importantChanges );

				EditorApplication.update += WaitForCompile;
			}
			else if( EditorPrefs.GetInt( "UltimateButtonVersion" ) < importantChanges )
			{
				// Set the current menu to the version changes page.
				NavigateForward( versionChanges );

				// Set the version to current so they won't see this page again.
				EditorPrefs.SetInt( "UltimateButtonVersion", importantChanges );

				EditorApplication.update += WaitForCompile;
			}
		}

		static void WaitForCompile ()
		{
			if( EditorApplication.isCompiling )
				return;

			EditorApplication.update -= WaitForCompile;
			
			InitializeWindow();
		}
	}
}