   M S B A M L   `   `   `  ����A  =Keskus, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:KeskusKeskus  5          Keskus.RoomsV   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      &localclr-namespace:Keskus     5        N� Title$  RoomsV��5	      .��5      � .+�5   
   �� $��Ruumid��6   $
��Left=�6"   $��	38,29,0,0q�6=   $	��Top=�6P   $	��310��6h   $��50��6t   $��0,5,5,5q�6�   $��21.333(�6�   �� 5   
   -   5         ��Name$ 	listRooms��5      $
��Left=�6&   $	��356��6A   $��	38,93,0,0q�6N   $	��Top=�6a   $	��397��6y   $��1��6�    �� View 5      �� 5      -   5        listRoomsGrid��.P�5      �� $��ID��6S    �� Width$ 50��6_    �� DisplayMemberBinding 6%   ��  �� Path$ RoomID�6_   �� 5      $��Nimetus��6Q   $	 180��6b    6%   �� $
 Name�6b   �� 5      $��	Istekohad��6R   $	 100��6e    6%   �� $ Seats�6e   �� 5      $��Aktiivne��6S   $ 50��6e    6%   �� $ Active�6e   5      5      �� 5   
   -   5         ��Name$ btnUpdateRoom��5      $��Muuda andmed��6(   $��Right=�6?   $��	0,0,31,28q�6[   $��Bottom=�6n   $	��106��6�   $��45��6�    J� RenderTransformOrigin$ 0.498,-0.088&�6�   $��2��6�   ��5      �� 5       �� 
TargetType" ��6   	 �� Triggers	 5      �� 5      
 �� Binding
 6&   �� $ SelectedItem� �� ElementName$ 	listRooms�� �� Value 5!      Z� .a�5"      ��  �� Property � {� �� Value .  6:   5#      5%      5'      �� 5*   
   $
��Left=�6   $	��249��6,   $��454,115,0,0q�69   $	��Top=�6N   $	��217��6f   ��5+      �� 5,       �� Color$ 	#FFF0F0F0��6"   .+�5/      �� -   5        $ btnPlan1��5/      $��89��6)   $	��217��66   
����������	#FF7DBA1D$��3q�6m   |�.   J� IsHitTestVisible .  ��.  6�   �� 50      -   5        $ btnPlan3��50      $��70��6'   ���	#FF7DBA1D$��3q�6K   
 �� Top$ 87��6_   $	��101��6o   
�������|�.   .  ��.  6�   �� 51      -   5        $ btnPlan2��51      ���	#FFFFFFFF$��95��6?   ���	#FF7DBA1D$��3q�6d   $	 154��6x   $	��101��6�   |�.  ��.   .  6�   �� 52      -   5         ��Name$ label��52      $��fuajee��6#    �� Left$	 141��64   $	 170��6F   {�.  ��53      �� 54      # � ��6&   56      58      �� 59   
   -   5        $ label1��59      $��Keskuse plaan:��6    $
��Left=�69   $��
454,84,0,0q�6T   $	��Top=�6h   5<      5=      