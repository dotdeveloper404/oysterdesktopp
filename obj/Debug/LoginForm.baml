   M S B A M L   `   `   `  ????D  @OysterVPN, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:OysterVPN	OysterVPN  5	   
       OysterVPN.LoginForm   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   >d2http://schemas.microsoft.com/expression/blend/2008    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   b ^SharpVectors.Converters.Wpf, Version=1.7.6.0, Culture=neutral, PublicKeyToken=b532964b8548be771svgc&http://sharpvectors.codeplex.com/svgc/  5   	   Z VFontAwesome.WPF, Version=4.7.0.37774, Culture=neutral, PublicKeyToken=0758b07a11a4f466-fa$http://schemas.fontawesome.io/icons/  5   	   %WpfTestclr-namespace:OysterVPN  5	   
   $	??500??5
   	   $	??880??6     ? WindowStartupLocation$  CenterScreen=?5   	    ? 
ResizeMode$ CanMinimize=?6.    ? Topmost .  ? 	Resources 5       ?? 
TargetType"   OysterVPN.PasswordBoxMonitor ?? Property ?? Value ?? 
TargetType J? SnapsToDevicePixels	 ?? CornerRadius
 ??Name ?? Triggers P? Property P? Value ?? 
TargetName   MyWaterMarkStyle ??Name
 ? Tag ]? 
Conditions ?? Property ?? Value  MyButton  passwordbox ??Name??%?	  'G?       &	      &	 ?    &	 p    ?? 5   
   " G?5   	   .?5      ??  ?@ IsMonitoring . 5      ?? 5       ? ?? 5      ?? 5      " G?6&   .R?5      ?? $??Bd??6!   #??z??5      #??z??5      #??z??5       . $	 20,20,20,20??5   0   ._?5      ? .+?5   "   ?? $
 PART_ContentHost??6/   # z 5      ?? 5   "   $??	txtPrompt??5#      $??Enter your password??5   ,   $??11,8,0,0q?5       
???????$y?	Collapsed=?5"      5#      5$      5%       5&      P? 5'       ? {? .  . ?5)   "   ?? $ Bd??6)   # ? ??5+   -    ? ???? 5,   "   # ? ??5-   -    ? ??5.      P? 5/       ?@ PasswordLength$ 0??6[   . ?50   "   ?? $ 	txtPrompt??6?    ? y?$ Visible=?6V   51      53      55      56      ?? 58   
   " ??6)   .?59      ??  ? ?? 5:      ?? 5;      " ??6&   .R?5<      ? .+?5=      ?? 
??????????	#FFEAEAEA$??1q?6P   ?? 5>      $
 PART_ContentHost??6+   $??5,0,0,0q?6E   $??Center=?6V   ?? 5?      $ WaterMarkLabel??66   $??15,0,0,0q?6$   #??z 6N   $??Center=?6n   $y?	Collapsed=?5@      
???????$??Poppins)?6E   5A       5B      ]? 5C       5D   "   ?? 5E   &    ? ??$  ??6@   ./?5G   "   ?? $ WaterMarkLabel??6?    ? y?$ Visible=?6[   5H      P? 5I       ? {? .  . ?5J   "   ??  ? ?? ?	#FFEAEAEA6?   5K      5M      5O      5P      ?? 5R   
   " ??6!   .?5S      ??  ?@?OverridesDefaultStyle . 66   ?? 5T       ?@?Cursor$
 Hand??6'   ?? 5U       ? ?? 5W      ?? 5X      " ??6&   .R?5Z      ?? $??border??6!   $??0q?6/   
???   ?#??z??6W   ._?5[      ?? $??Center=?6/   $??Center=?6L   5\       5]      P? 5^       ?@J?IsMouseOver . . ?5_   "   ??  ? ?? ?	#FF00B9396?   5`      5c      5f      5g      ?? 5i   
   " G?6.   .R?5j      ?? $ Bd??5k   	   #??z??5j      ???	#FFE6DDDD#??z??5l   	   $	 20??6=   ._?5m      ?? $??PART_ContentHost??6   # z 67   5n       5o      P? 5p       ? {? .  . ?5q      ?? # ? ??63   $ Bd??6u    ? ???? 5r      # ? ??63    ? ??5s      P? 5t       ? ??$
 Auto??6+   . ?5u      ??  ? ??$	 100??61   5v      P? 5w       ? ??$
 Auto??6,   . ?5x      ??  ? ??$ 20??62   5y      5{      5      .??5?      ? .+?5?   
   (  "SharpVectors.Converters.SvgViewbox     OptimizePath .     TextAsGeometry .    Source$ oysergreen.svg??6F   $??Center=?6_   $??Center=?6|   $	??145??6?   $??584,51,132,366q?6?   $??34??6?   ?? 5?   
   -   5         ??Name$ Slid1??5?   -   $y?Visible=?6   $??
10,10,14,0q?6<   $??2??6P   .+?5?      ? $	??460??6   .+?5?      ?? -   5        $??txtEmailAddress??5?   ^   #??[  5?      $ info@example.com??5?      $??12,0,0,0q?6(   $??Center=?6;   $
??Left=?6u   $??42??6?   $??517,117,0,0q?6?    ?? TextWrapping$
 Wrap=?6?   $	??Top=?6?   $	??297??6?   $??Poppins)?6?   ???	#FFEAEAEA$	??0.1q?5?   4   .??6L    5?      ??%?   '??       '?? U     ?? 5?      " ??6    .?5?      ??  ?@??CornerRadius$ 20??6=   5?      ?? 5?      " ??6    5?      5?      .??6)   .??5?   )   5?      G? 5?      -   5         G?Name$ txtPassword??5?   h   $??12,0,0,0q?6   $??Center=?62    G? PasswordChar$ *??6U   $
??Left=?6}   $??42??6?   $??517,175,0,0q?6?   $	??Top=?6?   $	??297??6?   $??Poppins)?6?   ???	#FFD4D7DA$??1q?6  
?????????5?      ?? 5?      " G?5?   	   .?5?      ??  ?@ IsMonitoring . 5?      ?? 5?       ? ?? 5?   "   ?? 5?   &   " G?66   .R?5?   *   ?? $??Bd??61   #??z??5?      #??z??5?      #??z??5?       . $	 20,20,20,20??5?   0   ._?5?   .   ? .+?5?   2   ?? $
 PART_ContentHost??6?   # z 5?      ?? 5?   2   $??	txtPrompt??5?      $??Enter your password??5?   <   $??17,8,0,0q?5?      
