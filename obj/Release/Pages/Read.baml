   M S B A M L   `   `   `  ����   ModbusExaminer    ModbusExaminer.Pages.Read   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     Z VFirstFloor.ModernUI, Version=1.0.9.0, Culture=neutral, PublicKeyToken=bc9b0c37bf06c6a90mui&http://firstfloorsoftware.com/ModernUI  $��613.604��$��564.31��  � 	Resources     RoundButtonTemplate �� 
TargetType �� Property �� Value �� 
TargetType �� CornerRadius �� Triggers P� Property P� Value  ViewButtonTemplate��%%  &	        &	     �� " ��.���  � ��
 �  ����  � ��
 �������  � �� �� " ��.R��� $ 15��#��z��$��1q�._��� $��Center=�$��Center=� P�  �@J�IsMouseOver . . ���  � �� �	#FF331818P�  � �� . . ���  � �� �	#FF8F0606�� " ��.���  � ��
 � � ���  � ��
 �������  � �� �� " ��.R��� #��z��._��� $��Center=�$��Center=� P�  �@J�IsMouseOver . . ���  � �� �	#FF819981P�  � �� . . ���  � ��
 � d �.���   ContentRoot#��[ $��16,28,9.6,16q�	 � ColumnDefinitions	 �� $
��104* ��� $
��269* �.+��� -   
 ��Name$
 label��$��IP��$
��Left=�$��	10,10,0,0q�$	��Top=��� -    ��Name$ IP��$
��Left=�$��23��$��	31,10,0,0q� �� TextWrapping$
 Wrap=�$	��Top=�$	��117�� J� RenderTransformOrigin$ -0.063,0.626&�����  �� Path$ DataContext.IPAddress��� -   $
 label_Copy1��$
��Port��$
��Left=�$��49.6,10,0,0q�$	��Top=�$��1���� -   $
 port��$
��Left=�$��23��$��83.6,10,0,0q�$
 Wrap=�$	��Top=�$��55��$ 1.179,0.429&�$��1������ $ DataContext.Port��� -   $
 label_Copy2��$��Start Address��$
��Left=�$��	10,64,0,0q�$	��Top=��� -   $ start_address��$
��Left=�$��23��$��	93,64,0,0q�$
 Wrap=�$	��Top=�$��61������ $ DataContext.Register��� -   $
 label_Copy3��$��ID��$
��Left=�$��
223,15,0,0q�$	��Top=�$��18��$��1���� -   $ slave_id��$
��Left=�$��23��$��
246,10,0,0q�$
 Wrap=�$	��Top=�$��55��$��1������ $ DataContext.DeviceId��� -	   $
 label_Copy4��$��Count
��$
��Left=�$��49.6,64,0,0q�$	��Top=�$��1���� -
   $ 	count_reg��$
��Left=�$��23��$��98.6,64,0,0q�$
 Wrap=�$	��Top=�$��40��$��1������ $ DataContext.Count��� -    ��Name$ 
readbutton��$	��Add��$
��Left=�$��
50,120,0,0q�$	��Top=�$��89��$��1��&   System.Windows.Controls.DataGrid  -     Name$ ConnectionsGrid��   SelectionMode$ Single=�   GridLinesVisibility$ 
Horizontal=�   SelectionUnit$ FullRow=�$��Stretch=�$��
9,164,37,0q�$	��Top=�$��2��$	��289��   AutoGenerateColumns .     
IsReadOnly .    CanUserResizeRows .  ���� *ConnectionInfo+   Columns 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn   Header$ Address��   MinWidth$ 50Q�   CanUserResize .  1  +System.Windows.Controls.DataGridBoundColumn   Binding �� *AddressPort+  $ Slave ID��$ 50Q� .   �� *Id+  $ Status��$ 50Q� .   �� *Status+ �� UpdateSourceTrigger$ PropertyChanged=�4  .System.Windows.Controls.DataGridTemplateColumn   .     CellTemplate �� .R��� -   $��80��$��20��$��0��$��0,0,0,0q�$��Center=�#��[ .��	View Data   .   �� .R��� -   #��[  $	��100��$��20��$��0��$��0,0,0,0q�$��Center=����#FFF.��Remove�� -   $
 label_Copy5��$��Type
��$
��Left=�$��
219,64,0,0q�$	��Top=�$��1���� -    ��Name$ devicetypeCmbox��$��1��$
��Left=� �� SelectedIndex$ 0��$��
249,64,0,0q�$	��Top=�$	��160��.@��� $��Holding Registers��$
��Left=�$	��160���� $��Input Registers��$
��Left=�$	��160���� $��Input Coils��$
��Left=�$	��160���� $��Output Coils��$
��Left=�$	��160���� -   $��LoadBtn��$��Load Configuration��$
��Left=�$��
31,472,0,0q�$	��Top=�$	��147��$��2���� -   $ SaveBtn��$��Save Configuration��$
��Left=�$��181,472,0,0q�$	��Top=�$	��147��$��1���� -   $��oneBasedCheckBox��$��One Based Addresses��$
��Left=�$��
10,120,0,0q�$	��Top=�$	��154��