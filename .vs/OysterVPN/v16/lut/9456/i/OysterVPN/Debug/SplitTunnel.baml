   M S B A M L   `   `   `  ????D  @OysterVPN, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:OysterVPN	OysterVPN  5   	       OysterVPN.SplitTunnel   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   )localclr-namespace:OysterVPN     5   	   $??Poppins)?5   	     ? Title$  Split Tunnel??5	   	   $??365.333??6   $	??310??6/    ? WindowStartupLocation$ CenterScreen=?6;    ? 
ResizeMode$ NoResize=?6`    ? ShowInTaskbar .   ? Topmost .  ? 	Resources 5         MyButton ?? 
TargetType ?? Property ?? Value	 ?? 
TargetType
 ?? Triggers P? Property P? Value??%?  &	        ?? 5   
   " ??6!   .?5      ??  ?@?OverridesDefaultStyle . 66   ?? 5       ?@?Cursor$
 Hand??6'   ?? 5       ? ?? 5      ?? 5      "	 ??6&   .R?5      ?? $??border??6!   $??0q?6/   
???   ?#??z??6W   ._?5      ?? $??Center=?6/   $??Center=?6L   5      
 5      P? 5       ?@J?IsMouseOver . . ?5   "   ??  ?@J?Opacity$	 0.8Q?6<   5      5      5      5      5      .??5       ? .+?5$   
   ?? $%??When Connected To Oyster VPN : ??6   $
??Left=?6:   $??	20,22,0,0q?6U   $	??Top=?6h   $	??267??6?   ?? 5%   
   -   5        $??	SitesList??5%   ;    ?? TextWrapping$
 Wrap=?6    ?? AcceptsReturn . $
??Left=?6L   $	??168??6g   $??
23,110,0,0q?6t   $	??Top=?6?   $	??250??6?   ?? 5'   
   -   5         ??Name$ btnSave??5'   4   #??[  6   $	??150??6E   $??Center=?6Q   $??Bottom=?6n   $??
79,0,65,10q?6?   $
??Save??6?   $??28??6?   $??Poppins)?6?   ???	#FF01407D ?? 	ClickMode$ Press=?6?   
?????????6?   Z?  J? Effect 5(      3  -System.Windows.Media.Effects.DropShadowEffect  5)         	Direction$
 -112Q?6#      ShadowDepth$ 3Q?64      Color$ 	#FFDADADA??6D      RenderingBias$ Quality=?6V   5+      	? 5-   
   -   5        $??AllAppsUseVpn??5-   (    	? 	GroupName$ radio??6   $??All Apps Use The Vpn??6=   $
??Left=?6\   $??	23,53,0,0q?6w   $	??Top=?6?   $??9(?6?   	? 5.   
   -   5        $??
DontUseVpn??5.   (   $ radio??6   $,??&Do not Allow Selected Sites To Use Vpn??6:   $
??Left=?6k   $??	23,72,0,0q?6?   $	??Top=?6?   $??9(?6?   	? 5/   
   -   5        $??UseVpn??5/   (   $ radio??6   $)??#Only Allow Selected Apps To Use Vpn??67   $
??Left=?6e   $??	23,91,0,0q?6?   $	??Top=?6?   $??9(?6?   51      52      