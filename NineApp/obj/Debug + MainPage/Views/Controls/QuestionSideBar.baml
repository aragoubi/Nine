   M S B A M L   `   `   `  ���� T  PMahApps.Metro, Version=1.1.2.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4fV;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.MetroMahApps.Metro.Controls  B >NineApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullC&clr-namespace:Nine.ViewModels.ControlsNine.ViewModels.Controls )   #Nine.Views.Controls.QuestionSideBar   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     PControls;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.Metro     8local&clr-namespace:Nine.ViewModels.Controls     .���   � RowDefinitions  �� $��70 ��� $��* ��� $
��Auto �.+�� $��0�� � ColumnDefinitions �� $��70 ��� $��* �.+��� -   $��0��   MetroCircleButtonStyle#���   $��0��$��50��$��50��.���� $��20��$��20��
���   � J� OpacityMask !�   appbar_lines_horizontal_4 !� Visual# [  j� Stretch$
 Fill=��� $��1��$��Segoe UI Light)�
������ $��8 0 0 0q�$��24(�$��Stretch=�$��Center=�	Questions�� $��1��$��Disabled=�$��0 0 0 10q� �� PanningMode$ VerticalFirst=� �� PanningDeceleration$ 0.001Q�.���� -    ��Name$ ThumbsContainer������ *	Questions+
 � Tag �� ���� .R��� 	 �� Orientation$	 Vertical=����� .R��� $��
10 10 10 0q����� *Key+���� 
 �� Path$
 Tag.GoToPage� �� RelativeSource ��  �� AncestorType" ����`�   ExerciceKindToThumbStyle `� 	Converter# [ .1��� $	
 Key��� $
 Tag.CurrentPageIndex� �� " ���� $
 
Value.Kind����� $
 
Value.Name�� $��2��y��� *IsTeacherMode+  BooleanToVisibility �� 	Converter# [  �� $��* ��� $��* �.+��� $��0��$��5 5 5 5q�$��50������ *CreateExercise+.��	Nouveau�� $��2��$��5 5 5 5q�$��50������ *StartCurrentExercise+y��� *
CanBeStart+# [ .��	Démarrer�� $��2��$��5 5 5 5q�$��50������ *CollectCurrentExercise+y��� *
CanCollect+# [ .��	Collecter