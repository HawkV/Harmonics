   M S B A M L   `   `   `  ����  
Harmonic_1    Harmonic_1.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Y UDynamicDataDisplay, Version=0.3.0.0, Culture=neutral, PublicKeyToken=5b7d744a7263923fb ^DynamicDataDisplay.Controls, Version=1.0.0.0, Culture=neutral, PublicKeyToken=5b7d744a7263923f^ ZDynamicDataDisplay.Maps, Version=0.1.0.0, Culture=neutral, PublicKeyToken=5b7d744a7263923fW SDynamicDataDisplay.Coastline, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullCd34http://research.microsoft.com/DynamicDataDisplay/1.0     5      -   5          �Name$	  Win��5       � Title$ Harmonic��5   	   $	��768��6   $
��1210��6(   .��5      � .+�5(   
   � ��5)      �� 5*       �� EndPoint$ 0.5,1&�6&    �� 
StartPoint$ 0.5,0&�67   .��5+      �  � Color$ 	#FF3B3B3B��6#    � Offset$ 0Q�65   � 5,      $ 	#FFADADAD��6#   $ 1Q�65   5-      .+�50      �� -   5        $��border1��50   ?   
�������$��15q�6*    �� CornerRadius$ 17��6N   $��12,10,0,480q�6`   $
��Left=�6u   $	��475��6�   ._�51      � -   5         �Name$ A_Grid��51      ���	#FFFFFFFF$��-5q�6>   .+�52      �� -   5         ��Name$ Select_Cont��52   !   $
��Left=�66   $��
10,10,0,45q�6Q   $	��435��6e   �� 53      -   5        $!��Удалить сигнал��53      $
��Left=�66   $��
282,0,0,10q�6Q   $	��163��6e   $��30��6�   $��Bottom=�6�   �� 54      -   5        $#��Добавить сигнал��54      $
��Left=�67   $��	10,0,0,10q�6R   $	��163��6e   $��30��6�   $��Bottom=�6�   55      56      � 5H      -   5        $ VectGrid��5H      $��12,0,0,0q�6%   $
��Left=�67   $	��475��6R   $	��475��6^   $��Bottom=�6k   .+�5I      8  2Microsoft.Research.DynamicDataDisplay.ChartPlotter  -   5        	  Name$	 vectPlotter��5I   "   5J        5L      -	   5        $	 
sinPlotter��5L      $��493,10,10,345q�62   3  -Microsoft.Research.DynamicDataDisplay.Plotter
   Children.
 5M      =  7Microsoft.Research.DynamicDataDisplay.VerticalAxisTitle  $��12(�6'   .��65   y68   ?  9Microsoft.Research.DynamicDataDisplay.HorizontalAxisTitle  5N      $��12(�6)   .��67   x6:   5O        5Q      -
   5        $	 specPlotter��5Q      $��
493,0,10,0q�63   $	��340��6G   $��Bottom=�6T   .
 5R        $��12(�6'   .��65   A68     5S      $��12(�6)   .��67   F6:   5T      5V      5W      5X      