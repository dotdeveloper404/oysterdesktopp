   M S B A M L   `   `   `  ???? D  @OysterVPN, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:OysterVPN	OysterVPN       OysterVPN.MessageBoxCustom   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     )localclr-namespace:OysterVPN     b ^MaterialDesignThemes.Wpf, Version=4.0.0.2422, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes    ? Title$  MessageBoxWindow??$	??200??$	??300?? ? WindowStartupLocation$ CenterScreen=? ? WindowStyle$
 None=?
??????? ? Topmost .  ? ShowInTaskbar .     MaterialDesignBody#???   $??Regular"?$??10(?&   System.Windows.Media.TextOptions   TextFormattingMode$ Ideal=? ? 
ResizeMode$ NoResize=?   TextRenderingMode$
 Auto=?  MaterialDesignFont#???  $??Normal"? ? 	Resources 	 ?? MergedDictionaries
 ?? Source?? 	 ?? $h
 bpack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Defaults.xaml???? $t
 npack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Primary/MaterialDesignColor.Blue.xaml???? $s
 mpack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Accent/MaterialDesignColor.Lime.xaml??%    .???  ? ColumnDefinitions ?? $	??19* ??? $
??178* ??? $
??295* ? ? RowDefinitions ?? $
??auto ??? $??* ?.+?#  MaterialDesignThemes.Wpf.Card  -     Name$ 
cardHeader??$??0??$??10q?$??0q?+  %MaterialDesignThemes.Wpf.ShadowAssist   ShadowDepth$ Depth5=????	#FF0065C4 ! PrimaryHueDarkForegroundBrush#???  $y?Visible=?$??3??.???? .+??? -    ??Name$ txtTitle??$??Center=?$??Stretch=?   MaterialDesignTitleTextBlock#???  $??20(?Message Title?? $??Right=?$??0,5,5,0q?$	??Top=?$??2??.+??? -    ??Name$ btnClose??$??35??$??35??
??????????	#FF0065C4 * &MaterialDesignFloatingActionDarkButton#??[ ???	#FF0065C4.??'  !MaterialDesignThemes.Wpf.PackIcon     Kind$ Close=?? $??1??$??3??.+?? $??20q? ?? $
??auto ??? $
??auto ??? $??* ?.+?? $??0?? ?? $
??auto ??? $??* ?.+??  9MaterialDesignThemes.Wpf.Transitions.TransitioningContent  C  =MaterialDesignThemes.Wpf.Transitions.TransitioningContentBase   OpeningEffects ;  5MaterialDesignThemes.Wpf.Transitions.TransitionEffect     Kind$ FadeIn=?  $ SlideInFromBottom=?.???? -    ??Name$ 
txtMessage??$??Center=? ?? 
IsReadOnly . $??0??$??5q?)  #MaterialDesignThemes.Wpf.HintAssist   Hint$  ??$??18(? % !MaterialDesignFloatingHintTextBox#??[  ?? 
CaretBrush ?	#FFDCD9E0? $??1??$??0,20,0,5q? ?? $??* ??? $
??auto ??? $
??auto ??? $??* ?.+??? -   $ btnOk??$??1??  MaterialDesignRaisedButton#??[ $??5q?$	??100??$??OK?? ? ToolTip$ Ok???? -   $ 	btnCancel??$??2??#??[ $??5q?$	??100??$??Cancel??$ Cancel???? -   $ btnYes??$??1??#??[ $??5q?$	??100??$	??Yes??$	 Yes?????	#FF0065C4???	#FFE9E6EE?? -   $ btnNo??$??2??#??[ $??5q?$	??100??$??No??$ No?????	#FFB73A51???	#FFDDDCDE