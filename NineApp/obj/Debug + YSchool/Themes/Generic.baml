   M S B A M L   `   `   `  ���� B  >NineApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:Nine.Views.ControlsNine.Views.Controls  E ARadialMenu, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullM5clr-namespace:RadialMenu.Controls;assembly=RadialMenuRadialMenu.Controls T PMahApps.Metro, Version=1.1.2.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4fV;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.MetroMahApps.Metro.Controls e aDe.TorstenMandelkow.MetroChart, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d5b8d1bbc99c4a2awTclr-namespace:De.TorstenMandelkow.MetroChart;assembly=De.TorstenMandelkow.MetroChartDe.TorstenMandelkow.MetroChart W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089  �� MergedDictionaries �� Source   Key  RedNine �� Color  RedNineLight  RedNineDark  RedAccentBackground  RedAccentBorder  LessonTileBackground  LessonTitleTileBackground  FlyoutBackground 	 ContentBackground 
 SideBarBackground �� 
TargetType �� Property �� Value �� 
TargetType A�Name �� Triggers	 P� Property
 P� Value  MasterSwitch �� BasedOn  RedAccentBackgroundBrush  RedAccentBorderBrush+    %Nine.Views.Controls.TouchScrollViewer � ColumnDefinitions � RowDefinitions �Name#    IsContentManipulationEnabled J� IsManipulationEnabled ��Name ��Name �� Orientation    VerticalOffset � Value �� ScrollableHeight � Maximum �� ViewportHeight �� ViewportSize* �� #ComputedVerticalScrollBarVisibility    HorizontalOffset �� ScrollableWidth �� ViewportWidth, �� %ComputedHorizontalScrollBarVisibility �� Padding  �� CornerRadius  OverridableButton  LessonThumb! K� ZIndex  RedNineBrush" � Points# 8� X$ 8� Y% J� OpacityMask& j� Stretch  appbar_check' !� Visual  LessonThumbActive  appbar_draw_pixel_fill_grid  appbar_list_check  ClassicLayerSelecter  ClassicLayerSelecterActive
( � Tag) J� SnapsToDevicePixels* �� Triggers+ �� 
TargetName  
GrayBrush3, �� ScaleX- �� ScaleY. �� CenterX/ �� CenterY  TileButtonStyle  student_icon  teacher_icon0 J� IsHitTestVisible
1 �� Top2 �� Left�� D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    :x,http://schemas.microsoft.com/winfx/2006/xaml     .Controls!clr-namespace:Nine.Views.Controls  D
RadialMenu5clr-namespace:RadialMenu.Controls;assembly=RadialMenu  GMahApps;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.Metro  ^chartTclr-namespace:De.TorstenMandelkow.MetroChart;assembly=De.TorstenMandelkow.MetroChart    �� $) #/Resources/ConverterDictionary.xaml?��� $&  /Resources/EntypoDictionary.xaml?��� $$ /Resources/IconDictionary.xaml?��� $# /Resources/TagDictionary.xaml?��� $I Cpack://application:,,,/MahApps.Metro;component/Styles/Controls.xaml?��� $F @pack://application:,,,/MahApps.Metro;component/Styles/Fonts.xaml?��� $G Apack://application:,,,/MahApps.Metro;component/Styles/Colors.xaml?��� $L Fpack://application:,,,/MahApps.Metro;component/Styles/Accents/Red.xaml?��� $R Lpack://application:,,,/MahApps.Metro;component/Styles/Accents/BaseLight.xaml?��� ��RedNine  #ED3423����RedNineLight  #ED7D72����RedNineDark  #C73427����RedAccentBackground  #E33C2C����RedAccentBorder  #E22312����LessonTileBackground  #E51400����LessonTitleTileBackground  	#CCE51400����FlyoutBackground  #2B2B2B����ContentBackground  #E5E5E5����SideBarBackground  #DCDCDC���� RedNineBrush  # [ �� RedNineLightBrush  # [ �� RedNineDarkBrush  # [ �� RedAccentBackgroundBrush  # [ �� RedAccentBorderBrush  # [ �� LessonTileBackgroundBrush  # [ �� "LessonTitleTileBackgroundBrush  # [ �� FlyoutBackgroundBrush  # [ �� ContentBackgroundBrush  # [	 �� SideBarBackgroundBrush  # [
 �� MasterSwitch  " ��.���  � ��
 ���� ��  � �� �#B0B0B0��  � �� �#000��  � �� �� " ��.R�� .+�A� $��4q�
������ $��0��#��z��A� $ ellipse��$��4q�#��z��$��2��
������ �� $��Center=�$��Center=� P� 	 �@J�IsMouseOver
 . . ���  � �� �#898989P� 	 � ��
 . . ���  � �� �#494949�� MasterSwitchActive  " ��# [ .���  � ��# [ ��  � ��# [ ��  � �� �#FFF P� 	 �@J�IsMouseOver
 . . ���  � ��# [ P� 	 � ��
 . . ���  � ��# [ �� " ��.���  �@�FocusVisualStyle Z� �� " ��.���  �@�FocusVisualStyle Z� �� " ��.���  �@�FocusVisualStyle Z� �� "   .���  �@�FocusVisualStyle Z� ��  � ��$ Visible=���  � ��$ Visible=���  � �� �� "   .R��  �� $��* ��� $
��Auto � �� $��* ��� $
��Auto �.+�� $' !PART_ScrollContentPresenterParent��$��0��$��0��# z .+��� $! PART_ScrollContentPresenter���� $ PART_VerticalScrollBar��$ Vertical=�$��0��$��1��# z # z # z #y�z �� $ PART_HorizontalScrollBar��$ 
Horizontal=�$��1��$��0��# z # z # z #y�z �� OverridableButton  " ��.���  �@�OverridesDefaultStyle . ��  � {� . ��  � �� �#111111��  � ��$ 1q���  � ��$ 1q���  � �� �#CCCCCC��  � �� �#F7F7F7��  � �� �� " ��.R��� # z��#��z��#��z��$  0��#��z��._��� #��z��$��Center=�$��Center=� P� 	 �@J�IsMouseOver
 . . ���  � �� �#EFEFEF��  � �� �#AFAFAFP� 	 � ��
 . . ���  � �� �#D8D8D9��  � �� �#333333P� 	 � {�
 .  . ���  � �� �#FAFAFA��  � �� �#E3E3E3��  � �� �#AFAFAF�� LessonThumb  " ��# [ .���  � �� �� " ��.R��� # z��$��2q�#��z��$  0��#��z��._��� #��z��$��Center=�$��Center=��� LessonThumbActive  " ��# [ .���  � {� .  ��  � �� �� " ��.R�� .+��� $! 0��# z��$��2q�#��z��$  0��#��z��._��� #��z��$��Center=�$��Center=�� $! 10��#��[ $��Right=�$	��Top=�" 8� $# 0Q�$$ 0Q�8� $# 24Q�$$ 0Q�8� $# 24Q�$$ 24Q��� $! 20�����#FFFFFF$��Right=�$	��Top=�$��10��$��8��$��3q�% !� $
& Fill=�#' [  P� 	 � {�
 .  . ���  � �� �#F9F9F9��  � ��# [ �� LessonThumbTeacherActive  " ��# [ .���  � �� �#F9F9F9��  � �� �#0000FF�� LessonThumbBothActive  " ��# [ .���  � �� �� " ��.R�� .+��� $! 0��# z��$��2q�#��z��$  0��#��z��._��� #��z��$��Center=�$��Center=�� $! 10�����#AA00FF$��Right=�$	��Top=�" 8� $# 0Q�$$ 0Q�8� $# 24Q�$$ 0Q�8� $# 24Q�$$ 24Q��� $! 20�����#FFFFFF$��Right=�$	��Top=�$��10��$��8��$��3q�% !� $
& Fill=�#' [  P� 	 � {�
 .  . ���  � �� �#F9F9F9��  � �� �#AA00FF�� ExerciseThumbGraphic  " ��# [ .���  � �� �� " ��.R��� .+��� # z��#��z��
�������$��2q�._��� $��50��$��50�����	#FF555555$��20q�% !� $& Uniform=�#' �  �� $��Right=�
���   �$
��Bold"��� #��z���� ExerciseThumbGraphicActive  " ��# [ .���  � �� �� " ��.R��� .+��� # z��
���   �
�������$��2q�._��� $��50��$��50�����	#FF555555$��20q�% !� $& Uniform=�#' �  �� $��Right=�
���   �$
��Bold"��� #��z���� ExerciseThumbQuiz  " ��# [ .���  � �� �� " ��.R��� .+��� # z��#��z��
�������$��2q�._��� $��50��$��50�����	#FF555555$��20q�% !� $& Uniform=�#' �  �� $��Right=�
���   �$
��Bold"��� #��z���� ExerciseThumbQuizActive  " ��# [ .���  � �� �� " ��.R��� .+��� # z��
���   �
�������$��2q�._��� $��50��$��50�����	#FF555555$��20q�% !� $& Uniform=�#' �  �� $��Right=�
���   �$
��Bold"��� #��z���� ClassicLayerSelecter  " ��# [ .���  � �� �� " ��.R��� # z��$��2q�#��z��$  0��#��z��._��� #��z��$��Center=�$��Center=��� QuestionLayerSelecter  " ��# [ .���  � �� �#4960EA��  � �� �#FFFFFF��  � �� �#999999 P� 	 �@J�IsMouseOver
 . . ���  � �� �#3E54DB��  � �� �#FFFFFF��  � �� �#888888P� 	 � ��
 . . ���  � �� �#3E54DB��  � �� �#FFFFFF��  � �� �#444444�� AnswerLayerSelecter  " ��# [ .���  � �� �#56CF66��  � �� �#FFFFFF��  � �� �#999999 P� 	 �@J�IsMouseOver
 . . ���  � �� �#4FC45F��  � �� �#FFFFFF��  � �� �#888888P� 	 � ��
 . . ���  � �� �#4FC45F��  � �� �#FFFFFF��  � �� �#444444�� ClassicLayerSelecterActive  " ��# [ .���  � {� .  ��  � �� �� " ��.R�� .+��� $! 0��# z��$��2q�#��z��$  0��#��z��._��� #��z��$��Center=�$��Center=�� $! 10��#��[ $��Right=�$	��Top=�" 8� $# 0Q�$$ 0Q�8� $# 24Q�$$ 0Q�8� $# 24Q�$$ 24Q��� $! 20�����#FFFFFF$��Right=�$	��Top=�$��10��$��8��$��3q�% !� $
& Fill=�#' [  P� 	 � {�
 .  . ���  � �� �#111111��  � �� �#F9F9F9��  � ��# [ �� QuestionLayerSelecterActive  " ��# [  P� 	 � {�
 .  . ���  � �� �#4960EA��  � �� �#FFFFFF�� AnswerLayerSelecterActive  " ��# [  P� 	 � {�
 .  . ���  � �� �#56CF66��  � �� �#FFFFFF�� FlyoutButtonStyle  " ��.���  � ��$ 30����  � ��$	 0,5q���  � �� �� " ��.R�� $
 grid�� �� $��15 ��� $��20 ��� $��* �.+��� $��1��
�������$��20��$��20��#% z( �� $��2��$��17,0,0,0q���. #) z) $
��Auto��$��Segoe UI Light)�$��16(�$��Center=�* P� 	 �@J�	IsFocused
 . P� 	 �@��IsDefaulted
 . P� 	 �@J�IsMouseOver
 . . ��� $
+ grid��# [  � ��P� 	 � ��
 . P� 	 � {�
 .  ��  � ��$ 14(��� TileButtonStyle  " ��.���  � ��$	 140����  � ��$	 140����  � ��
 �������  � ��# [ ��  �@J�RenderTransformOrigin$ 0.5,0.5&���  � �� �� " ��.R��� #��z��#��z��$��2q�._��� ��. * P� 	 � ��
 . . ���  � z� �� $
, 0.98Q�$
- 0.98Q�$	. 0.5Q�$	/ 0.5Q���  TileAuthenticateStudentStyle  " ��# [ .���  � �� �	#FF3E3E3E��  � ��$ 18(���  � �� �� " ��.R��� #��z��$��1q�$  0��._�� #��z�� �� $��50 ��� $��* �.+��� 
�������$��45��$��45��$��0��% !� #' [ $& Uniform=�� 
�������$��1��.+��� ��. $��Center=�$
��15 0q�* P� 	 � ��
 . . ���  � z� �� $
, 0.98Q�$
- 0.98Q�$	. 0.5Q�$	/ 0.5Q���  TileAuthenticateTeacherStyle  " ��# [ .���  � �� �	#FF3E3E3E��  � ��$ 18(���  � �� �� " ��.R��� #��z��$��1q�$  0��._�� #��z�� �� $��50 ��� $��* �.+��� 
�������$��45��$��45��$��0��% !� #' [ $& Uniform=�� 
�������$��1��.+��� ��. $��Center=�$
��15 0q�* P� 	 � ��
 . . ���  � z� �� $
, 0.98Q�$
- 0.98Q�$	. 0.5Q�$	/ 0.5Q��� NineCheckBox  " ��.���  �@�FocusVisualStyle Z� ��  �@J�SnapsToDevicePixels . ��  � �� �#444444��  � ��$ 30����  � ��$ 30����  � �� �� " ��.R�� 
������ #��z��#��z��.+�(� .���� $��	35.297001��$��	30.292999��.+�F� #��z��p����    �x р���x р��<��    <�Q    ��n���?�@KL ��P �@KL ����@x}���Q@x}x�&1,@Q@KL x�&1,@	�� $��	-9 -7 0 0q�0 .   P� 	 �@J�IsMouseOver
 . . ���  � �� �#000000P� 	 �@d�	IsChecked$

 TrueY�. ���  � �� �� " ��.R�� 
������ #��z��#��z��.+�(� .���� $��	35.296875��$��	30.292969��.+�F� #��z�������    ix �    �;�р���;�Q������ C(@�@x}Ȑ�@x}����@KL ���Q@KL ��� ,@Q�Av��� ,@Q��� zo��?Q     zo��?	F� ���#E22312F����f�    ��6��� ��	� ~� ��{a ��� с�E�q�k�U-/ �f�    	�� $��	-9 -7 0 0q�0 .  �� NineRadioButton  " 	�.���  �@�FocusVisualStyle Z� ��  �@J�SnapsToDevicePixels . ��  � �� �#444444��  � ��$ 30����  � ��$ 30����  � �� �� " 	�.R�� 
������ #��z��#��z��.+�(� .���� $��30��$��30��.+�A� $��26��$��26��#��z��$��4���� $��	-8 -4 0 0q�0 .   P� 	 �@J�IsMouseOver
 . . ���  � �� �#000000P� 	 �@d�	IsChecked$

 TrueY�. ���  � �� �� " 	�.R�� 
������ #��z��#��z��.+�(� .���� $��30��$��30��.+�A� $��26��$��26��#��z��$��4��A� $1 7��$2 7��$��12��$��12�����#E22312�� $��	-8 -4 0 0q�0 .  %(   '��       '��       '��       '         