???????$y?	Collapsed=?5?      5?      5?   /   5?   +    5?   *   P? 5?   .    ? {? .  . ?5?   2   ?? $ Bd??69   # ? ??5?   -    ? ???? 5?   2   # ? ??5?   -    ? ??5?   /   P? 5?   .    ?@ PasswordLength$ 0??6k   . ?5?   2   ?? $ 	txtPrompt??6O    ? y?$ Visible=?6f   5?   /   5?   '   5?      5?      5?      ?? 5?      -   5         ?? 	IsDefault . $??0q?5?   +   $??Login??6?   $??Poppins)?6P   
???????$??528,322,36,102q?6x    J? RenderTransformOrigin$ 0.5,0.5&?6?    5?      ??%_   '??       ?? 5?      " ??6    .?5?      ??  ?@??CornerRadius$ 20??6=   5?      5?      ??5?      ?? 5?       ?? EndPoint$ 0.1,0&?6.    ?? 
StartPoint$ 1.4,0&?6?   .??5?      ?   ? Color$  	#FF00B939??6+   ! ? Offset$	! 1.0Q?6=   ? 5?      $  	#FF005CB3??6+   $	! 0.2Q?6=   5      ??5     ?? 5     " ?? ResourceKey$" MyButton??6)   5     "  FontAwesome.WPF.ImageAwesome  5     -   5        $??loader??5  L   #   Icon$# Refresh=?6"   $y?	Collapsed=?61   $   Spin$ . $??18??6f   $??18??6r   $??640,322,150,30q?6}   ?? 5     -   5        % ??Name$% 	btnForgot??5     #??[ 6,   $??Forgot Your Password???6O   $
??Left=?6o   $??660,268,0,0q?6?   $	??Top=?5     $	??147??61   
?????? 
?????? $??21??6q   $??Poppins)?6}   ???	#FF0065C4$ 
0.887,0.46&?6?   6?   ?? 5     	& ?? X2$& 0??6   	' ?? Y2$	' 600??6   ???	#FFDAD8D8$??4??6?   $??424,-8,422,1q?6S   ? 5      $
??Left=?6   $	??440??62   $??	10,10,0,0q?6?   $	??Top=?6R   $	??396??6j   .+?5!     ?? -   5        $??Slide1??5!  !   $??23,21,30,57q?6/   .+?5"     ?? $	??264??6    $??/slider_1@1x.png??6-   $
??None=?6G   ?? 5#     $
??Wrap=?6$   $??42??68   $??0,0,26,0q?6D   $8??2/OysterVPN;component/assets/fonts/Poppins/#Poppins)?6V   $??Center=?6?   $W??Q1Go anonymous to secure your privacy and to  protecting your precious data online??6?   $??Right=?6
  $	??270??6&  5$     ?? 5%     -   5        $??Slide2??5%  5   $y?Hidden=?6!   $??23,21,30,57q?6C   .+?5&     ?? $	??264??6    $??/slider_2@1x.png??6-   $
