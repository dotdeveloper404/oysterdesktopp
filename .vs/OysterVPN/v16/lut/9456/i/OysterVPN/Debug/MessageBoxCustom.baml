   M S B A M L   `   `   `  ????D  @OysterVPN, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:OysterVPN	OysterVPN  5   	        OysterVPN.MessageBoxCustom   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   )localclr-namespace:OysterVPN     5   	   b ^MaterialDesignThemes.Wpf, Version=4.0.0.2422, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5   	     ? Title$  MessageBoxWindow??5   	   $	??200??6"   $	??300??6/    ? WindowStartupLocation$ CenterScreen=?5	   	    ? WindowStyle$
 None=?6.   
??????? ? Topmost .  ? ShowInTaskbar .     MaterialDesignBody#???   5   	   $??Regular"?5   	   $??10(?5   	   &   System.Windows.Media.TextOptions   TextFormattingMode$ Ideal=?5   	    ? 
ResizeMode$ NoResize=?5   	      TextRenderingMode$
 Auto=?5   	     MaterialDesignFont#???  5   	   $??Normal"?6;    ? 	Resources 5      	 ?? MergedDictionaries
 ?? Source?? 5   
   	 5      ?? 5      $h
 bpack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Defaults.xaml??6%   ?? 5      $t
 npack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Primary/MaterialDesignColor.Blue.xaml??6%   ?? 5      $s
 mpack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Accent/MaterialDesignColor.Lime.xaml??6%   %    5      .??5      ?  ? ColumnDefinitions 5   
   ?? 5       $	??19* ?6   ?? 5!      $
??178* ?6   ?? 5"      $
??295* ?6    ? RowDefinitions 5$   
   ?? 5%      $
??auto ?6   ?? 5&      $??* ?6   .+?5(   
   #  MaterialDesignThemes.Wpf.Card  -   5          Name$ 
cardHeader??5(      $??0??62   $??10q?6?   $??0q?6L   +  %MaterialDesignThemes.Wpf.ShadowAssist   ShadowDepth$ Depth5=?6W   ???	#FF0065C4 ! PrimaryHueDarkForegroundBrush#???  6?   $y?Visible=?6?   $??3??6?   .??5)      ?? .+?5*      ?? -   5         ??Name$ txtTitle??5*      $??Center=?6.   $??Stretch=?6K      MaterialDesignTitleTextBlock#???  6g   $??20(?6?   Message Title6?   5+      5,      ?? 5-   
   $??Right=?6   $??0,5,5,0q?62   $	??Top=?6D   $??2??6\   .+?5.      ?? -   5         ??Name$ btnClose??5.      $??35??6>   $??35??6I   
??????????	#FF0065C4 * &MaterialDesignFloatingActionDarkButton#??[ 6?   ???	#FF0065C4.??5/      '  !MaterialDesignThemes.Wpf.PackIcon     Kind$ Close=?6*   50      51      ? 52   
   $??1??6   $??3??6   .+?53      ? $??20q?6    54      ?? 55      $
??auto ?6$   ?? 56      $
??auto ?6$   ?? 57      $??* ?6$   .+?5:      ? $??0??6    5;      ?? 5<      $
??auto ?6(   ?? 5=      $??* ?6(   .+?5@      ?  9MaterialDesignThemes.Wpf.Transitions.TransitioningContent  C  =MaterialDesignThemes.Wpf.Transitions.TransitioningContentBase   OpeningEffects 5A      ;  5MaterialDesignThemes.Wpf.Transitions.TransitionEffect  5B         Kind$ FadeIn=?6>     5C      $ SlideInFromBottom=?6>   .??5E      ?? -   5         ??Name$ 
txtMessage??5E   "   $??Center=?66    ?? 
IsReadOnly . $??0??6e   $??5q?6r   )  #MaterialDesignThemes.Wpf.HintAssist   Hint$  ??6}   $??18(?6?    % !MaterialDesignFloatingHintTextBox#??[ 6?    ?? 
CaretBrush ?	#FFDCD9E06?   5F      5H      ? 5I      $??1??6   $??0,20,0,5q?6$    5J      ?? 5K      $??* ?6+   ?? 5L      $
??auto ?6+   ?? 5M      $
??auto ?6+   ?? 5N      $??* ?6+   .+?5Q      ?? -   5        $ btnOk??5Q      $??1??6@     MaterialDesignRaisedButton#??[ 6P   $??5q?6?   $	??100??6?   $??OK??6?    ? ToolTip$ Ok??6?   ?? 5R      -   5        $ 	btnCancel??5R      $??2??6H   #??[ 6X   $??5q?6?   $	??100??6?   $??Cancel??6?   $ Cancel??6?   ?? 5S      -   5        $ btnYes??5S      $??1??6B   #??[ 6R   $??5q?6?   $	??100??6?   $	??Yes??6?   $	 Yes??6?   ???	#FF0065C4???	#FFE9E6EE6?   ?? 5T      -   5        $ btnNo??5T      $??2??6@   #??[ 6P   $??5q?6?   $	??100??6?   $??No??6?   $ No??6?   ???	#FFB73A51???	#FFDDDCDE6?   5U      5W      5X      5Y      5Z      