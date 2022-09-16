This is a WPF usercontrol for displaying user updates through an alert bar. There are four types of alerts: success, danger, warning or information. The color scheme and icons for each are based on the type.


***************
GUI
***************

In the xaml you must reference the namespace to add the usercontrol:

    <Window ...
            xmlns:mbar="clr-namespace:AlertBarWpf;assembly=AlertBarWpf">



Using this reference place the control on the form. I typically position this above any other controls:

    <mbar:AlertBarWpf x:Name="msgbar" />



An optional IconVisibility parameter to remove icons from all alert messages. There is also a Theme parameter to adjust the look of the bar:

    <mbar:AlertBarWpf x:Name="msgbar" IconVisibility="False" Theme="Outline" />



***************
Code Behind
***************

To make use of the control we trigger it in the xaml.cs. Here are some examples:

    msgbar.Clear();
    msgbar.SetDangerAlert("Select an Item.");
    msgbar.SetSuccessAlert("Orders Successfully Processed!", 5); //will auto-close after 5 seconds
    msgbar.SetWarningAlert("No order info found so declared a re-order.");
    msgbar.SetInformationAlert("Sale begins on Aug 1");