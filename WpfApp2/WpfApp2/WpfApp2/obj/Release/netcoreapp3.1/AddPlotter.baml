   M S B A M L   `   `   `  ���� H  DPlotterDataGH, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:WpfApp2WpfApp2      WpfApp2.AddPlotter   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     'localclr-namespace:WpfApp2       � Title$  Add/Edit Plotter��$	��390��$	��390��$	��390��$	��800��.��� .+�� $��0��$��0��$��3��.+�� $
��Left=�.+���  �� Orientation$ 
Horizontal=�.+��� $ 
Horizontal=�$��300px��$
��60px��.+��� $��Pictures/logo.png��� $��2��$��1��.+��� $��0��$��0��$��10��$��10��
���   ��� -    ��Name$ lblPlotSettings��$��Plotter Settings��$��Center=�$��Center=�$��24(�$��0��$��0��$��10���� -   $ lblPlotNaam��$��Right=�$��Plotter naam:��$��Right=�$��Center=�$��24(�$��0��$��1���� -   $ 	lblPlotIP��$��Right=�$��Plotter IP:��$��Right=�$��Center=�$��24(�$��0��$��2���� -   $ lblPlotType��$��Right=�$��Plotter Type:��$��Right=�$��Center=�$��24(�$��0��$��3���� -    ��Name$ tbxPlotNaam�� �� TextWrapping$
 Wrap=�$��1��$��1��$��4��$��2q��� -   $ 	tbxPlotIP��$
 Wrap=�$��1��$��2��$��4��$��2q��� -    ��Name$ 	btnCancel��$��5��$��Annuleer��$
��Left=�$��Bottom=�$��10q�$��50��$	��100��
���   ���Z� �� -   $ btnNext��$��5��$��2��$��4��$��Verder��$��Right=�$��Bottom=�$��10q�$��50��$	��100��
���   ���Z� �� -	    ��Name$ cbxPlotterType��$��3��$��4��$��1��$��2q��� -
   $ 	btnDelete��$��5��$��1��$��Delete��$
��Left=�$��Bottom=�$��	10,0,0,10q�$��50��$	��100��
���  ��
���  ����Z�  � ColumnDefinitions �� $	��170 ��� $	��110 ��� $	��110 ��� $	��110 ��� $��* ��� $��10 � � RowDefinitions �� $
��60px ��� $
��40px ��� $
��40px ��� $
��40px ��� $
��10px ��� $��* ��� $��1��$
��Left=�$��532,102,0,0q�$��3��$	��Top=�$	��120�� �� $
��Auto ��� $��20 ��� $��* ��� $��10 � �� $
��40px ��� �� $
��40px �