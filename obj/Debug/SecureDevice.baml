   M S B A M L   `   `   `  ????D  @OysterVPN, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:OysterVPN	OysterVPN  5   	       OysterVPN.SecureDevice   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   )localclr-namespace:OysterVPN     5   	     ? UseLayoutRounding  .  ?? BitmapScalingMode$ HighQuality=?5       ? Icon$ vpngreen.ico??5	   
    ? Title$ Secure All Your Devices??5   	   $	??600??5      $	??380??6   $??Poppins)?5   	   $y?Visible=?6    ? 
ResizeMode$ CanMinimize=?63    ? 	Resources 5         MyButton ?? 
TargetType ?? Property ?? Value	 ?? 
TargetType
 ?? Triggers P? Property P? Value??%?  &	        ?? 5   
   " ??6!   .?5      ??  ?@?OverridesDefaultStyle . 66   ?? 5       ?@?Cursor$
 Hand??6'   ?? 5       ? ?? 5      ?? 5      "	 ??6&   .R?5      ?? $??border??6!   $??0q?6/   
???   ?#??z??6W   ._?5      ?? $??Center=?6/   $??Center=?6L   5      
 5      P? 5       ?@J?IsMouseOver . . ?5   "   ??  ?@J?Opacity$	 0.8Q?6<   5      5!      5#      5$      5(      .??5*      ? .+?5-   
   ?? $ HighQuality=?6   $??28,136,44,292q?6@   $??/Secure2x.png??6X   ?? 5/   
   $??15(?6   $??WrapWithOverflow=?6"    ?? TextAlignment$ Center=?6B   $>??8Your subscription includes access to all OysterVPN apps!??6[   $??
32,323,0,0q?6?   $??55??6?   $	??311??6?   $	??Top=?6?   $
??Left=?6?   ?? 51   
   -   5         ??Name$ btnSetup??51   B   $??2??6   #??[  6    $??Get More Apps??6T   $??Center=?6l   $	??Top=?6?   $	??330??6?   $??34??6?   ???	#FF009A62
???????$??Poppins)?6?   $??	0,482,0,0q?6?    52      ??%^   '??       ?? 53      " ??6   .?54      ??  ?@??CornerRadius$ 8??65   55      56      58      5:      5=      