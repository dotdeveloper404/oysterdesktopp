   M S B A M L   `   `   `  ????D  @OysterVPN, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:OysterVPN	OysterVPN  5   	       OysterVPN.ForgotPassword   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   )localclr-namespace:OysterVPN     5   	   b ^MaterialDesignThemes.Wpf, Version=4.0.0.2422, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5   	     ? Title$  Forgot Password??5	   	   $	??615??6!   $	??380??6.   $??Poppins)?5   	   $y?Visible=?6    ? 
ResizeMode$ CanMinimize=?63    ? WindowStyle$ 
ToolWindow=?6L    ? ShowInTaskbar .  ? Icon$ assets/VPN-Icon-Green.png??6z   ??5      ?? 5   
    ?? ImageSource$! assets/white_background.jpg??6    ? 	Resources 5         MyButton ?? 
TargetType ?? Property	 ?? Value
 ?? 
TargetType ?? Triggers P? Property P? Value??%?  &	        ?? 5   
   " ??6!   .?5      ??  ?@?OverridesDefaultStyle	 . 66   ?? 5       ?@?Cursor$
	 Hand??6'   ?? 5       ? ??	 5      ?? 5      "
 ??6&   .R?5      ?? $??border??6!   $??0q?6/   
???   ?#??z??6W   ._?5      ?? $??Center=?6/   $??Center=?6L   5       5      P? 5       ?@J?IsMouseOver . . ?5   "   ??  ?@J?Opacity$		 0.8Q?6<   5      5       5"      5#      5$      .??5&      ? .+?5(   
   ? $??	0,0,0,481q?6   .+?5)   
   ?? $??Center=?6   $??61??6-   $	??178??69   $??0q?6F   $??Center=?6Q   $??assets/logo-dark.png??6n    J? RenderTransformOrigin$ 0.419,0.536&?6?   5+      ? 5.   
   $??-4,106,0,267q?6   .+?50      ?? -   5        $??txtEmailAddress??50   L   $??12,0,0,0q?6   $??Center=?6)   $
??Left=?6c   $??43??6~   $??	39,38,0,0q?6?    ?? TextWrapping$
 Wrap=?6?   $	??Top=?6?   $	??301??6?   $??Poppins)?6?   ???	#FFD4D7DA$??1q?6  .??6A   52      ??%^   '??       ?? 53      " ??6   .?54      ??  ?@??CornerRadius$	 8??69   55      56      .??6%   58      ?? 5E      $
??Left=?6   $??1??63   $??	52,38,0,0q?6>   $	??Top=?6Q   $??95??6i   
???????5F      ?? 5G      $
??Left=?6   $??	59,29,0,0q?63   $
??Wrap=?6F   $??Email Address??6Z   $	??Top=?6o   $	??109??6?   $??16??6?   ?? 5H      $??*??6   $
??Left=?6    $??
138,24,0,0q?6;   $	??Top=?6O   ???	#FF00B939$??21??6~   $??16??6?   $ 0.792,0.452&?6?   6?   ? 5J      -   5        $??ForgetLoadingGrid??5J      $
??Left=?6.   $??35??6I   $??138,169,0,0q?6U   $	??Top=?6j   $??86??6?   .+?5L      (? -   5        $??LoginLoadingBar??5L      $y?Hidden=?61   $	??200??6F   $	??200??6R   $??Center=?5M      $??Center=?5N      $??2??6,   $??-56,-82,-57,-83q?6@   .??5O      #   OysterVPN.CircularProgressBar  $	??301??60   $	??728??6=   5P      5R      ?? 5U      -   5         ??Name$ 	btnSubmit??5U   7   #??[  6   $??Submit??6J   $
??Left=?6[   $	??Top=?6w   $	??307??6?   $??31??6?   ???	#FF0065C4
???????$??Poppins)?6?   $??
33,126,0,0q?6?    5V      ??%^   '??       ?? 5W      " ??6   .?5X      ??  ?@??CornerRadius$	 8??69   5Y      5Z      5\      ?? 5^      -   5        $??txtValidation??5^      $
??Left=?6.   $
??Wrap=?6J   $	??Top=?6^   $	??330??6v    ?? TextAlignment$
 Left=?6?   ???	#FFFF0101$??	39,95,0,0q?6?   5`      5e      5f      