??None=?6G   ?? 5'     $
??Wrap=?6$   $??Center=?68   $??40??6U   $??	31,0,32,0q?6a   $	??280??6t   $8??2/OysterVPN;component/assets/fonts/Poppins/#Poppins)?6?   $??Center=?6?   $W??Q2Go anonymous to secure your privacy and to  protecting your precious data online??6?   5(     ?? 5)     -	   5        $??Slide3??5)  5   $y?Hidden=?6!   $??23,21,30,57q?6C   .+?5*     ?? $	??264??6    $??/slider_3@1x.png??6-   $
??None=?6G   ?? 5+     $
??Wrap=?6$   $??Center=?68   $??40??6U   $??	31,0,32,0q?6a   $	??280??6t   $8??2/OysterVPN;component/assets/fonts/Poppins/#Poppins)?6?   $??Center=?6?   $W??Q3Go anonymous to secure your privacy and to  protecting your precious data online??6?   5-     ? 50     $??
0,371,0,10q?6   .+?51     ?? $??26??6%   $??91,25,146,8q?61   $??Center=?6F   $??Center=?6c   $??Dont have an account???6~   
???????
???   ?$8??2/OysterVPN;component/assets/fonts/Poppins/#Poppins)?6?   $	??159??6  ?? 52     -
   5        $% 	btnSignup??52  C   #??[ 6!   $??Sign Up??6V   $
??Left=?6h   $??236,23,6,10q?6?   $	??Top=?6?   $??50??6?   ???	#FFFAFAFA???	#FFFBFBFB$??21??6?   $??Poppins)?6?   ???	#FF0065C4$ 
0.887,0.46&?6;  $
??Bold"?6^  54     ?? 57     -   5        $% btnNext??57     #??[ 6.   $
??Left=?6P   $??372,202,0,0q?6k   $	??Top=?6?   $??14??6?   $??16??6?   
?????????58     ?? 59     ( ?? ImageSource$( rightarrow.png??6)   ) j? Stretch$) Uniform=?6F   5;     ?? 5<     -   5        $% btnBack??5<     #??[ 6/   $
??Left=?6Q   $??	0,202,0,0q?6l   $	??Top=?6   $??14??6?   $??16??6?   
?????????5=     ?? 5>     $( leftarrow.png??6)   $) Uniform=?6E   5@     5B     ?? 5C     -   5        $??txtValidation??5C     $
??Left=?62   $??528,238,0,0q?6M   $
??Wrap=?6b   $	??Top=?6x   $	??286??6?   ???	#FFDE1A1A6?   5I     5L     5R     5V     