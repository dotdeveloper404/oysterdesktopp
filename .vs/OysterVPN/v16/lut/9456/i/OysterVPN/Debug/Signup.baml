   M S B A M L   `   `   `  ????D  @OysterVPN, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:OysterVPN	OysterVPN  5   	       OysterVPN.Signup   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   )localclr-namespace:OysterVPN     5   	   b ^SharpVectors.Converters.Wpf, Version=1.7.6.0, Culture=neutral, PublicKeyToken=b532964b8548be772svgc1&http://sharpvectors.codeplex.com/svgc/  5      b ^MaterialDesignThemes.Wpf, Version=4.0.0.2422, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5	   	     ? Title$  Sign Up??5
   	   $	??615??6   $	??380??6*    ? WindowStartupLocation$ CenterScreen=?5   	    ? Icon$ assets/VPN-Icon-Green.png??5      $??Poppins)?5   	   $y?Visible=?6    ? 
ResizeMode$ CanMinimize=?63   ??5      ?? 5   
    ?? ImageSource$! assets/white_background.jpg??6    ? 	Resources 5         MyButton ?? 
TargetType ?? Property ?? Value	 ?? 
TargetType
 ?? Triggers P? Property P? Value  passwordbox ??Name ?? CornerRadius J? SnapsToDevicePixels ?? 
TargetName??%?  &	        &	 ?    ?? 5   
   " ??6!   .?5      ??  ?@?OverridesDefaultStyle . 66   ?? 5       ?@?Cursor$
 Hand??6'   ?? 5       ? ?? 5      ?? 5      "	 ??6&   .R?5      ?? $??border??6!   $??0q?6/   
???   ?#??z??6W   ._?5      ?? $??Center=?6/   $??Center=?6L   5      
 5      P? 5       ?@J?IsMouseOver . . ?5   "   ??  ?@J?Opacity$	 0.8Q?6<   5       5"      5$      5%      ?? 5'   
   "	 G?6.   .R?5(      ?? $ Bd??5)   	   #??z??5(      ???	#FFE6DDDD#??z??5*   	   $ 8??6=   ._?5+      ?? $??PART_ContentHost??6   # z 67   5,      
 5-      P? 5.       ? {? .  . ?5/      ?? # ? ??63   $ Bd??6u    ? ???? 50      # ? ??63    ? ??51      P? 52       ? ??$
 Auto??6+   . ?53      ??  ? ??$	 100??61   54      P? 55       ? ??$
 Auto??6,   . ?56      ??  ? ??$ 20??62   57      59      5;      .??5=      ? .+?5@   
   (  "SharpVectors.Converters.SvgViewbox     OptimizePath .     TextAsGeometry .    Source$ oysergreen.svg??6G   $??114,10,120,532q?6_   $??Center=?6w   $??Center=?6?   ? 5B   
   $??8,127,-8,392q?6   .+?5E      ?? -   5        $??txtEmailAddress??5E   L   $??12,0,0,0q?6   $??Center=?6)   $
??Left=?6c   $??42??6~   $??	27,14,0,0q?6?    ?? TextWrapping$
 Wrap=?6?   $	??Top=?6?   $	??297??6?   $??Poppins)?6?   ???	#FFD4D7DA$??1q?6  .??6C   5F      ??%^   '??       ?? 5G      " ??6   .?5H      ??  ?@??CornerRadius$ 8??69   5I      5J      .??6%   5K      ?? 5Z      $
??Left=?6   $??1??63   $??	37,14,0,0q?6>   $	??Top=?6Q   $??42??6i   
???????5[      ?? 5\      $
??Left=?6   $??39,4,0,0q?63   $
??Wrap=?6E   $??Email Address??6Y   $	??Top=?6n   $??83??6?   $??16??6?   ?? 5]      $??*??6   $
??Left=?6    $??	66,-3,0,0q?6;   $	??Top=?6N   ???	#FF00B939$??21??6}   $??16??6?    J? RenderTransformOrigin$ 0.5,0.5&?6?   6?   5_      ? 5b   
   $??13,199,-13,313q?6   .+?5c      G? -   5         G?Name$ txtPassword??5c   ?   $??12,0,0,0q?6   #??[ 6.   $??Center=?6V    G? PasswordChar$ *??6y   $
??Left=?6?   $??42??6?   $??	23,12,0,0q?6?   $	??Top=?6?   $	??297??6?   $??Poppins)?6   ???	#FFD4D7DA$??1q?6-  
??????? 5d      ??%^   '??       ?? 5e      " ??6   .?5f      ??  ?@??CornerRadius$ 8??69   5g      5h      5i      ?? 5k      $
??Left=?6   $??2??63   $??	34,12,0,0q?6>   $	??Top=?6Q   $??62??6i   
???????6t   ?? 5l      $
??Left=?6   $??36,4,0,0q?63   $
??Wrap=?6E   $??Password??6Y   $	??Top=?6i   $??83??6?   $??16??6?   ?? 5m      $??*??6   $
??Left=?6    $??	88,-1,0,0q?6;   $	??Top=?6N   ???	#FF00B939$??21??6}   $??16??6?   $ 0.5,0.5&?6?   5s      ? 5u   
   $??10,273,-10,242q?6    ? ColumnDefinitions 5v      ?? 5w      $??8* ?6#   ?? 5x      $	??41* ?6#   ?? 5y      $
??138* ?6#   .+?5{      G? -   5        $ txtRePassword??5{   ?   $??12,0,0,0q?6   #??[ 6.   $??Center=?6V   $ *??6y   $
??Left=?6?   $??42??6?   $??	10,14,0,0q?6?   $	??Top=?6?   $	??297??6?   $??Poppins)?6  ???	#FFD4D7DA$??1q?6/  
???????$??2??6Y  $??1??6m  ?? 5}      $
??Left=?6   $??2??63   $??	17,14,0,0q?6>   $	??Top=?6Q   $	??108??6i   
???????$??2??6?   $??1??6?   ?? 5~      $
??Left=?6   $??20,4,0,0q?63   $
??Wrap=?6E   $??Re-enter password??6Y   $	??Top=?6r   $	??154??6?   $??16??6?   $??2??6?   $??1??6?   ?? 5      $??*??6   $
??Left=?6    $??	49,-2,0,0q?6;   $	??Top=?6N   ???	#FF00B939$??21??6}   $??16??6?   $ 0.5,0.5&?6?   $??2??6?   5?      ? 5?   
   $??0,349,0,171q?6   .+?5?      ?? -   5         ??Name$ checkBoxTerms??5?      $??
43,0,40,26q?6.   ??5?      ?? 5?      ??6    ?? *
TermsUrl+ ?? TextDecorations 6A   Z? .?6\   Terms and Conditions5?      5?      ?? 5?      -   5         ??Name$ txtValidate??5?       ?? TextAlignment$ Center=?60   ???	#FFDA2E2E$
??Bold"?6^   $??WrapWithOverflow=?6p   $??	-5,31,0,0q?6?   $	??Top=?6?   $
??Auto??6?    ? Cursor$
 Hand??6?   $??Poppins)?6?   $??35??6?   $??Center=?6?   5?      5?      ?? 5?   
   -   5         ??Name$ 	btnSignup??5?   3   #??[  6   $??Sign Up??6F   $
??Left=?6X   $	??Top=?6t   $	??324??6?   $??34??6?   ???	#FF0065C4
???????$??Poppins)?6?   $??
22,420,0,0q?6?    5?      ??%^   '??       ?? 5?      " ??6   .?5?      ??  ?@??CornerRadius$ 8??65   5?      5?      5?      ? 5?   
   -   5        $??SignupLoadingGrid??5?      $
??Left=?6*   $??35??6E   $??137,424,0,0q?6Q   $	??Top=?6f   $??86??6~   .+?5?      (? -   5        $??SignupLoadingBar??5?      $y?Hidden=?6.   $	??200??6C   $	??200??6O   $??Center=?5?      $??Center=?5?      $??2??6,   $??-56,-82,-57,-83q?6@   .??5?      #   OysterVPN.CircularProgressBar  $	??301??6,   $	??728??69   5?      5?      ? 5?   
   $??
0,516,0,10q?6   .+?5?      ?? -	   5        $??BackToLogin??5?   7   #??[  6   $??Back to Login??6J   $??Center=?6b   $	??152??6   $??28??6?   $??Poppins)?6?   ???	#FF00B939$??0q?6?   $
??Bold"?6  ??6?   Z? ??6?   Z? 6  5?      5?      5?      