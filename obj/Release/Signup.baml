   M S B A M L   `   `   `  ???? D  @OysterVPN, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:OysterVPN	OysterVPN      OysterVPN.Signup   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     )localclr-namespace:OysterVPN     b ^SharpVectors.Converters.Wpf, Version=1.7.6.0, Culture=neutral, PublicKeyToken=b532964b8548be772svgc1&http://sharpvectors.codeplex.com/svgc/  b ^MaterialDesignThemes.Wpf, Version=4.0.0.2422, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes    ? Title$  Sign Up??$	??615??$	??380?? ? WindowStartupLocation$ CenterScreen=? ? Icon$ assets/VPN-Icon-Green.png??$??Poppins)?$y?Visible=? ? 
ResizeMode$ CanMinimize=?????  ?? ImageSource$! assets/white_background.jpg?? ? 	Resources    MyButton ?? 
TargetType ?? Property ?? Value	 ?? 
TargetType
 ?? Triggers P? Property P? Value  passwordbox ??Name ?? CornerRadius J? SnapsToDevicePixels ?? 
TargetName??%3  &	        &	     ?? " ??.???  ?@?OverridesDefaultStyle . ??  ?@?Cursor$
 Hand????  ? ?? ?? "	 ??.R??? $??border??$??0q?
???   ?#??z??._??? $??Center=?$??Center=?
 P?  ?@J?IsMouseOver . . ???  ?@J?Opacity$	 0.8Q??? "	 G?.R??? $ Bd??#??z?????	#FFE6DDDD#??z??$ 8??._??? $??PART_ContentHost??# z 
 P?  ? {? .  . ??? # ? ??$ Bd?? ? ???? # ? ?? ? ??P?  ? ??$
 Auto??. ???  ? ??$	 100??P?  ? ??$
 Auto??. ???  ? ??$ 20??.??? .+?(  "SharpVectors.Converters.SvgViewbox     OptimizePath .     TextAsGeometry .    Source$ oysergreen.svg??$??114,10,120,532q?$??Center=?$??Center=?? $??8,127,-8,392q?.+??? -   $??txtEmailAddress??$??12,0,0,0q?$??Center=?$
??Left=?$??42??$??	27,14,0,0q? ?? TextWrapping$
 Wrap=?$	??Top=?$	??297??$??Poppins)????	#FFD4D7DA$??1q?.?? ??%9   '??       ?? " ??.???  ?@??CornerRadius$ 8??.???? $
??Left=?$??1??$??	37,14,0,0q?$	??Top=?$??42??
????????? $
??Left=?$??39,4,0,0q?$
??Wrap=?$??Email Address??$	??Top=?$??83??$??16???? $??*??$
??Left=?$??	66,-3,0,0q?$	??Top=????	#FF00B939$??21??$??16?? J? RenderTransformOrigin$ 0.5,0.5&?? $??13,199,-13,313q?.+?G? -    G?Name$ txtPassword??$??12,0,0,0q?#??[ $??Center=? G? PasswordChar$ *??$
??Left=?$??42??$??	23,12,0,0q?$	??Top=?$	??297??$??Poppins)????	#FFD4D7DA$??1q?
??????? ??%9   '??       ?? " ??.???  ?@??CornerRadius$ 8???? $
??Left=?$??2??$??	34,12,0,0q?$	??Top=?$??62??
????????? $
??Left=?$??36,4,0,0q?$
??Wrap=?$??Password??$	??Top=?$??83??$??16???? $??*??$
??Left=?$??	88,-1,0,0q?$	??Top=????	#FF00B939$??21??$??16??$ 0.5,0.5&?? $??10,273,-10,242q? ? ColumnDefinitions ?? $??8* ??? $	??41* ??? $
??138* ?.+?G? -   $ txtRePassword??$??12,0,0,0q?#??[ $??Center=?$ *??$
??Left=?$??42??$??	10,14,0,0q?$	??Top=?$	??297??$??Poppins)????	#FFD4D7DA$??1q?
???????$??2??$??1???? $
??Left=?$??2??$??	17,14,0,0q?$	??Top=?$	??108??
???????$??2??$??1???? $
??Left=?$??20,4,0,0q?$
??Wrap=?$??Re-enter password??$	??Top=?$	??154??$??16??$??2??$??1???? $??*??$
??Left=?$??	49,-2,0,0q?$	??Top=????	#FF00B939$??21??$??16??$ 0.5,0.5&?$??2??? $??0,349,0,171q?.+??? -    ??Name$ checkBoxTerms??$??
43,0,40,26q????? ???? *
TermsUrl+ ?? TextDecorations Z? .?Terms and Conditions?? -    ??Name$ txtValidate?? ?? TextAlignment$ Center=????	#FFDA2E2E$
??Bold"?$??WrapWithOverflow=?$??	-5,31,0,0q?$	??Top=?$
??Auto?? ? Cursor$
 Hand??$??Poppins)?$??35??$??Center=??? -    ??Name$ 	btnSignup??#??[  $??Sign Up??$
??Left=?$	??Top=?$	??324??$??34?????	#FF0065C4
???????$??Poppins)?$??
22,420,0,0q? ??%9   '??       ?? " ??.???  ?@??CornerRadius$ 8??? -   $??SignupLoadingGrid??$
??Left=?$??35??$??137,424,0,0q?$	??Top=?$??86??.+?(? -   $??SignupLoadingBar??$y?Hidden=?$	??200??$	??200??$??Center=?$??Center=?$??2??$??-56,-82,-57,-83q?.??#   OysterVPN.CircularProgressBar  $	??301??$	??728??? $??
0,516,0,10q?.+??? -	   $??BackToLogin??#??[  $??Back to Login??$??Center=?$	??152??$??28??$??Poppins)????	#FF00B939$??0q?$
??Bold"???Z? ??Z? 