   M S B A M L   `   `   `  ���� H  DPlotterDataGH, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:WpfApp2WpfApp2      WpfApp2.InitSetup   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     'localclr-namespace:WpfApp2     T PMahApps.Metro, Version=2.0.0.0, Culture=neutral, PublicKeyToken=51482d6f650b2b3f6mah,http://metro.mahapps.com/winfx/xaml/controls    � Title$  Setup��$	��850��$	��450��$	��450��$	��450��$	��857��.��� -    �Name$ InitSetupGrid�� � RowDefinitions �� $
��Auto ��� $��20 ��� $��* ��� $��10 � � ColumnDefinitions �� $��40 ��� $��* ��� $��40 �.+�� $
��Left=�$��2��.+���  �� Orientation$ 
Horizontal=�.+��� $ 
Horizontal=�$��300px��$
��60px��.+��� $��Pictures/logo.png��� $��Right=�$��2��.+��� .+��� $ 
Horizontal=�.+��� $ 
Horizontal=�$
��60px��$
��60px��� $��2��$��1��$��Stretch=�$��Stretch=�.+��� 
���   �$��Stretch=�$��-2q��  �� $	��170 ��� $	��110 ��� $	��110 ��� $	��110 ��� $��* ��� $��10 � �� $
��60px ��� $
��40px ��� $
��40px ��� $
��40px ��� $
��40px ��� $
��10px ��� $��* �.+��� -    ��Name$ lblSetup��$��6��$��SETUP��$��Center=�$��Center=�$��24(��� -   $ lblBedrijfsnaam��$��0��$��1��$��Right=�$�� Bedrijfsnaam��$	��Top=�$��20(��� -   $ lblContactpersoon��$��0��$��2��$��Right=�$��Contactpersoon��$	��Top=�$��20(��� -   $ lblEmail��$��0��$��3��$��Right=�$��Email��$	��Top=�$��20(��� -   $ lblTelefoonnummer��$��0��$��4��$��Right=�$��Telefoonnummer��$	��Top=�$��20(��� -    ��Name$ tbxBedrijfsnaam��$��4��$��1��$��1��$��2q� �� TextWrapping$
 Wrap=��� -   $ tbxContactpersoon��$��4��$��1��$��2��$��2q�$
 Wrap=��� -	   $ tbxEmail��$��4��$��1��$��3��$��2q�$
 Wrap=��� -
   $ tbxTelefoonnummer��$��4��$��1��$��4��$��2q�$
 Wrap=��� -    ��Name$ btnNL��$y�Hidden=�$��1��$��6��$
��Left=�$	��Top=�$��69��$	��100����Z� ���� 	 �� ImageSource$	 Pictures/NL.png���� -   $ btnFR��$y�Hidden=�$��2��$��6��$
��Left=�$	��Top=�$��69��$	��100����Z� ���� $	 Pictures/FR.png���� -   $ btnUK��$y�Hidden=�$��3��$��6��$
��Left=�$	��Top=�$��69��$	��100����Z� ���� $	 Pictures/UK.png���� -   $ 	btnCancel��$��6��$��Annuleer��$
��Left=�$��Bottom=�$��10q�$��50��$	��100��
���   ���Z� �� -   $ btnNext��$��6��$��2��$��4��$��Verder��$��Right=�$��Bottom=�$��10q�$��50��$	��100��
���   ���Z� �� -   
 ��Name$
 cbxSendData��$'��!Data opsturen naar Goedhart groep��$��1��$��10��$
��Left=�$��6��$	��Top=