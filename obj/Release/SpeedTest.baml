   M S B A M L   `   `   `  ???? D  @OysterVPN, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:OysterVPN	OysterVPN      OysterVPN.SpeedTest   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     )localclr-namespace:OysterVPN       ? Title$  	SpeedTest??$	??650??$	??850?? ? Icon$ assets/VPN-Icon-Green.png?? ? Topmost .  ? 
ResizeMode$ CanMinimize=? ? WindowState$ Normal=? ? WindowStartupLocation$ CenterScreen=?????  ?? ImageSource$! assets/white_background.jpg?? ? 	Resources    MyButton ?? 
TargetType	 ?? Property
 ?? Value ?? 
TargetType ?? Triggers P? Property P? Value  CustomHeader
 ? Tag ?? Triggers??%  &	        &	     '?? _    ?? " ??.??? 	 ?@?OverridesDefaultStyle
 . ?? 	 ?@?Cursor$

 Hand???? 	 ? ??
 ?? " ??.R??? $??border??$??0q?
???   ?#??z??._??? $??Center=?$??Center=? P?  ?@J?IsMouseOver . . ??? 	 ?@J?Opacity$	
 0.8Q??? " ??.R?? #??z??$??35??.+??? #??z??#??z??$
??10,0q?#??z $??Center=??? " ??.??? 	 ? ??

 ?   ??? 	 ? ??
 ?#EFEFEF?? 	 ? ??$
 Normal"? P?  ?@J?IsMouseOver . . ??? 	 ? ??$

 Bold"?.???  ? ColumnDefinitions ?? $??7* ??? $
??162* ?.+??? $??Poppins)?$f??`The speed index ranks download speed (higher is better) and conneciton latency (lower is better)??$??10,16,10,481q?$??2???? -   $??lvSpeedTest??$??	0,60,0,75q?$??Poppins)?$??2?? ?? View ?? .P???  ?? Width$	 400?? ?? DisplayMemberBinding ?? *Name+???? -   #??[ ???#01407D$??35??
???????$??13(?$
 Name??.??Name?? $	 130?? ?? *
SpeedIndex+???? -   #??[ ???#01407D$??35??
???????$??13(?$ Speed Index??.??Speed Index?? $	 110?? ?? *	Latency+???? -   #??[ ???#01407D$??35??
???????$??13(?$ Latency??.??	Latency?? $	 185?? ?? *DownloadSpeed+???? -   #??[ ???#01407D$??35??
???????$??13(?$ Download Speed??.??Download Speed???? " ??.??? 	 ? ??

 ??????? 	 ? ??$
 0,0,0,1q??? 	 ? ??$	
 0,8q??? -    ??Name$ 
btnRunTest??#??[  $??Run Test??$
??Left=?$	??Top=?$	??137??$??36?????	#FF0065C4
???????$??Poppins)?$??633.5,550,0,0q?$??1?? ??%9   '??       ?? " ??.??? 	 ?@??CornerRadius$
 8??